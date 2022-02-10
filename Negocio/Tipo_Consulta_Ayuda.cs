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
    public class Tipo_Consulta_Ayuda : NegocioBase<Entidades.Tipo_Consulta_Ayuda>
    {
        public enum Tipos
        {
            SISTEMA = 1,
            GESTION = 2,
            OTRA = 3
        }

        public Tipo_Consulta_Ayuda(Entidades.App.Token paramToken) : base("tipoconsulta_id", "SIN", "Tipo_Consultas_Ayuda", "tipoconsulta")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Entidades.Tipo_Consulta_Ayuda Mapear(DataRow dr)
        {
            Entidades.Tipo_Consulta_Ayuda obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Tipo_Consulta_Ayuda MapearCompleto(DataRow dr)
        {
            Entidades.Tipo_Consulta_Ayuda obj = Mapear(dr);
            return obj;
        }

        public override Entidades.Tipo_Consulta_Ayuda MapearSimple(DataRow dr)
        {
            Entidades.Tipo_Consulta_Ayuda obj = new Entidades.Tipo_Consulta_Ayuda();
            obj.tipoconsulta_id = Resources.Validaciones.valNULLINT(dr["tipoconsulta_id"]);
            obj.tipoconsulta_nombre = Resources.Validaciones.valNULLString(dr["tipoconsulta_nombre"]);
            return obj;
        }

        public override Entidades.Tipo_Consulta_Ayuda ObjetoNuevo()
        {
            Entidades.Tipo_Consulta_Ayuda obj = new Entidades.Tipo_Consulta_Ayuda();
            return obj;
        }

        public override ObjectMessage Save(Entidades.Tipo_Consulta_Ayuda Obj)
        {
            throw new NotImplementedException();
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  Select * from Tipo_Consultas_Ayuda ";
            
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
