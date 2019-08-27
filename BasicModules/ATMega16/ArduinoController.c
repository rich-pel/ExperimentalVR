// Clock Speed
#define F_CPU 16000000UL // 16 MHz

#include <avr/io.h>
#include <util/delay.h>



int main(void)
{
	// start
	
	// set bit 5 of the data direction of port b to 1
	// indicates that we want to use port b as output
	DDRB |= 0B00100000;
	// Alternative:
	// einmal festlegen, welche ports sind eingang,
	// hier: welche sind ausgang:
	// 0 für Eingang, 1 für ausgang bsp: 0x00 nur eingänge; 0xFF nur ausgänge
	// fünfter und sechster als ausgang: DDRB = 0b01100000;
		
	int fa = 250;
	long intervall; // gibt es long?
	int value;
	int channel = 0;
	
    // Update
    while (1) 
    {
		// set Port B5 to high
		PORTB |= 0B00100000;
		_delay_ms(1000);
		PORTB &= 0B11011111;
		_delay_ms(300);
    }
	
	
	return 0; // wird nie erreicht
}



