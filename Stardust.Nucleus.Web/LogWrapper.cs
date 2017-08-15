using System;
using System.Diagnostics;
using Stardust.Interstellar.Rest.Common;
using Stardust.Particles;

namespace Stardust.Nucleus.Web
{
    public class LogWrapper : ILogger
    {


        public void Error(Exception error)
        {
            if (error == null) return;
            Logging.Exception(error);
        }

        public void Message(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            Logging.DebugMessage(message);
        }


        public void Message(string format, params object[] args)
        {
            Message(string.Format(format, args));
        }

        public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            if(exception.IsInstance())
                Logging.Exception(exception);
            else Logging.DebugMessage(formatter(state,exception));
            return true;
        }
    }
}