using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class StatisticsReportsController : ControllerBaseV2
    {

        #region Proyectos 

        [HttpGet]
        public ActionResult Proyectos_PartialGridStatus()
        {
            List<Entidades.Proyecto> lst = new List<Entidades.Proyecto>();
            lst = new Negocio.Proyectos(GetToken()).ListarEnCurso();
            string _partialViewName = "Proyectos_PartialGridStatus";
            return PartialView(_partialViewName, lst);
        }
        #endregion


    }
}