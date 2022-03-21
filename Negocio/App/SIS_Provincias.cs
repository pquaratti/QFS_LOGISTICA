using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.App;

namespace Negocio.App
{
    public class SIS_Provincias : NegocioBase<Entidades.App.SIS_Provincia>
    {
        public SIS_Provincias(Entidades.App.Token paramToken) : base("prv_id", "prv_activo", "Provincias", "prv") { Token = paramToken; }



        public override Entidades.App.SIS_Provincia MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public override Entidades.App.SIS_Provincia Mapear(DataRow dr)
        {
            Entidades.App.SIS_Provincia obj = MapearSimple(dr);
            return obj;
        }

        public static Entidades.App.SIS_Provincia MapearStatic(DataRow dr)
        {
            Entidades.App.SIS_Provincia obj = new Entidades.App.SIS_Provincia();
            obj.prv_id = Resources.Validaciones.valNULLINT(dr["prv_id"]);
            obj.prv_nombre = Resources.Validaciones.valNULLString(dr["prv_nombre"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.prv_id.ToString());
            return obj;
        }

        public override Entidades.App.SIS_Provincia MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Provincia obj = Mapear(dr);
            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM SIS_Provincias ";
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

        public override Entidades.App.SIS_Provincia ObjetoNuevo()
        {
            return new Entidades.App.SIS_Provincia();
        }

        public override ObjectMessage Save(SIS_Provincia Obj)
        {
            throw new NotImplementedException();
        }
    }
}