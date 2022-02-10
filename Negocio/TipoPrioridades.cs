using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.App;

namespace Negocio
{
    public class TipoPrioridades : NegocioBase<Entidades.Tipo_Prioridad>
    {
        public TipoPrioridades(Entidades.App.Token paramToken) : base("tprioridad_id", "sin", "Tipo_Prioridad", "tprioridad")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tipo_Prioridad Mapear(DataRow dr)
        {
            Entidades.Tipo_Prioridad obj = MapearSimple(dr);
                               
            return obj;
        }

        public override Tipo_Prioridad MapearCompleto(DataRow dr)
        {
            Entidades.Tipo_Prioridad obj = Mapear(dr);
            return obj;
        }

        public override Tipo_Prioridad MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tipo_Prioridad MapearStatic(DataRow dr)
        {
            Entidades.Tipo_Prioridad obj = new Entidades.Tipo_Prioridad();
            obj.tprioridad_id = Resources.Validaciones.valNULLINT(dr["tprioridad_id"]);
            obj.tprioridad_nombre = Resources.Validaciones.valNULLString(dr["tprioridad_nombre"]);
            obj.tprioridad_orden = Resources.Validaciones.valNULLINT(dr["tprioridad_orden"]);
            obj.tprioridad_css_text = Resources.Validaciones.valNULLString(dr["tprioridad_css_text"]);
            obj.tprioridad_css_background = Resources.Validaciones.valNULLString(dr["tprioridad_css_background"]);
            obj.tprioridad_css = Resources.Validaciones.valNULLString(dr["tprioridad_css"]);

            return obj;
        }
        public override Tipo_Prioridad ObjetoNuevo()
        {
            Entidades.Tipo_Prioridad obj = new Entidades.Tipo_Prioridad();
            return obj;
        }

        public void DatosObligatorios(Entidades.Tipo_Proyecto Obj)
        {
            throw new NotImplementedException();
        }

        public override ObjectMessage Save(Tipo_Prioridad Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Tipo_Prioridad");
                row["tprioridad_id"] = Obj.tprioridad_id;
                row["tprioridad_nombre"] = Obj.tprioridad_nombre;

                if (Obj.tprioridad_id.Equals(0))
                {
                    Obj.tprioridad_id = db.SQLInsert(row, "tprioridad_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "tprioridad_id=@tprioridad_id", "tprioridad_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tprioridad_id",Obj.tprioridad_id)
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
            sQuery = "  SELECT * FROM Tipo_Prioridad ";
          
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

    }
}
