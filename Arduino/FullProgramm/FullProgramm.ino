#include <SPI.h>

// BUTONS Pins
const int pinButtonMedium = 2; // Signal Medium Button (yellow with led) pin
const int pinButtonMediumLed = 3;  // (SUPPORT Analogic) Signal Medium Button (yellow with led) integrated Led pin
const int pinButtonBig = 4; // Signal Big Button (Green with led) pin
const int pinButtonBigLed = 5; // (SUPPORT Analogic) Signal Big Button (Green with led) integrated Led pin

// Rotative Interruptors (using 74HC165) pins
const int pinSH_LD = 8; // SH/LD (Latch) relié à D8
// Pin 12 => CLK (Clock) relié à D12
// Pin 13 => QH (Data) relié à D13

// Declare debounce delay
const unsigned long debounceDelay = 50; // ms
const unsigned long sendInterval = 300; // ms
const bool DEBUG = false;

// --- STRUCTURE POUR UN BOUTON ---
struct Button {
  int pin;
  int state; // état stable
  int lastReading; // dernière lecture brute
  unsigned long lastDebounceTime;
};

// --- INSTANCES DES BOUTONS ---
Button buttonMedium = {pinButtonMedium, HIGH, HIGH, 0};
Button buttonBig = {pinButtonBig, HIGH, HIGH, 0};

const int numChips = 5;       // Nombre de 74HC165 en série
uint8_t buffer[numChips];     // Tableau pour stocker les octets lus
uint8_t previousBuffer[numChips]; // initialisation à 0
unsigned long lastSendTime = 0;


void setup() {
  
  // Medium Button
  pinMode(pinButtonMedium, INPUT_PULLUP);
  pinMode(pinButtonMediumLed, OUTPUT);

  // Big Button
  pinMode(pinButtonBig, INPUT_PULLUP);
  pinMode(pinButtonBigLed, OUTPUT);

  // Interruptors
  pinMode(pinSH_LD, OUTPUT);
  digitalWrite(pinSH_LD, HIGH);

  // Start Serial
  Serial.begin(9600);

  // Configurer SPI
  SPI.begin();
  SPI.beginTransaction(SPISettings(250000, MSBFIRST, SPI_MODE0));

  // Light up buttons leds
  analogWrite(pinButtonMediumLed, 255);
  analogWrite(pinButtonBigLed, 255);

  // Boot Debug Logs
  Serial.print("Arduino prêt, lecture de ");
  Serial.print(numChips);
  Serial.println(" SN74HC165 via SPI");
}


// Fonction de lecture avec debounce
int readButton(Button &btn) {
  int reading = digitalRead(btn.pin);

  if (reading != btn.lastReading) {
    btn.lastDebounceTime = millis();
    btn.lastReading = reading;
    if (DEBUG) {
      Serial.print("raw change pin ");
      Serial.print(btn.pin);
      Serial.print(" reading=");
      Serial.println(reading);
    }
  }

  if ((millis() - btn.lastDebounceTime) > debounceDelay) {
    if (reading != btn.state) {
      btn.state = reading;
      return (reading == LOW) ? 1 : 0;
    }
  }

  return -1;
}


void read165(uint8_t* dest) {
  // Charger les entrées parallèles
  digitalWrite(pinSH_LD, LOW);
  delayMicroseconds(5);
  digitalWrite(pinSH_LD, HIGH);

  // Lire les octets depuis le chaînage
  for (int i = numChips - 1; i >= 0; i--) {
    dest[i] = SPI.transfer(0);
  }
}


void loop() {
  unsigned long now = millis();

  // Vérifier si Unity a envoyé quelque chose
  if (Serial.available()) {
    String input = Serial.readStringUntil('\n');
    input.trim();

    if (input == "ping") {
      Serial.println("pong"); // Réponse handshake
      return; // On ne fait rien d'autre dans ce cycle
    }
  }

// Lire les boutons
  int stateMedium = readButton(buttonMedium);
  int stateBig = readButton(buttonBig);

  if (stateMedium >= 0) {
    Serial.print("DATA_BTN_MED_");
    Serial.print(1 - buttonMedium.state);
    Serial.println();
  }

  if (stateBig >= 0) {
    Serial.print("DATA_BTN_BIG_");
    Serial.print(1 - buttonBig.state);
    Serial.println();
  }

  if (now - lastSendTime >= sendInterval) {
    lastSendTime = now;
    read165(buffer);

    bool changed = false;
    for (int i = 0; i < numChips; ++i) {
      if (buffer[i] != previousBuffer[i]) {
        changed = true;
        break;
      }
    }

    if (changed) {
      Serial.print("DATA_INTR_");
      for (int chip = 0; chip < numChips; ++chip) {
        for (int bit = 7; bit >= 0; --bit) {
          Serial.print((buffer[chip] >> bit) & 1);
        } if (chip < numChips - 1) Serial.print("|");
      } Serial.println();

      // mettre à jour le précédent
      for (int i = 0; i < numChips; ++i) {
        previousBuffer[i] = buffer[i];
      }
    }
  }
}