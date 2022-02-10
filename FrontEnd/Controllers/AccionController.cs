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
    public class AccionController : ControllerBase<Entidades.App.SIS_Accion>
    {
        public override JsonResult Active(string id)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.App.SIS_Acciones n = new Negocio.App.SIS_Acciones(GetToken());
            string usu = Filters.VerificarToken.ConsultarToken().UserID;
            oM = n.RecuperarLogico(Convert.ToInt32(usu), Convert.ToInt32(id));
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet); 
        }

        [HttpGet]
        public override ActionResult AddOrEdit(string id = "")
        {
            Negocio.App.SIS_Acciones   negocio = new Negocio.App.SIS_Acciones(GetToken());
            Entidades.App.SIS_Accion obj;

            if (id == "")
            {
                obj = new Entidades.App.SIS_Accion();
            }
            else
            {
                obj = negocio.ObtenerPorID(id);
            }

            return View(obj);
        }
        
        public override ActionResult Index()
        {
            return View();
        }

        public override JsonResult ListarSelect(string filterID)
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        public override JsonResult Delete(string borrarID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            Negocio.App.SIS_Acciones  n = new Negocio.App.SIS_Acciones(GetToken());
            oM = n.DeleteLogico(borrarID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }
       
        public override JsonResult LoadTableAjax()
        {
            Entidades.App.DatatableJS dtAjax = GetListTableAjax<Entidades.App.SIS_Accion>(new Negocio.App.SIS_Acciones(GetToken()));
            return Json(new { draw = dtAjax.Draw, recordsFiltered = dtAjax.totalRecords, recordsTotal = dtAjax.totalRecords, data = dtAjax.dataReturn }, JsonRequestBehavior.AllowGet);
        }

        public override JsonResult Save(SIS_Accion obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                Negocio.App.SIS_Acciones negocio = new Negocio.App.SIS_Acciones(GetToken());
                obj.acc_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.IdEncriptado));
                oM = negocio.Save(obj);
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialViewModalABM(int id)
        {
            Negocio.App.SIS_Acciones negocio = new Negocio.App.SIS_Acciones(GetToken());

            Entidades.App.SIS_Accion item;

            if (id == 0)
            {
                item = new Entidades.App.SIS_Accion();
                item.acc_id = 0;
                item.IdEncriptado = Negocio.App.Security.EncriptarID(item.acc_id.ToString());
            }
            else
            {
                item = negocio.ObtenerPorID(id.ToString());
            }

            return PartialView("_ModalFormABM", item);
        }
    }
}