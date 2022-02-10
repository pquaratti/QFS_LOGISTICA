using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades.App;

namespace FrontEnd.Controllers
{
    [Filters.VerificarToken]
    public class UsuarioController : ControllerBase<Entidades.App.SIS_Usuario>
    {
        public override JsonResult Active(string id)
        {
            throw new NotImplementedException();
        }

        [Filters.VerificarModule]
        public override ActionResult AddOrEdit(string id = "")
        {
            Entidades.App.SIS_Usuario obj;

            if (id.Length == 0)
            {
                obj = new SIS_Usuario();
                obj.IdEncriptado = Negocio.App.Security.EncriptarID("0");
            }
            else
            {
                Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(GetToken());
                obj = negocio.ObtenerPorIDEncriptado(id);
            }

            ViewBag.Organizaciones = Negocio.DDL.ListarOrganizaciones(GetToken(), true);

            return View(obj);
        }

        public override JsonResult Delete(string id)
        {
            id = Negocio.App.Security.DesencriptarID(id);
            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(GetToken());

            Entidades.App.ObjectMessage oM = negocio.DeleteLogico(id);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [Filters.VerificarModule]
        public override ActionResult Index()
        {
            return View();
        }

        public override JsonResult ListarSelect(string filterID)
        {
            throw new NotImplementedException();
        }

        public override JsonResult LoadTableAjax()
        {
            Entidades.App.DatatableJS dtAjax = GetListTableAjax<Entidades.App.SIS_Usuario>(new Negocio.App.SIS_Usuarios(GetToken()));
            return Json(new { draw = dtAjax.Draw, recordsFiltered = dtAjax.totalRecords, recordsTotal = dtAjax.totalRecords, data = dtAjax.dataReturn }, JsonRequestBehavior.AllowGet);
        }

        public override JsonResult Save(SIS_Usuario obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            var esNuevo = false;

            try
            {
                obj.usu_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(obj.IdEncriptado));

                if (obj.usu_id.Equals(0))
                {
                    esNuevo = true;
                }

                Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(GetToken());
                oM = negocio.Save(obj);

                if (oM.Success)
                {
                    obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.usu_id.ToString());

                    oM.RedirectNew = esNuevo;
                    oM.urlRedirect = Url.Action("AddOrEdit", "Usuario", new { id = obj.IdEncriptado });
                }
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AgregarPerfil(string usuario, string perfil)
        {
            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios();

            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            if (perfil.Trim().Length == 0)
            {
                oM.Success = false;
                oM.Message = "No se pudo actualizar el perfil";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }

            int _usuarioID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(usuario));
            int _perfilID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(perfil));

            oM = negocio.AsignarPerfil(_usuarioID, _perfilID);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult QuitarPerfil(string usuario, string perfil)
        {
            Entidades.App.ObjectMessage oM;

            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios();

            int _usuarioID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(usuario));
            int _perfilID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(perfil));

            oM = negocio.QuitarPerfil(_usuarioID, _perfilID);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PVModulos(string id)
        {
            int _usuID = Convert.ToInt32(Negocio.App.Security.DesencriptarID(id));
            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios();
            string _partialViewName = "_PerfilesAsignados";
            List<Entidades.App.SIS_Modulo> items = negocio.ListarModulos(_usuID);
            return PartialView(_partialViewName, items);
        }

        [HttpPost]
        public JsonResult ActualizarDatosPerfil(Entidades.App.SIS_Usuario unUsuario)
        {
            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios();

            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            
            unUsuario.usu_id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(unUsuario.IdEncriptado));

            oM = negocio.ActualizarPerfil(unUsuario);

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PartialModalChangePassword(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.SIS_Usuario item = new Entidades.App.SIS_Usuario();
            item.usu_id = Convert.ToInt32(cabeceraID);

            string _partialViewName = "_ModalChangePassword";
            return PartialView(_partialViewName, item);
        }

        [HttpPost]
        public JsonResult SaveModalChangePassword(Entidades.App.SIS_Usuario obj)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            // Cambio el password
            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(GetToken());

            oM = negocio.ChangePassword(obj);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ChangePassword(string passwordOld, string passwordNew, string passwordNew2)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            // Valido que ponga una contraseña anterior
            if (passwordOld.Trim().Length == 0)
            {
                oM.Success = false;
                oM.Message = "Debe ingresar una contraseña!";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }

            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(GetToken());
            Entidades.App.SIS_Usuario usuario = negocio.ObtenerPorID(GetToken().UserID);

            // Valido que la contraseña anterior sea igual a la ingresada
            if (passwordOld != Negocio.App.Security.Desencriptar(usuario.usu_password))
            {
                oM.Success = false;
                oM.Message = "La actual contraseña ingresada no coincide con la vigente. Vuelva a ingresar la contraseña actual";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }

            // Valido que la contraseña nueva sea igual a la validación
            if (passwordNew != passwordNew2)
            {
                oM.Success = false;
                oM.Message = "La contraseña nueva no coincide con su verificación!";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }

            // Cambio el password
            usuario.usu_password = passwordNew;

            oM = negocio.ChangePassword(usuario);
            
            oM.Success = true;

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DesbloquearUsuario(string accionID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.App.SIS_Usuarios(GetToken()).DesbloquearUsuario(accionID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BloquearUsuario(string accionID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.App.SIS_Usuarios(GetToken()).BloquearUsuario(accionID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReiniciarIntentosFallidos(string accionID)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();
            oM = new Negocio.App.SIS_Usuarios(GetToken()).ReiniciarIntentosFallidos(accionID);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public override JsonResult GetSelect2Items(string q)
        {
            var list = new List<Entidades.App.Select2Item>();
            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(GetToken());

            foreach (var item in negocio.ListarPorTexto(q, "30"))
            {
                list.Add(new Select2Item(item.usu_id.ToString(), item.usu_nombre + " " + item.usu_apellido + " (" + item.usu_documento + ") "));
            };

            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteSession(string id)
        {
            var _id = Convert.ToInt32(Negocio.App.Security.DesencriptarID(id));
            Negocio.App.SIS_Usuarios_Login negocio = new Negocio.App.SIS_Usuarios_Login(GetToken());

            Entidades.App.ObjectMessage oM = negocio.Delete(_id);
            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #region PermisosEspeciales 

        [HttpGet]
        public ActionResult PartialModalPermisosEspeciales(string cabeceraID, string detalleID, string subdetalleID)
        {
            Entidades.App.Token oToken = GetToken();

            Negocio.App.SIS_Usuarios negocioUSU = new Negocio.App.SIS_Usuarios(oToken);
            Negocio.App.SIS_Permisos_Especiales negocioPEE = new Negocio.App.SIS_Permisos_Especiales(oToken);

            Entidades.App.SIS_Usuario usu = negocioUSU.ObtenerPorID(cabeceraID);

            ViewBag.Usuario = usu;
            ViewBag.PermisosEspeciales = negocioPEE.ListarSimple();
            ViewBag.PermisosEspecialesAsignados = negocioUSU.ListarPermisosEspeciales(usu.usu_id);

            string _partialViewName = "_ModalPartialGridDataPermisosEspeciales";
            return PartialView(_partialViewName);
        }

        [HttpPost]
        public JsonResult AgregaPermisoEspecial(string usuarioID, string permisoEspecialID, bool changeStatus)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            if (permisoEspecialID.Trim().Length == 0)
            {
                oM.Success = false;
                oM.Message = "No se pudo actualizar la acción";
                return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
            }

            var _idPermisoEspecial = Negocio.App.Security.DesencriptarID(permisoEspecialID);
            var _usuarioID = Negocio.App.Security.DesencriptarID(usuarioID);

            Negocio.App.SIS_Usuarios negocio = new Negocio.App.SIS_Usuarios(GetToken());

            if (changeStatus == true)
                oM = negocio.AsignarPermisoEspecial(Convert.ToInt32(_usuarioID), Convert.ToInt32(_idPermisoEspecial));
            else
                oM = negocio.QuitarPermisoEspecial(Convert.ToInt32(_usuarioID), Convert.ToInt32(_idPermisoEspecial));

            return Json(new { Result = oM }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}