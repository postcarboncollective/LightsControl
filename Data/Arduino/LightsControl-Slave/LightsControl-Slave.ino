#include <FastLED.h>
#define NUM_LEDS 200
#define DATA_PIN 5
#define ENABLE_PIN 8
CRGB leds[NUM_LEDS];

byte id = 0;
bool ledState = false;
int numLeds = 30;

byte msg[8];

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
  if(Serial.available() > 8)
  {
    if(Serial.read() != 255) return;
    for(int i=0; i<8; i++)
    {
      msg[i] = Serial.read(); 
      // Serial.print(msg[i]);
      // Serial.print(" ");
    }
    // Serial.println();
    // Blink();

    if(id < 8)
    {
      if(bitRead(msg[0],id) == false) return;  
    }
    else if(id < 16)
    {
      if(bitRead(msg[1],id-8) == false) return;
    }

    switch(msg[2])
    {
      case 0: // Off
        Full(CRGB(0,0,0));
        break;
      case 1: // Full
        Full(CRGB(msg[3],msg[4],msg[5]));
        break;
      case 2: // Set
        Full(CRGB(0,0,0));
        Set(CRGB(msg[3],msg[4],msg[5]),msg[6],msg[7]);
        break;
      case 3: // Fill
        Fill(CRGB(msg[3],msg[4],msg[5]),msg[6]);
        break;
      case 4: // iFill
        iFill(CRGB(msg[3],msg[4],msg[5]),msg[6]);
        break;
      case 200: // Init
        numLeds = msg[3];
        break;
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
  int which = (led/200.0)*numLeds;
  for(int i=0; i<length; i++)
  {
    leds[(which+i)%numLeds] = color;
  }
  FastLED.show();
}

void Fill(CRGB color, int led)
{
  int which = (led/200.0)*numLeds;
  for(int i=0; i<numLeds; i++)
  {
    if(i < which) leds[i] = color;
    else leds[i] = CRGB(0,0,0);
  }
  FastLED.show();
}

void iFill(CRGB color, int led)
{
  int which = (led/200.0)*numLeds;
  for(int i=0; i<numLeds; i++)
  {
    int index = ((numLeds-1) - i);
    if(index > which) leds[index] = color;
    else leds[index] = CRGB(0,0,0);
  }
  FastLED.show();
}

void Blink()
{
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
}