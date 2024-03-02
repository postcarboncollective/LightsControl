#include <FastLED.h>
#define NUM_LEDS 100
#define DATA_PIN 3
#define LEDPIN 13
#define ID 0;
CRGB leds[NUM_LEDS];

int numLeds = 30;

int readMessageState = 0;
String arduino = "";
String function = "";
String p1 = "";
String p2 = "";
String p3 = "";
String p4 = "";
String p5 = "";

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
        else if(function == "Set")
        {
          Fill(CRGB(0,0,0));
          Set(CRGB(p1.toInt(),p2.toInt(),p3.toInt()),p4.toInt(),p5.toInt());
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
        p5 = "";
      }
    }
    else
    { 
      switch(readMessageState)
      {
        case 0:
          arduino += val;
          break;
        case 1:
          function += val;
          break;
        case 2:
          p1 += val;
          break;
        case 3:
          p2 += val;
          break;
        case 4:
          p3 += val;
          break;
        case 5:
          p4 += val;
          break;
        case 6:
          p5 += val;
          break;
      }
    }
  }
}

void Set(CRGB color, int led, int length)
{
  if(length == 0) length = 1;
  for(int i=0; i<length; i++)
  {
    leds[(led+i)%numLeds] = color;
  }
  FastLED.show();
}

void Fill(CRGB color)
{
  for(int i=0; i<NUM_LEDS; i++)
  {
    leds[i] = color;
  }
  FastLED.show();
}