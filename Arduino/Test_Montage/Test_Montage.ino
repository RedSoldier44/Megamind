#include <SPI.h>

const int pinSH_LD = 8;  // SH/LD (Latch) relié à D8

void setup() {
  pinMode(pinSH_LD, OUTPUT);
  digitalWrite(pinSH_LD, HIGH);

  Serial.begin(9600);

  // Configurer SPI
  SPI.begin();
  // Le 74HC165 lit les données sur le front montant du clock
  // Donc mode SPI = MODE0
  SPI.beginTransaction(SPISettings(1000000, MSBFIRST, SPI_MODE0));

  Serial.println("Lecture SN74HC165 via SPI");
}

uint16_t read165() {
  // 1. Charger les entrées parallèles
  digitalWrite(pinSH_LD, LOW);
  delayMicroseconds(5);
  digitalWrite(pinSH_LD, HIGH);

  // 2. Lire deux octets en série
  uint8_t highByte = SPI.transfer(0);
  uint8_t lowByte  = SPI.transfer(0);

  // Selon le chaînage, l’ordre peut être inversé (à tester)
  return ((highByte << 8) | lowByte);
}

void loop() {
  uint16_t val = read165();

  Serial.print("Bits lus: ");
  for (int i = 15; i >= 0; i--) {
    Serial.print((val >> i) & 1);
  }
  Serial.println();

  delay(500);
}