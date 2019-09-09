// ATMEGA16 C-Language File
#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/lcd.h>
#define Baud 2400
#define MYUBRR F_CPU/16/Baud-1

//Author: Bastian Urban, Dennis Schlehuber
//Company: FH-Gelsenkirchen
//Date: 04.06.2009
//Comment: UART_TEST





void USART_Init (unsigned int ubrr)
{
//set baud rate
UBRRH = (unsigned char) (ubrr>>8);
UBRRL = (unsigned char)ubrr;
//ENABLE RECIEVER AND TRANSMITTER
UCSRB = (1<<RXEN)|(1<<TXEN)|(1<<RXCIE);
//SET FRAME FORMAT 8data, 2stopbit
UCSRC = (1<<URSEL) | (1<<USBS) | (3<<UCSZ0);
}

void USART_Transmit(unsigned char data)
{
//Wait for empty tranismit buffer
while (!(UCSRA &(1<<UDRE)));
//Put data into buffer, sends the data
UDR=data;
}


unsigned char USART_Reciever (void)
{
//Warten auf daten zu sein empfangen
while (!(UCSRA & (1<<RXC)));
//bekommen und zurückgebe empfangen daten von Vorrat

return UDR;

}

void USART_sendstring (char *s)
{
	while (*s)
	{
	USART_Transmit(*s);
	s++;
	}
}

ISR (USART_RXC_vect)
{
USART_Reciever();
lcd_data (UDR);
}

//Begin of Main function

int main ()
{

	//INIT of port A
	PORTA=0xFF;
	DDRA=0xFF;
	//INIT of port B
	PORTB=0x00;
	DDRB=0x00;
	//INIT of port C
	PORTC=0x00;
	DDRC=0x00;
	//INIT of port D
	PORTD=0xFF;
	DDRD=0xFF;
	USART_Init (MYUBRR);
	lcd_init ();
	lcd_clear ();
asm volatile ("sei"::);
char string[]=("Hello World \n\r");
while(1)
{		
	while(PINB !=(0xFF))
	{
	USART_sendstring (string);
	_delay_ms(500);
	}
}

}




