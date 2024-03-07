#include <FastLED.h>
#define NUM_LEDS 200
#define DATA_PIN 3
#define ENABLE_PIN 8
#define ID 0;
CRGB leds[NUM_LEDS];

bool ledState = false;
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
  pinMode(13, OUTPUT);
  pinMode(ENABLE_PIN, OUTPUT);
  delay(10);
  digitalWrite(ENABLE_PIN, LOW);
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

    if(ledState == false)
    {
      ledState = true;
      digitalWrite(13, HIGH);
    }
    else
    {
      ledState = false;
      digitalWrite(13, LOW);
    }

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
          Full(CRGB(0,0,0));
        }
        else if(function == "Full")
        {
          Full(CRGB(p1.toInt(), p2.toInt(), p3.toInt()));
        }
        else if(function == "Set")
        {
          Full(CRGB(0,0,0));
          Set(CRGB(p1.toInt(), p2.toInt(), p3.toInt()), p4.toInt(), p5.toInt());
        }
        else if(function == "Fill")
        {
          Fill(CRGB(p1.toInt(), p2.toInt(), p3.toInt()), p4.toInt());
        }
        else if(function == "iFill")
        {
          iFill(CRGB(p1.toInt(), p2.toInt(), p3.toInt()), p4.toInt());
        }
        else if(function == "Init")
        {
          numLeds = p1.toInt();
        }
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

void Full(CRGB color)
{
  for(int i=0; i<numLeds; i++)
  {
    leds[i] = color;
  }
  FastLED.show();
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

void Fill(CRGB color, int led)
{
  for(int i=0; i<numLeds; i++)
  {
    if(i < led) leds[i] = color;
    else leds[i] = CRGB(0,0,0);
  }
  FastLED.show();
}

void iFill(CRGB color, int led)
{
  for(int i=0; i<numLeds; i++)
  {
    int index = ((numLeds-1) - i);
    if(index > led) leds[index] = color;
    else leds[index] = CRGB(0,0,0);
  }
  FastLED.show();
}