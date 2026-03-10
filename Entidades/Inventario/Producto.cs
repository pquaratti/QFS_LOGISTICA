using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class Producto : EntidadBase
    {
        [Key]
        public int pro_id { get; set; }

        [Display(Name = "Código interno")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pro_codigo_interno { get; set; }

        [Display(Name = "Código de barras")]
        public string pro_codigo_barras { get; set; }

        [Display(Name = "Descripción corta")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pro_descripcion_corta { get; set; }

        [Display(Name = "Descripción larga")]
        public string pro_descripcion_larga { get; set; }

        [KeyRelation]
        [Display(Name = "Rubro")]
        [Required(ErrorMessage = "Campo requerido")]
        public RubroProducto RubroProducto { get; set; }

        [KeyRelation]
        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Campo requerido")]
        public CategoriaProducto CategoriaProducto { get; set; }

        [KeyRelation]
        [Display(Name = "Subcategoría")]
        [Required(ErrorMessage = "Campo requerido")]
        public SubcategoriaProducto SubcategoriaProducto { get; set; }

        [KeyRelation]
        [Display(Name = "Unidad de medida")]
        [Required(ErrorMessage = "Campo requerido")]
        public UnidadMedida UnidadMedida { get; set; }

        [Display(Name = "Presentación")]
        public string pro_presentacion { get; set; }

        [Display(Name = "Marca")]
        public string pro_marca { get; set; }

        [Display(Name = "Modelo")]
        public string pro_modelo { get; set; }

        [Display(Name = "Tipo de producto")]
        public string pro_tipo_producto { get; set; }

        [Display(Name = "Requiere trazabilidad")]
        public bool pro_requiere_trazabilidad { get; set; }

        [Display(Name = "Requiere lote")]
        public bool pro_requiere_lote { get; set; }

        [Display(Name = "Requiere vencimiento")]
        public bool pro_requiere_vencimiento { get; set; }

        [Display(Name = "Requiere serie")]
        public bool pro_requiere_serie { get; set; }

        public decimal pro_stock_minimo { get; set; }
        public decimal pro_stock_maximo { get; set; }
        public decimal pro_punto_reposicion { get; set; }

        [Display(Name = "Estado")]
        public string pro_estado { get; set; }

        [Display(Name = "Observaciones")]
        public string pro_observaciones { get; set; }

        public bool pro_activo { get; set; }

        public Producto()
        {
            pro_id = 0;
            pro_activo = true;
            pro_estado = "ACTIVO";
            RubroProducto = new RubroProducto();
            CategoriaProducto = new CategoriaProducto();
            SubcategoriaProducto = new SubcategoriaProducto();
            UnidadMedida = new UnidadMedida();
        }
    }
}
