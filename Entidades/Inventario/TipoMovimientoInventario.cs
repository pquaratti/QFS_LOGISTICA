using System.ComponentModel.DataAnnotations;

namespace Entidades.Inventario
{
    public class TipoMovimientoInventario : EntidadBase
    {
        [Key]
        public int timi_id { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string timi_codigo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string timi_nombre { get; set; }

        [Display(Name = "Signo")]
        [Required(ErrorMessage = "Campo requerido")]
        public string timi_signo { get; set; }

        public bool timi_activo { get; set; }

        public TipoMovimientoInventario()
        {
            timi_id = 0;
            timi_activo = true;
        }
    }
}
