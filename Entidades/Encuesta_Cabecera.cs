using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Encuesta : EntidadBase
    {

        [Display(Name = "Fec.Ini")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime enc_fec_desde { get; set; }

        [Display(Name = "Fec.Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime enc_fec_hasta { get; set; }

        [KeyAttribute]
        public int enc_id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Campo requerido")]
        public string enc_titulo { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Campo requerido")]
        public string enc_descripcion { get; set; }

        [KeyRelationAttribute]
        [Display(Name = "Tipo de Encuesta")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Tipo_Encuesta Tipo { get; set; }
        
        [KeyRelationAttribute]
        [Display(Name = "Área")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.App.SIS_Area Area { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool enc_activo { get; set; }

        [Display(Name = "Fecha de Cierre")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public bool Cerrado { get; set; }

        public Encuesta()
        {
            this.enc_activo = true;
            this.Area = new App.SIS_Area();
            this.Tipo = new Tipo_Encuesta();
        }

    }

  
}
