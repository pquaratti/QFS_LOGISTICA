using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoEstadoPedidoSalida : EntidadBase
    {

        [KeyAttribute]
        public int tepsa_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tepsa_nombre { get; set; }


        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool tepsa_activo { get; set; }

        public TipoEstadoPedidoSalida()
        {
            this.tepsa_activo = true;
            this.tepsa_id = 0;

        }

    }


}
