/****************************************
*		Versuch 4 - UART Senden			*
*	Stephan Hänisch                     *
*	Matthias Gnida                      *
*		Ying Liu                        *
*										*
*										*
*****************************************/

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/util/lcd.h>


#define BAUD 2400 
#define MYUBRR F_CPU/16/BAUD-1

void USART_Init( unsigned int ubrr) 
{ 
	/* Set baud rate */ 
	UBRRH = (unsigned char)(ubrr>>8); 
	UBRRL = (unsigned char)ubrr; 
	/* Enable receiver and transmitter */ 
	UCSRB = (1<<RXEN)|(1<<TXEN)|(1<<RXCIE); 
	/* Set frame format: 8data, 2ststop bit */
	UCSRC = (1<<URSEL)|(1<<USBS)|(3<<UCSZ0); 


}

void USART_Transmit( unsigned char data ) 
{ 
	/* Wait for empty transmit buffer */ 
	while ( !( UCSRA & (1<<UDRE)) ) ; 
	/* Put data into buffer, sends the data */ 
	UDR = data; 
}

unsigned char USART_Receive(void)
{
while (!(UCSRA & (1<<RXC)) );
return UDR;
}


int uart_putc(unsigned char c)
{
    while (!(UCSRA & (1<<UDRE)))  /* warten bis Senden moeglich */
    {
    }                             
 
    UDR = c;                      /* sende Zeichen */
    return 0;
}
 
 
void uart_puts (char *s)
{
    while (*s)
    {   /* so lange *s != '\0' also ungleich dem "String-Endezeichen" */
        uart_putc(*s);
        s++;
    }
}

ISR(USART_RXC_vect) 
{
   USART_Receive();
   lcd_data(UDR);
} 


void main( void ) 
{
	USART_Init(MYUBRR);

	//Init Port A
	PORTA=0xFF;
	DDRA=0xFF;

	//Init Port B
	PORTB=0x00;
	DDRB=0x00;

	//Init Port D
	PORTD=0xFF;
	DDRD=0xFF;

	lcd_init();
	lcd_clear();
	
	asm volatile ("sei"::);

	char testmuster = 0xAA;
	char string[]={"Hallo Welt!\n\r"};

	while(1)
	{
	if (PINB!=0xFF) 
		{	
		if (PINB==0xFF)
			{
			uart_puts(string);
			}
		
		}	
	}
} 




