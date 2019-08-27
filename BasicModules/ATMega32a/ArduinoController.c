// Clock Speed
#define F_CPU 16000000UL // 16 MHz

#include <avr/io.h>
#include <util/delay.h>

void  readEKG()
{
	
	// set filter parameter	
	int fa = 250;
	
}

void readEMG()
{
	// set filter parameter
	int fa = 250;
	
	// filter signal
	
}

/*
	Returns the value of the Sensor 
	Sensor: 0 - 5 Volt
	return 

*/
int analogRead5()
{
	return 0;	
}


// does something
void uart_init()
{
	UBRR0H = (51 >> 8);
	UBRR0L = 51;
	
	UCSR0B = (1<<TXEN0) | (1<<RXEN0);
	UCSR0C = (1<<UCSZ00) | (1<<UCSZ01); //8bit

}


void uart_outputchar(char ch)
{
	while(!(UCSR0A &(1<<UDRE0)))
	;
	UDR0 = ch;
}


void uart_output(char *calledstring)
{
	for (int i=0; i<255; i++)
	{
		if (calledstring[i] != 0)
		{
			uart_outputchar(calledstring[i]);
		}
		else
		{
			break;
		}
	}
}

// interrupt function
ISR(PCINT2_vect)
{
	if((PIND & (1 << PIND4)) == 0)
	{
		PORTD ^= (1<<LED1);
		_delay_ms(250);
	}
}




int main(void)
{
	// start / setup
	
	// set bit 5 of the data direction of port b to 1
	// indicates that we want to use port b as output
	DDRB |= 0B00100000;
	// Alternative:
	// einmal festlegen, welche ports sind eingang,
	// hier: welche sind ausgang:
	// 0 für Eingang, 1 für ausgang bsp: 0x00 nur eingänge; 0xFF nur ausgaenge
	// fünfter und sechster als ausgang: DDRB = 0b01100000;
		
		
	DDRD |= (1<<5);
	char INPUT[255];
	uart_init();
	
		
	
    // Update
    while (1) 
    {
		// set Port B5 to high
		PORTB |= 0B00100000;
		_delay_ms(1000);
		PORTB &= 0B11011111;
		_delay_ms(300);
		
    }
	
	
	return 0; // wird nie erreicht
}



