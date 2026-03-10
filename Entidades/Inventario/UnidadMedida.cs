using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class UnidadMedida : EntidadBase
    {
        [Key]
        public int unimed_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string unimed_nombre { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string unimed_codigo { get; set; }

        [Display(Name = "Activo")]
        public bool unimed_activo { get; set; }

        public UnidadMedida()
        {
            unimed_id = 0;
            unimed_activo = true;
        }
    }
}
