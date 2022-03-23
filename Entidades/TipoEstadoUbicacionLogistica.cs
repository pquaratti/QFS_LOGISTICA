using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoEstadoUbicacionLogistica : EntidadBase
    {

        [KeyAttribute]
        public int teubilog_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string teubilog_nombre { get; set; }


        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool teubilog_activo { get; set; }

        public TipoEstadoUbicacionLogistica()
        {
            this.teubilog_activo = true;
            this.teubilog_id = 0;

        }

    }

  
}
