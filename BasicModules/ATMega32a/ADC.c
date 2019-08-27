/*
 * ADC.c
 *
 * Created: 06/08/2019 18:23:21
 *  Author: Richard
 */ 

#define F_CPU 8000000UL
#include <avr/io.h>
#include <avr/interrupt.h>


#define LED1 6
#define POT1 1


int main(void)
{
	//PWM Setup
	TCCR0A = (1<<WGM00) | (1 << COM0A1);
	TCCR0B = (1<<CS01);
	
	//ADC Setup
	ADMUX = (1<<REFS0) | (1<<MUX0);
	ADCSRA = (1<<ADEN) | (1<<ADPS0) | (1<<ADPS1) | (1<<ADPS2);
	
	sei();
	
	DDRD = (1<<LED1);
	PORTD = 0x00;
	
	DDRC = 0x00;
	PORTC = 0x00;
	
	
	while(1)
	{
		ADCSRA |= (1<<ADSC);
		while (ADCSRA & (1<<ADSC)){
		}
		OCR0A = ADC/4;
	}
}