using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Vistas
{

    public class PreguntaCantidad
    {
        public Encuesta_Pregunta Pregunta { get; set; }
        public int Cantidad { get; set; }
    }

    public class EncuestaResultado
    {

        /*
         Cantidad de usuarios que finalizaron la encuesta.
        - Cantidad de usuarios que están en progreso 
        - Cantidad de usuarios que iniciaron la encuesta en el día.
        - Tiempo promedio en responder una encuesta de inicio a fin.

        - Listado de preguntas con el total de sus respuestas seleccionadas (Cuantas veces por cada respuesta) 

        - Listado de las preguntas con el peor resultado seleccionado.
        - Listado de las preguntas con el mejor resultado seleccionado. 
         */

        public int CantidadUsuariosFinalizaron { get; set; }
        public int CantidadUsuariosEnProgreso { get; set; }
        public int CantidadUsuariosActualmente { get; set; }
        public decimal TiempoPromedio { get; set; }
        public List<Encuesta_Pregunta> Preguntas { get; set; }
        public List<PreguntaCantidad> Peores { get; set; }
        public List<PreguntaCantidad> Mejores { get; set; }

        public Encuesta Encuesta { get; set; }
        public List<PreguntaCantidad> PreguntasCantidades { get; set; }
    }
}
