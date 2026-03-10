using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class CategoriaProducto : EntidadBase
    {
        [Key]
        public int catpro_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string catpro_nombre { get; set; }

        [Display(Name = "Descripción")]
        public string catpro_descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool catpro_activo { get; set; }

        public CategoriaProducto()
        {
            catpro_id = 0;
            catpro_activo = true;
        }
    }
}
