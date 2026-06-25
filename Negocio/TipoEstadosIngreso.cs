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
    public class TipoEstadosIngreso : NegocioBase<Entidades.TipoEstadoIngreso>
    {


        public TipoEstadosIngreso(Entidades.App.Token paramToken) : base("teing_id", "teing_activo", "Tipo_Estado_Ingreso", "teing")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoEstadoIngreso ObjetoNuevo()
        {
            Entidades.TipoEstadoIngreso obj = new Entidades.TipoEstadoIngreso();
            obj.teing_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.teing_id));
            return obj;
        }

        public override void PermiteGuardar(TipoEstadoIngreso obj)
        {

        }

        public override ObjectMessage Save(TipoEstadoIngreso Obj)
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
        public override TipoEstadoIngreso Mapear(DataRow dr)
        {
            Entidades.TipoEstadoIngreso obj = MapearSimple(dr);
            return obj;
        }

        public override TipoEstadoIngreso MapearCompleto(DataRow dr)
        {
            Entidades.TipoEstadoIngreso obj = Mapear(dr);
            return obj;
        }

        public override TipoEstadoIngreso MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoEstadoIngreso MapearStatic(DataRow dr)
        {
            Entidades.TipoEstadoIngreso obj = new Entidades.TipoEstadoIngreso();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Estado_Ingreso ";

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
    }
}
