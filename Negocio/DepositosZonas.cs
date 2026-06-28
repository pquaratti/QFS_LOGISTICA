using Entidades;
using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Negocio
{
    public class DepositosZonas : NegocioBase<Entidades.DepositoZona>
    {
        public DepositosZonas(Entidades.App.Token paramToken) : base("depzon_id", "depzon_activo", "Depositos_Zonas", "depzon")
        {
            Token = paramToken;
            TokenFilter = true;
        }

        public override DepositoZona ObjetoNuevo()
        {
            DepositoZona obj = new DepositoZona();
            obj.depzon_id = 0;
            obj.IdEncriptado = App.Security.EncriptarID(Convert.ToString(obj.depzon_id));
            return obj;
        }

        public override void PermiteGuardar(DepositoZona obj)
        {
            if (obj.Deposito == null || obj.Deposito.depo_id <= 0)
                throw new Exception("Debe seleccionar un depósito.");
            if (string.IsNullOrWhiteSpace(obj.depzon_codigo))
                throw new Exception("Debe ingresar el código de la zona.");
            if (string.IsNullOrWhiteSpace(obj.depzon_nombre))
                obj.depzon_nombre = obj.depzon_codigo;
            if (obj.depzon_largo <= 0)
                obj.depzon_largo = 520;
            if (obj.depzon_ancho <= 0)
                obj.depzon_ancho = 260;
        }

        public override ObjectMessage Save(DepositoZona Obj)
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

        public List<DepositoZona> ListarPorDeposito(string depositoID)
        {
            int depoID = Resources.Repositorio.IsNumeric(depositoID) ? Convert.ToInt32(depositoID) : Convert.ToInt32(App.Security.DesencriptarID(depositoID));
            List<ObjectParameter> filtros = new List<ObjectParameter>();
            filtros.Add(new ObjectParameter() { Name = "depzon_depo_id", Value = depoID });
            filtros.Add(new ObjectParameter() { Name = "depzon_activo", Value = 1 });
            return ListarConFiltros(filtros, "", "depzon_codigo");
        }

        public List<DepositoZona> ListarConFiltros(List<ObjectParameter> listaFiltros, string cantRegistros, string orden)
        {
            List<DepositoZona> lst = new List<DepositoZona>();
            string sWhere = "";
            List<SqlParameter> lstParamsSQL = new List<SqlParameter>();
            foreach (var itemFiltro in listaFiltros)
            {
                sWhere += itemFiltro.Name + "=@" + itemFiltro.Name + " and ";
                lstParamsSQL.Add(new SqlParameter(itemFiltro.Name, itemFiltro.Value));
            }
            sWhere += " 1=1 ";
            var _queryFilter = GetFilterTokenQuery();
            if (_queryFilter.Trim().Length > 0)
            {
                sWhere += " and " + _queryFilter;
                lstParamsSQL.AddRange(GetFilterTokenParams());
            }
            sQuery = QueryDefault(cantRegistros, sWhere, orden);
            DataTable dt = db.SQLSelect(sQuery, lstParamsSQL);
            foreach (DataRow row in dt.Rows) lst.Add(Mapear(row));
            return lst;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false) { throw new NotImplementedException(); }
        public override DepositoZona Mapear(DataRow dr) { DepositoZona obj = MapearSimple(dr); obj.Deposito = Depositos.MapearStatic(dr); return obj; }
        public override DepositoZona MapearCompleto(DataRow dr) { return Mapear(dr); }
        public override DepositoZona MapearSimple(DataRow dr) { return MapearStatic(dr); }
        public static DepositoZona MapearStatic(DataRow dr) { DepositoZona obj = new DepositoZona(); obj = MapearReflection(obj, dr); return obj; }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT " + sTOP + " * FROM Depositos_Zonas ";
            sQuery += " LEFT JOIN Depositos ON depo_id = depzon_depo_id ";
            if ((sWHERE != "")) sQuery += " WHERE " + sWHERE;
            if ((sOrderBy != "")) sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }
    }
}