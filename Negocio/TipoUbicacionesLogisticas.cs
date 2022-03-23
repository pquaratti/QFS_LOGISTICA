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
    public class TipoUbicacionesLogisticas : NegocioBase<Entidades.TipoUbicacionLogistica>
    {


        public TipoUbicacionesLogisticas(Entidades.App.Token paramToken) : base("tubilog_id", "tubilog_activo", "Tipo_Ubicaciones_Logisticas", "tubilog")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoUbicacionLogistica ObjetoNuevo()
        {
            Entidades.TipoUbicacionLogistica obj = new Entidades.TipoUbicacionLogistica();
            obj.tubilog_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.tubilog_id));
            return obj;
        }

        public override void PermiteGuardar(TipoUbicacionLogistica obj)
        {

        }

        public override ObjectMessage Save(TipoUbicacionLogistica Obj)
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
        public override TipoUbicacionLogistica Mapear(DataRow dr)
        {
            Entidades.TipoUbicacionLogistica obj = MapearSimple(dr);
            return obj;
        }

        public override TipoUbicacionLogistica MapearCompleto(DataRow dr)
        {
            Entidades.TipoUbicacionLogistica obj = Mapear(dr);
            return obj;
        }

        public override TipoUbicacionLogistica MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoUbicacionLogistica MapearStatic(DataRow dr)
        {
            Entidades.TipoUbicacionLogistica obj = new Entidades.TipoUbicacionLogistica();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Ubicaciones_Logisticas ";

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
