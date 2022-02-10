using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Parametros_Generales : EntidadBase 
    {
        public int pag_id { get; set; }

        [Display(Name = "Parametro")]
        [Required(ErrorMessage = "Campo requerido")]
        public string pag_nombre { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Campo requerido")]
        public decimal pag_valor { get; set; }

        [Display(Name = "Dato")]
      
        public string pag_dato { get; set; }

     
        [Display(Name = "Desde")]
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime pag_desde { get; set; }


        [Display(Name = "Hasta")]
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime pag_hasta { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public Boolean pag_activo { get; set; }

        [Display(Name = "Centro")]
        public int pag_cem_id { get; set; }


        public Parametros_Generales()
        {
            this.pag_desde = DateTime.Now;
            this.pag_hasta = DateTime.Now.AddYears(500);
            this.pag_activo = true;
        }






    }
}
