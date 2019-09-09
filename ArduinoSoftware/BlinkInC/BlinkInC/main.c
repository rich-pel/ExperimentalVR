/*
 * BlinkInC.c
 *
 * Created: 17/07/2019 16:29:02
 * Edit:	27/08/2019 15:00:00
 * Author : Richard and Marten 
 *
 */ 


// Clock Speed
#define F_CPU 16000000UL // 16 Hz

#include <avr/io.h>
#include <util/delay.h>

// tell compiler about existing functions 
void readEKG();
void readEMG();

int analogRed();

// UART initialization (set bits in specific register (Handbook))
void uart_init()
{
	UBRR0H = (51 >> 8);
	UBRR0L = 51;
	
	UCSR0B = (1<<TXEN0) | (1<<RXEN0);
	UCSR0C = (1<<UCSZ00) | (1<<UCSZ01); //8bit
}


// it probably changes anyway
void uart_outputchar(char ch)
{
	// this makes the program wait until "bit" is received   
	while(!(UCSR0A &(1<<UDRE0)))
	;
	// not sure why ch?
	UDR0 = ch;
}


// interrupt function
//void ISR(PCINT2_vect)
//{
	//// if button was pressed, turn on or off ... not useful for us
	//if((PIND & (1 << PIND4)) == 0)
	//{
		//PORTD ^= (1<<LED1);
		//_delay_ms(250);
	//}
//}



int main(void)
{
	// start / setup
	
	
	// set bit 5 of the data direction of port b to 1
	// indicates that we want to use port b as output
	DDRB |= 0B00100000;
	// Alternative:
	// einmal festlegen, welche ports sind eingang/ausgang
	// 0 für Eingang, 1 für ausgang bsp: 0x00 nur eingänge; 0xFF nur ausgaenge
	// fünfter und sechster als ausgang: DDRB = 0b01100000;
	
	int myInt = 10;
	
    // Update
    while (1) 
    {
		// set Port B5 to high
		PORTB |= 0B00100000;
		_delay_ms(100);
		PORTB &= 0B11011111;
		_delay_ms(100);
    }
}

