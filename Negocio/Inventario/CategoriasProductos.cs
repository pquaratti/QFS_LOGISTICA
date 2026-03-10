using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio.Inventario
{
    public class CategoriasProductos : NegocioBase<CategoriaProducto>
    {
        public CategoriasProductos(Token token) : base("catpro_id", "catpro_activo", "Categorias_Productos", "catpro")
        {
            Token = token;
            TokenFilter = true;
        }

        public override CategoriaProducto ObjetoNuevo()
        {
            var obj = new CategoriaProducto();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override ObjectMessage Save(CategoriaProducto Obj)
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

        public override CategoriaProducto Mapear(DataRow dr) => MapearStatic(dr);
        public override CategoriaProducto MapearCompleto(DataRow dr) => Mapear(dr);
        public override CategoriaProducto MapearSimple(DataRow dr) => Mapear(dr);

        public static CategoriaProducto MapearStatic(DataRow dr)
        {
            var obj = new CategoriaProducto();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Categorias_Productos ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            var list = new List<DLLObject>();
            if (agregaDefault) list.Add(new DLLObject() { Value = "0", Text = "Seleccione" });
            foreach (var item in ListarActivos())
                list.Add(new DLLObject() { Value = item.catpro_id.ToString(), Text = item.catpro_nombre });
            return list;
        }
    }
}
