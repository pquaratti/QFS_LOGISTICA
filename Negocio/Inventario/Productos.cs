using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Negocio.Inventario
{
    public class Productos : NegocioBase<Producto>
    {
        public Productos(Token token) : base("pro_id", "pro_activo", "Productos", "pro")
        {
            Token = token;
            TokenFilter = true;
        }

        public override Producto ObjetoNuevo()
        {
            var obj = new Producto();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override void PermiteGuardar(Producto obj)
        {
            if (obj.CategoriaProducto == null || obj.CategoriaProducto.catpro_id <= 0)
                throw new Exception("Debe seleccionar una categoría.");
            if (obj.UnidadMedida == null || obj.UnidadMedida.unimed_id <= 0)
                throw new Exception("Debe seleccionar unidad de medida.");
            if (obj.pro_stock_minimo < 0 || obj.pro_stock_maximo < 0 || obj.pro_punto_reposicion < 0)
                throw new Exception("Los parámetros de stock no pueden ser negativos.");
            if (obj.pro_stock_maximo > 0 && obj.pro_stock_minimo > obj.pro_stock_maximo)
                throw new Exception("El stock mínimo no puede ser mayor al stock máximo.");

            var filtros = new List<ObjectParameter>() {
                new ObjectParameter(){ Name="pro_codigo_interno", Value=obj.pro_codigo_interno }
            };
            if (obj.pro_id > 0) filtros.Add(new ObjectParameter() { Name = "pro_id", Value = obj.pro_id });
            var existe = ListarConFiltros(new List<ObjectParameter>() { new ObjectParameter(){ Name="pro_codigo_interno", Value=obj.pro_codigo_interno } })
                .Any(x => x.pro_id != obj.pro_id);
            if (existe) throw new Exception("Ya existe un producto con el código interno indicado.");
        }

        public override ObjectMessage Save(Producto Obj)
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

        public override Producto Mapear(DataRow dr)
        {
            var obj = MapearSimple(dr);
            obj.CategoriaProducto = CategoriasProductos.MapearStatic(dr);
            obj.UnidadMedida = UnidadesMedidas.MapearStatic(dr);
            return obj;
        }
        public override Producto MapearCompleto(DataRow dr) => Mapear(dr);
        public override Producto MapearSimple(DataRow dr) => MapearStatic(dr);

        public static Producto MapearStatic(DataRow dr)
        {
            var obj = new Producto();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Productos ";
            sQuery += "LEFT JOIN Categorias_Productos ON catpro_id = pro_catpro_id ";
            sQuery += "LEFT JOIN Unidades_Medidas ON unimed_id = pro_unimed_id ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            var list = new List<DLLObject>();
            if (agregaDefault) list.Add(new DLLObject() { Value = "0", Text = "Seleccione" });
            foreach (var item in ListarActivos())
                list.Add(new DLLObject() { Value = item.pro_id.ToString(), Text = item.pro_codigo_interno + " - " + item.pro_descripcion_corta });
            return list;
        }
    }
}
