/*
 * analog.c
 *
 *  Author: Marten
 */ 

#include "analog.h"
#include <avr/interrupt.h>


typedef struct
{
	uint16_t buffer[ADC_BUFFER_SIZE];
	uint8_t headWrite;
	uint8_t headRead;
} ADCBuffer;

// create buffers for all pre-defiened analog inputs (A0 - A5 max)
ADCBuffer Buffers[ADC_INPUT_MAX];


void nextConversion()
{
	// since we can only read one analog input at a time,
	// we have to switch through them after each ADC process
	static uint8_t CurrentInput = A0;
	
	// Voltage reference from AVcc (5V on ATMega328p)
	ADMUX = (1 << REFS0);
	
	// set analog channel
	// starting bits 1 to 4 (3:0) of ADMUX determine the channel number
	ADMUX |= CurrentInput++;
	if (CurrentInput >= ADC_INPUT_MAX)
	{
		CurrentInput = A0;	// start again at A0
	}
	
	// start conversion
	ADCSRA |= (1<<ADSC);
}

void analogInit()
{	
	// Turn on ADC (Analog Digital Converter)
	ADCSRA = (1 << ADEN);
	
	// Enable Interrupt call when conversion is finished
	ADCSRA |= (1 << ADIE);
	
	// Set clock division factor to 128
	// 16Mhz / 128 = 125kHz ADC reference clock
	ADCSRA |= (1 << ADPS0) | (1 << ADPS1) | (1 << ADPS2);
	
	nextConversion();
}

Bool getNextValue(uint8_t input, uint32_t* outValue)
{
	ADCBuffer* b = &Buffers[input];
	
	if (b->headRead != b->headWrite)
	{
		*outValue = b->buffer[b->headRead++];
		
		if (b->headRead >= ADC_BUFFER_SIZE)
		{
			b->headRead = 0;
		}
		
		return TRUE;
	}
	return FALSE;
}

void writeNextValue(uint8_t input, uint16_t value)
{
	if (input >= ADC_INPUT_MAX)
	{
		return;
	}
	
	ADCBuffer* b = &Buffers[input];
	b->buffer[b->headWrite++] = value;
	
	if (b->headWrite >= ADC_BUFFER_SIZE)
	{
		b->headWrite = 0;
	}
}

uint16_t getInputValue(uint8_t input)
{	
	ADCBuffer* b = &Buffers[input];
	
	if (input >= ADC_INPUT_MAX)
	{
		// out of bounds
		return ADC_ERROR;
	}
	
	if (ADC_BUFFER_SIZE == 1)
	{
		// no need for further processing with just one value
		return b->buffer[0];
	}
	
	uint32_t valueSum = 0;
	uint32_t valueCounter = 0;
	uint32_t nextValue = 0;
	
	while(getNextValue(input, &nextValue))
	{
		valueSum += nextValue;
		valueCounter++;
	}
	
	if (valueCounter == 0)
	{
		// no new values available, return last known
		return b->buffer[b->headRead];
	}
	
	return valueSum / valueCounter;
}

// interrupt: gets called once Analog-Digital Conversion (ADC) is done
ISR(ADC_vect)
{
	// in the ADMUX register, the starting bits 1 to 4 (3:0)
	// determine the channel number. so mask out bits 5 to 8 (4:7)
	uint8_t channel = ADMUX & 0x0F;
	
	// the first 10 bits in ADC describe the
	// converted value 0 - 1023
	uint16_t value = ADC & 0x03FF;	// 10 bit mask
	
	writeNextValue(channel, value);
	nextConversion();
}