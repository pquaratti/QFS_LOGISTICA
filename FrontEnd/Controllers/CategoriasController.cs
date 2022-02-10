using Entidades.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class CategoriasController : ControllerBaseV2
    {

        public ActionResult Colaborador()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PartialModalABMCategoriaColaborador(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Categoria item = new Entidades.Categoria();
            item = new Negocio.Categorias(GetToken()).ObjetoNuevo();
            if (cabeceraID != "0")
            {
                item = new Negocio.Categorias(GetToken()).ObtenerPorID(cabeceraID);
            }
            string _partialViewName = "_ModalABMCategoriaColaborador";
            return PartialView(_partialViewName, item);
        }

        [HttpGet]
        public ActionResult PartialGridDataCategoriaColaborador()
        {
            List<Entidades.Categoria> lst = new List<Entidades.Categoria>();
            lst = new Negocio.Categorias(GetToken()).Listar();
           
            if (lst.Count > 0)
            {
                string _partialViewName = "_GridDataCategoriaColaborador";
                return PartialView(_partialViewName, lst);
            }
            else
            {
                Entidades.App.ObjectMessage oM = new ObjectMessage();
                oM.Message = "No hay Categorias para mostrar";
                return PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", oM);
            }
        }

        [HttpPost]
        public JsonResult DeleteCategoriaColaborador(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.Categorias n = new Negocio.Categorias(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(BorrarID));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult SaveModalCategoriaColaborador(Entidades.Categoria obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            oM = new Negocio.Categorias(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

       
    }
}