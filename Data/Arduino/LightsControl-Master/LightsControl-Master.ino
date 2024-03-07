int enablePin = 8;

void setup() 
{
  Serial.begin(9600);
  pinMode(enablePin, OUTPUT);
  delay(10);
  digitalWrite(enablePin, HIGH);
}

void loop() 
{
  if(Serial.available() > 0)
  {
    char val = Serial.read();
    Serial.print(val);
  }
}