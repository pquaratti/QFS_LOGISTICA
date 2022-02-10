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
    public class Tipo_Eventos : NegocioBase<Entidades.Tipo_Evento>
    {
        public Tipo_Eventos(Entidades.App.Token paramToken) : base("evet_id", "sin", "Tipo_Evento", "evet")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tipo_Evento Mapear(DataRow dr)
        {
            Entidades.Tipo_Evento obj = MapearSimple(dr);
                               
            return obj;
        }

        public override Tipo_Evento MapearCompleto(DataRow dr)
        {
            Entidades.Tipo_Evento obj = Mapear(dr);
            return obj;
        }

        public override Tipo_Evento MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Tipo_Evento MapearStatic(DataRow dr)
        {
            Entidades.Tipo_Evento obj = new Entidades.Tipo_Evento();
            obj.evet_id = Resources.Validaciones.valNULLINT(dr["evet_id"]);
            obj.evet_contenido = Resources.Validaciones.valNULLString(dr["evet_contenido"]);

            return obj;
        }

        public override Tipo_Evento ObjetoNuevo()
        {
            Entidades.Tipo_Evento obj = new Entidades.Tipo_Evento();
            return obj;
        }

        public override ObjectMessage Save(Tipo_Evento Obj)
        {
            throw new NotImplementedException();
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Evento ";
          
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
