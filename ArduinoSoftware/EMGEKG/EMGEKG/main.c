/*
 * main.c
 *
 * Author : Richard and Marten 
 *
 */ 

// Clock Speed
#define F_CPU 16000000UL // 16 MHz

#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include "serial.h"
#include "analog.h"


void sendInputChannel(uint8_t channel)
{
	//static char strBuffer[16];
	
	uint16_t lastADC = getInputValue(channel);
	if (lastADC == ADC_ERROR)
	{
		writeString("ERROR", TRUE);
	}
	else
	{
		// write as string
		//strBuffer[0] = 'A';
		//strBuffer[1] = channel + '0';
		//strBuffer[2] = ':';
		//strBuffer[3] = ' ';
		//intToStr(lastADC, &strBuffer[4]);
		//writeString(strBuffer, TRUE);
		
		// write values
		writeUInt16(0xFFFF); // header
		
		// 1111 0000 0000 0000		channel number (max 15)
		// 0000 0011 1111 1111		channel value  (max 1023)
		writeUInt16(lastADC + (channel << 12));	// 4 last bits determine the channel number
	}
}

int main(void)
{
	// Init
	serialInit(F_CPU, 9600);
	analogInit();
	sei(); // don't forget to set the internal interrupts ON!
	
    // Update
    while (1) 
    {		
		_delay_ms(10); // can we get rid of this?
		
		sendInputChannel(A0);
		sendInputChannel(A1);
		//sendInputChannel(A2);
		//sendInputChannel(A3);
		//sendInputChannel(A4);
		//sendInputChannel(A5);
    }
}

