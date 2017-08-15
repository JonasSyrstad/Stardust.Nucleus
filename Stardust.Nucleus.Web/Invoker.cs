using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Stardust.Particles;

namespace Stardust.Nucleus.Web
{
    public class Invoker: ApiControllerActionInvoker
    {
        public override async Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            using (LogScope.Create($"Entering {actionContext.ActionDescriptor.ControllerDescriptor.ControllerName}.{actionContext.Request.RequestUri} handler"))
            {
                try
                {
                    HttpContext.Current.Items.Add("logContextName",actionContext.ActionDescriptor.ControllerDescriptor.ControllerName);
                    return await base.InvokeActionAsync(actionContext, cancellationToken);
                }
                catch (Exception ex)
                {
                    ex.Log();
                    throw;
                }
            }
        }
    }

    public sealed class LogScope : IDisposable
    {
        private readonly int _lineNumber;
        private readonly string _caller;
        private readonly string _filepaht;
        private readonly string _message;
        private Stopwatch _timer;

        public static LogScope Create(string message, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null, [CallerFilePath] string filepaht = null)
        {
            return new LogScope(message, lineNumber, caller, filepaht);

        }
        public static LogScope Create([CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null, [CallerFilePath] string filepaht = null)
        {
            return new LogScope(null, lineNumber, caller, filepaht);

        }
        internal LogScope(string message, int lineNumber = 0, string caller = null, string filepaht = null)
        {
            _lineNumber = lineNumber;
            _caller = caller;
            _filepaht = filepaht;
            _message = $"{message}";
            _timer = Stopwatch.StartNew();
            Logging.DebugMessage($"Beginning action {caller} at line {lineNumber} in {filepaht}");
            if (message.ContainsCharacters())
                Logging.DebugMessage(message);
        }
        public void Dispose()
        {
            _timer.Stop();
            Logging.DebugMessage($"ending  action {_caller} at line {_lineNumber} in {_filepaht}. Execution time {_timer.ElapsedMilliseconds}ms");
        }
    }
}