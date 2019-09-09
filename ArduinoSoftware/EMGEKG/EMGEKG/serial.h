/*
 * serial.h
 *
 *  Author: Marten
 */ 

#ifndef SERIAL_H_
#define SERIAL_H_

#include "tools.h"

#define TX_BUFFER_SIZE 255  // 255 max!

void serialInit(long long CPUFreq, int baudRate);
void writeString(char* ptr, Bool bNewLine); // must have null terminator!
void writeInt8(int8_t number);
void writeInt32(int32_t number);

#endif /* SERIAL_H_ */