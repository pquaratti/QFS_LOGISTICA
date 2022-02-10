using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace Negocio.App
{
    public class Setup
    {
        SQLDb db;
        string sQuery = "";

        public Setup()
        {
            db = new SQLDb();
        }

        public void Iniciar()
        {
            // CLEAN DE TABLAS DE SISTEMAS
            sQuery = "  TRUNCATE TABLE SIS_PERFILES_ACCIONES; ";
            sQuery += " TRUNCATE TABLE SIS_ACCIONES; ";
            sQuery += " TRUNCATE TABLE SIS_PERFILES; ";
            sQuery += " TRUNCATE TABLE SIS_USUARIOS_MODULOS; ";
            sQuery += " TRUNCATE TABLE SIS_MODULOS; ";
            sQuery += " TRUNCATE TABLE SIS_USUARIOS; ";

            int idMenu = 0;

            // INSERTO ACCIONES DEFAULT
            sQuery += " INSERT INTO SIS_ACCIONES (acc_nombre,acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden,acc_menu,acc_activo) ";
            sQuery += " VALUES ('Dashboard','Dashboard General','Dashboard','Home',0,'fa fa-home',1,1,1); ";
            idMenu += 1;

            // MENU DE SISTEMA
            sQuery += " INSERT INTO SIS_ACCIONES (acc_nombre,acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden,acc_menu,acc_activo) ";
            sQuery += " VALUES ('Sistema','Menú - Sistema','','',0,'fa fa-home',1,1,1); ";
            idMenu += 1;

            sQuery += " INSERT INTO SIS_ACCIONES (acc_nombre,acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden,acc_menu,acc_activo) ";
            sQuery += " VALUES ('Usuario','Submenú - Listado de usuarios del sistema','Usuario','Index',2,'sin',1,1,1); ";
            idMenu += 1;

            sQuery += " INSERT INTO SIS_ACCIONES (acc_nombre,acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden,acc_menu,acc_activo) ";
            sQuery += " VALUES ('Acciones','Submenú - Listado de acciones del sistema','Accion','Index',2,'sin',2,1,1); ";
            idMenu += 1;

            sQuery += " INSERT INTO SIS_ACCIONES (acc_nombre,acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden,acc_menu,acc_activo) ";
            sQuery += " VALUES ('Modulos','Submenú - Listado de módulos del sistema','Modulo','Index',2,'sin',2,1,1); ";
            idMenu += 1;

            sQuery += " INSERT INTO SIS_ACCIONES (acc_nombre,acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden,acc_menu,acc_activo) ";
            sQuery += " VALUES ('Editar Usuario','Acción - Edición de usuario','Usuario','AddOrEdit',2,'sin',0,0,1); ";
            idMenu += 1;

            // INSERTO EL MÓDULO
            sQuery += " INSERT INTO SIS_MODULOS (mod_id, mod_nombre,mod_descripcion,mod_activo) ";
            sQuery += " VALUES (1, 'SISTEMA', 'SISTEMA',1); ";

            // INSERT DE PERFIL DEFAULT
            sQuery += " INSERT INTO SIS_PERFILES (prf_nombre,prf_mod_id) ";
            sQuery += " VALUES ('Administrador Sistema',1); ";

            // INSERTO TODAS LAS ACCIONES AL PERFIL
            sQuery += " INSERT INTO SIS_PERFILES_ACCIONES (pac_acc_id,pac_prf_id) ";
            sQuery += " SELECT acc_id,1 FROM SIS_ACCIONES";
            
            //CREO EL USUARIO POR DEFAULT - PASSWORD pabale19
            sQuery += " INSERT INTO SIS_USUARIOS (usu_nickname, usu_password,usu_administrador) ";
            sQuery += " VALUES ('administrador','WbbwDDHSR9DPxaLqxYFZ9A==',1); ";

            // ASIGNO EL SISTEMA CON EL PERFIL PARA EL USUARIO
            sQuery += "INSERT INTO SIS_Usuarios_Modulos (usi_usu_id,usi_mod_id,usi_prf_id) ";
            sQuery += "VALUES (1,1,1); ";
            
            db.SQLExecuteNonQuery(sQuery);
        }


    }
}
