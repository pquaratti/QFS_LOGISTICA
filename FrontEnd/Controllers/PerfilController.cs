using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades.App;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    [Filters.VerificarModule]
    public class PerfilController : ControllerBase<Entidades.App.SIS_Perfil>
    {
        public override JsonResult Active(string id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult AddOrEdit(string id = "")
        {
            throw new NotImplementedException();
        }

        public override JsonResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Index()
        {
            throw new NotImplementedException();
        }

        public override JsonResult ListarSelect(string filterID)
        {
            int _moduloID = 0;

            if (Resources.Repositorio.IsNumeric(filterID))
                _moduloID = Convert.ToInt32(filterID);
            else
            {
                if (filterID.Length == 0)
                    _moduloID =0;
                else
                    _moduloID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(filterID));
            }
                
            Negocio.App.SIS_Perfiles negocio = new Negocio.App.SIS_Perfiles(GetToken());

            List<Entidades.App.DLLObject> items = negocio.ListarDLL(true,_moduloID);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public override JsonResult LoadTableAjax()
        {
            throw new NotImplementedException();
        }

        public override JsonResult Save(SIS_Perfil obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                Negocio.App.SIS_Perfiles negocio = new Negocio.App.SIS_Perfiles(GetToken());
                obj.prf_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.IdEncriptado));
                oM = negocio.Save(obj);
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialViewGridPerfiles(string moduloID)
        {
            Negocio.App.SIS_Perfiles negocio = new Negocio.App.SIS_Perfiles(GetToken());

            string _partialViewName = "_PartialGridData";
            List<Entidades.App.SIS_Perfil> items = negocio.ListarPorModuloID(moduloID);
            return PartialView(_partialViewName, items);
        }

        public ActionResult PartialViewModalABM(string moduloID, string perfilID)
        {
            Negocio.App.SIS_Perfiles negocio = new Negocio.App.SIS_Perfiles(GetToken());

            string _partialViewName = "_ModalFormABM";
            Entidades.App.SIS_Perfil item = new Entidades.App.SIS_Perfil();
            item.prf_id = 0;
            item.IdEncriptado = Negocio.App.Security.EncriptarID(perfilID);
            item.prf_mod_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(moduloID));
            
            return PartialView(_partialViewName, item);
        }

        public ActionResult PartialViewGridAcciones(string perfilID)
        {
            Negocio.App.SIS_Perfiles negocio = new Negocio.App.SIS_Perfiles(GetToken());

            string _partialViewName = "_ModalPartialGridDataAcciones";
            
            Entidades.App.SIS_Perfil item = new SIS_Perfil();
            item = negocio.ObtenerPorIDEncriptado(perfilID);
            item.Acciones = negocio.ListarAcciones(item.prf_id);
            
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult AgregarAccion(string perfilID, string accionID, bool changeStatus)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            if (accionID.Trim().Length == 0)
            {
                oM.Success = false;
                oM.Message = "No se pudo actualizar la acción";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }

            var _idPerfil = Negocio.App.Security.DesencriptarID(perfilID);
            var _idAccion = Negocio.App.Security.DesencriptarID(accionID);

            Negocio.App.SIS_Perfiles negocio = new Negocio.App.SIS_Perfiles(GetToken());

            if (changeStatus == true)
                oM = negocio.AsignarAccion(Convert.ToInt32(_idPerfil), Convert.ToInt32(_idAccion));
            else
                oM = negocio.QuitarAccion(_idPerfil, _idAccion);
            
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


    }
}