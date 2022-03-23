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
    public class TipoManipulacionesLogisticas : NegocioBase<Entidades.TipoManipulacionLogistica>
    {


        public TipoManipulacionesLogisticas(Entidades.App.Token paramToken) : base("tmanilog_id", "tmanilog_activo", "Tipo_Manipulaciones_Logisticas", "tmanilog")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoManipulacionLogistica ObjetoNuevo()
        {
            Entidades.TipoManipulacionLogistica obj = new Entidades.TipoManipulacionLogistica();
            obj.tmanilog_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.tmanilog_id));
            return obj;
        }

        public override void PermiteGuardar(TipoManipulacionLogistica obj)
        {

        }

        public override ObjectMessage Save(TipoManipulacionLogistica Obj)
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
        public override TipoManipulacionLogistica Mapear(DataRow dr)
        {
            Entidades.TipoManipulacionLogistica obj = MapearSimple(dr);
            return obj;
        }

        public override TipoManipulacionLogistica MapearCompleto(DataRow dr)
        {
            Entidades.TipoManipulacionLogistica obj = Mapear(dr);
            return obj;
        }

        public override TipoManipulacionLogistica MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoManipulacionLogistica MapearStatic(DataRow dr)
        {
            Entidades.TipoManipulacionLogistica obj = new Entidades.TipoManipulacionLogistica();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Manipulaciones_Logisticas ";

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
