using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Stardust.Interstellar.Rest.Annotations;
using Stardust.Nucleus;
using Stardust.Nucleus.Web;
using Stardust.Particles;

namespace Stardust.WebTest.App_Start
{
    [IocConfiguration]
    public class TestBlueprint:BlueprintBase
    {
        protected override void ConfigureIoc(IConfigurator configuration)
        {
            Logging.DebugMessage("test");       
            configuration.BindWebApiController<ITest>().To<TestService>().SetTransientScope();
        }
    }

    public class TestService : ITest
    {
        public string Test(string msg)
        {
            return $"Message:{msg}"; 
        }
    }

    [IRoutePrefix("api")]
    public interface ITest
    {
        [HttpGet, Route("test/{msg}")]
        string Test([In(InclutionTypes.Path)]string msg);
    }
}