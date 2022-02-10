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
    public class Provincias : NegocioBase<Entidades.Provincia>
    {
        public Provincias(Entidades.App.Token paramToken) : base("prv_id", "prv_activo", "Provincias", "prv") { Token = paramToken; }

        public override Entidades.App.ObjectMessage Save(Entidades.Provincia Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();

            DataRow row = db.Estructura("Provincia");
            row["prv_nombre"] = Obj.prv_nombre;
            row["prv_nombre_abreviado"] = Obj.prv_nombre_abreviado;
            
            if (Obj.prv_id.Equals(0))
            {
                row["prv_activo"] = 1;
                db.SQLInsert(row, "prv_id");
            }
            else
            {
                db.SQLUpdate(row, "prv_id=@id", "prv_id", new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("id",Obj.prv_id)
                });
            }

            return oM;
        }

        public override Entidades.Provincia MapearSimple(DataRow dr)
        {
            Entidades.Provincia obj = new Entidades.Provincia();
            obj.prv_id = Resources.Validaciones.valNULLINT(dr["prv_id"]);
            obj.prv_nombre = Resources.Validaciones.valNULLString(dr["prv_nombre"]);
            obj.prv_nombre_abreviado = Resources.Validaciones.valNULLString(dr["prv_nombre_abreviado"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.prv_id.ToString());

            return obj;
        }

        public override Entidades.Provincia Mapear(DataRow dr)
        {
            Entidades.Provincia obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Provincia MapearCompleto(DataRow dr)
        {
            Entidades.Provincia obj = Mapear(dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Provincias ";
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
        
        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Entidades.Provincia ObjetoNuevo()
        {
            return new Entidades.Provincia();
        }
    }
}