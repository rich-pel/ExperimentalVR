// ** ATMEGA16 C-Language File ** 
// When you program this file use device ATMEGA16 
#include <C:/avr/include/avr/io.h> 
#include <C:/avr/include/util/delay.h> 
#include <C:/avr/include/util/lcd.h> 
#include <C:/avr/include/avr/interrupt.h>
#define WRITE_ADDRESS 0x9E 
#define READ_ADDRESS 0x9F 
#define START_CONVERSION 0xEE 
#define READ_TEMPERATURE 0xAA 
#define BAUD 2400
#define MYUBRR F_CPU/16/BAUD-1
// Author : Pascal,Chen,Pierre (known as Gruppe 4)
// Company : FH-Gelsenkirchen 
// Date : 29.05.2009 
// Comment : Tempsensor send to PC
// Versuch 4; Aufg. 5.3.2 
void ERROR (void ); 
void TWI_Init(void); 
void TWI_Send (void); 
void TWI_Start(void); 
void TWI_Stopp(void); 
void TWI_SendAddr(unsigned char address); 
void TWI_SendComm(unsigned char command); 
unsigned char TWI_Receive(); 
void TWI_Rec(void); 
// Begin Of Function Main 

void USART_Init (unsigned int ubrr)
{
	/* Set baud rate*/
	UBRRH = (unsigned char) (ubrr>>8);
	UBRRL = (unsigned char) ubrr;
	/*enable receiver and transmitter*/
	UCSRB = (1<<RXEN)|(1<<TXEN) | (1<<RXCIE);
	/*Set frame format: 8data, 2 ststop bit*/
	UCSRC = (1<<URSEL)|(1<<USBS)|(3<<UCSZ0);
}


void USART_Transmit (unsigned char data)
{
	/*Wait for empty transmit buffer*/
	while (!(UCSRA & (1<<UDRE)));
	/*Put data into buffer, sends the data*/
	UDR = data;
}

unsigned char USART_Receive (void)
{
	/*Wait for the data to be received*/
	while (!(UCSRA & (1<<RXC)))
	;
	/*Get and return received data from buffer*/
	return UDR;
}

int main(void) 
{ 
unsigned char Temperature=0; 
char Buffer[15]; 
PORTA=0xFF; // Init of Port A 
DDRA=0xFF; 
PORTC=0xFF; // Init of Port C 
DDRC=0xFF; 
lcd_init(); // Init of LCD 
lcd_clear(); // Erase LCD 
USART_Init(MYUBRR);
int i;
asm volatile ("sei"::);
while (1) 
{ 

	TWI_Init(); 
	TWI_Send(); 
	Temperature=TWI_Receive(); 
	itoa(Temperature, Buffer, 10 );// Convert HEX-Value to String 
	lcd_clear(); 
	lcd_string(&Buffer); // Display String on LCD 
	
	if (strcmp (Buffer,"196") == 0){
		strcpy(Buffer,"Fehler");
	}
	
	for(i=0;i<strlen(Buffer);i++)
	{
					
		USART_Transmit(Buffer[i]);
	}
				
	USART_Transmit(' ');
	USART_Transmit('C');
	USART_Transmit('e');
	USART_Transmit('l');
	USART_Transmit('s');
	USART_Transmit('i');
	USART_Transmit('u');
	USART_Transmit('s');
	USART_Transmit('\n');
	USART_Transmit('\r');
	
	_delay_ms(2000); // Time Delay 
} 
return 0; 
} 

// End of Function main 
// Begin of Function TWI_Init 
void TWI_Init(void) 
{ 
TWCR = 0b00000100; // Activate TWI 
TWSR = 0; // Reset Status and Prescaler 
TWBR = 72; // Bit Rate Register 6.25kHz 
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


