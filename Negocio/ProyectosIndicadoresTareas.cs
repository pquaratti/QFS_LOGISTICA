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
    public class ProyectosIndicadoresTareas : NegocioBase<Entidades.Proyecto_Indicador_Tarea>
    {
        public ProyectosIndicadoresTareas(Entidades.App.Token paramToken) : base("pit_id", "sin", "Proyectos_Indicadores_Tareas", "pit")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Proyecto_Indicador_Tarea Mapear(DataRow dr)
        {
            Entidades.Proyecto_Indicador_Tarea obj = MapearSimple(dr);
            obj.Indicador = Negocio.ProyectosIndicadores.MapearStatic(dr);

            return obj;
        }

        public override Proyecto_Indicador_Tarea MapearCompleto(DataRow dr)
        {
            Entidades.Proyecto_Indicador_Tarea obj = Mapear(dr);
            return obj;
        }

        public override Proyecto_Indicador_Tarea MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Proyecto_Indicador_Tarea MapearStatic(DataRow dr)
        {
            Entidades.Proyecto_Indicador_Tarea obj = new Proyecto_Indicador_Tarea();
            obj.pit_id = Resources.Validaciones.valNULLINT(dr["pit_id"]);
            obj.Indicador = new Proyecto_Indicador() { pryind_id = Resources.Validaciones.valNULLINT(dr["pit_pryind_id"]) };
            obj.Indicador.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Indicador.pryind_id.ToString());
            obj.Tarea = new Proyecto_Tarea() { prytar_id = Resources.Validaciones.valNULLINT(dr["pit_prytar_id"]) };
            obj.Tarea.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Tarea.prytar_id.ToString());
            obj.pit_porcentaje = Resources.Validaciones.valNULLDecimal(dr["pit_porcentaje"]);
            obj.pit_finalizada = Resources.Validaciones.valNULLBool(dr["pit_finalizada"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.pit_id.ToString());
            return obj;
        }

        public override Proyecto_Indicador_Tarea ObjetoNuevo()
        {
            Entidades.Proyecto_Indicador_Tarea obj = new Proyecto_Indicador_Tarea();
            return obj;
        }

        public bool TareaPreviamenteAsignada(Proyecto_Indicador_Tarea obj)
        {

            string sQuery = QueryDefault("", " pit_pryind_id = @pryind_id and pit_prytar_id = @prytar_id ", "");
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("pryind_id",obj.Indicador.pryind_id),
                    new System.Data.SqlClient.SqlParameter("prytar_id",obj.Tarea.prytar_id)
                });
            return (dt_bus.Rows.Count > 0);
        }

        public bool PorcentajeCorrecto(List<Proyecto_Indicador_Tarea> lst)
        {
            
            decimal porcentaje = 0;
            foreach (Proyecto_Indicador_Tarea elemento in lst)
            {
                porcentaje += elemento.pit_porcentaje;
                if (porcentaje > 100) return false;
            }
            return porcentaje.Equals(100);
        }

        public override void PermiteGuardar(Proyecto_Indicador_Tarea obj)
        {

            if (TareaPreviamenteAsignada(obj))
                throw new Exception("La tarea fue asignada al indicador previamente.");

        }

        public ObjectMessage SaveMetas(Proyecto_Indicador_Tarea Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Proyectos_Indicadores_Tareas");

                row["pit_porcentaje"] = Obj.pit_porcentaje;


                db.SQLUpdate(row, "pit_id=@pit_id", "pit_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("pit_id",Obj.pit_id)
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

        public ObjectMessage TareaCompletada(int pit_id)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Proyectos_Indicadores_Tareas");

                row["pit_finalizada"] = true;


                db.SQLUpdate(row, "pit_id=@pit_id", "pit_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("pit_id",pit_id)
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

        public override ObjectMessage Save(Proyecto_Indicador_Tarea Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Proyectos_Indicadores_Tareas");
                row["pit_prytar_id"] = Obj.Tarea.prytar_id;
                row["pit_pryind_id"] = Obj.Indicador.pryind_id;

                if (Obj.pit_id.Equals(0))
                {
                    row["pit_fec_alta"] = DateTime.Now;
                    row["pit_usu_id_alta"] = Token.UserID;
                    Obj.pit_id = db.SQLInsert(row, "pit_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "pit_id=@pit_id", "pit_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("pit_id",Obj.pit_id)
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
            sQuery = "  SELECT * FROM Proyectos_Indicadores_Tareas ";
            sQuery += " INNER JOIN Proyectos_Tareas on prytar_id=pit_prytar_id ";
            sQuery += " INNER JOIN Proyectos_Indicadores ON pryind_id=pit_pryind_id ";
            
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

        public List<Entidades.Proyecto_Indicador_Tarea> ListarPorProyecto(int proy_id)
        {
            List<Entidades.Proyecto_Indicador_Tarea> lst = new List<Proyecto_Indicador_Tarea>();

            sQuery = QueryDefault("", "proy_id=@proy_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id", proy_id)
            });
            
            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Proyecto_Indicador_Tarea obj = Mapear(row);


                lst.Add(obj);
            }


            return lst;
        }


        public List<Entidades.Proyecto_Indicador_Tarea> ListarTareasPorIndicador(int pryind_id)
        {
            List<Entidades.Proyecto_Indicador_Tarea> lst = new List<Proyecto_Indicador_Tarea>();

            sQuery = QueryDefault("", "pryind_id=@pryind_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("pryind_id", pryind_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Proyecto_Indicador_Tarea obj = Mapear(row);


                lst.Add(obj);
            }


            return lst;
        }



        public List<Entidades.Proyecto_Indicador> ListarIndicadorPorTarea(int prytar_id)
        {
            List<Entidades.Proyecto_Indicador> lst = new List<Proyecto_Indicador>();

            sQuery = QueryDefault("", "prytar_id=@prytar_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("prytar_id", prytar_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Proyecto_Indicador_Tarea obj = Mapear(row);


                lst.Add(obj.Indicador);
            }


            return lst;
        }

        #endregion

        #region Funcionalidad Especial

        public Entidades.Vistas.Graficos.PieChart VisualizarPorcentajesPieChart (List<Entidades.Proyecto_Indicador_Tarea> items)
        {
            List<Entidades.Vistas.Graficos.PieChartElement> lst = new List<Entidades.Vistas.Graficos.PieChartElement>();
            foreach(Proyecto_Indicador_Tarea elemento in items)
            {
                lst.Add(new Entidades.Vistas.Graficos.PieChartElement(
                    elemento.Tarea.prytar_nombre,
                    elemento.pit_porcentaje,
                    100));
            }
            return new Entidades.Vistas.Graficos.PieChart(lst);
        }

        public Entidades.App.ObjectMessage RollBack(string id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                Entidades.Proyecto_Indicador_Tarea datos = ObtenerPorID(id);

                db.SQLExecuteNonQuery("UPDATE Proyectos_Indicadores_Tareas SET pit_porcentaje=null WHERE pit_pryind_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
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

        public decimal CalcularValorActual(Entidades.Proyecto_Indicador indicador)
        {
            decimal acumulador = indicador.pryind_valor_base;
            decimal total = indicador.pryind_valor_meta - indicador.pryind_valor_base;
            List<Entidades.Proyecto_Indicador_Tarea> tareas = ListarTareasPorIndicador(indicador.pryind_id);
            foreach(Proyecto_Indicador_Tarea tarea in tareas)
            {
                if(tarea.pit_finalizada)
                    acumulador += total * (tarea.pit_porcentaje / 100);
            };
            return acumulador;
        }

        #endregion
    }
}
