using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades.App;


namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    public class LocalidadesController : ControllerBaseV2
    {
        // GET: Dependencia


        [HttpGet]
        public JsonResult ListarSelect(string filterID)
        {
            Entidades.App.Token oToken = GetToken();
            List<Entidades.App.DLLObject> items = new List<Entidades.App.DLLObject>();

            items = Negocio.DDL.ListarLocalidadesPorProvincias(GetToken(), true, Convert.ToInt32(filterID));

            return Json(items, JsonRequestBehavior.AllowGet);
        }

    }
}