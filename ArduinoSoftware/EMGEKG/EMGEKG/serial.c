/*
 * serial.h
 *
 *  Author: Marten
 */ 

#include "serial.h"
#include <avr/interrupt.h>
char TXBuffer[TX_BUFFER_SIZE];	// ring buffer

uint8_t HeadWrite = 0;
uint8_t HeadRead = 0;


void serialInit(long long CPUFreq, int baudRate)
{	
	// see formula source: ATmega328p documentation Table19-1 (page 146)
	int brc = ((CPUFreq/16/baudRate) - 1);
	
	// writing the calculated baud rate (BRC) into UBRR0 register
	// UBRR register's width is 12 bit! so we have to split up the BRC
	UBRR0L = brc;			// set the lower 8 bits
	UBRR0H = (brc >> 8);	// set the remaining 4 bits via right shifting
	
	// Enable USART transmitter + enable interrupt when USART transmit is complete!
	UCSR0B = (1 << TXEN0) | (1 << TXCIE0);
	
	// set character size to 8 bits!
	UCSR0C = (1 << UCSZ00) | (1 << UCSZ01);
	
	// enable interrupts
	sei();
}

void sendNext()
{
	if (HeadRead != HeadWrite)
	{
		// everything written into the UDR0 register (1 byte) will be send out!
		UDR0 = TXBuffer[HeadRead++];
		
		if (HeadRead >= TX_BUFFER_SIZE)
		{
			HeadRead = 0;
		}
	}
}

void sendNextIfEmpty()
{
	// if shifting out UDR0 is done, send next byte
	if (UCSR0A & (1 << UDRE0))
	{
		sendNext();
	}
}

void appendByte(char c)
{
	TXBuffer[HeadWrite++] = c;
	
	if (HeadWrite >= TX_BUFFER_SIZE)
	{
		HeadWrite = 0;
	}
}

void writeString(char* ptr)
{
	// assert null terminator!
	for (uint8_t i = 0; ptr[i] != 0; ++i)
	{
		appendByte(ptr[i]);
	}
	
	sendNextIfEmpty();
}

void writeInt8(int8_t number)
{
	appendByte(number);
	sendNextIfEmpty();
}

void writeInt32(int32_t number)
{
	appendByte(number);
	appendByte(number >> 8);
	appendByte(number >> 16);
	appendByte(number >> 24);
	sendNextIfEmpty();
}

// interrupt: gets called when shifting out UDR0 is done (USART transmit complete)
ISR(USART_TX_vect)
{
	sendNext();
}