/*
 * tools.h
 *
 *  Author: Marten
 */ 


#ifndef TOOLS_H_
#define TOOLS_H_

#include <avr/io.h>

// let's invent a bool type since C doesn't know one
typedef uint8_t Bool;
#define TRUE 1
#define FALSE 0

// requires a pre-set char buffer of some size
char* intToStr(int i, char b[]);

#endif /* TOOLS_H_ */