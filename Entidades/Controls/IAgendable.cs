using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Controls
{
    public interface IAgendable
    {
        DateTime agendable_fecha { get; set; }
        string agendable_titulo { get; set; }
        int agendable_id { get; set; }
        DateTime agendable_fecha_hasta { get; set; }
    }
}
