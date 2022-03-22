using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoContactoCliente : EntidadBase
    {

        [KeyAttribute]
        public int tipcontcli_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tipcontcli_nombre { get; set; }

        public TipoContactoCliente()
        {
            this.tipcontcli_id = 0;

        }

    }

  
}
