// *ATMEGA16 C-LANGUAGE FILE**

// When you program this file use device ATMEGA16

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/avr/signal.h>
#include <C:/avr/include/avr/eeprom.h>



// Author: Gruppe 4
// Company: FH GE
// Date: 25.05.2009
// Comment: wird schon funzen

// Variables Declarations


// PWM
void FAST_PWM(int vergleichswert)
{
	//Setzen des Vergleichwertes
	OCR2 = vergleichswert;

	//Einrichten Register
	TCCR2 = (1<<COM21) | (1 << WGM21) | (1 << WGM20) | (1 << CS20);

}

void EEPROM_write(unsigned int uiAddress, unsigned char ucData)
{
/* Abwarten bis der vorherige Schreibvorgang abgeschlossen ist*/
while (EECR & (1<<EEWE))
;
/*Einrichten der Adresse und des Datenregisters*/
EEAR = uiAddress;
EEDR = ucData;
/*Setzen des Bit EEMWE auf logisch Eins*/
EECR |= (1<<EEMWE);
/*Start des Schreibvorganges durch Setzen von EEWE*/
EECR |= (1<<EEWE);
}

unsigned char EEPROM_read(unsigned int uiAdress)
{
while(EECR & (1<<EERE))
;

EEAR = uiAdress;

EECR |= (1<<EERE);

return EEDR;
}


//Begin of Main-Function


unsigned char __attribute__((section(".eeprom"))) sinus[16] = {0x80,0xB0,0xD9,0xF2,0xFF,0xF2,0xD9,0xB0,0x80,0x4F,0x26,0x0D,0x00,0x0D,0x26,0x4F};
unsigned char __attribute__((section(".eeprom"))) rechteck[16] = {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF};
unsigned char __attribute__((section(".eeprom"))) saegezahn[16] = {0x00,0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88,0x99,0xAA,0xBB,0xCC,0xDD,0xEE,0xFF};


int main ()
{
// Init of Port A
PORTA=0x00;
DDRA=0x00;
// Init of PortB

PORTB=0x00;
DDRB=0x00;
// Init of Port C
PORTC=0x00;
DDRC=0x00;
//Init of Port D
PORTD=0xFF;
DDRD=0xFF;

// Global Disable Interrupts
asm volatile ("cli"::);

int i;

int save = 0;

unsigned char array[16];

int CHANNEL;

char time;

CHANNEL = 0;

//Start AD
ADMUX = (1<<ADLAR) | (0<<REFS1) | (1<<REFS0);
ADMUX |= CHANNEL;
ADCSRA = (1<<ADEN);

while(1){


	ADCSRA |=(1<<ADSC);

	while(ADSC==1){
		;
	}

	time = ADCH;

	if(PINB == 0xFF){

		if(save == 1){
		eeprom_read_block(array,sinus,16);
		save = 1;
			for(i=0;i<16;i++){

			FAST_PWM(array[i]);
			_delay_ms(time);
			}

		} 

		if(save== 2){
		eeprom_read_block(array,rechteck,16);
		save = 2;
			for(i=0;i<16;i++){
			FAST_PWM(array[i]);
			_delay_ms(time);
			}

		} 

		if(save == 3){
		eeprom_read_block(array,saegezahn,16);
		save = 3;
		for(i=0;i<16;i++){
			FAST_PWM(array[i]);
			_delay_ms(time);
		}

		}
		}
	else{
		if(PINB == 0b11111110 ){
		eeprom_read_block(array,sinus,16);
		save = 1;
		for(i=0;i<16;i++){

			FAST_PWM(array[i]);
			_delay_ms(time);
		}

		} 
	

		if(PINB == 0xFD){
		eeprom_read_block(array,rechteck,16);
		save = 2;
		for(i=0;i<16;i++){
			FAST_PWM(array[i]);
			_delay_ms(time);
		}

		} 

		if(PINB == 0xFB){
		eeprom_read_block(array,saegezahn,16);
		save = 3;
		for(i=0;i<16;i++){
			FAST_PWM(array[i]);
			_delay_ms(time);
		}

		} 

	}
}





}
