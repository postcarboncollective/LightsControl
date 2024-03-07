#define ENABLE_PIN 8

void setup() 
{
  Serial.begin(9600);
  pinMode(ENABLE_PIN, OUTPUT);
  delay(10);
  digitalWrite(ENABLE_PIN, HIGH);
}

void loop() 
{
  if(Serial.available() > 0)
  {
    Serial.print(Serial.read());
  }
}