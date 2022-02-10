using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Mesa_Ayuda : EntidadBase
    {
        public int mesa_id { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Campo requerido")]
        public DateTime mesa_fecha { get; set; }

        [Display(Name = "Consulta / Inconveniente")]
        [Required(ErrorMessage = "Campo requerido")]
        public string mesa_problema { get; set; }

        [Display(Name = "Solución")]
        public string mesa_solucion { get; set; }

        [Display(Name = "Tipo Consulta")]
        public Entidades.Tipo_Consulta_Ayuda TipoConsulta { get; set; }

        public DateTime mesa_fec_cerrada { get; set; }
        public bool Cerrada { get; set; }
        public Entidades.App.SIS_Usuario UsuarioSolicita { get; set; }
        public Entidades.App.SIS_Usuario UsuarioResponsable { get; set; }

        public Mesa_Ayuda()
        {
            this.mesa_id = 0;
            this.TipoConsulta = new Tipo_Consulta_Ayuda();
            this.UsuarioSolicita = new App.SIS_Usuario();
            this.UsuarioResponsable = new App.SIS_Usuario();
        }

    }

}
