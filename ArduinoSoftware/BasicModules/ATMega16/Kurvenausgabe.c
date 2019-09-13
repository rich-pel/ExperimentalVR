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

unsigned char __attribute__((section(".eeprom"))) sinus[16] = {0x7D,0xAF,0xD4,0xF0,0xFA,0xF0,0xD4,0xAF,0x7D,0x4B,0x25,0x0A,0x00,0x0A,0x25,0x4B};
unsigned char __attribute__((section(".eeprom"))) rechteck[16] = {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xFA,0xFA,0xFA,0xFA,0xFA,0xFA,0xFA,0xFA};
unsigned char __attribute__((section(".eeprom"))) saegezahn[16] = {0x00,0x10,0x21,0x32,0x42,0x53,0x64,0x74,0x85,0x96,0xA6,0xB7,0xC8,0xD8,0xE9,0xFA};

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

	int i;
	int speichern=0;
	unsigned char array[16];

	while (1)
		{
		if (PINB == 0xFF)
			{
			if (speichern == 1)
				{
				eeprom_read_block(array,sinus,16);
				speichern=1;
				for (i=1;i<16;i++)
					{
					FAST_PWM(array[i]);
					// Delay
					_delay_ms(50);
					}
				}
			}
			{
			if (speichern == 2)
				{
				eeprom_read_block(array,rechteck,16);
				speichern=2;
					{
					for (i=1;i<16;i++)
						{
						FAST_PWM(array[i]);	
						// Delay
						_delay_ms(50);
						}
					}
				}
			}
			if (speichern == 3)
				{
				eeprom_read_block(array,saegezahn,16);
				speichern=3;
				for (i=1;i<16;i++)
					{
					FAST_PWM(array[i]);
					// Delay
					_delay_ms(50);
					}
				}
		else
			{
			if (PINB == 0xFE)
				{
				eeprom_read_block(array,sinus,16);
				speichern=1;
				for (i=1;i<16;i++)
					{
					FAST_PWM(array[i]);
					// Delay
					_delay_ms(50);
					}
				}
			}
			{
			if (PINB == 0xFD)
				{
				eeprom_read_block(array,rechteck,16);
				speichern=2;
				for (i=1;i<16;i++)
					{
					FAST_PWM(array[i]);
					// Delay
					_delay_ms(50);
					}
				}
			}
			{
			if (PINB == 0xFB)
				{
				eeprom_read_block(array,saegezahn,16);
				speichern=3;
				for (i=1;i<16;i++)
					{
					FAST_PWM(array[i]);
					// Delay
					_delay_ms(50);
					}
				}
			}
		}
	return 1;
	}

