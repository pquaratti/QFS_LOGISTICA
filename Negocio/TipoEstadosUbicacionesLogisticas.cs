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
    public class TipoEstadosUbicacionesLogisticas : NegocioBase<Entidades.TipoEstadoUbicacionLogistica>
    {


        public TipoEstadosUbicacionesLogisticas(Entidades.App.Token paramToken) : base("teubilog_id", "teubilog_activo", "Tipo_Estado_Ubicacion_Logistica", "teubilog")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoEstadoUbicacionLogistica ObjetoNuevo()
        {
            Entidades.TipoEstadoUbicacionLogistica obj = new Entidades.TipoEstadoUbicacionLogistica();
            obj.teubilog_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.teubilog_id));
            return obj;
        }

        public override void PermiteGuardar(TipoEstadoUbicacionLogistica obj)
        {

        }

        public override ObjectMessage Save(TipoEstadoUbicacionLogistica Obj)
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
        public override TipoEstadoUbicacionLogistica Mapear(DataRow dr)
        {
            Entidades.TipoEstadoUbicacionLogistica obj = MapearSimple(dr);
            return obj;
        }

        public override TipoEstadoUbicacionLogistica MapearCompleto(DataRow dr)
        {
            Entidades.TipoEstadoUbicacionLogistica obj = Mapear(dr);
            return obj;
        }

        public override TipoEstadoUbicacionLogistica MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoEstadoUbicacionLogistica MapearStatic(DataRow dr)
        {
            Entidades.TipoEstadoUbicacionLogistica obj = new Entidades.TipoEstadoUbicacionLogistica();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Estado_Ubicacion_Logistica ";

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
