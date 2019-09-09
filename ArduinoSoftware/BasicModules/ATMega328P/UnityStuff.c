
// ##########################################
// #		From Lecture					#
// ##########################################	

// original in java

// We want to have different controlling option

Serial myPort;
Biquad notch50Hz;
Biquad TP15Hz;
Biquad HP1Hz;
Biquad HP15Hz;

Dervative myDerivative;
Averaging myAveraging;
Averaging myAveraging2;
Thresholding myThresholding;

int[] data1 = new int[]; // not legth depens on filter, rigth?
int data_index; // because its a ringspeicher

byte[] inBuffer = new byte[17]; // 17 cause:
int[] inUInt = new int[17];
int newDataPoint = 0;

int counter = 0;

PrintWriter output;

boolean isFFT = false;
boolean isRecording = false;
boolean isNotch50Hz = false;
boolean isTP15Hz = false;
boolean isHP1Hz = false;
boolean isHP15Hz = false;
boolean isDerivative = false;
boolean isSquaring = false;
boolean isAveraging = false;
boolean isThresholding = false;

//used to:
int t1;

// used to:
int serialCounter = 0;

// used to:
int datasumme = 0; 
int x_scale = 1;  


void Start() 
{
	// display init:
	// stuff
	
	
	// list of avadible Serial Ports:
	println(Serial.list());
	
	// Serielle Schnittstelle oeffnen
	// >> Hier bitte den String aus der Fusszeile der ArduinoIDE einfuegen
	myPort = new Serial(this, "/dev/cu.usbmodem1411", 57600);
	
	// Vorbereitung fuer die serielle Schnittstelle
	// serialEvent ausloesen, wenn CR/LF auftritt
	myPort.bufferUntil('\n');
	
	// Ausgabedatei fuer Recording festlegen
	output = createWriter("data.txt");
	
	
	// Datenarray auf Null setzten, aufgrund der Mittelwertberechung
	// in SerialEvent
	for(int i= 0; i < with-1; i++)
	{
		data1[i] =0;
		
	}
	
	// Biquad-Filter initialisieren
	// ============================
	
	// Notch 50 Hz Filter
	// Berechnet mit earlevel.com
	
	// a0 = 0.60031988282
	
	// Signalableitung
	myDerivative = new Derivative();
	myAveraging = new Averaging();
	myAveraging2 = new Averaging();
	
	myThresholding = new Thresholding();
	
	// fft wird in draw() umgesetzt
	int fft_n = 256; // why???
	
	FFT fft = new FFT(fft_n);
	double[] fft_re = new double[fft_n];
	double[] fft_im = new double[fft_n];
	
	void draw()
	{
		
		int t = millis();
		
		int mittelwert = datensumme / width;
		
		beginShape(LINES);
		// TODO: Zeichen des Ringspeichers, ab data_index
		
		
		if(isFFT)
		{
			// 256 Werte vor data_index kopieren	
		}
		else // normalen Zeitgraph zeichen (isFFT = false)
		{
			int max = 0;
			int draw_index = data_index;
			for(int i =0; i < width-1; i++)
			{
				vertex(i*x_scale, (height/2)-((data1[draw_index)-mittelwert/2));
				// Ringspeicherende betrachten:
				if(draw_index == width - 1){	draw_index = -1;}		
				vertex((i+1)*x_scale, (height/2)-((data1[draw_index)-mittelwert/2));
				darw_index++;
			}
			endShape();
			//println(max);
			//println(millis()-t);
		}
	}
	
	
	// Daten vom Arduino ueber die serielle Schnittstelle empfangen:
	// ==========================
	// Daten werden als ASCII-Zeichenfolge mit CR/LF gesendert
	
	void serialEvent(Serial myPort)
	{
		int count = myPort.available();
		// println(count); count ist in der Regel 5
		newDataPoint1 = int(trim(inBuffer));
		
		if( isNotch50Hz) newDataPoint1 = notch50Hz.calc(newDataPoint1);
		if (isTP15Hz) newDataPoint1 = TP15Hz.calc(newDataPoint1);
		if (isHP1Hz) newDataPoint1 = HP1Hz.calc(newDataPoint1);
		if (isHP15Hz) newDataPoint = HP15Hz.calc(newDataPoint);
		
		if (isDerivative) newDataPoint = myDerivative.calc(newDataPoint);
		if (isAveraging) newDataPoint = myAveraging.calc(newDataPoint);
		if (isSquaring)
		{
			newDataPoint = newDataPoint * newDataPoint;
			newDataPoint = myAveraging2.calc(newDataPoint);
		}
		if(isThresholding)
		{
			newDataPoint = myThresholding.calc(newDataPoint);
			if(newDataPoint > 100)
			{
				println(round(600000/(millis() -t1)));
				t1 = millis();
			
			}
		}
		data1[data_index] = newDataPoint;
		
		
		// geitender mittelwert
		datasumme = datasumme + data1[index];
				
		if(data_index != width - 1)
		{
			datasumme = datasumme - data1[data_index +1];
		}
		else
		{
			datasumme = datasumme - data1[0];
			
		}
		// geitender mittelwert ende

		
		if(isRecording)
		{
			// save it
		}
		
		data_index ++;
		if (data_index > width-1)
		{
			data_index =0;
		}
	}
	
	void KeyPress(){
		// unity 
	}
	
	
	
	
}