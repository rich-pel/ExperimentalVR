/*
 * BlinkInC.c
 *
 * Created: 17/07/2019 16:29:02
 * Author : Richard and Marten 
 */ 



#define F_CPU 16000000UL

#include <avr/io.h>
#include <util/delay.h>



int main(void)
{
	// start
	
	// set bit 5 of the data direction of port b to 1
	// indicates that we want to use port b as output
	DDRB |= 0B00100000;
	
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

