/*
 * USART1.c
 *
 * Created: 06/08/2019 18:34:45
 *  Author: Richard
 */ 

#define F_CPU 8000000UL
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define BAUD 9600UL
//#define UBRR0_VALUE ((F_CPU/(16*BAUD)) - 1)

int main(void)
{
	UBRR0H = (51 >> 8);
	UBRR0L = 51;
	
	UCSR0B = (1<<TXEN0) | (1<<RXEN0);
	UCSR0C = (1<<UCSZ00) | (1<<UCSZ01); //8bit
	
	DDRD |= (1<<5); //LED
	
	while(1)
	{
		//senden
		//UDR0 = '3';
		//_delay_ms(1000);
		
		//empfangen
		while (!(UCSR0A & (1<<RXC0)))
		;
		
		// mit empfangen eines ausrufezeichens geht die led an, bis eine anderer buschstabe kommt
		if (UDR0 == 33) //ASCII '!'
		{
			PORTD |= (1<<5);
		}
		else
		{
			PORTD &= ~(1<<5);
		}
	}
}