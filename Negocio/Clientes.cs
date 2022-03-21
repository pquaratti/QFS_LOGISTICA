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
    public class Clientes : NegocioBase<Entidades.Cliente>
    {


        public Clientes(Entidades.App.Token paramToken) : base("cli_id", "cli_activo", "Clientes", "cli")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override Cliente ObjetoNuevo()
        {
            Entidades.Cliente obj = new Entidades.Cliente();
            obj.cli_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.cli_id));
            return obj;
        }

        public override void PermiteGuardar(Cliente obj)
        {
            if (!Resources.Repositorio.ValidaMail(obj.cli_mail))
                throw new Exception("El Mail no es válido.");
            if ((obj.cli_razon_social.Length < 1))
                throw new Exception("Debe ingresar una Razón Social.");
        }

        public override ObjectMessage Save(Cliente Obj)
        {
            ObjectMessage oM = new ObjectMessage();
          
            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura(nombreTablaPrincipal);
                oM = SaveReflection(Obj, row, true); 
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

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mappers
        public override Cliente Mapear(DataRow dr)
        {
            Entidades.Cliente obj = MapearSimple(dr);
            return obj;
        }

        public override Cliente MapearCompleto(DataRow dr)
        {
            Entidades.Cliente obj = Mapear(dr);
            return obj;
        }

        public override Cliente MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Cliente MapearStatic(DataRow dr)
        {
            Entidades.Cliente obj = new Entidades.Cliente();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Clientes ";

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
