using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Weblitz.MessageBoard.Web.Controllers.Factories
{
    public class ControllerFactory : DefaultControllerFactory
    {
        public static Func<Type, object> CreateDependency = (type) => Activator.CreateInstance(type);

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;

            return CreateDependency(controllerType) as IController;
        }
    }
}