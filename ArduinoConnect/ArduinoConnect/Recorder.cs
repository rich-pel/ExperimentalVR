using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace ArduinoConnect
{
    public static class Recorder
    {
        struct RecPoint
        {
            public byte Channel;
            public ushort Value;
            public uint Millisecond;
        }

        public enum ERecordingState
        {
            Stopped,
            Recording,
            Stopping
        }

        public static ERecordingState RecordingState
        {
            get
            {
                if (Stream != null && !bStop) return ERecordingState.Recording;
                else if (Stream != null && bStop) return ERecordingState.Stopping;
                else return ERecordingState.Stopped;
            }
        }

        public static TimeSpan RecordingTime { get { return Watch.Elapsed; } }

        static FileStream Stream = null;
        static Thread WriteThread;
        static object myLock = new object();
        static bool bStop = false;
        static Queue<RecPoint> WriteBuffer = new Queue<RecPoint>();
        static Stopwatch Watch = new Stopwatch();


        public static bool StartRecording(string FilePath)
        {
            lock (myLock)
            {
                if (Stream != null)
                {
                    Logger.Log("We're already recording!", ELogType.Warning);
                    return false;
                }

                try
                {
                    Stream = new FileStream(FilePath, FileMode.Create);
                }
                catch (Exception e)
                {
                    Logger.Log(e.Message, ELogType.Error);
                    Stream = null;
                    return false;
                }

                if (WriteThread != null && WriteThread.IsAlive)
                {
                    Logger.Log("Write Thread already running! This should never happen!", ELogType.Error);
                    return false;
                }

                bStop = false;
                WriteThread = new Thread(Record);
                Watch.Start();
                WriteThread.Start();
                return true;
            }
        }

        public static bool StopRecording()
        {
            if (Stream == null)
            {
                Logger.Log("Cannot stop recording, no recording started yet!", ELogType.Warning);
                return false;
            }

            if (bStop)
            {
                Logger.Log("Recording is already shutting down...", ELogType.Warning);
                return false;
            }

            bStop = true;
            return true;
        }

        static void Record()
        {
            while (!bStop)
            {
                while (WriteBuffer.Count > 0)
                {
                    RecPoint p = WriteBuffer.Dequeue();
                    Stream.WriteByte(p.Channel);
                    Stream.Write(BitConverter.GetBytes(p.Value), 0, sizeof(ushort));
                    Stream.Write(BitConverter.GetBytes(p.Millisecond), 0, sizeof(uint));
                }
            }

            Stream.Close();
            Stream = null;

            lock(myLock)
            {
                Watch.Stop();
            }

            Logger.Log("Recording stopped successfully!", ELogType.Log);
        }

        internal static void OnPackageReceived(ushort channel, ushort value)
        {
            lock (myLock)
            {
                if (Stream == null || bStop)
                {
                    return;
                }

                WriteBuffer.Enqueue(new RecPoint
                {
                    Channel = (byte)channel,
                    Value = value,
                    Millisecond = (uint)Watch.ElapsedMilliseconds
                });
            }
        }
    }
}
