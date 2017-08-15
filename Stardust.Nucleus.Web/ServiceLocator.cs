using System.Collections.Generic;
using Stardust.Interstellar.Rest.Extensions;

namespace Stardust.Nucleus.Web
{
    internal class ServiceLocator : IServiceLocator
    {
        public T GetService<T>()
        {
            return Resolver.Activate<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            return Resolver.GetAllInstances<T>();
        }
    }
}