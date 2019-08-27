// ** ATMEGA16 C-Language File **

// When compiling this file use device ATMEGA16

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>


//	Author	:	Gruppe Jaisfeld
//	Company	:	FH-Gelsenkirchen
//	Date	:	05.06.2009
//	Comment	:


// Funktion zum Einlesen eines Analogwertes
// Rückgabewert ist der zu 8-Bit gewandelte Analogwert
// Übergabeparameter ist der AD-Kanal
char AD_READ (int CHANNEL)
// Initialisierung des ADC
{
	ADMUX = (1 << ADLAR) | (0 << REFS1) | (1 << REFS0);
	ADMUX|=CHANNEL;
	ADCSRA = (1 << ADEN);
	// Start der Umwandlung
	ADCSRA|= (1 << ADSC);
	// Abwarten der Umwandlung
	while (ADSC == 1);
	// Schreiben des Rückgabewertes 
	PORTD=ADCH;
	return ADCH;
};

// Begin of Main-Function

int main ()
{
// Init of Port A
PORTA=0x00;
DDRA=0x00;
//Init of Port B
PORTB=0x00;
DDRB=0x00;
// Init of Port C
PORTC=0x00;
DDRC=0x00;
// Init of Port D
PORTD=0xFF;
DDRD=0xFF;

while (1)
	{
		AD_READ(0);
	}
return 1;
}
