using Hospitales.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Filters
{
    public class VerificarModule : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            Entidades.App.Token oToken = Filters.VerificarToken.ConsultarToken();

            if (oToken.ModuloID == null)
                oToken.ModuloID = "0";

            if (oToken.ModuloID.Equals("0"))
                filterContext.Result = new RedirectResult("~/Home/Modules");

            if (Filters.VerificarAccion.ListarAcciones(oToken.ModuloID).Count.Equals(0))
                filterContext.Result = new RedirectResult("~/Home/Modules");

        }

    }
}