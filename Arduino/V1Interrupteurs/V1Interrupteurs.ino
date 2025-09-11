#include <SPI.h>

const int pinSH_LD = 8;       // SH/LD (Latch) relié à D8
const int numChips = 5;       // Nombre de 74HC165 en série (modifiable)
uint8_t buffer[ numChips ];   // Tableau pour stocker les octets lus

void setup() {
  pinMode(pinSH_LD, OUTPUT);
  digitalWrite(pinSH_LD, HIGH);

  Serial.begin(9600);

  // Configurer SPI
  SPI.begin();
  SPI.beginTransaction(SPISettings(250000, MSBFIRST, SPI_MODE0)); // 

  Serial.print("Lecture de ");
  Serial.print(numChips);
  Serial.println(" SN74HC165 via SPI");
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
  read165(buffer);

  Serial.print("Bits lus: ");
  for (int chip = 0; chip < numChips; chip++) {
    for (int bit = 7; bit >= 0; bit--) {
      Serial.print((buffer[chip] >> bit) & 1);
    }
    if (chip < numChips - 1) Serial.print(" | "); // séparateur entre 8 bits
  }
  Serial.println();

  delay(500);
}