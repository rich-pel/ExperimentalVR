// Programm: Einzelzeichendarstellung mittels UART
// Authoren Andreas Multhaupt, Rui He
// Erstellungsdatum: 04.06.2009
// Geändert am: 08.06.2009
// Version 1.0

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/avr/signal.h>
#include <C:/avr/include/util/lcd.h>



#define BAUD 2400
#define MYUBRR F_CPU/16/BAUD-1

void USART_Init (unsigned int ubrr)
{
UBRRH = (unsigned char) (ubrr>>8);
UBRRL = (unsigned char) ubrr;
UCSRB = (1<<RXEN)|(1<<TXEN) | (1<<RXCIE);
UCSRC = (1<<URSEL)|(1<<USBS)|(3<<UCSZ0);
}


void USART_Transmit (unsigned char data)
{
/*Wait for empty transmit buffer*/
while (!(UCSRA & (1<<UDRE)));
/*Put data into buffer, sends the data*/
UDR = data;
}

unsigned char USART_Receive (void)
{
/*Wait for the data to be received*/
while (!(UCSRA & (1<<RXC)))
;
/*Get and return received data from buffer*/
return UDR;
}





ISR (USART_RXC_vect) {
   //irgendein Code
   	USART_Receive();
	lcd_data(UDR);
} 


int main ()
{
// Init of Port A
PORTA=0xFF;
DDRA=0xFF;

// Init of PortB
PORTB=0x00;
DDRB=0x00;

// Init of PortB
PORTC=0x00;
DDRC=0x00;

//Init of Port D
PORTD=0xFF;
DDRD=0xFF;

lcd_init ();
lcd_clear ();

USART_Init(MYUBRR);

//asm volatile ("sei"::);
//sei();





int i;
int wait_end = 0;

unsigned char buffer[20];
strcpy(buffer,"hallo welt");
//lcd_string(buffer);


	while(1)
	{
	i=0;
		
		if (PINB!=0xFF)
		{

			USART_Transmit('F');
			USART_Transmit('I');
			USART_Transmit('N');
			USART_Transmit('A');
			USART_Transmit('L');
			USART_Transmit('L');
			USART_Transmit('Y');
			while (!( datenempfang
			{		
			// Interrupt RX auslösen
			// eingehende Daten darstellen
			lcd_clear ();
			lcd_string ("FINALLY");
			lcd_command(0b00010000);
			_delay_ms(500);	
			}		
		}
		
	}
}
/*
for(i=0; i<9; i++)
{
lcd_command(0b00011000);
_delay_ms(500);
}

for(i=0; i<9; i++)
{
lcd_command(0b00011100);
_delay_ms(500);
}
*/


