/*
 * USART2.c
 *
 * Created: 06/08/2019 18:48:08
 *  Author: Richard
 */ 
#define F_CPU 8000000UL
#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

void uart_init()
{
	UBRR0H = (51 >> 8);
	UBRR0L = 51;
	
	UCSR0B = (1<<TXEN0) | (1<<RXEN0);
	UCSR0C = (1<<UCSZ00) | (1<<UCSZ01); //8bit

}


void uart_outputchar(char ch)
{
	while(!(UCSR0A &(1<<UDRE0)))
	;
	UDR0 = ch;
}


void uart_output(char *calledstring)
{
	for (int i=0; i<255; i++)
	{
		if (calledstring[i] != 0)
		{
			uart_outputchar(calledstring[i]);
		}
		else
		{
			break;
		}
	}
}


char uart_getnew()
{
	while(!(UCSR0A & (1<<RXC0)))
	;
	return UDR0;
}


void uart_readnew(char *calledstring)
{
	DDRD |= (1<<5);
	char ch;
	int cntr = 0;
	
	while (1)
	{
		
		ch = uart_getnew();
		
		if (ch == 13) // New Line
		{
			calledstring[cntr] = 0;
			return;
		}
		else
		{
			calledstring[cntr] = ch;
			cntr++;
		}
	}
}


int main(void)
{
	DDRD |= (1<<5);
	char INPUT[255];
	uart_init();
	
	while(1)
	{
		uart_readnew(INPUT);
		uart_output(INPUT);
		uart_output("\r\n");
		
		
		//if (strcmp (INPUT, "ASDF") == 0)
		//{
		//PORTD |= (1<<5);
		//}
		//else
		//{
		//PORTD &= ~(1<<5);
		//}
		
	}
}