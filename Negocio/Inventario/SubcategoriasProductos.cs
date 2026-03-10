using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio.Inventario
{
    public class SubcategoriasProductos : NegocioBase<SubcategoriaProducto>
    {
        public SubcategoriasProductos(Token token) : base("subcatpro_id", "subcatpro_activo", "Subcategorias_Productos", "subcatpro")
        {
            Token = token;
            TokenFilter = true;
        }

        public override SubcategoriaProducto ObjetoNuevo()
        {
            var obj = new SubcategoriaProducto();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override ObjectMessage Save(SubcategoriaProducto Obj)
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

        public override SubcategoriaProducto Mapear(DataRow dr)
        {
            var obj = MapearStatic(dr);
            obj.RubroProducto = RubrosProductos.MapearStatic(dr);
            obj.CategoriaProducto = CategoriasProductos.MapearStatic(dr);
            return obj;
        }
        public override SubcategoriaProducto MapearCompleto(DataRow dr) => Mapear(dr);
        public override SubcategoriaProducto MapearSimple(DataRow dr) => MapearStatic(dr);

        public static SubcategoriaProducto MapearStatic(DataRow dr)
        {
            var obj = new SubcategoriaProducto();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Subcategorias_Productos ";
            sQuery += "LEFT JOIN Rubros_Productos ON rubpro_id = subcatpro_rubpro_id ";
            sQuery += "LEFT JOIN Categorias_Productos ON catpro_id = subcatpro_catpro_id ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            var list = new List<DLLObject>();
            if (agregaDefault) list.Add(new DLLObject() { Value = "0", Text = "Seleccione" });
            foreach (var item in ListarActivos())
                list.Add(new DLLObject() { Value = item.subcatpro_id.ToString(), Text = item.subcatpro_nombre });
            return list;
        }
    }
}
