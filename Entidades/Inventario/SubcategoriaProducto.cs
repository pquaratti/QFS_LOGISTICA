using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class SubcategoriaProducto : EntidadBase
    {
        [Key]
        public int subcatpro_id { get; set; }

        [KeyRelation]
        [Display(Name = "Rubro")]
        [Required(ErrorMessage = "Campo requerido")]
        public RubroProducto RubroProducto { get; set; }

        [KeyRelation]
        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Campo requerido")]
        public CategoriaProducto CategoriaProducto { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string subcatpro_nombre { get; set; }

        [Display(Name = "Descripción")]
        public string subcatpro_descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool subcatpro_activo { get; set; }

        public SubcategoriaProducto()
        {
            subcatpro_id = 0;
            subcatpro_activo = true;
            RubroProducto = new RubroProducto();
            CategoriaProducto = new CategoriaProducto();
        }
    }
}
