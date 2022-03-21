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
    public class Depositos : NegocioBase<Entidades.Deposito>
    {


        public Depositos(Entidades.App.Token paramToken) : base("depo_id", "depo_activo", "Depositos", "depo")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override Deposito ObjetoNuevo()
        {
            Entidades.Deposito obj = new Entidades.Deposito();
            obj.depo_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.depo_id));
            return obj;
        }

        public override void PermiteGuardar(Deposito obj)
        {

        }

        public override ObjectMessage Save(Deposito Obj)
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

        public List<Entidades.Deposito> ListarDepositosPorPlanta(string planta_id)
        {

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (Convert.ToInt32(planta_id) > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "planta_id", Value = planta_id });

            paramsFilter.Add(new ObjectParameter() { Name = "depo_activo", Value = 1 });
            List<Entidades.Deposito> renglones = new List<Entidades.Deposito>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }


        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mappers
        public override Deposito Mapear(DataRow dr)
        {
            Entidades.Deposito obj = MapearSimple(dr);
            obj.Planta = Negocio.Plantas.MapearStatic(dr);
            return obj;
        }

        public override Deposito MapearCompleto(DataRow dr)
        {
            Entidades.Deposito obj = Mapear(dr);
            return obj;
        }

        public override Deposito MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Deposito MapearStatic(DataRow dr)
        {
            Entidades.Deposito obj = new Entidades.Deposito();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Depositos ";
            sQuery += " LEFT JOIN Plantas ON planta_id = depo_planta_id ";

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
