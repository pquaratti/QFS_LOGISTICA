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
    public class ProyectosObjetivosIndicadores : NegocioBase<Entidades.Proyecto_Objetivo_Indicador>
    {
        public ProyectosObjetivosIndicadores(Entidades.App.Token paramToken) : base("poi_id", "sin", "Proyectos_Objetivos_Indicadores", "pit")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Proyecto_Objetivo_Indicador Mapear(DataRow dr)
        {
            Entidades.Proyecto_Objetivo_Indicador obj = MapearSimple(dr);
            obj.Indicador = Negocio.ProyectosIndicadores.MapearStatic(dr);
            obj.Objetivo = Negocio.ProyectosObjetivos.MapearStatic(dr);

            return obj;
        }

        public override Proyecto_Objetivo_Indicador MapearCompleto(DataRow dr)
        {
            Entidades.Proyecto_Objetivo_Indicador obj = Mapear(dr);
            return obj;
        }

        public override Proyecto_Objetivo_Indicador MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Proyecto_Objetivo_Indicador MapearStatic(DataRow dr)
        {
            Entidades.Proyecto_Objetivo_Indicador obj = new Proyecto_Objetivo_Indicador();
            obj.poi_id = Resources.Validaciones.valNULLINT(dr["poi_id"]);
            obj.Indicador = new Proyecto_Indicador() { pryind_id = Resources.Validaciones.valNULLINT(dr["poi_pryind_id"]) };
            obj.Indicador.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Indicador.pryind_id.ToString());
            obj.Objetivo = new Proyecto_Objetivo() { pryobj_id = Resources.Validaciones.valNULLINT(dr["poi_pryobj_id"]) };
            obj.Objetivo.IdEncriptado = Negocio.App.Security.EncriptarID(obj.Objetivo.pryobj_id.ToString());
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.poi_id.ToString());
            return obj;
        }

        public override Proyecto_Objetivo_Indicador ObjetoNuevo()
        {
            Entidades.Proyecto_Objetivo_Indicador obj = new Proyecto_Objetivo_Indicador();
            return obj;
        }

        public bool IndicadorPreviamenteAsignado(Proyecto_Objetivo_Indicador obj)
        {

            string sQuery = QueryDefault("", " poi_pryind_id = @pryind_id and poi_pryobj_id = @pryobj_id ", "");
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("pryind_id",obj.Indicador.pryind_id),
                    new System.Data.SqlClient.SqlParameter("pryobj_id",obj.Objetivo.pryobj_id)
                });
            return (dt_bus.Rows.Count > 0);
        }

        public override void PermiteGuardar(Proyecto_Objetivo_Indicador obj)
        {

            if (IndicadorPreviamenteAsignado(obj))
                throw new Exception("El indicador fue asignado al objetivo previamente.");

        }

        public override ObjectMessage Save(Proyecto_Objetivo_Indicador Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Proyectos_Objetivos_Indicadores");
                row["poi_pryobj_id"] = Obj.Objetivo.pryobj_id;
                row["poi_pryind_id"] = Obj.Indicador.pryind_id;

                if (Obj.poi_id.Equals(0))
                {
                    Obj.poi_id = db.SQLInsert(row, "poi_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "poi_id=@poi_id", "poi_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("poi_id",Obj.poi_id)
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
            sQuery = "  SELECT * FROM Proyectos_Objetivos_Indicadores ";
            sQuery += " INNER JOIN Proyectos_Objetivos on pryobj_id=poi_pryobj_id ";
            sQuery += " INNER JOIN Proyectos_Indicadores ON pryind_id=poi_pryind_id ";
            
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

       

        public List<Entidades.Proyecto_Objetivo_Indicador> ListarObjetivosPorIndicador(int pryind_id)
        {
            List<Entidades.Proyecto_Objetivo_Indicador> lst = new List<Proyecto_Objetivo_Indicador>();

            sQuery = QueryDefault("", "pryind_id=@pryind_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("pryind_id", pryind_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Proyecto_Objetivo_Indicador obj = Mapear(row);


                lst.Add(obj);
            }


            return lst;
        }



        public List<Entidades.Proyecto_Indicador> ListarIndicadoresPorObjetivo(int pryobj_id)
        {
            List<Entidades.Proyecto_Indicador> lst = new List<Proyecto_Indicador>();

            sQuery = QueryDefault("", "pryobj_id=@pryobj_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("pryobj_id", pryobj_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Proyecto_Objetivo_Indicador obj = Mapear(row);


                lst.Add(obj.Indicador);
            }


            return lst;
        }

        #endregion

        #region Funcionalidad Especial


        public decimal CalcularValorMeta(int pryobj_id)
        {

            decimal valor_meta = 0;
            List<Entidades.Proyecto_Indicador> indicadores = ListarIndicadoresPorObjetivo(pryobj_id);
            foreach (Entidades.Proyecto_Indicador indicador in indicadores)
            {
                valor_meta += indicador.pryind_valor_meta;
            }
            return valor_meta;

        }

        public decimal CalcularValorInicial(int pryobj_id)
        {

            decimal valor_inicial = 0;
            List<Entidades.Proyecto_Indicador> indicadores = ListarIndicadoresPorObjetivo(pryobj_id);
            foreach (Entidades.Proyecto_Indicador indicador in indicadores)
            {
                valor_inicial += indicador.pryind_valor_base;
            }
            return valor_inicial;

        }

        public bool ObjetivosSinIndicadores(string proy_id)
        {
            DataTable dt_bus = CantidadDeIndicadoresPorObjetivoDeProyecto(proy_id);
            bool vacio = false;
            foreach (DataRow row in dt_bus.Rows)
            {
                vacio = (Resources.Validaciones.valNULLINT(row["Cantidad"])==0);
                if(vacio) return true;
            }
            return false;

        }
        private DataTable CantidadDeIndicadoresPorObjetivoDeProyecto(string proy_id)
        {
            string sQuery = "SELECT COUNT(poi_pryind_id) AS Cantidad, poi_pryobj_id as Objetivo FROM Proyectos_Objetivos_Indicadores " +
                "RIGHT JOIN Proyectos_Objetivos on pryobj_id = poi_pryobj_id where pryobj_proy_id = @proy_id group by poi_pryobj_id";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id",proy_id)
            });
            return dt_bus;
        }

        //public Entidades.Vistas.Graficos.PieChart VisualizarPorcentajesPieChart (List<Entidades.Proyecto_Objetivo_Indicador> items)
        //{
        //    List<Entidades.Vistas.Graficos.PieChartElement> lst = new List<Entidades.Vistas.Graficos.PieChartElement>();
        //    foreach(Proyecto_Objetivo_Indicador elemento in items)
        //    {
        //        lst.Add(new Entidades.Vistas.Graficos.PieChartElement(
        //            elemento.Objetivo.pryobj_nombre,
        //            elemento.Indicador.,
        //            100));
        //    }
        //    return new Entidades.Vistas.Graficos.PieChart(lst);
        //}

        //public Entidades.App.ObjectMessage RollBack(string id)
        //{
        //    Entidades.App.ObjectMessage oM = new ObjectMessage();

        //    try
        //    {
        //        Entidades.Proyecto_Objetivo_Indicador datos = ObtenerPorID(id);

        //        db.SQLExecuteNonQuery("UPDATE Proyectos_Objetivos_Indicadores SET poi_porcentaje=null WHERE poi_pryind_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
        //            new System.Data.SqlClient.SqlParameter("id",datos.Indicador.pryind_id)
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        oM.Success = false;
        //        oM.Message = ex.Message;
        //    }

        //    return oM;
        //}


        #endregion
    }
}
