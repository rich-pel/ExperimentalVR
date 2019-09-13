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
void writeUInt8(uint8_t number);
void writeUInt16(uint16_t number);

#endif /* SERIAL_H_ */