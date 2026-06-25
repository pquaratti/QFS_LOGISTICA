using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoEstadoStock : EntidadBase
    {

        [KeyAttribute]
        public int testk_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string testk_nombre { get; set; }


        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool testk_activo { get; set; }

        public TipoEstadoStock()
        {
            this.testk_activo = true;
            this.testk_id = 0;

        }

    }


}
