using Entidades.App;
using Newtonsoft.Json;
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
    public class IndicadoresController : ControllerBaseV2
    {

        #region Gestión Indicadores del Proyecto

        [HttpGet]
        public ActionResult PartialModalABMIndicador(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.Proyecto_Indicador item = new Entidades.Proyecto_Indicador();
            item.Proyecto.IdEncriptado = cabeceraID;

            var _id = detalleID;

            if (!Resources.Repositorio.IsNumeric(detalleID))
                _id = Negocio.App.Security.DesencriptarID(detalleID);
             
            if (Convert.ToInt32(_id) > 0) 
                item = new Negocio.ProyectosIndicadores(GetToken()).ObtenerPorID(_id.ToString());
            
            string _partialViewName = "_ModalABMIndicadores";

            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalIndicador(Entidades.Proyecto_Indicador obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            obj.Proyecto.proy_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.Proyecto.IdEncriptado));

            oM = new Negocio.ProyectosIndicadores(GetToken()).Save(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(string BorrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.ProyectosIndicadores n = new Negocio.ProyectosIndicadores(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.Delete(Convert.ToInt32(Negocio.App.Security.DesencriptarID(BorrarID)));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion
 
        [HttpGet]
        public JsonResult ListarSelect(string filterID)
        {
            Entidades.App.Token oToken = GetToken();
            List<Entidades.App.DLLObject> items = new List<Entidades.App.DLLObject>();

            items = Negocio.DDL.ListarIndicadoresPorProyecto(GetToken(), true, filterID);

            return Json(items, JsonRequestBehavior.AllowGet);
        }


    }
}