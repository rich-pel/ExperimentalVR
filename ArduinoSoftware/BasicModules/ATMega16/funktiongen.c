// Programm: FAST PWM
// Erstellt: 25.05.2009
// Geändert: -
// Verwendetes Board: ATMega16

// Einbinden der Header
// ATMEGA16 C-Language File
#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/avr/eeprom.h>
#include <C:/avr/include/util/delay.h>

unsigned char __attribute__((section(".eeprom"))) sinus[16] = {0x80,0xb0,0xd9,0xf2,0xff,0xf2,0xd9,0xb9,0x80,0x4f,0x26,0x0d,0x00,0x0d,0x26,0x4f};
unsigned char __attribute__((section(".eeprom"))) rect[16] = {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF};
unsigned char __attribute__((section(".eeprom"))) saeg[16] = {0x00,0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88,0x99,0xaa,0xbb,0xcc,0xdd,0xee,0xFF};

//Funktion zum Starten des PWM Betriebs
//Fast PWM
//Übergabeparameter ist der Vergleichswert

void FAST_PWM (int vergleichswert){

//Vergleichswert setzen
OCR2= vergleichswert;

}

void read_sin(int del)
{
		int lauf;
		unsigned char array[16];
		eeprom_read_block(array,sinus,16);
		for (lauf=0;lauf!=15;lauf++)
		{
			FAST_PWM(array[lauf]);
			_delay_ms(del);
		}
} 

void read_rect(int del)
{
	int lauf;
	unsigned char array[16];
	eeprom_read_block(array,rect,16);
	for (lauf=0;lauf!=15;lauf++)
	{
		FAST_PWM (array[lauf]);
		_delay_ms(del);
	}
} 

void read_saeg(int del)
{
	int lauf;
	unsigned char array[16];
	eeprom_read_block(array,saeg,16);
	for (lauf=0;lauf!=15;lauf++)
	{
		FAST_PWM (array[lauf]);
		_delay_ms(del);
	}
} 


void EEPROM_write(unsigned int uiAdress, unsigned char ucData)
{
//Schreibvorgang abwarten
while(EECR & (1<<EEWE));
EEAR = uiAdress;
EEDR = ucData;
EECR |= (1<<EEMWE);
EECR |= (1<<EEWE);
}  

unsigned char EEPROM_read (unsigned int uiAdress)
{
//Schreibvorgang abwarten
while(EECR &(1<<EEWE));
//Einrichten Adressreg.
EEAR = uiAdress;
//Start Lesen
EECR |= (1<<EERE);
//Rückgabewert
return EEDR;
}


char AD_READ (){

//Start der Umwandlung
ADCSRA|= (1 << ADSC);
//Abwarten der Umwandlung
while(ADSC==1);
//schreiben des Rückgabewertes
return ADCH;
}


//Begin of Main function

int main ()
{
//INIT of port A
PORTA=0x00;
DDRA=0x00;
//INIT of port B
PORTB=0x00;
DDRB=0x00;
//INIT of port C
PORTC=0x00;
DDRC=0x00;
//INIT of port D
PORTD=0xFF;
DDRD=0xFF;
TCCR2=0b01101001;
asm volatile ("cli"::);

// Initialisierung des ADC
//int CHANNEL = 0;
ADMUX = (1 << ADLAR) | (0 << REFS1) | (1 << REFS0);
//ADMUX|=CHANNEL;
ADCSRA= (1 << ADEN);
//asm volatile ("sei"::);

int ADERG;
while(1)
{
	while (PINB==0x01)
		{
		ADERG = AD_READ();
		read_sin(ADERG);
		}
	while (PINB==0xFD)
		{
		ADERG = AD_READ();
		read_rect(ADERG);
		}
	while (PINB==0xFB)
		{
		ADERG = AD_READ();
		read_saeg(ADERG);
		}

}

}










