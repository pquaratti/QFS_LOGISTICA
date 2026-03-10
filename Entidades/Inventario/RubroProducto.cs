using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class RubroProducto : EntidadBase
    {
        [Key]
        public int rubpro_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string rubpro_nombre { get; set; }

        [Display(Name = "Descripción")]
        public string rubpro_descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool rubpro_activo { get; set; }

        public RubroProducto()
        {
            rubpro_id = 0;
            rubpro_activo = true;
        }
    }
}
