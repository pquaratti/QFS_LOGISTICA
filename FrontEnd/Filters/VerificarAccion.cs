using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Filters
{
    public class VerificarAccion : ActionFilterAttribute
    {
        public enum AccionesSimple
        {

        }

        public string Accion { get; set; }
        public string Controlador { get; set; }
        public string NombrePermiso { get; set; }
        public bool JsonResult { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Verificar el permiso - Buscar en la session o directamente en la base de datos.


        }

        public static List<Entidades.App.SIS_Accion> ListarAcciones(string modulo)
        {
            Entidades.App.Token token = VerificarToken.ConsultarToken();

            if (token != null)
            {
                Negocio.App.SIS_Usuarios negocioUSU = new Negocio.App.SIS_Usuarios();
                
                return negocioUSU.ListarAcciones(Convert.ToInt32(token.UserID), modulo);
            }
            else
            {
                return null;
            }
        }

        //public static List<Entidades.App.SIS_Accion> ListarAcciones(string modulo)
        //{
        //    if (HttpContext.Current.Session["Acciones"] == null)
        //    {
        //        Entidades.App.Token token = VerificarToken.ConsultarToken();

        //        if (token != null)
        //        {
        //            Negocio.App.SIS_Usuarios negocioUSU = new Negocio.App.SIS_Usuarios();
        //            HttpContext.Current.Session["Acciones"] = negocioUSU.ListarAcciones(Convert.ToInt32(token.UserID), modulo);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
            
        //    return HttpContext.Current.Session["Acciones"] as List<Entidades.App.SIS_Accion>;
        //}

        public static bool TienePermiso(string pController, string pAccion, string modulo)
        {
            List<Entidades.App.SIS_Accion> lstAcciones = ListarAcciones(modulo);

            if (lstAcciones.Count > 0)
            {
                Entidades.App.SIS_Accion accion = lstAcciones.Where(w => w.acc_controller.ToLower() == pController.ToLower() && w.acc_accion.ToLower() == pAccion.ToLower()).FirstOrDefault();

                if (accion != null)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }

        }

    }
}