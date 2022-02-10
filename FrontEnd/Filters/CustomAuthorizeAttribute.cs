using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Filters
{

    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public bool ReturnJSON { get; set; }
        public string RedirectPage { get; set; }
        public string NombreModulo { get; set; }
        public string Accion { get; set; }
        public string Controlador { get; set; }

        public CustomAuthorizeAttribute(bool jsonFailResult = false, string redirectPage = "DENEGADO", string nombreModulo = "", string nombreAccion = "", string nombreControlador = "")
        {
            this.ReturnJSON = jsonFailResult;
            this.RedirectPage = redirectPage;
            this.NombreModulo = nombreModulo;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                bool _tienePermiso = false;

                if (Roles.Trim().Length > 0)
                    _tienePermiso = (VerificarAccion.ListarAcciones(NombreModulo).Where(w => w.acc_accion.ToUpper() == Roles.ToUpper()).Count() > 0);
                
                if (Controlador.Trim().Length > 0 && Accion.Trim().Length > 0)
                    _tienePermiso = (VerificarAccion.ListarAcciones(NombreModulo).Where(w => w.acc_accion.ToUpper() == Accion.ToUpper() && w.acc_controller.ToUpper() == Controlador.ToUpper()).Count() > 0);
                
                return _tienePermiso;
            }
            catch (Exception)
            {
                return false;
            }           
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (ReturnJSON == true)
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new JsonResult
                {
                    Data = new { Success = false, Message = "Unauthorized" },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                switch (RedirectPage.ToUpper())
                {
                    case "LOGIN":
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                        break;
                    default:
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "AccesoDenegado" } });
                        break;
                }

            }

        }

    }
}