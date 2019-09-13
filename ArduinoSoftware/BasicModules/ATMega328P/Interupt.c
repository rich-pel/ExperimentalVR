/*
 * Interupt.c
 *
 * Created: 06/08/2019 18:10:42
 *  Author: Richard
 */ 

Problem:

#define F_CPU 8000000UL
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED1 5
#define LED2 6
#define BUT1 2

int main(void)
{
	DDRD = 0b01100000;
	PORTD = 0b00000100;
	
	while(1)
	{
		PORTD |= (1<<LED2);
		_delay_ms(1500);
		PORTD &= ~(1<<LED2);
		_delay_ms(1500);
		
		
		if (!(PIND & (1<<BUT1)))
		{
			PORTD |= (1<<LED1);
		}
		else
		{
			PORTD &= ~(1<<LED1);
		}
	}
}





1. Lösung ohne entprellung
#define F_CPU 8000000UL
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED1 5
#define LED2 6
#define BUT1 2

int main(void)
{
	DDRD = 0b01100000;
	PORTD = 0b00000100;
	
	EIMSK = (1 << INT0);
	
	sei();
	
	while(1)
	{
		PORTD |= (1<<LED2);
		_delay_ms(1500);
		PORTD &= ~(1<<LED2);
		_delay_ms(1500);
		
		
		//if (!(PIND & (1<<BUT1)))
		//{
		//PORTD |= (1<<LED1);
		//}
		//else
		//{
		//PORTD &= ~(1<<LED1);
		//}
	}
	
}

ISR(INT0_vect)
{
	PORTD ^= (1<<LED1);
}

2. Entprellte ISR

ISR(INT0_vect)
{
	if((PIND & (1 << PIND2)) == 0)
	{
		PORTD ^= (1<<LED1);
		_delay_ms(250);
	}
}


3. Pin Change interrupt

#define F_CPU 8000000UL
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED1 5
#define LED2 6
#define BUT1 2


int main(void)
{
	DDRD =  0b01100000;
	PORTD = 0b00011100;
	
	//EIMSK = (1 << INT1);
	PCICR = (1 << PCIE2);
	PCMSK2 = (1 << PCINT19);
	
	sei();
	
	
	while(1)
	{
		PORTD |= (1 << LED2);
		_delay_ms(1000);
		PORTD &= ~(1 << LED2);
		_delay_ms(1000);
	}
}

ISR(PCINT2_vect)
{
	if((PIND & (1 << PIND4)) == 0)
	{
		PORTD ^= (1<<LED1);
		_delay_ms(250);
	}
}


4. Timer als Interrupt

#define F_CPU 8000000UL
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED1 5
#define LED2 6
#define BUT1 2


int main(void)
{
	TCCR0B = (1<<CS00) | (1 << CS02); //8Mhz/1024/255
	TIMSK0 = (1<<TOIE0);
	
	DDRD = 0b01100000;
	PORTD = 0b00000100;
	
	sei();
	
	while(1)
	{
		
	}
	
}

ISR(TIMER0_OVF_vect)
{
	PORTD ^= (1<<LED1);
}
