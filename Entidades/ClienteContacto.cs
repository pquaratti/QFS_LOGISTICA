using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ClienteContacto : EntidadBase
    {

        [KeyAttribute]
        public int clicont_id { get; set; }

        [Display(Name = "Contenido")]
        [Required(ErrorMessage = "Campo requerido")]
        public string clicont_contenido { get; set; }

        [Display(Name = "Detalle")]
        [Required(ErrorMessage = "Campo requerido")]
        public string clicont_detalle { get; set; }

        [KeyRelation]
        [Display(Name = "Tipo de Contacto")]
        [Required(ErrorMessage = "Campo requerido")]
        public TipoContactoCliente Tipo { get; set; }

        [KeyRelation]
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Campo requerido")]
        public Cliente Cliente { get; set; }

        public ClienteContacto()
        {

            this.clicont_id = 0;
            this.Cliente = new Cliente();
            this.Tipo = new TipoContactoCliente();

        }

    }

  
}
