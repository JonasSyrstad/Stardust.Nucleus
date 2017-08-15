using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using Stardust.Core;
using Stardust.Interstellar.Rest.Common;
using Stardust.Interstellar.Rest.Service;
using Stardust.Particles;

namespace Stardust.Nucleus.Web
{
    public abstract class BlueprintBase : BlueprintBase<Logger>
    {
        
    }

    public abstract class BlueprintBase<TLogger>:IBlueprint where TLogger: ILogging
    {
        public void Bind(IConfigurator configuration)
        {
            var coreBlueprint = new CoreFrameworkBlueprint<TLogger>();
            coreBlueprint.Bind(configuration);
            BindWebActivator(configuration);
            ConfigureIoc(configuration);
            if(BlueprintExtensions.HasRegisteredServices)
                ServiceFactory.FinalizeRegistration();
        }

        protected abstract void ConfigureIoc(IConfigurator configuration);

        private void BindWebActivator(IConfigurator configuration)
        {
            configuration.UnBind<IHttpActionInvoker>().AllAndBind().To<Invoker>().SetTransientScope();
            configuration.Bind<IControllerFactory>().To<StardustControllerFactory>().SetSingletonScope();
            configuration.Bind<IHttpControllerActivator>().To<StardustApiControllerActivator>().SetSingletonScope();
            configuration.Bind<IServiceParameterResolver>().To<CustomResolver>().SetSingletonScope();
            GlobalConfiguration.Configuration.DependencyResolver = new StardustDependencyResolver();
            ControllerBuilder.Current.SetControllerFactory(Resolver.Activate<IControllerFactory>());
        }

        public Type LoggingType => typeof(TLogger);
    }
}
