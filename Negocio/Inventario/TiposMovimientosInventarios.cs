using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio.Inventario
{
    public class TiposMovimientosInventarios : NegocioBase<TipoMovimientoInventario>
    {
        public TiposMovimientosInventarios(Token token) : base("timi_id", "timi_activo", "Tipos_Movimientos_Inventario", "timi")
        {
            Token = token;
            TokenFilter = true;
        }

        public override TipoMovimientoInventario ObjetoNuevo()
        {
            var obj = new TipoMovimientoInventario();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override ObjectMessage Save(TipoMovimientoInventario Obj)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
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

        public override TipoMovimientoInventario Mapear(DataRow dr) => MapearStatic(dr);
        public override TipoMovimientoInventario MapearCompleto(DataRow dr) => Mapear(dr);
        public override TipoMovimientoInventario MapearSimple(DataRow dr) => Mapear(dr);

        public static TipoMovimientoInventario MapearStatic(DataRow dr)
        {
            var obj = new TipoMovimientoInventario();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Tipos_Movimientos_Inventario ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            var list = new List<DLLObject>();
            if (agregaDefault) list.Add(new DLLObject() { Value = "0", Text = "Seleccione" });
            foreach (var item in ListarActivos())
                list.Add(new DLLObject() { Value = item.timi_id.ToString(), Text = item.timi_nombre });
            return list;
        }
    }
}
