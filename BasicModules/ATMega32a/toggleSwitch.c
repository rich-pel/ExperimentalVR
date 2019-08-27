/*
 * toggleSwitch.c
 *
 * Created: 16/08/2019 17:34:35
 *  Author: Richard
 
 Control LED by toggle switch
 
 */ 
// Clock Speed
#define F_CPU 16000000UL // 16 MHz

#include <avr/io.h>
#include <util/delay.h>


// summery:
void AnalogDigitalConverter()
{
	#define LED1 6
	#define POT1 1
	
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



int main(void)
{
	// start / setup

	
	// set bit 5 of the data direction of port b to 1
	// indicates that we want to use port b as output
	DDRB |= 0B00100000;
	
	sei(); // global interrupt flag
	


		
    // Update
    while (1) 
    {
		// set Port B5 to high
		PORTB |= 0B00100000;
		_delay_ms(1000);
		PORTB &= 0B11011111;
		_delay_ms(300);
		
		
		
		
		
		
    }
	
	return 0; // wird nie erreicht
}



