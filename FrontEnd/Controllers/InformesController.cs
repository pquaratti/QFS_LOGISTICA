using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    public class InformesController : Controller
    {
        // GET: Informes
        public ActionResult EjecucionGeneral()
        {
            return View();
        }
        public ActionResult EjecucionCombustible()
        {
            return View();
        }
        public ActionResult EjecucionViaticos()
        {
            return View();
        }
        public ActionResult EjecucionCompras()
        {
            return View();
        }
        public ActionResult EjecucionCajaChica()
        {
            return View();
        }


    }
}