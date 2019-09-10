using System;
using System.IO.Ports;
using System.Management;
using System.Threading;

namespace ArduinoConnect
{
    public static class Arduino
    {
        public static bool IsRunning { get; private set; }
        public static float LeftArm { get; private set; }
        public static float RightArm { get; private set; }

        const int PACKAGE_SIZE = 3;
        const float MAX_SENSOR_VALUE = 1023.0f;
        const byte PACKAGE_ARM_LEFT = 0;
        const byte PACKAGE_ARM_RIGHT = 1;
        const int INIT_ATTEMPT_SLEEP = 2000; // milliseconds

        // TODO: Try device path "\\.\COM5"
        static string[] ArduinoDeviceHints = { "Arduino", "USB" };

        static SerialPort Device;
        static Thread Loop = new Thread(Update);
        static bool bStop;

        static object myLock = new object();


        public static void Start()
        {
            if (Loop.IsAlive)
            {
                Logger.Log("Could not Start ArmGrabber since it's already running!", ELogType.Warning);
                return;
            }

            // no lock needed here since loop thread is not started anyway
            bStop = false;
            Loop.Start();
            IsRunning = true;
        }

        public static void Stop()
        {
            if (bStop)
            {
                Logger.Log("ArmGrabber is currently stopping", ELogType.Warning);
                return;
            }

            if (!Loop.IsAlive)
            {
                Logger.Log("ArmGrabber is already stopped!", ELogType.Warning);
                return;
            }

            Logger.Log("Stopping ArmGrabber...", ELogType.Log);
            lock (myLock)
            {
                bStop = true;
            }
        }

        static bool TryFindDevice()
        {
            Logger.Log("Looking for Devices...", ELogType.Log);
            Device = AutodetectArduinoDevice();
            if (Device == null)
            {
                Logger.Log("No Arduino Device found!", ELogType.Warning);
                return false;
            }

            try
            {
                Device.Open();
            }
            catch
            {
                Logger.Log("Opening Arduino Device (COM Port) failed!", ELogType.Warning);
                Device = null;
                return false;
            }

            if (!Device.IsOpen)
            {
                Logger.Log("Could not open device: " + Device.PortName, ELogType.Warning);
                Device = null;
                return false;
            }

            return true;
        }

        static void Update()
        {
            while (!TryFindDevice())
            {
                if (bStop)
                {
                    Logger.Log("STOPPED", ELogType.Log);
                    return;
                }

                Thread.Sleep(INIT_ATTEMPT_SLEEP);
            }

            if (Device == null || !Device.IsOpen)
            {
                Logger.Log("Device is not open! This should never happen!", ELogType.Error);
                return;
            }

            bool bStartFound = false;
            byte[] buffer = new byte[PACKAGE_SIZE];

            while (!bStop)
            {
                int bytesToRead = Device.BytesToRead;

                /*if (bytesToRead >= 20)
                {
                    byte[] test = new byte[20];
                    device.Read(test, 0, 20);

                    for (int i = 0; i < 20; ++i)
                    {
                        Console.Write(test[i] + (test[i] < 10 ? "   " : (test[i] < 100 ?  "  " : " ")));
                        if (i % 10 == 0)
                        {
                            Console.WriteLine();
                        }
                    }
                }*/

                // Start flag consits of two consecutive bytes of value 0xff
                // we discard all prior bytes before the start flag
                if (!bStartFound && bytesToRead > 2)
                {
                    for (int i = 0; i < bytesToRead; ++i)
                    {
                        Device.Read(buffer, 0, 1);
                        //Console.WriteLine("Looking for Start Flag["+i+"]: " + buffer[0]);

                        if (buffer[0] == 0xff)
                        {
                            Device.Read(buffer, 1, 1);
                            if (buffer[1] == 0xff)
                            {
                                // two successive 0xff bytes mark the start of a package
                                bStartFound = true;
                                break;
                            }
                        }
                    }
                }
                else if (bStartFound && bytesToRead >= PACKAGE_SIZE)
                {
                    Device.Read(buffer, 0, PACKAGE_SIZE);
                    HandleReceivedPackage(buffer);
                    bStartFound = false;
                }
            }

            Device.Close();
            lock (myLock)
            {
                IsRunning = false;
            }
            Logger.Log("STOPPED", ELogType.Log);
        }

        static void HandleReceivedPackage(byte[] package)
        {
            ushort discrete = BitConverter.ToUInt16(package, 1);
            //Console.WriteLine("Discrete value: " + discrete);

            float value = discrete / MAX_SENSOR_VALUE;
            lock (myLock)
            {
                switch (package[0])
                {
                    case PACKAGE_ARM_LEFT:
                        LeftArm = value;
                        break;
                    case PACKAGE_ARM_RIGHT:
                        RightArm = value;
                        break;
                    default:
                        Logger.Log("Unknown Arm Flag: " + package[0], ELogType.Error);
                        break;
                }
            }
        }

        static SerialPort AutodetectArduinoDevice()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);
            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();
                    Logger.Log("Found Device: " + desc, ELogType.Log);

                    if (StringContains(desc, ArduinoDeviceHints))
                    {
                        SerialPort serialPort = new SerialPort();
                        serialPort.PortName = deviceId;
                        serialPort.BaudRate = 9600; // bits per second
                        serialPort.Handshake = Handshake.None;
                        serialPort.Parity = Parity.None;
                        return serialPort;
                    }
                }
            }
            catch
            {
                /* Do Nothing */
            }
            return null;
        }

        static bool StringContains(string source, string[] lookingFor)
        {
            foreach (string str in lookingFor)
            {
                if (source.Contains(str)) return true;
            }
            return false;
        }
    }
}
