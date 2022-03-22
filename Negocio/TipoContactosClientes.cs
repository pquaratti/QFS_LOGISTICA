using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Entidades.App;

namespace Negocio
{
    public class TipoContactosClientes : NegocioBase<Entidades.TipoContactoCliente>
    {
        public TipoContactosClientes(Entidades.App.Token paramToken) : base("tipcontcli_id", "sin", "Tipo_Contacto_Cliente", "tipcontcli")
        {
            Token = paramToken;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override TipoContactoCliente Mapear(DataRow dr)
        {
            Entidades.TipoContactoCliente obj = MapearSimple(dr);
                               
            return obj;
        }

        public override TipoContactoCliente MapearCompleto(DataRow dr)
        {
            Entidades.TipoContactoCliente obj = Mapear(dr);
            return obj;
        }

        public override TipoContactoCliente MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoContactoCliente MapearStatic(DataRow dr)
        {
            Entidades.TipoContactoCliente obj = new Entidades.TipoContactoCliente();
            obj.tipcontcli_id = Resources.Validaciones.valNULLINT(dr["tipcontcli_id"]);
            obj.tipcontcli_nombre = Resources.Validaciones.valNULLString(dr["tipcontcli_nombre"]);
          

            return obj;
        }
        public override TipoContactoCliente ObjetoNuevo()
        {
            Entidades.TipoContactoCliente obj = new Entidades.TipoContactoCliente();
            obj.tipcontcli_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.tipcontcli_id));
            return obj;
        }

        public void DatosObligatorios(Entidades.Tipo_Proyecto Obj)
        {
            throw new NotImplementedException();
        }

        public override ObjectMessage Save(TipoContactoCliente Obj)
        {
            throw new NotImplementedException();
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Contacto_Cliente ";
          
            if ((sWHERE != ""))
            {
                sQuery += " WHERE " + sWHERE;
            }

            if ((sOrderBy != ""))
            {
                sQuery += " ORDER BY " + sOrderBy;
            }

            return sQuery;
        }

    }
}
