using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Evento : EntidadBase, Entidades.Controls.IAgendable
    {
        public DateTime eve_fecha_fin;

        public int eve_id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Campo requerido")]
        public string eve_titulo { get; set; }

        [Display(Name = "Tipo de Evento")]
        [Required(ErrorMessage = "Campo requerido")]
        public Entidades.Tipo_Evento Tipo { get; set; }

        [Display(Name = "Objetivo")]
        [Required(ErrorMessage = "Campo requerido")]
        public string eve_objetivo { get; set; }

        [Display(Name = "Enlace")]
        public string eve_link { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Campo requerido")]
        public string eve_pass { get; set; }

        [Display(Name = "Duración")]
        [Required(ErrorMessage = "Campo requerido")]
        public int eve_duracion { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime eve_fecha { get; set; }

        [Display(Name = "Fecha de cierre")]
        [Required(ErrorMessage = "Campo requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime eve_fec_cerrado { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Campo requerido")]
        public bool eve_activo { get; set; }

        public bool Finalizado { get; set; }
        public bool Cerrado { get; set; }
        public bool EnCurso { get; set; }

        #region Implementa Agendable
        public DateTime agendable_fecha { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string agendable_titulo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int agendable_id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime agendable_fecha_hasta { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
        public Evento()
        {
            this.eve_id = 0;
            this.Tipo = new Tipo_Evento();
        }

    }

  
}
