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
    public class ProyectosIndicadores : NegocioBase<Entidades.Proyecto_Indicador>
    {
        public ProyectosIndicadores(Entidades.App.Token paramToken) : base("pryind_id", "pryind_activo", "Proyectos_Indicadores", "prytar")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Proyecto_Indicador Mapear(DataRow dr)
        {
            Entidades.Proyecto_Indicador obj = MapearSimple(dr);
            obj.Proyecto = Negocio.Proyectos.MapearStatic(dr);

            return obj;
        }

        public override Proyecto_Indicador MapearCompleto(DataRow dr)
        {
            Entidades.Proyecto_Indicador obj = Mapear(dr);
            return obj;
        }

        public override Proyecto_Indicador MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Proyecto_Indicador MapearStatic(DataRow dr)
        {
            Entidades.Proyecto_Indicador obj = new Proyecto_Indicador();
            obj.pryind_cerrado = Resources.Validaciones.valNULLBool(dr["pryind_cerrado"]);
            obj.pryind_id = Resources.Validaciones.valNULLINT(dr["pryind_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.pryind_id.ToString());
            obj.pryind_nombre = Resources.Validaciones.valNULLString(dr["pryind_nombre"]);
            obj.pryind_descripcion = Resources.Validaciones.valNULLString(dr["pryind_descripcion"]);
            obj.Proyecto = new Proyecto()
            {
                proy_id = Resources.Validaciones.valNULLINT(dr["pryind_proy_id"]),
                IdEncriptado = Negocio.App.Security.EncriptarID(obj.Proyecto.proy_id.ToString())
            };
            obj.pryind_valor_base = Resources.Validaciones.valNULLDecimal(dr["pryind_valor_base"]);
            obj.pryind_valor_meta = Resources.Validaciones.valNULLDecimal(dr["pryind_valor_meta"]);
            obj.descripcion_combo = obj.pryind_nombre;
            obj.pryind_porc_evolucion = Resources.Validaciones.valNULLDecimal(dr["pryind_porc_evolucion"]);

            return obj;
        }

        public override Proyecto_Indicador ObjetoNuevo()
        {
            Entidades.Proyecto_Indicador obj = new Proyecto_Indicador();
            return obj;
        }

        public override void PermiteGuardar(Proyecto_Indicador obj)
        {
            if (Cerrado(obj.pryind_id))
                throw new Exception("El indicador ya se configuró previamente ");
        }

        private bool Cerrado(int id)
        {
            Proyecto_Indicador ind = ObtenerPorID(Convert.ToString(id));
            return ind.pryind_cerrado;
        }

        public override ObjectMessage Save(Proyecto_Indicador Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Proyectos_Indicadores");
                row["pryind_nombre"] = Obj.pryind_nombre;
                row["pryind_descripcion"] = Obj.pryind_descripcion;
                row["pryind_valor_base"] = Obj.pryind_valor_base;
                row["pryind_valor_meta"] = Obj.pryind_valor_meta;

                if (Obj.pryind_id.Equals(0))
                {
                    row["pryind_proy_id"] = Obj.Proyecto.proy_id;
                    row["pryind_activo"] = true;
                    row["pryind_fec_alta"] = DateTime.Now;
                    row["pryind_usu_id_alta"] = Token.UserID;
                    Obj.pryind_id = db.SQLInsert(row, "pryind_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "pryind_id=@pryind_id", "pryind_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("pryind_id",Obj.pryind_id)
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
            sQuery = "  SELECT * FROM Proyectos_Indicadores ";
            sQuery += " INNER JOIN Proyectos on proy_id = pryind_proy_id ";

            
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

        public List<Entidades.Proyecto_Indicador> ListarPorProyecto(int proy_id)
        {
            List<Entidades.Proyecto_Indicador> lst = new List<Proyecto_Indicador>();

            sQuery = QueryDefault("", "proy_id=@proy_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("proy_id", proy_id)
            });
            
            foreach (DataRow row in dt_bus.Rows)
            {
                Entidades.Proyecto_Indicador obj = Mapear(row);


                lst.Add(obj);
            }


            return lst;
        }

        public Entidades.App.ObjectMessage CerrarIndicador(string id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            try
            {
                Entidades.Proyecto_Indicador datosOrdenCompra = ObtenerPorID(id);

                db.SQLExecuteNonQuery("UPDATE Proyectos_Indicadores SET pryind_cerrado=1 WHERE pryind_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("id",id)
                });

                oM.Success = true;
                oM.Message = "El indicador se configuró exitosamente.";
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        

        #endregion

    }
}
