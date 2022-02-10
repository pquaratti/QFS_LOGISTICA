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
    public class Tipo_Proyecto_Recursos : NegocioBase<Entidades.Tipo_Proyecto_Recurso>
    {
        public Tipo_Proyecto_Recursos(Entidades.App.Token paramToken) : base("tproyrecurso_id", "sin", "Tipo_Proyecto_Recursos", "tproyrecurso")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tipo_Proyecto_Recurso Mapear(DataRow dr)
        {
            Entidades.Tipo_Proyecto_Recurso obj = MapearSimple(dr);
                               
            return obj;
        }

        public override Tipo_Proyecto_Recurso MapearCompleto(DataRow dr)
        {
            Entidades.Tipo_Proyecto_Recurso obj = Mapear(dr);
            return obj;
        }

        public override Tipo_Proyecto_Recurso MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tipo_Proyecto_Recurso MapearStatic(DataRow dr)
        {
            Entidades.Tipo_Proyecto_Recurso obj = new Entidades.Tipo_Proyecto_Recurso();
            obj.tproyrecurso_id = Resources.Validaciones.valNULLINT(dr["tproyrecurso_id"]);
            obj.tproyrecurso_nombre = Resources.Validaciones.valNULLString(dr["tproyrecurso_nombre"]);

            return obj;
        }

        public override Tipo_Proyecto_Recurso ObjetoNuevo()
        {
            Entidades.Tipo_Proyecto_Recurso obj = new Entidades.Tipo_Proyecto_Recurso();
            return obj;
        }

        public override ObjectMessage Save(Tipo_Proyecto_Recurso Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Tipo_Proyecto_Recursos");
                row["tproyrecurso_id"] = Obj.tproyrecurso_id;
                row["tproyrecurso_nombre"] = Obj.tproyrecurso_nombre;

                if (Obj.tproyrecurso_id.Equals(0))
                {
                    Obj.tproyrecurso_id = db.SQLInsert(row, "tproyrecurso_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "tproyrecurso_id=@tproyrecurso_id", "tproyrecurso_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tproyrecurso_id",Obj.tproyrecurso_id)
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
            sQuery = "  SELECT * FROM Tipo_Proyecto_Recursos ";
          
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
        public List<Entidades.Tipo_Proyecto_Recurso> Listar()
        {
            List<Entidades.Tipo_Proyecto_Recurso> lst = new List<Entidades.Tipo_Proyecto_Recurso>();

            sQuery = QueryDefault("", "", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>());
           
            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

    }
}
