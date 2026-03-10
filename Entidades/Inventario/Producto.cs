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
        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Campo requerido")]
        public CategoriaProducto CategoriaProducto { get; set; }

        [KeyRelation]
        [Display(Name = "Unidad de medida")]
        [Required(ErrorMessage = "Campo requerido")]
        public UnidadMedida UnidadMedida { get; set; }

        public bool pro_requiere_lote { get; set; }
        public bool pro_requiere_vencimiento { get; set; }
        public bool pro_requiere_serie { get; set; }
        public bool pro_requiere_trazabilidad { get; set; }
        public bool pro_controlado { get; set; }
        public decimal pro_stock_minimo { get; set; }
        public decimal pro_stock_maximo { get; set; }
        public decimal pro_punto_reposicion { get; set; }
        public bool pro_activo { get; set; }

        public Producto()
        {
            pro_id = 0;
            pro_activo = true;
            CategoriaProducto = new CategoriaProducto();
            UnidadMedida = new UnidadMedida();
        }
    }
}
