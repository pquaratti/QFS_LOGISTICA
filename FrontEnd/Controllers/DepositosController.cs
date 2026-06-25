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
    public class DepositosController : ControllerBaseV2
    {

        #region Gestión de Depositos

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MapaWmsDemo()
        {
            return View();
        }

        public ActionResult EditorPasillos(string depositoID)
        {
            Entidades.Deposito deposito = new Negocio.Depositos(GetToken()).ObtenerPorID(depositoID);
            return View(deposito);
        }

        [HttpGet]
        public JsonResult ListarPasillos(string depositoID)
        {
            List<Entidades.DepositoPasillo> lst = new Negocio.DepositosPasillos(GetToken()).ListarPorDeposito(depositoID);
            var data = lst.Select(x => new
            {
                id = x.IdEncriptado,
                dbId = x.depopas_id,
                codigo = x.depopas_codigo,
                nombre = x.depopas_nombre,
                descripcion = x.depopas_descripcion,
                x = x.depopas_x,
                y = x.depopas_y,
                largo = x.depopas_largo,
                ancho = x.depopas_ancho,
                orientacion = x.depopas_orientacion,
                posiciones = x.depopas_cantidad_posiciones,
                alturas = x.depopas_cantidad_alturas,
                alturaNivel = x.depopas_altura_nivel,
                pesoMaximo = x.depopas_peso_maximo
            });

            return Json(new { Result = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePasillo(Entidades.DepositoPasillo obj, bool generarUbicaciones = true)
        {
            ObjectMessage oM = generarUbicaciones ?
                new Negocio.DepositosPasillos(GetToken()).SaveYGenerarUbicaciones(obj) :
                new Negocio.DepositosPasillos(GetToken()).Save(obj);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePasillo(string BorrarID)
        {
            ObjectMessage oM = new Negocio.DepositosPasillos(GetToken()).DeleteLogicoConUbicaciones(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialGridDataDepositos(string plantaID)
        {
            List<Entidades.Deposito> lst = new Negocio.Depositos(GetToken()).ListarDepositosPorPlanta(plantaID);

            return lst.Count > 0 ?
                PartialView("_GridDataDepositos", lst) :
                PartialView("~/Views/Shared/_MensajeSinResultados.cshtml", new ObjectMessage() { Message = "No Hay Depositos para mostrar." });
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            ObjectMessage oM = new Negocio.Depositos(GetToken()).DeleteLogico(BorrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PartialModalABMDepositos(string cabeceraID, string detalleID, string subdetalleID)
        {
            return PartialView("_ModalABMDepositos", new Negocio.Depositos(GetToken()).ObtenerPorID(cabeceraID));
        }

        [HttpPost]
        public JsonResult SaveModalDepositos(Entidades.Deposito obj)
        {
            ObjectMessage oM = new Negocio.Depositos(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}