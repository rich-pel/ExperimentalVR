/*
 * analog.h
 *
 *  Author: Marten
 *
 *	Can grab the current values from Analog inputs A0 to A5. 
 *	Remember when using multiple inputs, memory is limited ;).
 *	The less inputs are grabbed, the more buffer you can assign.
 *
 *	ADC = Analog Digital Conversion
 */ 


#ifndef ANALOG_H_
#define ANALOG_H_

#include "tools.h"

#define ADC_BUFFER_SIZE 1  // 1-255 max!
#define A0 0
#define A1 1
#define A2 2
#define A3 3
#define A4 4
#define A5 5
#define ADC_INPUT_MAX 1+ A1 // set to A5 to include ALL analog inputs! this is inclusive!
#define ADC_ERROR 0xFFFF

void analogInit();
uint16_t getInputValue(uint8_t input);	// returns ADC_ERROR if input was not in range

#endif /* ANALOG_H_ */