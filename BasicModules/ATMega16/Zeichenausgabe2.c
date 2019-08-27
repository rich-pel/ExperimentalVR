// Programm: Zeichenausgabe
// Authoren Andreas Multhaupt, Rui He
// Erstellungsdatum: 28.05.2009
// Geändert am: -
// Version 1.0

// Includes:

#include <C:/avr/include/avr/io.h>
#include <C:/avr/include/stdlib.h>
#include <C:/avr/include/avr/interrupt.h>
#include <C:/avr/include/util/delay.h>
#include <C:/avr/include/avr/signal.h>
#include <C:/avr/include/util/lcd.h>

int main()
{
	PORTA=0xFF;
	DDRA=0xFF;
	lcd_init ();
	lcd_clear ();
	lcd_string ("Text");
	while (1)
	{
		lcd_clear ();
		lcd_string ("Text");
		lcd_command(0b00010000);
		_delay_ms(2000);
	}

}
