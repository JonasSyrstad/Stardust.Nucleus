using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Stardust.Interstellar.Rest.Common;

namespace Stardust.Nucleus.Web
{
    public class CustomResolver : IServiceParameterResolver, IWebMethodConverter
    {
        public IEnumerable<ParameterWrapper> ResolveParameters(MethodInfo methodInfo)
        {
            return null;
        }

        public List<HttpMethod> GetHttpMethods(MethodInfo method)
        {
            return null;
        }
    }
}