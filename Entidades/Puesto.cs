using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Puesto : EntidadBase
    {
        public int puesto_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string puesto_nombre { get; set; }
        public Entidades.App.SIS_Organizacion Organizacion { get; set; }
        public Entidades.App.SIS_Area Area { get; set; }

        public Puesto()
        {
            this.puesto_id = 0;
        }

    }
}
