using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoEstadoIngreso : EntidadBase
    {

        [KeyAttribute]
        public int teing_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string teing_nombre { get; set; }


        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool teing_activo { get; set; }

        public TipoEstadoIngreso()
        {
            this.teing_activo = true;
            this.teing_id = 0;

        }

    }


}
