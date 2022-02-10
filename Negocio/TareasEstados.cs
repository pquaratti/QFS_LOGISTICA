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
    public class TareasEstados : NegocioBase<Entidades.Tarea_Estado>
    {
                
        public enum Estados
        {
            EnProgreso = 1,
            Edicion = 2,
            Pendiente = 3,
            Finalizada = 4

        }
        public TareasEstados(Entidades.App.Token paramToken) : base("tarestado_id", "sin", "Tareas_Estados", "tarestado")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Entidades.Tarea_Estado Mapear(DataRow dr)
        {
            Entidades.Tarea_Estado obj = MapearSimple(dr);
            return obj;
        }

        public override Entidades.Tarea_Estado MapearCompleto(DataRow dr)
        {
            Entidades.Tarea_Estado obj = Mapear(dr);
            return obj;
        }

        public override Entidades.Tarea_Estado MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Entidades.Tarea_Estado MapearStatic(DataRow dr)
        {
            Entidades.Tarea_Estado obj = new Entidades.Tarea_Estado();
            obj.tarestado_id = Resources.Validaciones.valNULLINT(dr["tarestado_id"]);
            obj.tarestado_titulo = Resources.Validaciones.valNULLString(dr["tarestado_titulo"]);
            obj.tarestado_css_text = Resources.Validaciones.valNULLString(dr["tarestado_css_text"]);
            obj.tarestado_css_background = Resources.Validaciones.valNULLString(dr["tarestado_css_background"]);
            obj.tarestado_css_icon = Resources.Validaciones.valNULLString(dr["tarestado_css_icon"]);
            return obj;
        }
        public override Entidades.Tarea_Estado ObjetoNuevo()
        {
            Entidades.Tarea_Estado obj = new Entidades.Tarea_Estado();
            return obj;
        }

        public void DatosObligatorios(Entidades.Tarea_Estado Obj)
        {
            throw new NotImplementedException();
        }


        public override ObjectMessage Save(Entidades.Tarea_Estado Obj)
        {
            throw new NotImplementedException();
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "Select * from Tareas_Estados ";

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
