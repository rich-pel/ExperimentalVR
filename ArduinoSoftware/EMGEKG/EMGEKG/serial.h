/*
 * serial.h
 *
 *  Author: Marten
 */ 

#include <avr/io.h>

#define TX_BUFFER_SIZE 128	// 255 max!

void serialInit(long long CPUFreq, int baudRate);
void writeString(char* ptr);
void writeInt8(int8_t number);
void writeInt32(int32_t number);