using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Planta : EntidadBase
    {

        [KeyAttribute]
        public int planta_id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo requerido")]
        public string planta_nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string planta_descripcion { get; set; }


        [Display(Name = "Latitud")]
        [Required(ErrorMessage = "Campo requerido")]
        public string planta_latitud { get; set; }


        [Display(Name = "Longitud")]
        [Required(ErrorMessage = "Campo requerido")]
        public string planta_longitud { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Campo requerido")]
        public string planta_direccion { get; set; }

        [KeyRelation]
        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "Campo requerido")]
        public App.SIS_Provincia Provincia { get; set; }

        [KeyRelation]
        [Display(Name = "Localidad")]
        [Required(ErrorMessage = "Campo requerido")]
        public App.SIS_Localidad Localidad { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool planta_activo { get; set; }

        public Planta()
        {
            this.planta_activo = true;
            this.planta_id = 0;

        }

    }

  
}
