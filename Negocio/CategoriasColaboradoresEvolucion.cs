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
    public class CategoriasColaboradoresEvolucion : NegocioBase<Entidades.Categoria_Colaborador_Evolucion>
    {
        public CategoriasColaboradoresEvolucion(Entidades.App.Token paramToken) : base("cce_id", "sin", "Categorias_Colaboradores_Evolucion", "cce")
        {
            Token = paramToken;
        }

        public override Categoria_Colaborador_Evolucion MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Categoria_Colaborador_Evolucion MapearStatic(DataRow dr)
        {
            Entidades.Categoria_Colaborador_Evolucion obj = new Entidades.Categoria_Colaborador_Evolucion();
            obj.cce_id = Resources.Validaciones.valNULLINT(dr["cce_id"]);
            obj.cce_fecha = Resources.Validaciones.valNULLDateTime(dr["cce_fecha"]);
            obj.Colaborador = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["cce_usu_id"]) };
            obj.Categoria = new Entidades.Categoria() { cat_id = Resources.Validaciones.valNULLINT(dr["cce_cat_id"]) };
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.cce_id.ToString());
            return obj;
        }


        public override Categoria_Colaborador_Evolucion Mapear(DataRow dr)
        {
            Entidades.Categoria_Colaborador_Evolucion obj = MapearSimple(dr);
            obj.Colaborador = Negocio.App.SIS_Usuarios.MapearStatic(dr);
            obj.Categoria = Negocio.Categorias.MapearStatic(dr);
            return obj;
        }

        public override ObjectMessage Save(Categoria_Colaborador_Evolucion Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Categorias_Colaboradores_Evolucion");
                row["cce_usu_id"] = Obj.Colaborador.usu_id;
                row["cce_cat_id"] = Obj.Categoria.cat_id;
                row["cce_fecha"] = DateTime.Now;
                row["cce_usu_id_mod"] = Token.UserID;
                row["cce_doc"] = Obj.cce_doc;
                Obj.cce_id = db.SQLInsert(row, "cce_id").Valor;

                oM.Message = "Datos ingresados";
                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }
       
        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Categorias_Colaboradores_Evolucion ";
            sQuery += "LEFT JOIN Categorias ON cat_id = cce_cat_id ";
            sQuery += "LEFT JOIN SIS_Usuarios ON usu_id = cce_usu_id ";

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

        public List<Entidades.Categoria_Colaborador_Evolucion> ListarColaboradoresCategoriasEvolucion()
        {
            List<Entidades.Categoria_Colaborador_Evolucion> lst = new List<Entidades.Categoria_Colaborador_Evolucion>();

            sQuery = QueryDefault("", "", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>());

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public override Categoria_Colaborador_Evolucion MapearCompleto(DataRow dr)
        {
            throw new NotImplementedException();
        }

        public override Categoria_Colaborador_Evolucion ObjetoNuevo()
        {
            throw new NotImplementedException();
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public ObjectMessage RegistrarEvento(SIS_Usuario obj, string doc)
        {
            ObjectMessage oM = new ObjectMessage();
            Categoria_Colaborador_Evolucion evento = new Categoria_Colaborador_Evolucion();
            evento.Colaborador = obj;
            evento.cce_doc = doc;
            evento.Categoria = obj.Categoria;
            oM = Save(evento);
            return oM;

        }

        public List<Categoria_Colaborador_Evolucion> ListarPorColaborador(string usu_id)
        {
            List<Categoria_Colaborador_Evolucion> lst = new List<Categoria_Colaborador_Evolucion>();

            sQuery = QueryDefault("", "cce_usu_id=@usu_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id", usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst.OrderByDescending(x => x.cce_fecha).ToList();
        }

    }
}