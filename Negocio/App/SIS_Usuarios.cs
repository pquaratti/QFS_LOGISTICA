using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.App;

namespace Negocio.App
{
    public class SIS_Usuarios : NegocioBase<Entidades.App.SIS_Usuario>
    {
        
        public SIS_Usuarios(Entidades.App.Token tokenSesion = null) : base("usu_id")
        {
            this.Token = tokenSesion;
        }

        public override Entidades.App.SIS_Usuario MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Entidades.App.SIS_Usuario MapearStatic(DataRow dr)
        {
            Entidades.App.SIS_Usuario obj = new SIS_Usuario();
            obj.usu_id = Resources.Validaciones.valNULLINT(dr["usu_id"]);
            obj.usu_nickname = Resources.Validaciones.valNULLString(dr["usu_nickname"]);
            obj.usu_password = Resources.Validaciones.valNULLString(dr["usu_password"]);
            obj.usu_administrador = Resources.Validaciones.valNULLBool(dr["usu_administrador"]);
            obj.usu_fec_bloqueado = Resources.Validaciones.valNULLDateTime(dr["usu_fec_bloqueado"]);
            obj.usu_fec_eliminado = Resources.Validaciones.valNULLDateTime(dr["usu_fec_eliminado"]);
            obj.usu_fec_pass_changed = Resources.Validaciones.valNULLDateTime(dr["usu_fec_pass_changed"]);
            obj.usu_nombre = Resources.Validaciones.valNULLString(dr["usu_nombre"]);
            obj.usu_apellido = Resources.Validaciones.valNULLString(dr["usu_apellido"]);
            obj.usu_documento = Resources.Validaciones.valNULLString(dr["usu_documento"]);
            obj.usu_mail = Resources.Validaciones.valNULLString(dr["usu_mail"]);

            if (dr["usu_fec_eliminado"] != DBNull.Value)
            {
                obj.Eliminado = true;
                obj.usu_fec_eliminado = Convert.ToDateTime(dr["usu_fec_eliminado"]);
            }
            else
                obj.Eliminado = false;

            if (dr["usu_fec_bloqueado"] != DBNull.Value)
            {
                obj.Bloqueado = true;
                obj.usu_fec_eliminado = Convert.ToDateTime(dr["usu_fec_bloqueado"]);
            }
            else
                obj.Bloqueado = false;

            obj.usu_intentos = Resources.Validaciones.valNULLINT(dr["usu_intentos"]);

            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.usu_id.ToString());

            if (dr["usu_terminos_y_condiciones"] != DBNull.Value)
            {
                obj.AceptaTerminosYCondiciones = true;
                obj.usu_terminos_y_condiciones = Convert.ToDateTime(dr["usu_terminos_y_condiciones"]);
            }
            else
                obj.AceptaTerminosYCondiciones = false;

            obj.Organizacion = new SIS_Organizacion() { org_id = Resources.Validaciones.valNULLINT(dr["usu_org_id"]) };
            obj.Categoria = new Entidades.Categoria() { cat_id = Resources.Validaciones.valNULLINT(dr["usu_cat_id"]) };
            obj.Area = new Entidades.App.SIS_Area() { area_id = Resources.Validaciones.valNULLINT(dr["usu_area_id"]) };
            obj.usu_legajo = Resources.Validaciones.valNULLString(dr["usu_legajo"]);

            return obj;
        }




        public override Entidades.App.SIS_Usuario Mapear(DataRow dr)
        {
            Entidades.App.SIS_Usuario obj = MapearSimple(dr);
            obj.Categoria = Negocio.Categorias.MapearStatic(dr);
            obj.Organizacion = Negocio.App.SIS_Organizaciones.MapearStatic(dr);
            obj.Area = Negocio.App.SIS_Areas.MapearStatic(dr);
            return obj;
        }

        public override Entidades.App.SIS_Usuario MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Usuario obj = Mapear(dr);

            // Propiedades específicas

            return obj;
        }

        public Entidades.App.ObjectMessage ActualizarPerfil(Entidades.App.SIS_Usuario Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("SIS_Usuarios");
                //  row["usu_nombre"] = Obj.usu_nombre;
                //row["usu_apellido"] = Obj.usu_apellido;
                row["usu_mail"] = Obj.usu_mail;

                db.SQLUpdate(row, "usu_id=@id", "usu_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("id",Obj.usu_id)
                });

                oM.Message = "";
                oM.Success = true;
            }
            catch (Exception ex)
            {

                oM.Message = ex.Message;
                oM.Success = false;
            }

            return oM;
        }

        public override Entidades.App.ObjectMessage Save(Entidades.App.SIS_Usuario Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("SIS_Usuarios");
                row["usu_id"] = Obj.usu_id;
                row["usu_nickname"] = Obj.usu_nickname;
                row["usu_administrador"] = Obj.usu_administrador;
                row["usu_nombre"] = Obj.usu_nombre;
                row["usu_apellido"] = Obj.usu_apellido;
                row["usu_documento"] = Obj.usu_documento;
                row["usu_mail"] = Obj.usu_mail;
                row["usu_org_id"] = Obj.Organizacion.org_id;
                row["usu_cat_id"] = Obj.Categoria.cat_id;

                if (Obj.usu_id.Equals(0))
                {
                    if (!string.IsNullOrEmpty(Obj.usu_password))
                    {
                        row["usu_password"] = App.Security.Encriptar(Obj.usu_password);
                    }
                    else
                    {
                        row["usu_password"] = App.Security.Encriptar(App.Security.DefaultPassword());
                    }

                    Obj.usu_id = db.SQLInsert(row, "usu_id").Valor;

                }
                else
                {
                    db.SQLUpdate(row, "usu_id=@id", "usu_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("id",Obj.usu_id)
                });
                }

                oM.Success = true;
                oM.Message = "OK";
            }
            catch (Exception ex)
            {

                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public Entidades.App.ObjectMessage ChangePassword(Entidades.App.SIS_Usuario Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_password=@usu_password WHERE usu_id=@usu_id", new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_password", Negocio.App.Security.Encriptar(Obj.usu_password)),
                new System.Data.SqlClient.SqlParameter("usu_id",Obj.usu_id)
            });

            oM.Success = true;
            oM.Message = "OK";

            return oM;
        }


        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM SIS_Usuarios ";
            sQuery += " LEFT JOIN SIS_Organizaciones on org_id=usu_org_id ";
            sQuery += " LEFT JOIN Categorias on cat_id=usu_cat_id ";
            sQuery += " LEFT JOIN SIS_Areas on area_id=usu_area_id ";
            
            if ((sWHERE != ""))
            {
                sQuery += " WHERE " + sWHERE;
            }

            if ((sOrderBy != ""))
            {
                sQuery += " ORDER BY " + sOrderBy;
            }
            return sQuery;
        }


        #region Funciones de Negocio

        public Entidades.App.SIS_Usuario ObtenerUsuarioPorNickname(string nickname)
        {
            Entidades.App.SIS_Usuario obj;

            sQuery = QueryDefault("", "usu_nickname=@nickname", "");

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("nickname", nickname)
            });

            if (dt.Rows.Count > 0)
            {
                obj = new Entidades.App.SIS_Usuario();
                obj.usu_id = Resources.Validaciones.valNULLINT(dt.Rows[0]["usu_id"]);
                obj.usu_nickname = Resources.Validaciones.valNULLString(dt.Rows[0]["usu_nickname"]);
                obj.usu_intentos = Resources.Validaciones.valNULLINT(dt.Rows[0]["usu_intentos"]);
                obj.usu_password = Resources.Validaciones.valNULLString(dt.Rows[0]["usu_password"]);
              
                obj.usu_administrador = Resources.Validaciones.valNULLBool(dt.Rows[0]["usu_administrador"]);

                if (dt.Rows[0]["usu_fec_bloqueado"] is DBNull)
                    obj.Bloqueado = false;
                else
                    obj.Bloqueado = true;

                obj.usu_nombre = Resources.Validaciones.valNULLString(dt.Rows[0]["usu_nombre"]);
                obj.usu_apellido = Resources.Validaciones.valNULLString(dt.Rows[0]["usu_apellido"]);

                if (dt.Rows[0]["usu_terminos_y_condiciones"] is DBNull)
                    obj.AceptaTerminosYCondiciones = false;
                else
                {
                    obj.AceptaTerminosYCondiciones = true;
                    obj.usu_terminos_y_condiciones = Convert.ToDateTime(dt.Rows[0]["usu_terminos_y_condiciones"]);
                }

                obj.Organizacion = new SIS_Organizacion() { org_id = Resources.Validaciones.valNULLINT(dt.Rows[0]["usu_org_id"]) };
                obj.Categoria = new Entidades.Categoria() { cat_id = Resources.Validaciones.valNULLINT(dt.Rows[0]["usu_cat_id"]) };
                obj.Area = new SIS_Area() { area_id = Resources.Validaciones.valNULLINT(dt.Rows[0]["usu_area_id"]) };
                return obj;
            }
            else
            {
                return null;
            }
        }
        public List<Entidades.App.SIS_Usuario> ListarPorTexto(string texto, string cantidadRegistros = "")
        {
            List<Entidades.App.SIS_Usuario> lst = new List<Entidades.App.SIS_Usuario>();

            string sWhere = "";

            if (Resources.Repositorio.IsNumeric(texto))
            {
                sWhere = "usu_documento like @searchText ";
            }
            else
            {
                sWhere = "usu_apellido like @searchText OR usu_nombre like @searchText ";
            }

            sQuery = QueryDefault(cantidadRegistros, sWhere, "");

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("searchText", "%" + texto + "%")
            });
            foreach (DataRow row in dt.Rows)
            {
                lst.Add(MapearSimple(row));
            }
            return lst;
        }

        public List<SIS_Usuario> ListarPorCategoriaTexto(int categoria, string textoBusqueda)
        {
            string _where = " 1=1 ";
            List<SIS_Usuario> lst = new List<SIS_Usuario>();
            if (categoria > 0)
                _where += " and usu_cat_id=@categoria ";

            if (textoBusqueda.Trim().Length > 0)
            {
                if (Resources.Repositorio.IsNumeric(textoBusqueda))
                    _where += " and usu_documento like @searchText ";
                else
                    _where += "and (usu_apellido like @textoBusqueda OR usu_nombre like @textoBusqueda) ";
            }
                

            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("categoria",categoria),
                new System.Data.SqlClient.SqlParameter("textoBusqueda", "%" + textoBusqueda + "%")
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public List<SIS_Usuario> ListarPorSkillCategoriaTexto(int categoria, int skill, string textoBusqueda = "")
        {
            List<SIS_Usuario> lst = new List<SIS_Usuario>();

            if (skill > 0)
            {
                lst = new Negocio.ColaboradoresSkills(Token).ListarPorSkillCategoriaTexto(skill, categoria, textoBusqueda);
                return lst;
            }
            else
                lst = ListarPorCategoriaTexto(categoria, textoBusqueda);
            return lst;
        }

        public List<SIS_Usuario> ListarPorArea(int areaID, string textoBusqueda)
        {
            string _where = " usu_org_id=@organizacionID ";

            List<SIS_Usuario> lst = new List<SIS_Usuario>();
            if (areaID > 0)
                _where += " and usu_area_id=@areaID ";

            if (textoBusqueda.Trim().Length > 0)
            {
                if (Resources.Repositorio.IsNumeric(textoBusqueda))
                    _where += " and usu_documento like @searchText ";
                else
                    _where += "and (usu_apellido like @textoBusqueda OR usu_nombre like @textoBusqueda) ";
            }


            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("areaID",areaID),
                new System.Data.SqlClient.SqlParameter("organizacionID",Token.OrganizacionID),
                new System.Data.SqlClient.SqlParameter("textoBusqueda", "%" + textoBusqueda + "%")
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public void IntentoFallidoLogin(int usu_id)
        {
            db.SQLExecuteNonQuery("update SIS_Usuarios SET usu_intentos = isnull(usu_intentos,0) + 1 WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("id",usu_id)
            });
        }

        public Entidades.App.Token Autenticar(Entidades.App.SIS_Usuario unUsuario)
        {
            Entidades.App.Token obj;

            Entidades.App.SIS_Usuario objUsuOriginal = ObtenerUsuarioPorNickname(unUsuario.usu_nickname);

            if (objUsuOriginal != null)
            {
                string passwordEncrypt = Negocio.App.Security.Encriptar(unUsuario.usu_password);

                if (objUsuOriginal.Bloqueado)
                    throw new Exception("El usuario se encuentra bloqueado! consulte con el administrador");

                if (objUsuOriginal.usu_intentos < Resources.Repositorio.LoginMaxAttemps())
                {
                    if (objUsuOriginal.usu_password == passwordEncrypt)
                    {
                        obj = new Entidades.App.Token();
                        obj.ID = Guid.NewGuid().ToString();
                        obj.UserID = objUsuOriginal.usu_id.ToString();
                        obj.UserName = objUsuOriginal.usu_nombre + " " + objUsuOriginal.usu_apellido;
                        obj.Administrador = objUsuOriginal.usu_administrador;
                        obj.AceptaTerminosYCondiciones = objUsuOriginal.AceptaTerminosYCondiciones;
                        obj.OrganizacionID = objUsuOriginal.Organizacion.org_id.ToString();
                        obj.AreaID = objUsuOriginal.Area.area_id.ToString();

                        ActualizarDatosIngreso(objUsuOriginal.usu_id.ToString());

                        return obj;
                    }
                    else
                    {
                        IntentoFallidoLogin(objUsuOriginal.usu_id);
                        throw new Exception("Las credenciales no son válidas!");
                    }

                }
                else
                    throw new Exception("Usuarios Bloqueado!");
            }
            else
                throw new Exception("Las credenciales no son válidas!");
        }

        public List<Entidades.App.SIS_Accion> ListarAcciones(int usu_id, string mod_id)
        {
            List<Entidades.App.SIS_Accion> lst = new List<Entidades.App.SIS_Accion>();
            Negocio.App.SIS_Acciones negocioACC = new SIS_Acciones(Token);

            sQuery = "  select * from SIS_Usuarios_Modulos ";
            sQuery += " inner join SIS_Perfiles_Acciones on pac_prf_id=usi_prf_id ";
            sQuery += " inner join SIS_Acciones on acc_id=pac_acc_id ";
            sQuery += " where usi_usu_id=@usu_id and usi_mod_id=@mod_id ";

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id),
                new System.Data.SqlClient.SqlParameter("mod_id",mod_id)
            });

            foreach (DataRow row in dt.Rows)
            {
                Entidades.App.SIS_Accion obj = negocioACC.Mapear(row);
                lst.Add(obj);
            }

            negocioACC = null;
            return lst;
        }

        public List<Entidades.App.SIS_Modulo> ListarModulos(int usu_id)
        {
            List<Entidades.App.SIS_Modulo> lst = new List<Entidades.App.SIS_Modulo>();
            Negocio.App.SIS_Modulos negocioMOD = new SIS_Modulos(Token);
            Negocio.App.SIS_Perfiles negocioPERFIL = new SIS_Perfiles(Token);

            sQuery = "  select * from SIS_Usuarios_Modulos ";
            sQuery += " inner join SIS_Modulos on mod_id=usi_mod_id ";
            sQuery += " inner join SIS_Perfiles on prf_id=usi_prf_id ";
            sQuery += " where usi_usu_id=@usu_id ";

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id)
            });

            foreach (DataRow row in dt.Rows)
            {
                Entidades.App.SIS_Modulo obj = negocioMOD.Mapear(row);
                obj.Perfiles.Add(negocioPERFIL.Mapear(row));
                lst.Add(obj);
            }

            negocioMOD = null;
            return lst;
        }

        public ObjectMessage AsignarPerfil(int usu_id, int prf_id)
        {
            ObjectMessage oM = new ObjectMessage();

            // Si no tengo un perfil para el sistema, inserto el perfil
            Negocio.App.SIS_Perfiles negocioPER = new Negocio.App.SIS_Perfiles(Token);

            Entidades.App.SIS_Perfil perfil = negocioPER.ObtenerPorID(prf_id.ToString(), false);

            DataTable dt_bus = db.SQLSelect("SELECT COUNT(*) as total FROM SIS_Usuarios_Modulos WHERE usi_mod_id=@mod_id and usi_usu_id=@usu_id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("mod_id",perfil.prf_mod_id),
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id)
            });

            if (Convert.ToInt32(dt_bus.Rows[0]["total"]) > 0)
            {
                oM.Message = "Ya existe un perfil asignado para el módulo seleccionado";
                oM.Success = false;
            }
            else
            {
                DataRow rowPerfil = db.Estructura("SIS_Usuarios_Modulos");
                rowPerfil["usi_usu_id"] = usu_id;
                rowPerfil["usi_mod_id"] = perfil.prf_mod_id;
                rowPerfil["usi_prf_id"] = prf_id;
                db.SQLInsert(rowPerfil, "prf_id");

                oM.Message = "Perfil asignado exitosamente!";
                oM.Success = true;
            }

            return oM;
        }

        public ObjectMessage QuitarPerfil(int usu_id, int prf_id)
        {
            ObjectMessage oM = new ObjectMessage();

            sQuery = "DELETE FROM SIS_Usuarios_Modulos where usi_usu_id=@usu_id and usi_prf_id=@prf_id ";

            db.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id),
                new System.Data.SqlClient.SqlParameter("prf_id",prf_id)
            });

            oM.Success = true;
            oM.Message = "Perfil Eliminado del usuario";

            return oM;
        }


        public ObjectMessage ActualizarPassword(string usu_id, string passwordOld, string passwordNew)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            Entidades.App.SIS_Usuario usuOriginal = ObtenerPorID(usu_id, false);

            if (Negocio.App.Security.Desencriptar(usuOriginal.usu_password).Trim() != passwordOld.Trim())
            {
                oM.Success = false;
                oM.Message = "La contraseña actual no es válida";
                return oM;
            }

            string _passNewEncrypt = Negocio.App.Security.Encriptar(passwordNew);

            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_password=@passnew, usu_fec_pass_changed=GETDATE() WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("passnew",_passNewEncrypt),
                new System.Data.SqlClient.SqlParameter("id",usuOriginal.usu_id)
            });

            oM.Success = true;
            oM.Message = "Contraseña modificada exitosamente!";

            return oM;
        }

        public ObjectMessage ActualizarPasswordDirect(string usu_id, string password)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            string _passNewEncrypt = Negocio.App.Security.Encriptar(password);

            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_password=@passnew, usu_fec_pass_changed=GETDATE(), usu_intentos=0 WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("passnew",_passNewEncrypt),
                new System.Data.SqlClient.SqlParameter("id",usu_id)
            });

            oM.Success = true;
            oM.Message = "Contraseña modificada exitosamente!";

            return oM;
        }

        public void ActualizarDatosIngreso(string usu_id)
        {
            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_fec_last_logon=GETDATE(),usu_intentos=0 WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("id",usu_id)
            });
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override List<SIS_Usuario> ListarParaTableAjax(DatatableJS datatableFilters)
        {
            List<Entidades.App.SIS_Usuario> lst = new List<SIS_Usuario>();

            string sWhere = "(usu_nickname like @searchText or usu_nombre like @searchText or usu_apellido like @searchText) ";
            string sOrden = "";

            if (datatableFilters.MostrarTodos == false)
                sWhere += " and usu_fec_eliminado is null ";

            if (datatableFilters.sortColumnName.Length > 0)
                sOrden = datatableFilters.sortColumnName + " " + datatableFilters.direccion;

            sQuery = QueryDefault("", sWhere, sOrden);

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("searchText", "%" + datatableFilters.SearchValue + "%")
            });

            foreach (DataRow row in dt.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public ObjectMessage BloquearUsuario(string usu_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_fec_bloqueado=GETDATE() WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("id",usu_id)
            });

            oM.Success = true;
            oM.Message = "Usuario bloqueado exitosamente!";

            return oM;
        }

        public ObjectMessage DesbloquearUsuario(string usu_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_fec_bloqueado=NULL WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("id",usu_id)
            });

            oM.Success = true;
            oM.Message = "Usuario Desbloqueado exitosamente!";

            return oM;
        }

        public ObjectMessage ReiniciarIntentosFallidos(string usu_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_intentos=NULL WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("id",usu_id)
            });

            oM.Success = true;
            oM.Message = "Intentos fallidos reiniciados exitosamente!";

            return oM;
        }


        public override SIS_Usuario ObjetoNuevo()
        {
            return new SIS_Usuario();
        }

        public void RegistrarLogin(Entidades.App.Token unToken)
        {
            try
            {
                string _sqlInsert = "";
                _sqlInsert += " INSERT INTO SIS_Usuarios_Login (usl_usu_id,usl_token_id,usl_fec_ini,usl_ipv4,usl_navigator) ";
                _sqlInsert += " VALUES (@usl_usu_id,@usl_token_id,@usl_fec_ini,@usl_ipv4,@usl_navigator) ";


                db.SQLExecuteNonQuery(_sqlInsert, new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("usl_usu_id",unToken.UserID),
                    new System.Data.SqlClient.SqlParameter("usl_token_id",unToken.ID),
                    new System.Data.SqlClient.SqlParameter("usl_fec_ini",DateTime.Now),
                    new System.Data.SqlClient.SqlParameter("usl_ipv4", unToken.IPAddress),
                    new System.Data.SqlClient.SqlParameter("usl_navigator",unToken.NavigatorID)
                });

            }
            catch (Exception ex)
            {
                var _error = ex.Message;
            }
        }

        public void CloseLogin(Entidades.App.Token unToken)
        {
            try
            {
                if (unToken != null)
                {
                    db.SQLExecuteNonQuery("UPDATE SIS_Usuarios_Login SET usl_fec_fin=@fecha WHERE usl_token_id=@tokenID", new List<System.Data.SqlClient.SqlParameter>()
                    {
                        new System.Data.SqlClient.SqlParameter("fecha",DateTime.Now),
                        new System.Data.SqlClient.SqlParameter("tokenID",unToken.ID),
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Entidades.App.SIS_Usuario ObtenerUsuarioParaRecoveryPassword(string nickname, string mail)
        {
            Entidades.App.SIS_Usuario obj;

            sQuery = QueryDefault("", "usu_nickname=@nickname and usu_mail=@mail", "");

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("nickname", nickname),
                new System.Data.SqlClient.SqlParameter("mail", mail)
            });

            if (dt.Rows.Count > 0)
            {
                obj = MapearSimple(dt.Rows[0]);

                return obj;
            }
            else
            {
                return new SIS_Usuario();
            }
        }

        public ObjectMessage ConfirmarTerminosYCondiciones(string usu_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            db.SQLExecuteNonQuery("UPDATE SIS_Usuarios SET usu_terminos_y_condiciones=@fecha, usu_intentos=0 WHERE usu_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("fecha",DateTime.UtcNow),
                new System.Data.SqlClient.SqlParameter("id",usu_id)
            });

            oM.Success = true;
            oM.Message = "Terminos y condiciones confirmados exitosamente !";

            return oM;
        }

        public ObjectMessage ActualizarCategoria(SIS_Usuario usuario, string doc)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                List<System.Data.SqlClient.SqlParameter> lstParams = new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("cat_id", usuario.Categoria.cat_id),
                    new System.Data.SqlClient.SqlParameter("id", usuario.usu_id)
                };

                sQuery = " UPDATE SIS_Usuarios SET usu_cat_id=@cat_id ";
                sQuery += " WHERE usu_id=@id ";

                db.SQLExecuteNonQuery(sQuery, lstParams);

                
                oM.Message = "Datos actualizados";
                oM.Success = true;
                oM = new Negocio.CategoriasColaboradoresEvolucion(Token).RegistrarEvento(usuario, doc);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public ObjectMessage SaveModalColaborador(SIS_Usuario Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("SIS_Usuarios");
                row["usu_mail"] = Obj.usu_mail;
                row["usu_area_id"] = Obj.Area.area_id;
                row["usu_legajo"] = Obj.usu_legajo;
                
                if (Obj.usu_id.Equals(0))
                {
                    //if (!string.IsNullOrEmpty(Obj.usu_password))
                    //{
                    //    row["usu_password"] = App.Security.Encriptar(Obj.usu_password);
                    //}
                    //else
                    //{
                    //    row["usu_password"] = App.Security.Encriptar(App.Security.DefaultPassword());
                    //}

                    //Obj.usu_id = db.SQLInsert(row, "usu_id").Valor;
                }
                else
                {
                    db.SQLUpdate(row, "usu_id=@id", "usu_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("id",Obj.usu_id)
                });
                }

                oM.Success = true;
                oM.Message = "OK";
            }
            catch (Exception ex)
            {

                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        #endregion

        #region Permisos Especiales

        public bool TienePermisoEspecialAsignado(int usu_id, SIS_Permisos_Especiales.PermisosEspeciales permiso)
        {
            try
            {
                return ListarPermisosEspeciales(usu_id, (int)permiso).Any();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Entidades.App.SIS_Permiso_Especial> ListarPermisosEspeciales(int usu_id, int pee_id = 0)
        {
            List<Entidades.App.SIS_Permiso_Especial> lst = new List<Entidades.App.SIS_Permiso_Especial>();
            Negocio.App.SIS_Permisos_Especiales negocioPEE = new SIS_Permisos_Especiales(Token);

            sQuery = "  select * from SIS_Usuarios_Permisos_Especiales ";
            sQuery += " inner join SIS_Permisos_Especiales on pee_id=upe_pee_id ";
            sQuery += " where upe_usu_id=@usu_id ";

            if (pee_id > 0)
                sQuery += " and upe_pee_id=@pee_id";

            DataTable dt = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id),
                new System.Data.SqlClient.SqlParameter("pee_id",pee_id)
            });

            foreach (DataRow row in dt.Rows)
            {
                Entidades.App.SIS_Permiso_Especial obj = negocioPEE.Mapear(row);
                lst.Add(obj);
            }

            negocioPEE = null;
            return lst;
        }

        public ObjectMessage AsignarPermisoEspecial(int usu_id, int pee_id)
        {
            ObjectMessage oM = new ObjectMessage();

            DataRow rowPerfil = db.Estructura("SIS_Usuarios_Permisos_Especiales");
            rowPerfil["upe_usu_id"] = usu_id;
            rowPerfil["upe_pee_id"] = pee_id;
            db.SQLInsert(rowPerfil, "upe_id");

            oM.Success = true;
            oM.Message = "Permiso Especial Asignado al Usuario";

            return oM;
        }

        public ObjectMessage QuitarPermisoEspecial(int usu_id, int pee_id)
        {
            ObjectMessage oM = new ObjectMessage();

            sQuery = "DELETE FROM SIS_Usuarios_Permisos_Especiales where upe_usu_id=@usu_id and upe_pee_id=@pee_id ";

            db.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id),
                new System.Data.SqlClient.SqlParameter("pee_id",pee_id)
            });

            oM.Success = true;
            oM.Message = "Permiso Especial Eliminado del usuario";

            return oM;
        }

        public List<Entidades.App.SIS_Usuario_Login> ListarUsuariosLogueados()
        {
            List<Entidades.App.SIS_Usuario_Login> lst = new List<Entidades.App.SIS_Usuario_Login>();
            Negocio.App.SIS_Usuarios_Login negocioUSL = new Negocio.App.SIS_Usuarios_Login(Token);

            sQuery = " SELECT * FROM SIS_Usuarios " +
                "LEFT JOIN SIS_Usuarios_Login ON usl_usu_id = usu_id " +
                "WHERE usl_fec_fin IS NULL AND usu_fec_last_logon IS NOT NULL";

            DataTable dt_bus = db.SQLSelect(sQuery);

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.App.SIS_Usuario_Login obj = negocioUSL.Mapear(row);
                obj.Usuario.usu_nombre = Resources.Validaciones.valNULLString(row["usu_nombre"]);
                obj.Usuario.usu_apellido = Resources.Validaciones.valNULLString(row["usu_apellido"]);
                lst.Add(obj);
            }

            return lst;
        }



        #endregion

    }
}
