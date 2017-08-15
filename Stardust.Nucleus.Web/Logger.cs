using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Stardust.Continuum.Client;
using Stardust.Interstellar.Rest.Client;
using Stardust.Particles;

namespace Stardust.Nucleus.Web
{
    public class Logger : ILogging
    {
        static Logger()
        {
            ProxyFactory.CreateInstance<ILogStream>(ConfigurationManagerHelper.GetValueOnKey("continuum.baseUrl", ""));
            ContinuumClient.SetApiKey(ConfigurationManagerHelper.GetValueOnKey("continuum.apikey", ""));
            ContinuumClient.BaseUrl = ConfigurationManagerHelper.GetValueOnKey("continuum.baseUrl", "");
            ContinuumClient.Project = ConfigurationManagerHelper.GetValueOnKey("continuum.project", "");
            ContinuumClient.LimitMessageSize = 3000;
            ContinuumClient.Environment = ConfigurationManagerHelper.GetValueOnKey("service.environment", "dev");
            ProxyFactory.EnableExpectContinue100ForAll = false;
            ProxyFactory.EnableExpectContinue100ForPost = false;
        }

        public void Exception(Exception exception, string additionalDebugInformation = null)
        {
            ContinuumClient.AddStream(new StreamItem
            {
                LogLevel = LogLevels.Error,
                CorrelationToken = GetSupportCode(),
                Message = $"{exception.Message}{(additionalDebugInformation.ContainsCharacters() ? $" ({additionalDebugInformation})" : "")}",
                StackTrace = exception.StackTrace,
                UserName = UserId,
                Timestamp = DateTime.UtcNow,
                ServiceName = ConfigurationManagerHelper.GetValueOnKey("continuum.ServiceName"),
                Properties = new Dictionary<string, object>
                {
                    {"SourceAddress", LocationName},
                    {"MachineName", Environment.MachineName},
                    {"Version", ServiceVersion},
                    {"logContextName",GetLogContext() }
                }
            }, false);
            if (exception.InnerException != null) Exception(exception.InnerException, "innerException");
        }

        private string GetSupportCode()
        {
            return HttpContext.Current?.Items["x-supportCode"]?.ToString();
        }

        protected string UserId
        {
            get
            {
                return (HttpContext.Current?.User?.Identity as ClaimsIdentity)
                    ?.Claims.SingleOrDefault(s => string.Equals(s.Type, ClaimTypes.Upn, StringComparison.InvariantCultureIgnoreCase))?.Value;
            }
        }
        private static string LocationName { get; } = Environment.GetEnvironmentVariable("REGION_NAME")?.Replace(" ", "_").ToLower() ?? "we";
        public string ServiceVersion { get; } = ConfigurationManagerHelper.GetValueOnKey("app:Version");

        public void HeartBeat()
        {
            DebugMessage("I'm stil alive.", LogType.Information, "there is no cake!");
        }

        public void DebugMessage(string message, LogType entryType = LogType.Information, string additionalDebugInformation = null)
        {
            if (message.StartsWith("Type scanning initiated. Looking for implementations of")) return;
            ContinuumClient.AddStream(new StreamItem
            {
                LogLevel = LogLevels.Debug,
                CorrelationToken = GetSupportCode(),
                Message = $"{message}{(additionalDebugInformation.ContainsCharacters() ? $" ({additionalDebugInformation})" : "")}",
                UserName = UserId,
                Timestamp = DateTime.UtcNow,
                ServiceName = ConfigurationManagerHelper.GetValueOnKey("continuum.ServiceName"),
                Properties = new Dictionary<string, object>
                {
                    { "SourceAddress", LocationName },
                    { "MachineName", Environment.MachineName } ,
                    {"Version",ServiceVersion },
                    {"logContextName",GetLogContext() }

                }
            }, false);
        }

        private static string GetLogContext()
        {
            if (HttpContext.Current != null && HttpContext.Current.Items.Contains("logContextName"))
                return HttpContext.Current.Items["logContextName"]?.ToString();
            return "system";
        }

        public void SetCommonProperties(string logName)
        {
        }
    }
}