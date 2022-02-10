using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class ReportesController : ControllerBaseV2
    {
        //// GET: Reportes
        //public ActionResult PlanificacionIndividual(string id)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReportePlanificacionIndividual(id);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult ListadoLegajos(string parameters)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReporteListadoLegajos(parameters);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult PlanificacionGerencial(string parameters)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReportePlanificacionInformeGerencial(parameters);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult CombustibleTarjetas(string parameters)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReporteListadoTarjetas();
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult PlanillaViaticos(string id)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReportePlanillaViaticos(id);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult SolicitudPagoViaticos(string id, string idaux)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReporteSolicitudPagoViaticos(id, idaux);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult InformeAsignacionPresupuestaria(string id)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReporteAsignacionPresupuestaria(id);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult InformeAsignacionCredito(string id)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReporteAsignacionCredito(id);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //public ActionResult OrdenDeCompraIndividual(string id)
        //{
        //    Negocio.Reportes negocio = new Negocio.Reportes(GetToken());
        //    byte[] bytes = negocio.ReporteOrdenDeCompraIndividual(id);
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
        //    string mimeType = string.Empty;
        //    return new FileStreamResult(ms, "application/pdf");
        //}

        //#region Modals

        //[HttpGet]
        //public ActionResult PartialModalLegajos(string cabeceraID, string detalleID, string subdetalleID)
        //{
        //    string _partialViewName = "_ModalReporteLegajos";

        //    Entidades.App.Token token = GetToken();

        //    if (token.Administrador)
        //    {
        //        ViewBag.DistritosDDL = Negocio.DDL.ListarDistritosAdmin(token);
        //        ViewBag.SubdistritosDDL = Negocio.DDL.ListarSubDistritosAdmin(Filters.VerificarToken.ConsultarToken(), 0);
        //    }
        //    else
        //    {
        //        ViewBag.DistritosDDL = Negocio.DDL.ListarDistritos(token,false,true);
        //        ViewBag.SubdistritosDDL = Negocio.DDL.ListarSubDistritosV2(Filters.VerificarToken.ConsultarToken(), Convert.ToInt32(token.DistritoID));
        //    }

        //    return PartialView(_partialViewName);
        //}


        //#endregion

        //#region Helper


        //[HttpPost]
        //public JsonResult ParameterConfig(Entidades.App.ReportFilters filtros)
        //{
        //    Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

        //    filtros.customSeedKey = "";

        //    string paramJSON = Resources.Repositorio.JSONSerialize(filtros);
        //    string paramJSONEncrypt = Negocio.App.Security.EncriptarBasico(paramJSON);

        //    oM.Success = true;
        //    oM.ObjectRelation = paramJSONEncrypt;

        //    return Json(oM, JsonRequestBehavior.AllowGet);
        //}


        //#endregion

    }
}