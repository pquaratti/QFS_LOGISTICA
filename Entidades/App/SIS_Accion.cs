using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Accion : EntidadBase
    {

        public int acc_id { get; set; }

        [Display(Name = "Nombre")]
        public string acc_nombre { get; set; }

        [Display(Name = "Controlador")]
        public string acc_controller { get; set; }

        [Display(Name = "Accion")]
        public string acc_accion { get; set; }

        [Display(Name = "Depende de")]
        public int acc_id_padre { get; set; }
        public Entidades.App.SIS_Accion AccionPadre { get; set; }

        [Display(Name = "Ícono")]
        public string acc_icono { get; set; }

        [Display(Name = "Orden menú")]
        public int acc_orden { get; set; }

        [Display(Name = "Es menú")]
        public bool acc_menu { get; set; }

        [Display(Name = "Descripción")]
        public string acc_descripcion { get; set; }
        public DateTime acc_fec_baja { get; set; }
        public bool VinculadaAPerfil { get; set; }
        public SIS_Accion()
        {
            this.acc_id = 0;
        }


    }
}
