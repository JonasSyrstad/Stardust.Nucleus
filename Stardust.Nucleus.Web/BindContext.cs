using System;
using Stardust.Interstellar.Rest.Service;

namespace Stardust.Nucleus.Web
{
    internal class BindContext<T> : IWebApiBindContext<T>
    {

        private readonly IBindContext<T> _bind;

        public BindContext(IBindContext<T> bind)
        {
            _bind = bind;
        }

        public IScopeContext To<TService>() where TService : T
        {
            ServiceFactory.CreateServiceImplementation<T>();
            BlueprintExtensions.HasRegisteredServices = true;
            return _bind.To<TService>();
        }

        public IScopeContext ToConstructor(Func<object> creator)
        {
            ServiceFactory.CreateServiceImplementation<T>();
            BlueprintExtensions.HasRegisteredServices = true;
            return _bind.ToConstructor(creator);
        }
    }
}