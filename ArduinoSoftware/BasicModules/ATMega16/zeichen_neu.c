#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/avr/signal.h>
#include <C:/avr/include/util/lcd.h>

// Author: Gruppe 4
// Company: FH GE
// Date: 28.05.2009
// Comment: wird schon funzen

// Variables Declarations


// ISR-Sample


//Begin of Main-Function

#define BAUD 2400
#define MYUBRR F_CPU/16/BAUD-1

void USART_Init (unsigned int ubrr)
{
/* Set baud rate*/
UBRRH = (unsigned char) (ubrr>>8);
UBRRL = (unsigned char) ubrr;
/*enable receiver and transmitter*/
UCSRB = (1<<RXEN)|(1<<TXEN) | (1<<RXCIE);
/*Set frame format: 8data, 2 ststop bit*/
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

asm volatile ("sei"::);
//sei();





int i;
int wait_end = 0;

unsigned char buffer[20];
strcpy(buffer,"hallo welt");
//lcd_string(buffer);



	while(1)
	{
		
		if (PINB!=0xFF)
		{
			
			
				//USART_Init (MYUBRR);
				for(i=0;i<strlen(buffer);i++)
				{
					
					USART_Transmit(buffer[i]);
				}
				
				USART_Transmit('\n');
				USART_Transmit('\r');

				_delay_ms(500);
				
				
				
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





