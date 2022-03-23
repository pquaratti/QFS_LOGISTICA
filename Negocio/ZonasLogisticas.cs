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
    public class ZonasLogisticas : NegocioBase<Entidades.ZonaLogistica>
    {


        public ZonasLogisticas(Entidades.App.Token paramToken) : base("zonlog_id", "zonlog_activo", "Zonas_Logisticas", "zonlog")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        #region Funcionalidad

        public override ZonaLogistica ObjetoNuevo()
        {
            Entidades.ZonaLogistica obj = new Entidades.ZonaLogistica();
            obj.zonlog_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.zonlog_id));
            return obj;
        }

        public override void PermiteGuardar(ZonaLogistica obj)
        {

        }

        public override ObjectMessage Save(ZonaLogistica Obj)
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
        public override ZonaLogistica Mapear(DataRow dr)
        {
            Entidades.ZonaLogistica obj = MapearSimple(dr);
            return obj;
        }

        public override ZonaLogistica MapearCompleto(DataRow dr)
        {
            Entidades.ZonaLogistica obj = Mapear(dr);
            return obj;
        }

        public override ZonaLogistica MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static ZonaLogistica MapearStatic(DataRow dr)
        {
            Entidades.ZonaLogistica obj = new Entidades.ZonaLogistica();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Zonas_Logisticas ";

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
