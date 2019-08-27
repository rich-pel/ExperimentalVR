#define F_CPU 8000000UL // 8MHz
#include <avr/io.h>

#define LED1 5 
#define BUT1 2

int main(void)
{
	// Unser Ein-/Ausgangsregister
	DDRD = 0b00100000; // Bei eins der Ausgang, bei 0 ein eingang
	// Pull ups schalten (bei atmel git es keine pulldowns)
	// wenn es ein Eingang ist, leg es gest op der pull ein oder aus ist und somit
	// ein currentSing oder currentSRC ist
	PORTD = 0b00000100; // das ist PD2
	
	/* 
	Man kann einen schalter gegen high (5V) oder gegen low (-5V)  schalten
	Er macht es gegen masse 
	*/
	
	//DDRD |= (1<<LED1); alternativ
	//PORTD |= (1<<BUT1);
	
	while(1)
	{
		// PIND ist nicht PORTD - siehe Datenblatt
		if (!(PIND & (1<<BUT1)))
		{
			PORTD |= (1<<LED1);
		}
		else
		{
			PORTD &= ~(1<<LED1);
		}
	}
}

#define F_CPU 8000000UL
#include <avr/io.h>
#include <util/delay.h>

#define LED1 5
#define BUT1 2

typedef int bool; // es gibt keine booleans, also selber machen

int main(void)
{
	DDRD = 0b00100000;
	PORTD = 0b00000100;
	
	bool prev;
	
	prev = 0;
	
	while(1)
	{
		if ((!(PIND & (1<<BUT1))) & prev == 0)
		{
			PORTD |= (1<<LED1);
			prev = 1;
			_delay_ms(250);
		}
		else if (!(PIND & (1<<BUT1)) & prev == 1)
		{
			PORTD &= ~(1<<LED1);
			prev = 0;
			_delay_ms(250);
		}
	}
}