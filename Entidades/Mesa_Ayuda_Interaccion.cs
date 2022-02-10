using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Mesa_Ayuda_Interaccion : EntidadBase
    {
        public int mesainteraccion_id { get; set; }
        public Entidades.Mesa_Ayuda DatosAyuda { get; set; }
        public Entidades.App.SIS_Usuario UsuarioInteraccion { get; set; }
        public DateTime mesainteraccion_fecha { get; set; }

        [Display(Name = "Mensaje")]
        [Required(ErrorMessage = "Campo requerido")]
        public string mesainteraccion_mensaje { get; set; }

        public Mesa_Ayuda_Interaccion()
        {
            this.mesainteraccion_id = 0;
            this.DatosAyuda = new Mesa_Ayuda();
            this.UsuarioInteraccion = new App.SIS_Usuario();
        }

    }
}
