using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoUbicacionLogistica : EntidadBase
    {

        [KeyAttribute]
        public int tubilog_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tubilog_nombre { get; set; }

        [Display(Name = "Descipción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tubilog_descripcion { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool tubilog_activo { get; set; }

        public TipoUbicacionLogistica()
        {
            this.tubilog_activo = true;
            this.tubilog_id = 0;

        }

    }

  
}
