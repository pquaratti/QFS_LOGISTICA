using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UbicacionLogistica : EntidadBase
    {
        [KeyAttribute]
        public int ubilog_id { get; set; }

        public string ubilog_codigo { get; set; }

        [KeyRelation]
        public Entidades.TipoUbicacionLogistica TipoUbicacion { get; set; }

        [KeyRelation]
        public Entidades.TipoManipulacionLogistica TipoManipulacion { get; set; }

        [KeyRelation]
        public Entidades.TipoRotacionLogistica TipoRotacion { get; set; }

        [KeyRelation]
        public Entidades.TipoEstadoUbicacionLogistica TipoEstado { get; set; }

        [KeyRelation]
        public Entidades.ZonaLogistica Zona { get; set; }

        public string ubilog_secuencia_ruta { get; set; }

        [KeyRelation]
        public Entidades.Planta Planta { get; set; }

        [KeyRelation]
        public Entidades.Deposito Deposito { get; set; }

        [KeyRelation]
        public Entidades.DepositoPasillo Pasillo { get; set; }

        public int ubilog_posicion { get; set; }

        public string ubilog_nivel { get; set; }

        public decimal ubilog_altura { get; set; }

        public decimal ubilog_longitud { get; set; }

        public decimal ubilog_anchura { get; set; }

        public decimal ubilog_capacidad_cubica { get; set; }

        public decimal ubilog_peso_maximo { get; set; }
        public bool ubilog_multiples_articulos { get; set; }
        public bool ubilog_multiples_lotes { get; set; }

        public UbicacionLogistica()
        {
            this.ubilog_id = 0;
            this.TipoUbicacion = new TipoUbicacionLogistica();
            this.TipoManipulacion = new TipoManipulacionLogistica();
            this.TipoRotacion = new TipoRotacionLogistica();
            this.TipoEstado = new TipoEstadoUbicacionLogistica();
            this.Zona = new ZonaLogistica();
            this.Planta = new Planta();
            this.Deposito = new Deposito();
            this.Pasillo = new DepositoPasillo();
        }
    }
}
