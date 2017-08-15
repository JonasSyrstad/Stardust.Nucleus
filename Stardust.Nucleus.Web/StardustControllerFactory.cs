using System.Web.Mvc;
using System.Web.Routing;

namespace Stardust.Nucleus.Web
{
    internal sealed class StardustControllerFactory : DefaultControllerFactory
    {
        [Using]
        internal StardustControllerFactory(IControllerActivator activator)
            : base(activator)
        {

        }

        public override IController CreateController(RequestContext context, string controllerName)
        {

            try
            {
                var controller = base.CreateController(context, controllerName);
                return controller;
            }
            catch (System.Exception ex)
            {


                return null;
            }

        }
    }
}