using System;

namespace Stardust.Nucleus.Web
{
    public interface IWebApiBindContext<in T>
    {
        IScopeContext To<TService>() where TService : T;
        IScopeContext ToConstructor(Func<object> creator);
    }
}