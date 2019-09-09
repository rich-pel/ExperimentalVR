// ** ATMEGA16 C-Language File **

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/avr/eeprom.h>
#include <C:/avr/include/avr/signal.h>

//	Author	:	Gruppe Jaisfeld
//	Company	:	FH-Gelsenkirchen
//	Date	:	10.06.2009
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
	return 1;
}

// Funktion zum Starten des PWM-Betriebs
// Fast PWM
// Übergabeparameter ist der Vergleichswert
void FAST_PWM (int vergleichswert)
{
// Setzen des Vergleichwertes
OCR2 = vergleichswert;
// Einrichten des Steuerregisters
TCCR2 = (1 << COM21) | (1 << WGM21) | (1 << WGM20) | (1 << CS20);
};

void EEPROM_write (unsigned int uiAddress, unsigned char ucData)
{
/* Abwarten bis der vorherige Schreibvorgang abgeschlossen ist */
while (EECR & (1<<EEWE));
/* Einrichten der Adresse und des Datenregisters */
EEAR = uiAddress;
EEDR = ucData;
/* Setzen des Bit EEMWE auf logisch 1*/
EECR |= (1<<EEMWE);
/* Start des Schreibvorganges durch Setzen von EEWE */
EECR |= (1<<EEWE);
}

unsigned char EEPROM_read (unsigned int uiAddress)
{
/* Abwarten bis der vorherige Schreibvorgang abgeschlossen ist */
while (EECR & (1<<EEWE));
/* Einrichten des Adressregisters */
EEAR = uiAddress;
/* Start des Lesevorganges */
EECR |= (1<<EERE);
/* Rückgabewert ist der Inhalt des Datenregisters */
return EEDR;
}

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

// Global Disable Interrupts
asm volatile ("cli"::);

EEPROM_write(00,0x10);
EEPROM_write(01,0x4F);
EEPROM_write(02,0xF0);

while (1)
	{
	if (PINB == 0xFE)
		{
		FAST_PWM(EEPROM_read(00));
		// Delay
		_delay_ms(5000);
		}
	if (PINB == 0xFD)
		{
		FAST_PWM(EEPROM_read(01));
		// Delay
		_delay_ms(5000);
		}
	if (PINB == 0xFB)
		{
		FAST_PWM(EEPROM_read(02));
		// Delay
		_delay_ms(5000);
		}
	}
return 1;
}
