// ----------------------------------------------- Partie Horloge
#include <TimeLib.h>
// ----------------------------------------------- Partie Moteur
#include <Stepper.h>

const int stepsPerRevolution = 2048; // Define number of steps per rotation
Stepper myStepper = Stepper(stepsPerRevolution, 8, 10, 9, 11);
int stepsSinceOn = 0;
int currentStepsSinceOn = 0;
// ----------------------------------------------- Partie Capteur température et humidité et LED associée
#include "DHT.h"
#define DHTPIN 2
#define DHTTYPE DHT11

DHT dht(DHTPIN, DHTTYPE);
const int tempLedPin = 4;
// ----------------------------------------------- Partie Buzzer et LED associée
const int buzzerPin = 7;
const int alarmLedPin = 12;
int alarmTimer = 0;
bool alarmBool = false;
// ----------------------------------------------- Partie Reception de données
int ledState = 1;
int devLedState = 0;
int alarmState = 1;
int planning [3] = {1, 1, 1};
int horaires [3] = {10, 30, 50}; //9, 12, 19 pour les heures par défaut
bool pilulierStart = false;

void setup()
{
  Serial.begin(9600);
  
  pinMode(tempLedPin, OUTPUT);
  pinMode(alarmLedPin, OUTPUT);
  pinMode (buzzerPin, OUTPUT);
  pinMode (13, OUTPUT);
  
  dht.begin();
}

void loop()
{
  ReceveData();
  TempHumState();
  if(pilulierStart)
  {
    DevLedState();
    MotorState();
    AlarmState();
    SendData();
  }
  delay(1000);
}

void ReceveData()
{
  if (Serial.available() > 0) //Si de la donnée est reçu alors...
  {    
    String input = Serial.readStringUntil(':'); // la donnée est coupé en plusieurs paquets de deux informations : un nom et une valeur
    if (input != "") {
      String data = Serial.readStringUntil(';');
      unsigned long value = data.toInt();
      /*Serial.print(input);
      Serial.print(" : ");
      Serial.println(data);*/
      
      if(input == "dat"){ setTime(value); } // Nous attribuons chaque valeur reçue à sa variable associée (la référence est le nom)
      if(input == "led"){ ledState = value; }
      if(input == "ala"){ alarmState = value; }
      if(input == "dev"){ devLedState = value; }
      if(input == "hMa") { horaires[0] = value; }
      if(input == "hMi") { horaires[1] = value; }
      if(input == "hSo") { horaires[2] = value; }

      if(input == "pla")
      {
        for(int i=0; i < 3; i++) {
            planning[i] = data.substring(i, i+1).toInt();
        }
      }
    }
    pilulierStart = true;
  }
}

void DevLedState()
{
  if(devLedState == 1)
  {
    digitalWrite(13, HIGH);
  } else {
    digitalWrite(13, LOW);
  }
}

void MotorState()
{ 
  for(int i = 0; i < 3; i++)
  {
    if(second() == horaires[i] && planning[i] == 1)
    {
      myStepper.setSpeed(10); // ajuster la vitesse
      myStepper.step(stepsPerRevolution/2);
      alarmBool = true;
      currentStepsSinceOn++;
    }
  }
}

void AlarmState()
{   
  if(alarmBool && alarmTimer < 10)
  {
    if(alarmState != 0){
      tone(buzzerPin, 4000, 100); 
    }
    if(ledState != 0){
      digitalWrite(alarmLedPin, HIGH);
    }
    alarmTimer++;
  }else{
    digitalWrite(alarmLedPin, LOW);
    alarmBool = false;
    alarmTimer = 0;
  }
}

void SendData()
{
  if(currentStepsSinceOn > stepsSinceOn)
  {
    stepsSinceOn = currentStepsSinceOn;
  }
  String data = (String) stepsSinceOn + ";" + (String) dht.readTemperature() + ";" + (String) dht.readHumidity(); 
  Serial.println(data);
}

void TempHumState()
{
  if(dht.readHumidity() >= 25 || dht.readTemperature() >= 60)
  {
    digitalWrite(tempLedPin, HIGH);
  }else{
    digitalWrite(tempLedPin, LOW);
  }
}

/*void digitalClockDisplay() {
  // digital clock display of the time
  Serial.print(hour());
  printDigits(minute());
  printDigits(second());
  Serial.print(" ");
  Serial.print(day());
  Serial.print(" ");
  Serial.print(month());
  Serial.print(" ");
  Serial.print(year());
  Serial.println();
}

void printDigits(int digits) {
  // utility function for digital clock display: prints preceding colon and leading 0
  Serial.print(":");
  if (digits < 10)
  {
    Serial.print('0');
  }
  Serial.print(digits);
}*/
