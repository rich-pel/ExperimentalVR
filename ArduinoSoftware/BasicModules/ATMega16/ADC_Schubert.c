// ** ATMEGA16 C-Language File **

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/avr/signal.h>
#include <C:/avr/include/avr/eeprom.h>

//	Autor: Zhou, Schubert
//	Firma: FH-Gelsenkirchen
//	Projektgruppe: E
//	Datum. 25.05.09


char AD_READ (int CHANNEL)
{
	ADMUX=(1<<ADLAR)|(0<<REFS1)|(1<<REFS0);
	ADMUX|=CHANNEL;
	ADCSRA=(1<<ADEN);		// Enable
	ADCSRA|=(1<<ADSC);		// Start der Umwandlung
	while (ADSC==1);		// Abwarten der Umwandlung
	//PORTD=ADCH;
	return ADCH;
};

	// Funktion zum Starten des PWM-Betriebs
	// FAST PWM, Übergabeparameter ist der Vergleichswert
void FAST_PWM(int vergleichswert)
{
	// Setzen des Vergleichswertes
	OCR2 = vergleichswert;
	
	// Einrichten des Steuerregisters
	TCCR2 = (1 << COM21) | (1 << WGM21) | (1 << WGM20) | (1 << CS20);
};

void EEPROM_write(unsigned int uiAddress, unsigned char ucData)
{
	/* Abwarten bis der vorherige Schreibvorgang abgeschlossen ist */
	while(EECR & (1<<EEWE));
	
	/* Einrichten der Adresse und des Datenregisters */
	EEAR = uiAddress;
	EEDR = ucData;
	
	/* Setzen des Bit EEMWE auf logisch Eins */
	EECR |= (1<<EEMWE);
	
	/* Start des Schreibvorganges durch Setzen von EEWE */
	EECR |= (1<<EEWE);
};

unsigned char EEPROM_read(unsigned int uiAddress)
{
	/* Abwarten bis der vorherige Schreibvorgang abgeschlossen ist */
	while(EECR & (1<<EEWE));
	
	/* Einrichten des Adressregisters */
	EEAR = uiAddress;
	
	/* Start des Lesevorganges*/
	EECR |= (1<<EERE);
	
	/* Rückgabewert ist der Inhalt des Datenregisters */
	return EEDR;
};
	
	unsigned char __attribute__ ((section(".eeprom"))) sinus[16] = {128, 179, 217, 242, 255, 242, 217, 179, 128, 77, 38, 13, 0, 13, 38, 77};
	unsigned char __attribute__ ((section(".eeprom"))) rechteck[16] = {0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255};
	unsigned char __attribute__ ((section(".eeprom"))) saegezahn[16] = {0, 17, 34, 51, 68, 85, 102, 119, 136, 153, 170, 187, 204, 221, 238, 255};

void sin(delay_time)
{
	unsigned char lesen[16];
	int i;
	eeprom_read_block(lesen, sinus, 16);	

	for(i=0;i<16;i++) 
	{
		FAST_PWM(lesen[i]);
		_delay_ms(delay_time);
	}
}

void recht(delay_time)
{
	unsigned char lesen[16];
	int i;
	eeprom_read_block(lesen, rechteck, 16);	

	for(i=0;i<16;i++) 
	{
		FAST_PWM(lesen[i]);
		_delay_ms(delay_time);
	}
}

void saege(delay_time)
{
	unsigned char lesen[16];
	int i;
	eeprom_read_block(lesen, saegezahn, 16);	

	for(i=0;i<16;i++) 
	{
		FAST_PWM(lesen[i]);
		_delay_ms(delay_time);
	}
}

int main()
{
	unsigned char lesen[16];
	unsigned char halten;

		// Init Port A
	PORTA=0x00;
	DDRA=0x00;
		// Init Port B
	PORTB=0x00;
	DDRB=0x00;
		// Init Port C
	PORTC=0x00;
	DDRC=0x00;
		// Init Port D
	PORTD=0xFF;
	DDRD=0xFF;

//	EEPROM_write(00, 0x10);
//	EEPROM_write(01, 0x4F);
//	EEPROM_write(02, 0xF0);


	while(1)
	{
		if (PINB==0xFE)
		{
			halten=0xFE;
		}	
		if (PINB==0xFD)
		{
			halten=0xFD;
		}
		if (PINB==0xFB)
		{
			halten=0xFB;
		}


				switch (halten)
				{
				case 0xFE: sin(AD_READ(0));
				break;
				case 0xFD: recht(AD_READ(0));
				break;
				case 0xFB: saege(AD_READ(0));
				break;
				}

	}
	return 1;
}

	
