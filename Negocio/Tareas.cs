using Entidades;
using Entidades.App;
using Entidades.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Tareas : NegocioBase<Entidades.Tarea>
    {
        Negocio.TareasIndicadores negocioTARIND;
        public Tareas(Entidades.App.Token paramToken) : base("tar_id", "tar_activo", "Tareas", "tar")
        {
            Token = paramToken;
            negocioTARIND = new TareasIndicadores(Token);
        }


        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tarea Mapear(DataRow dr)
        {
            Entidades.Tarea obj = MapearSimple(dr);
            obj.Prioridad = Negocio.TipoPrioridades.MapearStatic(dr);
            obj.EstadoTarea = Negocio.TareasEstados.MapearStatic(dr);

            return obj;
        }

        public override Tarea MapearCompleto(DataRow dr)
        {
            Entidades.Tarea obj = Mapear(dr);
            obj.Organizacion = Negocio.App.SIS_Organizaciones.MapearStatic(dr);
            obj.Area = Negocio.App.SIS_Areas.MapearStatic(dr);
            return obj;
        }

        public override Tarea MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tarea MapearStatic(DataRow dr)
        {
            Entidades.Tarea obj = new Tarea();
            obj.tar_id = Resources.Validaciones.valNULLINT(dr["tar_id"]);
            obj.tar_nombre = Resources.Validaciones.valNULLString(dr["tar_nombre"]);
            obj.tar_descripcion = Resources.Validaciones.valNULLString(dr["tar_descripcion"]);
            obj.Prioridad = new Tipo_Prioridad() { tprioridad_id = Resources.Validaciones.valNULLINT(dr["tar_tprioridad_id"]) };
            obj.tar_fec_ini = Resources.Validaciones.valNULLDateTime(dr["tar_fec_ini"]);
            obj.tar_fec_fin = Resources.Validaciones.valNULLDateTime(dr["tar_fec_fin"]);
            obj.tar_fec_mod = Resources.Validaciones.valNULLDateTime(dr["tar_fec_mod"]);
            obj.tar_tiempo = Resources.Validaciones.valNULLDecimal(dr["tar_tiempo"]);
            obj.EstadoTarea = new Tarea_Estado() { tarestado_id = Resources.Validaciones.valNULLINT(dr["tar_tarestado_id"]) };
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.tar_id.ToString());
            obj.tar_porcentaje = Resources.Validaciones.valNULLINT(dr["tar_porcentaje"]);

            if (obj.EstadoTarea.tarestado_id == (int)Resources.Enums.TipoEstadosTarea.Finalizada)
                obj.tar_fec_fin_real = Resources.Validaciones.valNULLDateTime(dr["tar_fec_fin_real"]);

            obj.tar_numero = Resources.Validaciones.valNULLINT(dr["tar_numero"]);
            
            return obj;
        }
        public override Tarea ObjetoNuevo()
        {
            Entidades.Tarea obj = new Tarea();
            obj.tar_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.tar_id.ToString());
            return obj;
        }

        public override void PermiteGuardar(Tarea obj)
        {
            if (obj.tar_fec_fin < DateTime.Now)
                throw new Exception("La fecha de finalización ingresada no es válida.");
            if (obj.tar_fec_ini > obj.tar_fec_fin)
                throw new Exception("La fecha de finalización y de inicio no son válidas.");
            if (obj.tar_nombre.Length.Equals(0))
                throw new Exception("Debe ingresar un título para la tarea.");
            if (obj.tar_descripcion.Length.Equals(0))
                throw new Exception("Debe ingresar una descripción para la tarea.");
        }

        public override ObjectMessage Save(Tarea Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                Obj.Organizacion.org_id = Convert.ToInt32(Token.OrganizacionID);

                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Tareas");
                row["tar_nombre"] = Obj.tar_nombre;
                row["tar_descripcion"] = Obj.tar_descripcion;
                row["tar_tprioridad_id"] = Obj.Prioridad.tprioridad_id;
                row["tar_tarestado_id"] = Obj.EstadoTarea.tarestado_id;
                row["tar_fec_ini"] = Obj.tar_fec_ini;
                row["tar_fec_fin"] = Obj.tar_fec_fin;
                row["tar_tiempo"] = Obj.tar_tiempo;
                row["tar_porcentaje"] = Obj.tar_porcentaje;
                row["tar_fec_mod"] = Obj.tar_fec_mod;
             
                if (Obj.tar_id.Equals(0))
                {
                    row["tar_org_id"] = Obj.Organizacion.org_id;
                    row["tar_area_id"] = Obj.Area.area_id;
                    row["tar_tarestado_id"] = (int)Negocio.TareasEstados.Estados.Edicion;
                    row["tar_activo"] = true;
                    row["tar_fec_alta"] = DateTime.Now;
                    row["tar_usu_id_alta"] = Token.UserID;
                    row["tar_fec_mod"] = DateTime.Now;
                    row["tar_porcentaje"] = 0;
                    row["tar_numero"] = ProximoNumero(Obj);
                    Obj.tar_id = db.SQLInsert(row, "tar_id").Valor;
                    Obj.IdEncriptado = Negocio.App.Security.EncriptarID(Obj.tar_id.ToString());
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["tar_fec_mod"] = DateTime.Now;
                    row["tar_usu_id_mod"] = Token.UserID;
                    db.SQLUpdate(row, "tar_id=@tar_id", "tar_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tar_id",Obj.tar_id)
                    });

                    oM.Message = "Datos actualizados";
                }

                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tareas ";
            sQuery += " LEFT JOIN Tipo_Prioridad on tprioridad_id=tar_tprioridad_id ";
            sQuery += " LEFT JOIN Tareas_Estados on tarestado_id=tar_tarestado_id ";
            sQuery += " INNER JOIN SIS_Organizaciones on org_id=tar_org_id ";
            sQuery += " LEFT JOIN SIS_Areas on area_id=tar_area_id ";

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

        #region Funcionalidad Particular

        public Entidades.App.ObjectMessage PublicarTarea(string tar_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();
            if(negocioTARIND.ListarIndicadoresPorTarea(Convert.ToInt32(tar_id)).Count.Equals(0))
            {
                oM.Success = false;
                oM.Message = "No se puede publicar una tarea sin indicadores.";
                return oM;
            }
            try
            {
                db.SQLExecuteNonQuery("UPDATE Tareas SET tar_tarestado_id=@estado, tar_fec_mod=GETDATE() WHERE tar_id=@tar_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("estado",(int)Negocio.TareasEstados.Estados.Pendiente),
                    new System.Data.SqlClient.SqlParameter("tar_id",tar_id)
                });

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

        public ObjectMessage RegistrarEvento(Resources.Enums.TipoEventosTarea tipoEvento, Entidades.Tarea unaTarea)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Tareas_Eventos");
                row["tareve_tar_id"] = unaTarea.tar_id;
                row["tareve_tevetar_id"] = (int)tipoEvento;
                row["tareve_valor"] = 0;

                switch (tipoEvento)
                {
                    case Resources.Enums.TipoEventosTarea.Creacion:
                        row["tareve_detalle"] = "Creación de Tarea";
                        break;
                    case Resources.Enums.TipoEventosTarea.Modificacion_de_datos:
                        row["tareve_detalle"] = "Modificación de Tarea";
                        
                        break;
                    case Resources.Enums.TipoEventosTarea.Actualizar_evolucion:
                        row["tareve_detalle"] = "Actualiza evolución a " + unaTarea.tar_porcentaje.ToString() + "%";
                        row["tareve_valor"] = unaTarea.tar_porcentaje;
                        break;
                    default:
                        row["tareve_detalle"] = "";
                        break;
                }

                row["tareve_fecha"] = DateTime.Now;
                row["tareve_usu_id"] = Token.UserID;
                
                db.SQLInsert(row);

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

        public int ProximoNumero(Entidades.Tarea unaTarea)
        {
            var _id = 0;

            sQuery += "select isnull(MAX(tar_numero),0) + 1 as proximo_numero from Tareas where tar_org_id=@org_id ";

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() { 
                new System.Data.SqlClient.SqlParameter("org_id",unaTarea.Organizacion.org_id)
            });

            if (dt_bus.Rows.Count > 0)
                return Convert.ToInt32(dt_bus.Rows[0]["proximo_numero"]);

            return _id;
        }

        #endregion

        #region Listar

        public List<Tarea> ListarPorEstado(int estado)
        {
            List<Entidades.Tarea> lst = new List<Entidades.Tarea>();
            lst.Where(m => m.EstadoTarea.tarestado_id.Equals(estado)).ToList();
            return lst;
        }

        public List<Tarea> ListarPorEstadoTexto(int estado, string textoBusqueda)
        {
            string _where = "1=1";
            List<Entidades.Tarea> lst = new List<Entidades.Tarea>();
            if (estado > 0)
                _where += " and tar_tarestado_id=@estado ";

            if (textoBusqueda.Trim().Length > 0)
                _where += " and (tar_nombre like @textoBusqueda or tar_descripcion like @textoBusqueda)  ";

            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("estado",estado),
                new System.Data.SqlClient.SqlParameter("textoBusqueda", "%" + textoBusqueda + "%")
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        public List<Tarea> ListarPorIndicadorEstadoTexto(int indicador, int estado, string textoBusqueda = "")
        {
            List<Entidades.Tarea> lst = new List<Entidades.Tarea>();

            if (indicador > 0)
            {
                lst = negocioTARIND.ListarPorIndicadorEstadoTexto(indicador, estado, textoBusqueda);
                return lst;
            }
            else
                lst = ListarPorEstadoTexto(estado, textoBusqueda);
            return lst;
        }


        int EstadoSegunPorcentaje(Tarea Obj)
        {
            if (Obj.tar_porcentaje > 0 & Obj.tar_porcentaje < 100)
                return (int)Negocio.TareasEstados.Estados.EnProgreso;
            if (Obj.tar_porcentaje.Equals(100))
                return (int)Negocio.TareasEstados.Estados.Finalizada;
            return (int)Negocio.TareasEstados.Estados.Pendiente;
        }

        public ObjectMessage RealizarAvance(Tarea tarea)
        {
            int estadoNuevo = EstadoSegunPorcentaje(tarea);
            ObjectMessage oM = new ObjectMessage();
            try
            {
                List<System.Data.SqlClient.SqlParameter> lstParams = new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("estado", estadoNuevo),
                    new System.Data.SqlClient.SqlParameter("porcentaje", tarea.tar_porcentaje),
                    new System.Data.SqlClient.SqlParameter("id", tarea.tar_id)  
                };
                
                sQuery = " UPDATE Tareas SET tar_tarestado_id=@estado, tar_porcentaje=@porcentaje ";    

                if (estadoNuevo == (int)Resources.Enums.TipoEstadosTarea.Finalizada)
                {
                    sQuery += ", tar_fec_fin_real=@fec_fin_real ";
                    lstParams.Add(new System.Data.SqlClient.SqlParameter("fec_fin_real", DateTime.Now));
                }
                
                sQuery += " WHERE tar_id=@id";

                db.SQLExecuteNonQuery(sQuery, lstParams);

                oM.Message = "Datos actualizados";
                oM.Success = true;

                ActualizarEvolucionIndicadores(tarea.tar_id);

                RegistrarEvento(Resources.Enums.TipoEventosTarea.Actualizar_evolucion, tarea);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public void ActualizarEvolucionIndicadores(int tar_id)
        {
            sQuery = "  UPDATE ";
            sQuery += "     Tabla_A";
            sQuery += " SET ";
            sQuery += "     Tabla_A.pryind_porc_evolucion = Tabla_B.incidencia_real ";
            sQuery += " FROM ";
            sQuery += "     Proyectos_Indicadores AS Tabla_A ";
            sQuery += "     INNER JOIN (select tarind_pryind_id, SUM((tar_porcentaje * tarind_incidencia) / 100)  as incidencia_real From tareas_indicadores ";
            sQuery += "                 inner join tareas on tar_id=tarind_tar_id ";
            sQuery += "                 where tarind_pryind_id in (select tarind_pryind_id from Tareas_Indicadores where tarind_tar_id=@tar_id) ";
            sQuery += "                 group by tarind_pryind_id ";
            sQuery += "     ) AS Tabla_B ";
            sQuery += "          ON Tabla_A.pryind_id = Tabla_B.tarind_pryind_id ";

            db.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("tar_id", tar_id)
            });

        }
    }
    #endregion
}