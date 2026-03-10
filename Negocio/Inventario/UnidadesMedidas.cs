using Entidades.App;
using Entidades.Inventario;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio.Inventario
{
    public class UnidadesMedidas : NegocioBase<UnidadMedida>
    {
        public UnidadesMedidas(Token token) : base("unimed_id", "unimed_activo", "Unidades_Medidas", "unimed")
        {
            Token = token;
            TokenFilter = true;
        }

        public override UnidadMedida ObjetoNuevo()
        {
            var obj = new UnidadMedida();
            obj.IdEncriptado = App.Security.EncriptarID("0");
            return obj;
        }

        public override ObjectMessage Save(UnidadMedida Obj)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
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

        public override UnidadMedida Mapear(DataRow dr) => MapearStatic(dr);
        public override UnidadMedida MapearCompleto(DataRow dr) => Mapear(dr);
        public override UnidadMedida MapearSimple(DataRow dr) => Mapear(dr);

        public static UnidadMedida MapearStatic(DataRow dr)
        {
            var obj = new UnidadMedida();
            return MapearReflection(obj, dr);
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "SELECT * FROM Unidades_Medidas ";
            if (sWHERE != "") sQuery += " WHERE " + sWHERE;
            if (sOrderBy != "") sQuery += " ORDER BY " + sOrderBy;
            return sQuery;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            var list = new List<DLLObject>();
            if (agregaDefault) list.Add(new DLLObject() { Value = "0", Text = "Seleccione" });
            foreach (var item in ListarActivos())
                list.Add(new DLLObject() { Value = item.unimed_id.ToString(), Text = item.unimed_nombre });
            return list;
        }
    }
}
