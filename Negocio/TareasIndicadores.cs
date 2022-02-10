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
    public class TareasIndicadores : NegocioBase<Entidades.Tarea_Indicador>
    {

        public TareasIndicadores(Entidades.App.Token paramToken) : base("tarind_id", "sin", "Tareas_Indicadores", "pit")
        {
            Token = paramToken;
        }


        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tarea_Indicador Mapear(DataRow dr)
        {
            Entidades.Tarea_Indicador obj = MapearSimple(dr);
            obj.Indicador = Negocio.ProyectosIndicadores.MapearStatic(dr);
            obj.Tarea = Negocio.Tareas.MapearStatic(dr);
            obj.Tarea.Prioridad = Negocio.TipoPrioridades.MapearStatic(dr);
            obj.Indicador.Proyecto = Negocio.Proyectos.MapearStatic(dr);

            return obj;
        }

        public override Tarea_Indicador MapearCompleto(DataRow dr)
        {
            Entidades.Tarea_Indicador obj = Mapear(dr);
            return obj;
        }

        public override Tarea_Indicador MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tarea_Indicador MapearStatic(DataRow dr)
        {
            Entidades.Tarea_Indicador obj = new Tarea_Indicador();
            obj.tarind_id = Resources.Validaciones.valNULLINT(dr["tarind_id"]);
            obj.Indicador = new Proyecto_Indicador() { pryind_id = Resources.Validaciones.valNULLINT(dr["tarind_pryind_id"]) };
            obj.Indicador.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Indicador.pryind_id.ToString());
            obj.Tarea = new Tarea() { tar_id = Resources.Validaciones.valNULLINT(dr["tarind_tar_id"]) };
            obj.Tarea.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Tarea.tar_id.ToString());
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.tarind_id.ToString());
            obj.tarind_incidencia = Resources.Validaciones.valNULLDecimal(dr["tarind_incidencia"]);
            return obj;

        }
        public override Tarea_Indicador ObjetoNuevo()
        {
            Entidades.Tarea_Indicador obj = new Tarea_Indicador();
            return obj;
        }


        public bool TareaPreviamenteAsignada(Tarea_Indicador obj)
        {

            string sQuery = QueryDefault("", " tarind_pryind_id = @pryind_id and tarind_tar_id = @tar_id ", "");
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("pryind_id",obj.Indicador.pryind_id),
                    new System.Data.SqlClient.SqlParameter("tar_id",obj.Tarea.tar_id)
                });
            return (dt_bus.Rows.Count > 0);
        }



        public override void PermiteGuardar(Tarea_Indicador obj)
        {

            if (TareaPreviamenteAsignada(obj))
                throw new Exception("El Tarea fue asignada al Indicador previamente.");
            if (obj.tarind_incidencia<0 | obj.tarind_incidencia>100)
                throw new Exception("La incidencia tiene que ser un porcentaje de 0 a 100");
            if (!PorcentajeIndicadorCorrecto(obj))
                throw new Exception("La suma de incidencias sobre el indicador supera el 100%.");
        }

       

        public override ObjectMessage Save(Tarea_Indicador Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Tareas_Indicadores");
                row["tarind_tar_id"] = Obj.Tarea.tar_id;
                row["tarind_pryind_id"] = Obj.Indicador.pryind_id;
                row["tarind_incidencia"] = Obj.tarind_incidencia;

                if (Obj.tarind_id.Equals(0))
                {
                    Obj.tarind_id = db.SQLInsert(row, "tarind_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "tarind_id=@tarind_id", "tarind_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tarind_id",Obj.tarind_id)
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
            sQuery = "  SELECT * FROM Tareas_Indicadores ";
            sQuery += " LEFT JOIN Tareas on tar_id=tarind_tar_id ";
            sQuery += " LEFT JOIN Proyectos_Indicadores ON pryind_id=tarind_pryind_id ";
            sQuery += " LEFT JOIN Tipo_Prioridad ON tprioridad_id=tar_tprioridad_id ";
            sQuery += " LEFT JOIN Proyectos on proy_id=pryind_proy_id ";

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

       

        public List<Entidades.Tarea> ListarTareasPorIndicador(int pryind_id)
        {
            List<Entidades.Tarea> lst = new List<Tarea>();

            sQuery = QueryDefault("", "pryind_id=@pryind_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("pryind_id", pryind_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Tarea_Indicador obj = Mapear(row);

                lst.Add(obj.Tarea);
            }


            return lst;
        }

        public List<Entidades.Tarea_Indicador> ListarTareaIndicadorPorIndicador(int pryind_id)
        {
            List<Entidades.Tarea_Indicador> lst = new List<Tarea_Indicador>();

            sQuery = QueryDefault("", "pryind_id=@pryind_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("pryind_id", pryind_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Tarea_Indicador obj = Mapear(row);

                lst.Add(obj);
            }


            return lst;
        }

        public List<Entidades.Tarea_Indicador> ListarIndicadoresPorTarea(int tar_id)
        {
            List<Entidades.Tarea_Indicador> lst = new List<Tarea_Indicador>();

            sQuery = QueryDefault("", "tar_id=@tar_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("tar_id", tar_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Tarea_Indicador obj = Mapear(row);
                obj.Cobertura = 100;
                //obj.Cobertura = CalcularCobertura(obj);
                lst.Add(obj);
            }


            return lst;
        }

        
        public List<Tarea> ListarPorIndicadorEstadoTexto(int indicador, int estado, string textoBusqueda = "")
        {
            List<Entidades.Tarea> lst = new List<Entidades.Tarea>();
            
            string _where = " 1=1 ";

            if (indicador > 0)
                _where += " and tarind_pryind_id=@indicador ";


            if (estado > 0)
                _where += " and tar_tarestado_id=@estado ";

            if (textoBusqueda.Trim().Length > 0)
                _where += " and (tar_nombre like @textoBusqueda or tar_descripcion like @textoBusqueda)  ";

            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("indicador",indicador),
                new System.Data.SqlClient.SqlParameter("estado",estado),
                new System.Data.SqlClient.SqlParameter("textoBusqueda", "%" + textoBusqueda + "%")
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row).Tarea);
            }

            return lst;
        }

        #endregion

        #region Funcionalidad Especial


        public bool PorcentajeIndicadorCorrecto(Tarea_Indicador obj)
        {
            List<Tarea_Indicador> tareas = ListarTareaIndicadorPorIndicador(obj.Indicador.pryind_id);
            decimal porcentaje = 0;
            foreach (Tarea_Indicador elemento in tareas)
            {
                porcentaje += elemento.tarind_incidencia;
                if (porcentaje > 100) return false;
            }
            return true;
        }

        public bool PorcentajeCorrecto(List<Tarea_Indicador> lst)
        {

            decimal porcentaje = 0;
            foreach (Tarea_Indicador elemento in lst)
            {
                porcentaje += elemento.tarind_incidencia;
                if (porcentaje > 100) return false;
            }
            return porcentaje.Equals(100);
        }

        public decimal CalcularCobertura(Tarea_Indicador obj)
        {
            string sQuery = "SELECT SUM(tarind_incidencia) AS suma FROM Tareas_Indicadores WHERE tarind_pryind_id=@id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("id",obj.Indicador.pryind_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["suma"]);
        }


        public ObjectMessage SaveMetas(Tarea_Indicador Obj)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                DataRow row = db.Estructura("Tareas_Indicadores");
                row["tarind_incidencia"] = Obj.tarind_incidencia;
                db.SQLUpdate(row, "tarind_id=@tarind_id", "tarind_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("tarind_id",Obj.tarind_id)
                });
                oM.Message = "Datos actualizados";
                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public Entidades.App.ObjectMessage DeletePorTarea(string tar_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();
            try
            {
                db.SQLExecuteNonQuery("DELETE FROM Tareas_Indicadores WHERE tarind_tar_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("id",tar_id)
                });
                oM.Message = "Datos actualizados";
                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }
            return oM;
        }
        public Entidades.App.ObjectMessage RollBack(string id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                Entidades.Tarea_Indicador datos = ObtenerPorID(id);

                db.SQLExecuteNonQuery("UPDATE Tareas_Indicadores SET tarind_incidencia=null WHERE tarind_pryind_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("id",id)
                });

            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        public Entidades.Vistas.Graficos.PieChart VisualizarPorcentajesPieChart(List<Entidades.Tarea_Indicador> items)
        {
            List<Entidades.Vistas.Graficos.PieChartElement> lst = new List<Entidades.Vistas.Graficos.PieChartElement>();
            foreach (Tarea_Indicador elemento in items)
            {
                lst.Add(new Entidades.Vistas.Graficos.PieChartElement(
                    elemento.Tarea.tar_nombre,
                    elemento.tarind_incidencia,
                    100));
            }
            return new Entidades.Vistas.Graficos.PieChart(lst);
        }

        #endregion
    }
}
