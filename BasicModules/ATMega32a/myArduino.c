/*
 * myArduino.c
 *
 * Created: 30/07/2019 12:42:57
 *  Author: Richard
 */ 

// so schnell soll die CPU laufen
#define F_CPU 16000000UL // 16 MHz

#include <avr/io.h>
#include <util/delay.h>

// tell about functions


int main(void)
{
	
	// set bit 5 of the data direction of port b to 1
	// indicates that we want to use port b as output
	DDRB |= 0B00100000;
	// Alternative:
	// einmal festlegen, welche ports sind eingang, 
	// hier: welche sind ausgang:
	// 0 für Eingang, 1 für ausgang bsp: 0x00 nur eingänge; 0xFF nur ausgänge
	// fünfter und sechster als ausgang: DDRB = 0b01100000;
	
	int myInt = 10;
	
	// Update
	while (1)
	{
		// set Port B5 to high
		PORTB |= 0B00100000;
		_delay_ms(1000);
		PORTB &= 0B11011111;
		_delay_ms(300);
		
		// Alternativ:
		// PORTD |= (0<<5); // (...) bitmaske mit 1 << fünfmal nach rechts reingerückt 0x000000001 => 0b001000000 (so wie +=, nur verschoben in sich)
		// _delay_ms(1000);
		// PORTD &= ~(1<<5); // ~ invertiere alle bits, 0b110111111 dann verknütfe mit und &= dadurch bleiben alle wo eis ist so und wo null ist wird null (also 0b001000000 löschen)
		// _delay_ms(300);
		
		//Alternativ:
		//PORTD ^= (1<<5); // bit toggeln
		//_delay_ms(1000);
	}
	
	// wird nie erreicht
	return 0;
}





// basic to understand
// ##########################################
// #		From the EMG/EKG Shield			#
// ##########################################



#include "TimerOne.h"

int fa = 250;
long intervall;
int value;
int channel = 0;

void setup() {
	Serial.begin(57600);
	intervall = round(1000000 / fa);
	Timer1.initialize(intervall);
	Timer1.attachInterrupt(readEKG);
}

void readEKG() {
	value = analogRead(channel);
	Serial.println(value);
}

void loop() {
	
	// your programm
}
