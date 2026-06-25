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
    public class TipoEstadosStock : NegocioBase<Entidades.TipoEstadoStock>
    {


        public TipoEstadosStock(Entidades.App.Token paramToken) : base("testk_id", "testk_activo", "Tipo_Estado_Stock", "testk")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoEstadoStock ObjetoNuevo()
        {
            Entidades.TipoEstadoStock obj = new Entidades.TipoEstadoStock();
            obj.testk_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.testk_id));
            return obj;
        }

        public override void PermiteGuardar(TipoEstadoStock obj)
        {

        }

        public override ObjectMessage Save(TipoEstadoStock Obj)
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
        public override TipoEstadoStock Mapear(DataRow dr)
        {
            Entidades.TipoEstadoStock obj = MapearSimple(dr);
            return obj;
        }

        public override TipoEstadoStock MapearCompleto(DataRow dr)
        {
            Entidades.TipoEstadoStock obj = Mapear(dr);
            return obj;
        }

        public override TipoEstadoStock MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoEstadoStock MapearStatic(DataRow dr)
        {
            Entidades.TipoEstadoStock obj = new Entidades.TipoEstadoStock();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Estado_Stock ";

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
    }
}
