using Entidades.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.CustomAttributes
{
    public class ViewBagData : ActionFilterAttribute
    {
        public string Incluir { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            List<Entidades.App.SIS_Area> lstAreas = new Negocio.App.SIS_Areas(Filters.VerificarToken.ConsultarToken()).ListarSimple();
            lstAreas.Add(new SIS_Area() { area_id = 0, IdEncriptado = Negocio.App.Security.EncriptarID("0"), descripcion_combo = "Seleccione" });
            filterContext.Controller.ViewBag.Areas = lstAreas;
        }

    }
}