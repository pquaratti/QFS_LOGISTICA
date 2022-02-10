using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas
{
    public class EncuestaVista
    {
        public Encuesta_Usuario Intento { get; set; }
        public List<Encuesta_Pregunta> Preguntas { get; set; }

        public List<Encuesta_Respuesta> Respuestas { get; set; }

        public List<Encuesta_Usuario_Respuesta> Selecciones { get; set; }

    }
}
