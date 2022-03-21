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
    public class Localidades : NegocioBase<Entidades.App.SIS_Localidad>
    {
        Negocio.Departamentos negocioDTO;

        public Localidades(Entidades.App.Token paramToken) : base("loc_id", "loc_activo", "Localidades", "loc")
        {
            Token = paramToken;
            negocioDTO = new Departamentos(Token);
        }

        public override Entidades.App.SIS_Localidad MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Entidades.App.SIS_Localidad MapearStatic(DataRow dr)
        {
            Entidades.App.SIS_Localidad obj = new Entidades.App.SIS_Localidad();
            obj.loc_id = Resources.Validaciones.valNULLINT(dr["loc_id"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.loc_id.ToString());
            obj.loc_nombre = Resources.Validaciones.valNULLString(dr["loc_nombre"]);
            return obj;
        }

        public override Entidades.App.SIS_Localidad Mapear(DataRow dr)
        {
            Entidades.App.SIS_Localidad obj = MapearSimple(dr);
            obj. = Negocio.App.SIS_Provincias.MapearStatic(dr);
            return obj;
        }

        public override Entidades.App.SIS_Localidad MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Localidad obj = Mapear(dr);

            // Mapear las propiedades o listas de propiedades que complementan al objeto

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = " SELECT * From SIS_Localidades " +
                "LEFT JOIN SIS_Provincias on prv_id = loc_prv_id ";
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

        public override Entidades.App.SIS_Localidad ObjetoNuevo()
        {
            return new Entidades.App.SIS_Localidad();
        }

        public override ObjectMessage Save(Entidades.App.SIS_Localidad Obj)
        {
            throw new NotImplementedException();
        }
    }
}
