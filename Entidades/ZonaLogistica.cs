using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ZonaLogistica : EntidadBase
    {

        [KeyAttribute]
        public int zonlog_id { get; set; }

        [KeyRelation]
        [Display(Name = "Depósito")]
        public Entidades.Deposito Deposito { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string zonlog_nombre { get; set; }

        [Display(Name = "Descipción")]
        public string zonlog_descripcion { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo requerido")]
        public string zonlog_codigo{ get; set; }

        [Display(Name = "Posición X")]
        public decimal zonlog_x { get; set; }

        [Display(Name = "Posición Y")]
        public decimal zonlog_y { get; set; }

        [Display(Name = "Largo")]
        public decimal zonlog_largo { get; set; }

        [Display(Name = "Ancho")]
        public decimal zonlog_ancho { get; set; }

        [Display(Name = "Color")]
        public string zonlog_color { get; set; }

        [Display(Name = "Activo")]
        public bool zonlog_activo { get; set; }

        public ZonaLogistica()
        {
            this.zonlog_activo = true;
            this.zonlog_id = 0;
            this.Deposito = new Deposito();
            this.zonlog_color = "#3b82f6";
            this.zonlog_largo = 400;
            this.zonlog_ancho = 300;
        }

    }


}
