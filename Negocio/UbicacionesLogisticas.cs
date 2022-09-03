using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class UbicacionesLogisticas : NegocioBase<Entidades.UbicacionLogistica>
    {
        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override UbicacionLogistica Mapear(DataRow dr)
        {
            throw new NotImplementedException();
        }

        public override UbicacionLogistica MapearCompleto(DataRow dr)
        {
            throw new NotImplementedException();
        }

        public override UbicacionLogistica MapearSimple(DataRow dr)
        {
            throw new NotImplementedException();
        }

        public override UbicacionLogistica ObjetoNuevo()
        {
            throw new NotImplementedException();
        }

        public override ObjectMessage Save(UbicacionLogistica Obj)
        {
            throw new NotImplementedException();
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            throw new NotImplementedException();
        }
    }
}
