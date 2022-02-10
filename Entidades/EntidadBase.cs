using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class EntidadBase
    {
        public string IdEncriptado { get; set; }
        public int usu_id_alta { get; set; }
        public int usu_id_baja { get; set; }
        public int usu_id_mod { get; set; }

        public DateTime fec_alta { get; set; }
        public DateTime fec_baja { get; set; }
        public DateTime fec_mod { get; set; }

        public Boolean estado { get; set; }

        public string ReturnFormURL { get; set; }

        public string descripcion_combo { get; set; }
        public bool seleccion { get; set; }
        public string IntegrityValue { get; set; }

    }

    public class KeyRelationAttribute : Attribute
    {
    }
}
