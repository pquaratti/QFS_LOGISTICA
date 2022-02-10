using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Tipo_Proyecto
    {
        public int tproy_id { get; set; }

        [Display(Name = "Tipo de Proyecto")]
        [Required(ErrorMessage = "Campo requerido")]
        public string tproy_nombre { get; set; }

        public Entidades.App.SIS_Organizacion Organizacion { get; set; }

        public Tipo_Proyecto()
        {
            this.tproy_id = 0;
        }

    }
}
