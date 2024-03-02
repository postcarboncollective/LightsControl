#include <FastLED.h>
#define NUM_LEDS 400
#define DATA_PIN 3
#define LEDPIN 13
#define ID 0;
CRGB leds[NUM_LEDS];

int readMessageState = 0;
String arduino = "";
String function = "";
String p1 = "";
String p2 = "";
String p3 = "";
String p4 = "";

void setup() 
{
  Serial.begin(9600);
  FastLED.addLeds<NEOPIXEL, DATA_PIN>(leds, NUM_LEDS);
}

void loop() 
{
  ReadMessage();
}

void ReadMessage()
{
  if(Serial.available() > 0)
  {
    char val = Serial.read();
    if(val == '|')
    {
      readMessageState += 1;
    }
    else if(val == '\n')
    {
      if(arduino != "")
      {
        readMessageState = 0;
        if(function == "Off")
        {
          Fill(CRGB(0,0,0));
        }
        else if(function == "Fill")
        {
          Fill(CRGB(p1.toInt(),p2.toInt(),p3.toInt()));
        }
        // Serial.print(arduino);
        // Serial.print("|");
        // Serial.print(function);
        // Serial.print("|");
        // Serial.print(p1);
        // Serial.print("|");
        // Serial.print(p2);
        // Serial.print("|");
        // Serial.print(p3);
        // Serial.print('\n');
        arduino = "";
        function = "";
        p1 = "";
        p2 = "";
        p3 = "";
        p4 = "";
      }
    }
    else
    {
      if(readMessageState == 0) arduino += val;
      else if(readMessageState == 1) function += val;
      else if(readMessageState == 2) p1 += val;
      else if(readMessageState == 3) p2 += val;
      else if(readMessageState == 4) p3 += val;
      else if(readMessageState == 5) p4 += val;
    }
  }
}

void Fill(CRGB color)
{
  for(int i=0; i<NUM_LEDS; i++)
  {
    leds[i] = color;
  }
  FastLED.show();
}