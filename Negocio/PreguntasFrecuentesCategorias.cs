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
    public class PreguntasFrecuentesCategorias : NegocioBase<Entidades.Pregunta_Frecuente_Categoria>
    {
       
        public PreguntasFrecuentesCategorias(Entidades.App.Token paramToken) : base("pgfc_id", "sin", "Preguntas_Frecuentes_Categorias", "pgfc")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Pregunta_Frecuente_Categoria Mapear(DataRow dr)
        {
            Entidades.Pregunta_Frecuente_Categoria obj = MapearSimple(dr);
                               
            return obj;
        }

        public override Pregunta_Frecuente_Categoria MapearCompleto(DataRow dr)
        {
            Entidades.Pregunta_Frecuente_Categoria obj = Mapear(dr);
            return obj;
        }

        public override Pregunta_Frecuente_Categoria MapearSimple(DataRow dr)
        {
            Entidades.Pregunta_Frecuente_Categoria obj = new Entidades.Pregunta_Frecuente_Categoria();
            obj.pgfc_id = Resources.Validaciones.valNULLINT(dr["pgfc_id"]);
            obj.pgfc_nombre = Resources.Validaciones.valNULLString(dr["pgfc_nombre"]);
           
            return obj;
        }

        public override Pregunta_Frecuente_Categoria ObjetoNuevo()
        {
            Entidades.Pregunta_Frecuente_Categoria obj = new Entidades.Pregunta_Frecuente_Categoria();
            return obj;
        }

        public void DatosObligatorios(Entidades.Pregunta_Frecuente_Categoria Obj)
        {
            throw new NotImplementedException();
        }

        public override ObjectMessage Save(Pregunta_Frecuente_Categoria Obj)
        {
            throw new NotImplementedException();
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Preguntas_Frecuentes_Categorias ";
          
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
