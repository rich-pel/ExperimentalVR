/*
 * main.c
 *
 * Author : Richard and Marten 
 *
 */ 

// Clock Speed
#define F_CPU 16000000UL // 16 Hz

#include <avr/io.h>
#include <util/delay.h>
#include "serial.h"

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
	
	serialInit(F_CPU, 9600);
	
    // Update
    while (1) 
    {
		// set Port B5 to high
		PORTB |= 0B00100000;
		_delay_ms(100);
		PORTB &= 0B11011111;
		_delay_ms(100);
		
		//writeString("Test");
		writeInt32(42);
		writeInt8(0xff);
    }
}

