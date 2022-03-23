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
    public class TipoRotacionesLogisticas : NegocioBase<Entidades.TipoRotacionLogistica>
    {


        public TipoRotacionesLogisticas(Entidades.App.Token paramToken) : base("trolog_id", "trolog_activo", "Tipo_Rotaciones_Logisticas", "trolog")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override TipoRotacionLogistica ObjetoNuevo()
        {
            Entidades.TipoRotacionLogistica obj = new Entidades.TipoRotacionLogistica();
            obj.trolog_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.trolog_id));
            return obj;
        }

        public override void PermiteGuardar(TipoRotacionLogistica obj)
        {

        }

        public override ObjectMessage Save(TipoRotacionLogistica Obj)
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
        public override TipoRotacionLogistica Mapear(DataRow dr)
        {
            Entidades.TipoRotacionLogistica obj = MapearSimple(dr);
            return obj;
        }

        public override TipoRotacionLogistica MapearCompleto(DataRow dr)
        {
            Entidades.TipoRotacionLogistica obj = Mapear(dr);
            return obj;
        }

        public override TipoRotacionLogistica MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static TipoRotacionLogistica MapearStatic(DataRow dr)
        {
            Entidades.TipoRotacionLogistica obj = new Entidades.TipoRotacionLogistica();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tipo_Rotaciones_Logisticas ";

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
