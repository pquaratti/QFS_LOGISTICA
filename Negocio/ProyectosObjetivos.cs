using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ProyectosObjetivos : NegocioBase<Entidades.Proyecto_Objetivo>
    {
        public ProyectosObjetivos(Entidades.App.Token paramToken) : base("pryobj_id", "pryobj_activo", "Proyectos_Objetivos", "pryobj")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Proyecto_Objetivo Mapear(DataRow dr)
        {
            Entidades.Proyecto_Objetivo obj = MapearSimple(dr);
            obj.Prioridad = Negocio.TipoPrioridades.MapearStatic(dr);
            return obj;
        }

        public override Proyecto_Objetivo MapearCompleto(DataRow dr)
        {
            Entidades.Proyecto_Objetivo obj = Mapear(dr);
            return obj;
        }

        public override Proyecto_Objetivo MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Proyecto_Objetivo MapearStatic(DataRow dr)
        {
            Entidades.Proyecto_Objetivo obj = new Proyecto_Objetivo();
            obj.pryobj_id = Resources.Validaciones.valNULLINT(dr["pryobj_id"]);
            obj.pryobj_nombre = Resources.Validaciones.valNULLString(dr["pryobj_nombre"]);
            obj.pryobj_descripcion = Resources.Validaciones.valNULLString(dr["pryobj_descripcion"]);
            obj.Prioridad = new Tipo_Prioridad() { tprioridad_id = Resources.Validaciones.valNULLINT(dr["pryobj_tprioridad_id"]) };
            obj.pryobj_fec_ini = Resources.Validaciones.valNULLDateTime(dr["pryobj_fec_ini"]);
            obj.pryobj_fec_ven = Resources.Validaciones.valNULLDateTime(dr["pryobj_fec_ven"]);
            obj.pryobj_foto = Resources.Validaciones.valNULLString(dr["pryobj_foto"]);
            obj.ProyectoVinculado = new Proyecto()
            {
                proy_id = Resources.Validaciones.valNULLINT(dr["pryobj_proy_id"]),
                IdEncriptado = Negocio.App.Security.EncriptarID(dr["pryobj_proy_id"].ToString()),

            };
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.pryobj_id.ToString());
            obj.pryobj_codigo = Resources.Validaciones.valNULLString(dr["pryobj_codigo"]);
            obj.descripcion_combo = "OBJ-" + obj.pryobj_codigo;

            return obj;
        }

        public ObjectMessage ImportFoto(string pathFile, string pryobj_id)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                if (Convert.ToInt32(pryobj_id) > 0)
                {
                    db.SQLExecuteNonQuery("UPDATE Proyectos_Objetivos SET pryobj_foto=@pathFile WHERE pryobj_id=@pryobj_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("pathFile",pathFile),
                    new System.Data.SqlClient.SqlParameter("pryobj_id",pryobj_id)
                });
                    oM.Message = "OK";
                    oM.Success = true;
                }
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return oM;
        }

        public override Proyecto_Objetivo ObjetoNuevo()
        {
            Entidades.Proyecto_Objetivo obj = new Proyecto_Objetivo();
            return obj;
        }

        public override void PermiteGuardar(Proyecto_Objetivo obj)
        {
            
        }

        public override ObjectMessage Save(Proyecto_Objetivo Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Proyectos_Objetivos");
                row["pryobj_proy_id"] = Obj.ProyectoVinculado.proy_id;
                row["pryobj_nombre"] = Obj.pryobj_nombre;
                row["pryobj_descripcion"] = Obj.pryobj_descripcion;
                row["pryobj_tprioridad_id"] = Obj.Prioridad.tprioridad_id;
                row["pryobj_fec_ini"] = Obj.pryobj_fec_ini;
                row["pryobj_fec_ven"] = Obj.pryobj_fec_ven;
                row["pryobj_codigo"] = Obj.pryobj_codigo;
                row["pryobj_foto"] = Obj.pryobj_foto;

                if (Obj.pryobj_id.Equals(0))
                {
                    row["pryobj_activo"] = true;
                    row["pryobj_fec_alta"] = DateTime.Now;
                    row["pryobj_usu_id_alta"] = Token.UserID;
                    Obj.pryobj_id = db.SQLInsert(row, "pryobj_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["pryobj_id"] = Obj.pryobj_id;
                    row["pryobj_fec_mod"] = DateTime.Now;
                    row["pryobj_usu_id_mod"] = Token.UserID;
                    db.SQLUpdate(row, "pryobj_id=@pryobj_id", "pryobj_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("pryobj_id",Obj.pryobj_id)
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

        public ObjectMessage ActualizarImagen(Entidades.Proyecto_Objetivo obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                if (Convert.ToInt32(obj.pryobj_id) > 0)
                {
                    db.SQLExecuteNonQuery("UPDATE Proyectos_Objetivos SET pryobj_foto=@pathFile WHERE pryobj_id=@pryobj_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("pathFile",obj.pryobj_foto),
                    new System.Data.SqlClient.SqlParameter("pryobj_id",obj.pryobj_id)
                });
                    oM.Message = "Datos actualizados.";
                    oM.Success = true;
                }
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return oM;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Proyectos_Objetivos ";
            sQuery += " INNER JOIN Tipo_Prioridad on tprioridad_id=pryobj_tprioridad_id ";
            sQuery += " INNER JOIN Proyectos on proy_id=pryobj_proy_id ";

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

        public string QueryTotales(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = " select pryobj_id, pryobj_nombre,pryobj_descripcion, COUNT(*) as cant_indicadores, SUM(pryind_valor_base) as valor_base, SUM(pryind_valor_meta) as valor_meta, isnull(SUM(pryind_porc_evolucion),0) as sumatoria_porc_evolucion from Proyectos_Objetivos_Indicadores ";
            sQuery += " inner join Proyectos_Objetivos on pryobj_id=poi_pryobj_id  ";
            sQuery += " inner join Proyectos_Indicadores on pryind_id=poi_pryind_id ";

            if ((sWHERE != ""))
            {
                sQuery += " WHERE " + sWHERE;
            }

            sQuery += " group by pryobj_id, pryobj_nombre,pryobj_descripcion";

            return sQuery;
        }

        #region Funcionalidad particular 

        public List<Entidades.Proyecto_Objetivo> ListarPorProyecto(int pry_id)
        {
            List<Entidades.Proyecto_Objetivo> lst = new List<Proyecto_Objetivo>();

            sQuery = QueryDefault("", "proy_id=@proy_id", "") + ";" + QueryTotales("", "pryobj_proy_id=@proy_id", "");
            
            DataSet ds_bus = db.SQLSelectDS(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id",pry_id)
            });

            // MAPEO LA CABECERA
            foreach (DataRow row in ds_bus.Tables[0].Rows)
            {
                Entidades.Proyecto_Objetivo obj = Mapear(row);
                lst.Add(obj);
            }

            // MAPEO EL DETALLE DE TOTALES
            foreach (DataRow row in ds_bus.Tables[1].Rows)
            {
                Entidades.Proyecto_Objetivo obj = lst.Where(w => w.pryobj_id.Equals(Convert.ToInt32(row["pryobj_id"]))).FirstOrDefault();

                if (obj != null)
                {
                    obj.CantidadIndicadores = Resources.Validaciones.valNULLINT(row["cant_indicadores"]);
                    obj.ValorIncial = Resources.Validaciones.valNULLDecimal(row["valor_base"]);
                    obj.ValorMeta = Resources.Validaciones.valNULLDecimal(row["valor_meta"]);
                    obj.PorcentajeEvolucion = ObtenerEvolucionReal(obj.CantidadIndicadores, Resources.Validaciones.valNULLDecimal(row["sumatoria_porc_evolucion"]));
                }
            }

            return lst;
        }

        public decimal ObtenerEvolucionReal(int cantidadIndicadores, decimal sumatoriaPorcentajes)
        {
            decimal _evolucionReal = 0;

            if (cantidadIndicadores > 0)
            {
                if (sumatoriaPorcentajes > 0)
                    _evolucionReal = (sumatoriaPorcentajes * Convert.ToDecimal(100)) / (Convert.ToDecimal(cantidadIndicadores) * Convert.ToDecimal(100));
            }

            return _evolucionReal;
        }

        public string EstadoDelObjetivo (Entidades.Proyecto_Objetivo obj, decimal valor_actual, decimal valor_inicial, decimal valor_meta)
        {

            TimeSpan tiempofaltante = obj.pryobj_fec_ven - DateTime.Now;
            
            if (obj.pryobj_fec_ven < DateTime.Now & valor_actual < valor_meta)
                return "vencido";
            if (valor_actual >= valor_meta)
                return "terminado";
            if (valor_actual < valor_meta & tiempofaltante < new TimeSpan(48, 0, 0))
                return "porvencer";
            if (valor_actual < valor_meta & tiempofaltante >= new TimeSpan(48, 0, 0))
                return "ejecucion";

            return "default";
        }



        #endregion

    }
}
