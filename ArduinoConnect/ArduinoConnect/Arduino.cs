using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Management;
using System.Threading;

namespace ArduinoConnect
{
    public static class Arduino
    {
        public static bool IsRunning { get { return Loop.IsAlive; } }
        public static bool IsConnected { get { return ConnectedDevice != null && ConnectedDevice.IsOpen; } }
        public static float Arm { get; private set; }
        public static float Heart { get; private set; }

        // A Package consits of 4 bytes:
        // header (2 bytes): 0xff, 0xff
        // value  (2 bytes): first 6 bits -> channel A0 - A5, remaining 10 bits: channel value from 0 - 1023

        const byte PACKAGE_SIZE = 4; // the whole package, header + value
        const byte VALUE_SIZE = 2;   // just the value size of the package
        const float MAX_SENSOR_VALUE = 1023.0f;
        const int DEVICE_REFRESH_TIMER = 2000; // milliseconds

        // only one channel per package
        const ushort A0 = 0b1000000000000000;
        const ushort A1 = 0b0100000000000000;
        const ushort A2 = 0b0010000000000000;
        const ushort A3 = 0b0001000000000000;
        const ushort A4 = 0b0000100000000000;
        const ushort A5 = 0b0000010000000000;
        const ushort CHANNEL_MASK = 0b1111110000000000;
        const ushort VALUE_MASK   = 0b0000001111111111;

        static SerialPort ConnectedDevice;
        static Thread Loop = new Thread(Update);
        static bool bStop;

        static object myLock = new object();

        //static List<COMDevice> Devices = new List<COMDevice>() { new COMDevice { ID = "FAKE", Description = "test" } };
        public static string[] AvailableDevices { get; private set; } = new string[0];
        public static int DeviceIndex { get; private set; }


        public static void Startup()
        {
            if (Loop.IsAlive) return;

            bStop = false;
            Loop.Start();
        }

        public static void Shutdown()
        {
            if (IsConnected)
            {
                Disconnect();
            }
            lock (myLock)
            {
                bStop = true;
            }
        }

        public static void Connect(int Index)
        {
            if (!Loop.IsAlive)
            {
                Logger.Log("Call Startup() first!", ELogType.Warning);
                return;
            }

            if (IsConnected)
            {
                Logger.Log("Could not Connect since we're already connected!", ELogType.Warning);
                return;
            }

            lock (myLock)
            {
                if (Index < 0 || Index >= AvailableDevices.Length)
                {
                    Logger.Log("Given Device Index '" + Index + "' is out of bounds (" + AvailableDevices.Length + ")!", ELogType.Warning);
                    return;
                }

                try
                {
                    ConnectedDevice = new SerialPort();
                    ConnectedDevice.PortName = AvailableDevices[Index];
                    ConnectedDevice.BaudRate = 9600; // bits per second
                    ConnectedDevice.Handshake = Handshake.None;
                    ConnectedDevice.Parity = Parity.None;
                    ConnectedDevice.Open();
                }
                catch (Exception e)
                {
                    Logger.Log(e.Message, ELogType.Error);
                    ConnectedDevice?.Close();
                    ConnectedDevice = null;
                }
            }

            // just info for outer user
            DeviceIndex = Index;
            Logger.Log("Connected to: " + AvailableDevices[Index], ELogType.Log);
        }

        public static void Disconnect()
        {
            if (!Loop.IsAlive)
            {
                Logger.Log("Call Startup() first!", ELogType.Warning);
                return;
            }

            if (!IsConnected)
            {
                Logger.Log("Not connected!", ELogType.Warning);
                ConnectedDevice = null; //just to be sure
                return;
            }

            lock (myLock)
            {
                ConnectedDevice.Close();
                ConnectedDevice = null;
            }

            Logger.Log("Disconnected from "+ AvailableDevices[DeviceIndex], ELogType.Log);
        }

        static void Update()
        {
            bool bStartFound = false;
            byte[] buffer = new byte[PACKAGE_SIZE];

            while (!bStop)
            {
                while (!IsConnected)
                {
                    if (bStop)
                    {
                        Logger.Log("STOPPED", ELogType.Log);
                        return;
                    }

                    AvailableDevices = SerialPort.GetPortNames();
                    Thread.Sleep(DEVICE_REFRESH_TIMER);
                }

                if (ConnectedDevice == null || !ConnectedDevice.IsOpen)
                {
                    Logger.Log("Device is not open! This should never happen!", ELogType.Error);
                    continue;
                }

                int bytesToRead = ConnectedDevice.BytesToRead;
                if (bytesToRead < PACKAGE_SIZE)
                {
                    continue;
                }

                // Start flag consits of two consecutive bytes of value 0xff
                // we discard all prior bytes before the start flag
                if (!bStartFound)
                {
                    for (int i = 0; i < bytesToRead; ++i)
                    {
                        ConnectedDevice.Read(buffer, 0, 1);
                        //Console.WriteLine("Looking for Start Flag["+i+"]: " + buffer[0]);

                        if (buffer[0] == 0xff)
                        {
                            ConnectedDevice.Read(buffer, 1, 1);
                            if (buffer[1] == 0xff)
                            {
                                // two successive 0xff bytes mark the start of a package
                                bStartFound = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ConnectedDevice.Read(buffer, 0, VALUE_SIZE);
                    HandleReceivedPackage(buffer);
                    bStartFound = false;
                }
            }

            Logger.Log("STOPPED", ELogType.Log);
        }

        static void HandleReceivedPackage(byte[] package)
        {
            // determine channel
            ushort pkgValue = BitConverter.ToUInt16(package, 0);

            ushort channel = (ushort)(pkgValue & CHANNEL_MASK);
            ushort chValue = (ushort)(pkgValue & VALUE_MASK);

            //Console.WriteLine("Discrete value: " + discrete);

            float normalized = chValue / MAX_SENSOR_VALUE;
            lock (myLock)
            {
                switch (channel)
                {
                    case A0: // Arm value
                        Arm = chValue;
                        break;
                    case A1: // Heart value
                        Heart = chValue;
                        break;
                    default:
                        Logger.Log("Unknown Channel: " + channel, ELogType.Error);
                        break;
                }
            }
        }
    }
}
