// ** ATMEGA16 C-Language File **

// When compiling this file use device ATMEGA16

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>

//	Author	:	Gruppe Jaisfeld
//	Company	:	FH-Gelsenkirchen
//	Date	:	05.06.2009
//	Comment	:

// Variables Declarations
unsigned char led_status=0xFE;

// ISR-Sample
ISR (TIMER0_OVF_vect)
{
// Place your code here
}

// Begin of Main-Function

int main ()
{
// Init of Port A
PORTA=0x00;
DDRA=0x00;
//Init of Port B
PORTB=0xFF;
DDRB=0xFF;
// Init of Port C
PORTC=0x00;
DDRC=0x00;
// Init of Port D
PORTD=0x00;
DDRD=0x00;

// Global Disable Interrupts
asm volatile ("cli"::);
// While Loop for LED shifting
while (1)
	{
	// Move the LEDs
	led_status=led_status << 1;
	led_status=led_status | 1;
	if (led_status==0xFF)
		{
		led_status=0xFE;
		}
	// Turn on the LED
	PORTB=led_status;
	// Delay
	_delay_ms(2000);
	}
return 1;
	}
