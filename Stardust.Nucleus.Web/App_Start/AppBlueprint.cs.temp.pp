namespace $rootnamespace$
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Web.Http;
	using Stardust.Interstellar.Rest.Annotations;
	using Stardust.Interstellar.Rest.Client;
	using Stardust.Nucleus;
	using Stardust.Nucleus.Web;
	using Stardust.Particles;

	[IocConfiguration]//Remove this and call Resolver.LoadModuleConfiguration<AppBlueprint>(); from Global.asax Application_Start if you experience problems with early loading of the ioc bindings
	public partial class AppBlueprint:BlueprintBase
	{
		partial void LoadConfiguration(IConfigurator configuration)
		{

			//Examples:
			//Configures the IOC Container to use SomeServiceImplementation for ISomeService
		    //configuration.Bind<ISomeService>().To<SomeServiceImplementation>().SetSingletonScope();

		    //Uses the implementation as 
		    //configuration.Bind<SomeServiceImplementation>().ToSelf().SetRequestResponseScope();

            //Creates a webapi controller for ISomeService with the implementation SomeServiceImplementation
            //configuration.BindWebApiController<ISomeService>().To<SomeServiceImplementation>().SetRequestResponseScope();


            //Creates a webApi controller that proxies another service with the same interface/signature
            //configuration.BindWebApiController<ISomeService>().ToConstructor(() => ProxyFactory.CreateInstance<ISomeService>(ConfigurationManagerHelper.GetValueOnKey("someServiceLocation"))).SetRequestResponseScope();

		}

	}


	//WebApi Sample using Stardust.Interstellar.Rest

	//Design your api with respect of actions, paths, headers etc
	//[IRoutePrefix("api/sample")]
    //public interface ISomeService
    //{
    //    [HttpGet, Route("echo")]//curl -X GET 'http://localhost:54557/api/echo?message=test%20message'
    //    string Echo([In(InclutionTypes.Path)] string message);
    //}

	//Implement the api with no references to http or other transport mechanisms
	//internal class SomeServiceImplementation : ISomeService
    //{
    //    public string Echo(string message)
    //    {
    //        return message;
    //    }
    //}
}
