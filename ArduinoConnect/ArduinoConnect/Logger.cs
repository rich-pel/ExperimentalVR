using System.Collections.Generic;

namespace ArduinoConnect
{
    public enum ELogType
    {
        Log,
        Warning,
        Error
    }

    /*
     * This Logger is Thread safe!
     */
    public static class Logger
    {
        struct Message
        {
            public string Text;
            public ELogType Type;
        }

        const int MAX_LOGS = 100;

        static Queue<Message> Logs = new Queue<Message>();
        static object myLock = new object();

        internal static void Log(string message, ELogType type)
        {
            lock (myLock)
            {
                while (Logs.Count > MAX_LOGS)
                {
                    Logs.Dequeue();
                }
                Logs.Enqueue(new Message() { Text = message, Type = type });
            }
        }

        public static bool HasNewMessage(out string Message, out ELogType Type)
        {
            lock (myLock)
            {
                if (Logs.Count > 0)
                {
                    Message msg = Logs.Dequeue();
                    Message = msg.Text;
                    Type = msg.Type;
                    return true;
                }
            }

            Message = null;
            Type = ELogType.Log;
            return false;
        }
    }
}
