// Programm: Funktionsgenerator
// Autor: Rui He, Andreas Multhaupt
// Erstellt: 25.05.2009
// Geändert: -
// Verwendetes Board: ATMega16

// Einbinden der Header

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/avr/signal.h>



// ##############################

// Main-Funktion
int main ()
{
// Globales Freischalten von Interrupts
//asm volatile ("cli"::);
// Initialisieren und Startwertsetzen der Ports
PORTA=0x00;
DDRA=0x00;
PORTB=0x00;
DDRB=0x00;
PORTC=0x00;
DDRC=0x00;
PORTD=0xFF;
DDRD=0xFF;
 
int CHANNEL;
CHANNEL = 0;
ADMUX = (1 << ADLAR)| (0 << REFS1) | (1 << REFS0);
ADMUX |= CHANNEL;
ADCSRA= (1 << ADEN);

// Ausgabe in PORTD
while (1)
	{
	// Start der Umwandlung
	ADCSRA|= (1 << ADSC);
	//Abwarten der Umwandlung
	while(ADSC==1)
	;
	// Schreiben des Rückgabewertes
	PORTD = ADCH;
	}
return 1;
}

