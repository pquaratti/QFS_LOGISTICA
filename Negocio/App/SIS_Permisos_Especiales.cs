using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.App;

namespace Negocio.App
{
    public class SIS_Permisos_Especiales : NegocioBase<Entidades.App.SIS_Permiso_Especial>
    {
        public enum PermisosEspeciales
        {
            ADMINISTRADOR_PLANIFICACIONES = 1,
            ADMINISTRADOR_PRESUPUESTOS = 2,
            ADMINISTRADOR_CREDITO = 3
        }

        public SIS_Permisos_Especiales(Entidades.App.Token paramToken) : base("pee_id", "pee_activo", "SIS_Permisos_Especiales", "pee" ){ Token = paramToken; }

        public override SIS_Permiso_Especial MapearSimple(DataRow dr)
        {
            Entidades.App.SIS_Permiso_Especial obj = new SIS_Permiso_Especial();
            obj.pee_id = Resources.Validaciones.valNULLINT(dr["pee_id"]);
            obj.pee_nombre = Resources.Validaciones.valNULLString(dr["pee_nombre"]);
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.pee_id.ToString());
            obj.pee_activo = Resources.Validaciones.valNULLBool(dr["pee_activo"]);
            return obj;
        }

        public override SIS_Permiso_Especial Mapear(DataRow dr)
        {
            Entidades.App.SIS_Permiso_Especial obj = MapearSimple(dr);
            return obj;
        }

        public override SIS_Permiso_Especial MapearCompleto(DataRow dr)
        {
            Entidades.App.SIS_Permiso_Especial obj = Mapear(dr);

            // Propiedades específicas

            return obj;
        }

        public override Entidades.App.ObjectMessage Save(SIS_Permiso_Especial Obj)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();


            return oM;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM SIS_Permisos_Especiales ";
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



        public Entidades.App.ObjectMessage DeleteLogico(int id)
        {
            Entidades.App.ObjectMessage obj = new Entidades.App.ObjectMessage();
           
            return obj;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override SIS_Permiso_Especial ObjetoNuevo()
        {
            return new SIS_Permiso_Especial();
        }
    }
}
