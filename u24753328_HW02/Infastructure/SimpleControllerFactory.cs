using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static u24753328_HW02.Data.IPetRespository;
using System.Web.Routing;

namespace u24753328_HW02.Infastructure
{
    

      public class SimpleControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            Type controllerType = GetControllerType(requestContext, controllerName);

            if (controllerType == null)
            {
                
                return base.CreateController(requestContext, controllerName);
            }

            if (controllerType == typeof(HomeController) ||
                controllerType == typeof(PetsController) ||
                controllerType == typeof(AdoptController) ||
                controllerType == typeof(PostPetController) ||
                controllerType == typeof(DonationsController))
            {
                IPetRepository repository = new PetRespository();

                return (IController)Activator.CreateInstance(controllerType, repository);
            }

            return (IController)Activator.CreateInstance(controllerType);
        }
    }
}