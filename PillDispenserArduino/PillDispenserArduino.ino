// ----------------------------------------------- Clock part
#include <TimeLib.h>
// ----------------------------------------------- Step by step engine part
#include <Stepper.h>

const int stepsPerRevolution = 2048; // Define number of steps per rotation
Stepper myStepper = Stepper(stepsPerRevolution, 8, 10, 9, 11);
int stepsSinceOn = 0;
int currentStepsSinceOn = 0;
// ----------------------------------------------- Temperature and humidity ssensors and associated LEDs part
#include "DHT.h"
#define DHTPIN 2
#define DHTTYPE DHT11

DHT dht(DHTPIN, DHTTYPE);
const int tempLedPin = 4;
// ----------------------------------------------- Buzzer and associated LED part
const int buzzerPin = 7;
const int alarmLedPin = 12;
int alarmTimer = 0;
bool alarmBool = false;
// ----------------------------------------------- Data reception part
int ledState = 1;
int devLedState = 0;
int alarmState = 1;
int planning [3] = {1, 1, 1};
int horaires [3] = {10, 30, 50}; //9, 12, 19 for default hours
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
  if (Serial.available() > 0) // If data is received so...
  {    
    String input = Serial.readStringUntil(':'); // Data is cut into pairs of 2 information : a name and a value
    if (input != "") {
      String data = Serial.readStringUntil(';');
      unsigned long value = data.toInt();
      
      if(input == "dat"){ setTime(value); } // We attribute each value received to its associated value (the reference is the data name)
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
      myStepper.setSpeed(10); // Modify speed
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
