using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoRotacionLogistica : EntidadBase
    {

        [KeyAttribute]
        public int trolog_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string trolog_nombre { get; set; }

        [Display(Name = "Descipción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string trolog_descripcion { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool trolog_activo { get; set; }

        public TipoRotacionLogistica()
        {
            this.trolog_activo = true;
            this.trolog_id = 0;

        }

    }

  
}
