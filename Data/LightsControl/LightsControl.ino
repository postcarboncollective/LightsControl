#include <FastLED.h>
#define NUM_LEDS 100
#define DATA_PIN 3
#define LEDPIN 13
#define ID 0;
CRGB leds[NUM_LEDS];
int readMessageState = 0;

void setup() 
{
  Serial.begin(9600);
  FastLED.addLeds<NEOPIXEL, DATA_PIN>(leds, NUM_LEDS);
}

void loop() 
{
  if(Serial.available() > 0)
  {
    ReceiveMessage();
    readMessageState = 0;
    delay(100);
  }
  delay(1);
}

void ReceiveMessage()
{
  String arduino = "";
  String function = "";
  String p1 = "";
  String p2 = "";
  String p3 = "";
  while(Serial.available() > 0)
  {
    char val = Serial.read();
    if(val == '|')
    {
      readMessageState += 1;
    }
    else
    {
      if(readMessageState == 0) arduino += val;
      else if(readMessageState == 1) function += val;
      else if(readMessageState == 2) p1 += val;
      else if(readMessageState == 3) p2 += val;
      else if(readMessageState == 4) p3 += val;
    }
  }
  if(arduino != "")
  {
    // Serial.print(arduino);
    // Serial.print(function);
    // Serial.print(p1);
    // Serial.print(p2);
    // Serial.print(p3);
    if(function == "Fill")
    {
      Fill(CRGB(p1.toInt(), p2.toInt(), p3.toInt()));
      // Fill(CRGB(0,255,0));
    }
  }
}

void ReadLine()
{
  String message = "";
  while(Serial.available() > 0)
  {
    char val = Serial.read();
    message += val;
  }
  Serial.print(message);
}

void Fill(CRGB color)
{
  for(int i=0; i<NUM_LEDS; i++)
  {
    leds[i] = color;
  }
  FastLED.show();
  digitalWrite(LEDPIN, color.green / 255);
}

// void SplitString(char value, char delimiter)
// {
//   for(int i=0; i<value.Count; i++)
//   {

//   }
// }