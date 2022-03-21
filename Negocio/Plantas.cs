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
    public class Plantas : NegocioBase<Entidades.Planta>
    {


        public Plantas(Entidades.App.Token paramToken) : base("planta_id", "planta_activo", "Plantas", "planta")
        {
            Token = paramToken;
            TokenFilter = true;

        }

        #region Funcionalidad

        public override Planta ObjetoNuevo()
        {
            Entidades.Planta obj = new Entidades.Planta();
            obj.planta_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(Convert.ToString(obj.planta_id));
            return obj;
        }

        public override void PermiteGuardar(Planta obj)
        {
            if (!Resources.Repositorio.IsNumeric(obj.planta_latitud))
                throw new Exception("La latitud debe ser numérica.");
            if (!Resources.Repositorio.IsNumeric(obj.planta_longitud))
                throw new Exception("La longitud debe ser numérica.");
        }

        public override ObjectMessage Save(Planta Obj)
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


        public List<Entidades.Planta> ListarPorProvinciaLocalidad(string prv_id, string loc_id)
        {

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (Convert.ToInt32(prv_id) > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "prv_id", Value = prv_id });

            if (Convert.ToInt32(loc_id) > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "loc_id", Value = loc_id });

                paramsFilter.Add(new ObjectParameter() { Name = "planta_activo", Value = 1 });

            List<Entidades.Planta> renglones = new List<Entidades.Planta>();

            renglones = ListarConFiltros(paramsFilter);

            return renglones;

        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Mappers
        public override Planta Mapear(DataRow dr)
        {
            Entidades.Planta obj = MapearSimple(dr);
            return obj;
        }

        public override Planta MapearCompleto(DataRow dr)
        {
            Entidades.Planta obj = Mapear(dr);
            return obj;
        }

        public override Planta MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Planta MapearStatic(DataRow dr)
        {
            Entidades.Planta obj = new Entidades.Planta();
            obj = MapearReflection(obj, dr);

            return obj;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Plantas ";
            sQuery += " LEFT JOIN SIS_Provincias on prv_id = planta_prv_id ";
            sQuery += " LEFT JOIN SIS_Localidades on loc_id = planta_loc_id ";

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
