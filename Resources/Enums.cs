using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class Enums
    {
        public enum TipoEventosTarea
        {
            Creacion = 1,
            Modificacion_de_datos = 2,
            Actualizar_evolucion = 3
        }

        public enum TipoPrioridadesTarea
        {
            Alta = 1,
            Media = 2,
            Baja = 3
        }

        public enum TipoEstadosTarea
        {
            En_Progreso = 1,
            Edicion = 2,
            Pendiente = 3,
            Finalizada = 4
        }


    }
}
