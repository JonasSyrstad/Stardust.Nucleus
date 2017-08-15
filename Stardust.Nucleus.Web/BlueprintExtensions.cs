using Stardust.Interstellar.Rest.Client;

namespace Stardust.Nucleus.Web
{
    public static class BlueprintExtensions
    {
        public static IScopeContext ToServiceProxy<T>(this IBindContext<T> bindContext, string baseUrl)
        {
            return bindContext.ToConstructor(ProxyFactory.CreateProxy<T>(), () => ProxyFactory.CreateInstance<T>(baseUrl));
        }

        public static IScopeContext ToServiceProxy<T>(this IBindContext<T> bindContext, string baseUrl, string identifier)
        {
            return bindContext.ToConstructor(ProxyFactory.CreateProxy<T>(), () => ProxyFactory.CreateInstance<T>(baseUrl), identifier);
        }

        public static IWebApiBindContext<T> BindWebApiController<T>(this IConfigurator configuration)
        {
            return new BindContext<T>(configuration.Bind<T>());
        }

        internal static bool HasRegisteredServices;
    }
}