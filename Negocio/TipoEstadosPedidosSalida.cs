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
    public class TipoEstadosPedidosSalida : NegocioBase<Entidades.TipoEstadoPedidoSalida>
    {


        public TipoEstadosPedidosSalida(Entidades.App.Token paramToken) : base("tepsa_id", "tepsa_activo", "Tipo_Estado_Pedido_Salida", "tepsa")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoEstadoPedidoSalida ObjetoNuevo()
        {
            Entidades.TipoEstadoPedidoSalida obj = new Entidades.TipoEstadoPedidoSalida();
            obj.tepsa_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.tepsa_id));
            return obj;
        }

        public override void PermiteGuardar(TipoEstadoPedidoSalida obj)
        {

        }

        public override ObjectMessage Save(TipoEstadoPedidoSalida Obj)
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
        public override TipoEstadoPedidoSalida Mapear(DataRow dr)
        {
            Entidades.TipoEstadoPedidoSalida obj = MapearSimple(dr);
            return obj;
        }

        public override TipoEstadoPedidoSalida MapearCompleto(DataRow dr)
        {
            Entidades.TipoEstadoPedidoSalida obj = Mapear(dr);
            return obj;
        }

        public override TipoEstadoPedidoSalida MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoEstadoPedidoSalida MapearStatic(DataRow dr)
        {
            Entidades.TipoEstadoPedidoSalida obj = new Entidades.TipoEstadoPedidoSalida();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Estado_Pedido_Salida ";

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
