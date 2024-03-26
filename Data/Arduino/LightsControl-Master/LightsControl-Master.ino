#define ENABLE_PIN 8

bool ledState = false;

void setup() 
{
  Serial.begin(9600);
  pinMode(13, OUTPUT);
  pinMode(ENABLE_PIN, OUTPUT);
  delay(10);
  digitalWrite(ENABLE_PIN, HIGH);
}

void loop() 
{
  if(Serial.available() > 8)
  {
    for(int i=0; i<9; i++)
    {
      // Serial.write(Serial.read());
      Serial.print(Serial.read());
    }
    Serial.println();
    Blink();
  }
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