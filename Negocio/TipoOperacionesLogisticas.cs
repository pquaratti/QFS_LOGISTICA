using Entidades;
using Entidades.App;
using Entidades.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TipoOperacionesLogisticas : NegocioBase<Entidades.TipoOperacionLogistica>
    {


        public TipoOperacionesLogisticas(Entidades.App.Token paramToken) : base("topelog_id", "topelog_activo", "Tipo_Operaciones_Logisticas", "topelog")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoOperacionLogistica ObjetoNuevo()
        {
            Entidades.TipoOperacionLogistica obj = new Entidades.TipoOperacionLogistica();
            obj.topelog_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.topelog_id));
            return obj;
        }

        public override void PermiteGuardar(TipoOperacionLogistica obj)
        {

        }

        public override ObjectMessage Save(TipoOperacionLogistica Obj)
        {
            ObjectMessage oM = new ObjectMessage();
          
            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                oM = SaveReflection(Obj, row, true); 
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }
        #endregion

        #region Consultas y Listados

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mappers
        public override TipoOperacionLogistica Mapear(DataRow dr)
        {
            Entidades.TipoOperacionLogistica obj = MapearSimple(dr);
            return obj;
        }

        public override TipoOperacionLogistica MapearCompleto(DataRow dr)
        {
            Entidades.TipoOperacionLogistica obj = Mapear(dr);
            return obj;
        }

        public override TipoOperacionLogistica MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoOperacionLogistica MapearStatic(DataRow dr)
        {
            Entidades.TipoOperacionLogistica obj = new Entidades.TipoOperacionLogistica();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Operaciones_Logisticas ";

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

        #endregion

        #region Funcionalidad Especial

        #endregion
    }
}
