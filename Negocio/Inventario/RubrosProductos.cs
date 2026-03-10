using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio.Inventario
{
    public class RubrosProductos : NegocioBase<RubroProducto>
    {
        public RubrosProductos(Token token) : base("rubpro_id", "rubpro_activo", "Rubros_Productos", "rubpro")
        {
            Token = token;
            TokenFilter = true;
        }

        public override RubroProducto ObjetoNuevo()
        {
            var obj = new RubroProducto();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override ObjectMessage Save(RubroProducto Obj)
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

        public override RubroProducto Mapear(DataRow dr) => MapearStatic(dr);
        public override RubroProducto MapearCompleto(DataRow dr) => Mapear(dr);
        public override RubroProducto MapearSimple(DataRow dr) => Mapear(dr);

        public static RubroProducto MapearStatic(DataRow dr)
        {
            var obj = new RubroProducto();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Rubros_Productos ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            var list = new List<DLLObject>();
            if (agregaDefault) list.Add(new DLLObject() { Value = "0", Text = "Seleccione" });
            foreach (var item in ListarActivos())
                list.Add(new DLLObject() { Value = item.rubpro_id.ToString(), Text = item.rubpro_nombre });
            return list;
        }
    }
}
