using Entidades;
using Entidades.App;
using Entidades.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ClientesContactos : NegocioBase<Entidades.ClienteContacto>
    {


        public ClientesContactos(Entidades.App.Token paramToken) : base("clicont_id", "sin", "Clientes_Contactos", "clicont")
        {
            Token = paramToken;
        }

        #region Funcionalidad

        public override ClienteContacto ObjetoNuevo()
        {
            Entidades.ClienteContacto obj = new Entidades.ClienteContacto();
            obj.clicont_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.clicont_id));
            return obj;
        }

        public override void PermiteGuardar(ClienteContacto obj)
        {

        }

        public override ObjectMessage Save(ClienteContacto Obj)
        {
            ObjectMessage oM = new ObjectMessage();
          
            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                oM = SaveReflection(Obj, row); 
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }
        #endregion

        #region Consultas y Listados

        public List<Entidades.ClienteContacto> ListarClientesContactosPorClienteTipo(string cli_id ="0", string tipcontcli_id="0")
        {

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (Convert.ToInt32(cli_id) > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "cli_id", Value = cli_id });

            if (Convert.ToInt32(tipcontcli_id) > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "tipcontcli_id", Value = tipcontcli_id });

            paramsFilter.Add(new ObjectParameter() { Name = "cli_activo", Value = 1 });
            List<Entidades.ClienteContacto> renglones = new List<Entidades.ClienteContacto>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mappers
        public override ClienteContacto Mapear(DataRow dr)
        {
            Entidades.ClienteContacto obj = MapearSimple(dr);
            obj.Cliente = Negocio.Clientes.MapearStatic(dr);
            obj.Tipo = Negocio.TipoContactosClientes.MapearStatic(dr);
            return obj;
        }

        public override ClienteContacto MapearCompleto(DataRow dr)
        {
            Entidades.ClienteContacto obj = Mapear(dr);
            return obj;
        }

        public override ClienteContacto MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static ClienteContacto MapearStatic(DataRow dr)
        {
            Entidades.ClienteContacto obj = new Entidades.ClienteContacto();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Clientes_Contactos ";
            sQuery += " LEFT JOIN Clientes ON cli_id = clicont_cli_id ";
            sQuery += " LEFT JOIN Tipo_Contacto_Cliente ON tipcontcli_id = clicont_tipcontcli_id ";

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

        #endregion

        #region Funcionalidad Especial

        #endregion
    }
}
