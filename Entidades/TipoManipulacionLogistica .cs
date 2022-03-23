using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoManipulacionLogistica : EntidadBase
    {

        [KeyAttribute]
        public int tmanilog_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tmanilog_nombre { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tmanilog_codigo { get; set; }

        [Display(Name = "Descipción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tmanilog_descripcion { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool tmanilog_activo { get; set; }

        public TipoManipulacionLogistica()
        {
            this.tmanilog_activo = true;
            this.tmanilog_id = 0;

        }

    }

  
}
