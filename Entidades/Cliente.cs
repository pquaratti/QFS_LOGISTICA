using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Cliente : EntidadBase
    {

        [KeyAttribute]
        public int cli_id { get; set; }

        [Display(Name = "Razón Social")]
        [Required(ErrorMessage = "Campo requerido")]
        public string cli_razon_social { get; set; }

        [Display(Name = "CUIT")]
        [Required(ErrorMessage = "Campo requerido")]
        public string cli_cuit { get; set; }

        [Display(Name = "MAIL")]
        [Required(ErrorMessage = "Campo requerido")]
        public string cli_mail { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool cli_activo { get; set; }

        public Cliente()
        {
            this.cli_activo = true;
            this.cli_id = 0;

        }

    }

  
}
