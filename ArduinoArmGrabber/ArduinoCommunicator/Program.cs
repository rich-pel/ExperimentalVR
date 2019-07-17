using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Threading.Tasks;

namespace ArduinoArmGrabber
{
    static class Program
    {
        static float previousLeft = 0f;
        static float previousRight = 0f;


        static void Main(string[] args)
        {
            ArmGrabber.Start();

            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    PipeLogs();

                    if (ArmGrabber.LeftArm != previousLeft)
                    {
                        Console.WriteLine("Left Arm: " + ArmGrabber.LeftArm);
                        previousLeft = ArmGrabber.LeftArm;
                    }
                    if (ArmGrabber.RightArm != previousRight)
                    {
                        Console.WriteLine("Right Arm: " + ArmGrabber.RightArm);
                        previousRight = ArmGrabber.RightArm;
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            ArmGrabber.Stop();
            PipeLogs();
        }

        static void PipeLogs()
        {
            // Pipe logs to Console
            while (Logger.HasNewMessage(out string Message, out ELogType Type))
            {
                switch (Type)
                {
                    case ELogType.Log:
                        Console.WriteLine("[LOG] " + Message);
                        break;
                    case ELogType.Warning:
                        Console.WriteLine("[WARNING] " + Message);
                        break;
                    case ELogType.Error:
                        Console.WriteLine("[ERROR] " + Message);
                        break;
                }
            }
        }
    }
}

