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
    public class ColaboradoresSkillsEvolucion : NegocioBase<Entidades.Colaborador_Skill_Evolucion>
    {
        public ColaboradoresSkillsEvolucion(Entidades.App.Token paramToken) : base("cse_id", "sin", "Colaboradores_Skills_Evolucion", "cse")
        {
            Token = paramToken;
        }

        public override Colaborador_Skill_Evolucion MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Colaborador_Skill_Evolucion MapearStatic(DataRow dr)
        {
            Entidades.Colaborador_Skill_Evolucion obj = new Entidades.Colaborador_Skill_Evolucion();
            obj.cse_id = Resources.Validaciones.valNULLINT(dr["cse_id"]);
            obj.cse_descripcion = Resources.Validaciones.valNULLString(dr["cse_descripcion"]);
            obj.cse_fecha = Resources.Validaciones.valNULLDateTime(dr["cse_fecha"]);
            obj.cse_valor = Resources.Validaciones.valNULLINT(dr["cse_valor"]);
            obj.Colaborador = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["cse_usu_id"]) };
            obj.Skill = new Entidades.Skill() { skill_id = Resources.Validaciones.valNULLINT(dr["cse_skill_id"]) };
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.cse_id.ToString());
            return obj;
        }


        public override Colaborador_Skill_Evolucion Mapear(DataRow dr)
        {
            Entidades.Colaborador_Skill_Evolucion obj = MapearSimple(dr);
            obj.Colaborador = Negocio.App.SIS_Usuarios.MapearStatic(dr);
            obj.Skill = Negocio.Skills.MapearStatic(dr);
            return obj;
        }


        public override ObjectMessage Save(Colaborador_Skill_Evolucion Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {

                DataRow row = db.Estructura("Colaboradores_Skills_Evolucion");
                row["cse_usu_id"] = Obj.Colaborador.usu_id;
                row["cse_skill_id"] = Obj.Skill.skill_id;
                row["cse_valor"] = Obj.cse_valor;
                row["cse_descripcion"] = Obj.cse_descripcion;
                row["cse_fecha"] = DateTime.Now;
                row["cse_usu_id_mod"] = Token.UserID;
                Obj.cse_id = db.SQLInsert(row, "cse_id").Valor;
                
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
            sQuery = "  SELECT * FROM Colaboradores_Skills_Evolucion ";
            sQuery += "LEFT JOIN Skills ON skill_id = cse_skill_id ";
            sQuery += "LEFT JOIN SIS_Usuarios ON usu_id = cse_usu_id ";

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

        public List<Entidades.Colaborador_Skill_Evolucion> ListarColaboradoresSkillsEvolucion()
        {
            List<Entidades.Colaborador_Skill_Evolucion> lst = new List<Entidades.Colaborador_Skill_Evolucion>();

            sQuery = QueryDefault("", "", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>());

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public override Colaborador_Skill_Evolucion MapearCompleto(DataRow dr)
        {
            throw new NotImplementedException();
        }

        public override Colaborador_Skill_Evolucion ObjetoNuevo()
        {
            throw new NotImplementedException();
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public ObjectMessage RegistrarEvento(Colaborador_Skill obj, string descript)
        {
            ObjectMessage oM = new ObjectMessage();
            Colaborador_Skill_Evolucion evento = new Colaborador_Skill_Evolucion();
            evento.Colaborador = obj.Colaborador;
            evento.cse_descripcion = descript;
            evento.Skill = obj.Skill;
            evento.cse_valor = obj.colskill_puntaje;
            oM = Save(evento);
            return oM;

        }

        public List<Colaborador_Skill_Evolucion> ListarPorColaborador(string usu_id)
        {
            List<Colaborador_Skill_Evolucion> lst = new List<Colaborador_Skill_Evolucion>();

            sQuery = QueryDefault("", "cse_usu_id=@usu_id", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id", usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst.OrderByDescending(x=>x.cse_fecha).ToList();
        }
    }
}