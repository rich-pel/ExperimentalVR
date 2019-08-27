// ** ATMEGA16 C-Language File **

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/util/lcd.h>


//	Autor: Zhou, Schubert
//	Firma: FH-Gelsenkirchen
//	Projektgruppe: E
//	Datum. 04.06.09

#define BAUD 2400
#define MYUBRR F_CPU/16/BAUD-1
#define NoButton 255

#define WRITE_ADDRESS 0x9E 
#define READ_ADDRESS 0x9F 
#define START_CONVERSION 0xEE 
#define READ_TEMPERATURE 0xAA 

void ERROR (void ); 
void TWI_Init(void); 
void TWI_Send (void); 
void TWI_Start(void); 
void TWI_Stopp(void); 
void TWI_SendAddr(unsigned char address); 
void TWI_SendComm(unsigned char command); 
unsigned char TWI_Receive(); 
void TWI_Rec(void); 


// Begin of Function TWI_Init 
void TWI_Init(void) 
{ 
	TWCR = 0b00000100; 						// Activate TWI 
	TWSR = 0; 								// Reset Status and Prescaler 
	TWBR = 72; 								// Bit Rate Register 6.25kHz 
	_delay_ms(300); 
} 
// End of Function TWI_Init

// Begin of Function TWI_send 
void TWI_Send(void) 
{ 
	TWI_Start(); 
	TWI_SendAddr(WRITE_ADDRESS); 
	TWI_SendComm(START_CONVERSION); 
	TWI_Stopp(); 
	_delay_ms(5); 
} 
// End of Function TWI_Send 

// Begin of Function TWI_Receive 
unsigned char TWI_Receive(void) 
{ 
	unsigned char temp=0; 
	TWI_Start(); 
	TWI_SendAddr(WRITE_ADDRESS); 
	TWI_SendComm(READ_TEMPERATURE); 
	TWI_Start(); 
	TWI_SendAddr(READ_ADDRESS); 
	TWI_Rec(); 
	temp= TWDR; 
	TWI_Rec(); 
	TWI_Stopp(); 
	return temp; 
} 
// End of Function TWI_Send 

//Begin Of Function TWI_Start 
void TWI_Start(void) 
{ 
	TWCR=(1<<TWINT) | (1<<TWSTA) | (1<<TWEN); 
	while(!(TWCR & (1<<TWINT))) 
		{ 
		} 
} 
//End Of Function TWI_Start 

//Begin Of Function TWI_Stopp 
void TWI_Stopp(void) 
{ 
	TWCR=(1<<TWINT) |(1<<TWEN) |(1<<TWSTO); 
} 
//End Of Function TWI_Stopp 

//Begin Of Function TWI_SendComm 
void TWI_SendComm(unsigned char command) 
{ 
	TWDR=command; 
	TWCR=(1<<TWINT) | (1<<TWEN); 
	while(!(TWCR & (1<<TWINT))) 
		{ 
		} 
} 
//End Of Function TWI_SendComm 

//Begin Of Function TWI_SendAddr 
void TWI_SendAddr(unsigned char address) 
{ 
	TWDR=address; 
	TWCR = (1<<TWINT) | (1<<TWEN); 
	while(!(TWCR & (1<<TWINT))) 
		{ 
		} 	
} 
//End Of Function TWI_SendAddr

//Begin Of Function TWI_Rec 
void TWI_Rec (void) 
{ 
	TWCR=(1<<TWINT) | (1<<TWEN); 
	while(!(TWCR & (1<<TWINT))) 
		{ 
		} 
} 
//End Of Function TWI_Rec



void USART_Transmit(unsigned char data)
{
	while (!(UCSRA & (1<<UDRE)));
	UDR = data;
} 

unsigned char USART_Receive(void)
{
	while (!(UCSRA & (1<<RXC)));
	return UDR;
}

void USART_Init(unsigned int ubrr)
{
	UBRRH = (unsigned char)(ubrr>>8);			//baud rate
	UBRRL = (unsigned char)ubrr;

	UCSRB = (1<<RXEN) | (1<<TXEN) | (1<<RXCIE);	//Receiver/Transmitter aktivieren

	UCSRC = (1<<URSEL) | (1<<USBS) | (3<<UCSZ0);//Formatierung 8 Datenbits 2 Stopp-Bits = 10Bit Datenwortlänge
}

/*ISR(USART_RXC_vect)
{
	USART_Receive();

	lcd_data(UDR);
}*/



void USART_ctrl (char *s)
{
	while (*s)
		{
			USART_Transmit(*s);
			s++;
		}
}

void main (void)
{
		// Init Port A
	PORTA=0xFF;
	DDRA=0xFF;
		// Init Port B
	PORTB=0x00;
	DDRB=0x00;
		// Init Port C
	PORTC=0xFF;
	DDRC=0xFF;
		// Init Port D
	PORTD=0xFF;
	DDRD=0xFF;
	
	lcd_init();				// Display Init
	lcd_clear();			// Anzeige komplett löschen

	USART_Init(MYUBRR);
	
	sei();					// globale Interruptfreigabe

//	char string[]=("Hello World!!!\n\r");	
	
	unsigned char Temperature=0; 
	char Buffer[10]; 

	while (1) 
	{ 
		TWI_Init(); 
		TWI_Send(); 
		Temperature=TWI_Receive(); 
		itoa(Temperature, Buffer, 10);		// Convert HEX-Value to String 
		lcd_clear(); 
		lcd_string(&Buffer); 				// Display String on LCD 
		lcd_string(" Grad Celsius");
		USART_ctrl(&Buffer);
		USART_ctrl(" Grad Celsius\n\r");
		_delay_ms(2000); 					// Time Delay
	} 	
	return 0; 	

/*	while(1)
	{
		if (PINB!=NoButton)
		{
			USART_ctrl(string);
			_delay_ms(100);
		}
	}*/
}


