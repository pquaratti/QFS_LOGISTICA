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
    public class ColaboradoresSkills : NegocioBase<Entidades.Colaborador_Skill>
    {
        public ColaboradoresSkills(Entidades.App.Token paramToken) : base("colskill_id", "sin", "Colaboradores_Skills", "colskill")
        {
            Token = paramToken;
        }

        #region Colaboradores_Skills

        public override Colaborador_Skill MapearSimple(DataRow dr)
        {
            return MapearStatic(dr);
        }

        public static Colaborador_Skill MapearStatic(DataRow dr)
        {
            Entidades.Colaborador_Skill obj = new Entidades.Colaborador_Skill();
            obj.colskill_id = Resources.Validaciones.valNULLINT(dr["colskill_id"]);
            obj.colskill_puntaje = Resources.Validaciones.valNULLINT(dr["colskill_puntaje"]);
            obj.Colaborador = new Entidades.App.SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["colskill_usu_id"]) };
            obj.Skill = new Entidades.Skill() { skill_id = Resources.Validaciones.valNULLINT(dr["colskill_skill_id"]) };
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.colskill_id.ToString());
            return obj;
        }


        public override Colaborador_Skill Mapear(DataRow dr)
        {
            Entidades.Colaborador_Skill obj = MapearSimple(dr);
            obj.Colaborador = Negocio.App.SIS_Usuarios.MapearStatic(dr);
            return obj;
        }


        private void PermiteGuardar(Colaborador_Skill obj)
        {
            if (obj.colskill_puntaje>100 | obj.colskill_puntaje<0)
                throw new Exception("El puntaje asignado no es válido");
        }

        public ObjectMessage Save(Colaborador_Skill Obj, string descript)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                PermiteGuardar(Obj);
                DataRow row = db.Estructura("Colaboradores_Skills");
                row["colskill_usu_id"] = Obj.Colaborador.usu_id;
                row["colskill_skill_id"] = Obj.Skill.skill_id;
                row["colskill_puntaje"] = Obj.colskill_puntaje;

                if (!ExisteRegistro(Obj))
                {
                    Obj.colskill_id = db.SQLInsert(row, "colskill_id").Valor;
                    Obj.IdEncriptado = Negocio.App.Security.EncriptarID(Obj.colskill_id.ToString());
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    db.SQLUpdate(row, "colskill_usu_id=@colskill_usu_id and colskill_skill_id=@colskill_skill_id", "colskill_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("colskill_skill_id",Obj.Skill.skill_id),
                        new System.Data.SqlClient.SqlParameter("colskill_usu_id",Obj.Colaborador.usu_id)
                    });

                    oM.Message = "Datos actualizados";
                    oM = new Negocio.ColaboradoresSkillsEvolucion(Token).RegistrarEvento(Obj, descript);
                }

                oM.Success = true;
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        protected bool ExisteRegistro(Entidades.Colaborador_Skill obj)
        {
            string sQuery = "SELECT COUNT(*) AS registro FROM Colaboradores_Skills WHERE colskill_usu_id=@usu_id AND colskill_skill_id=@skill_id ";
            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id",obj.Colaborador.usu_id),
                new System.Data.SqlClient.SqlParameter("skill_id",obj.Skill.skill_id)
            });
            return Resources.Validaciones.valNULLINT(dt_bus.Rows[0]["registro"]) > 0;
        }



        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Colaboradores_Skills " +
                "LEFT JOIN Skills ON skill_id = colskill_skill_id " +
                "LEFT JOIN SIS_Usuarios ON usu_id = colskill_usu_id ";

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

        public List<Entidades.Colaborador_Skill> ListarColaboradoresSkills()
        {
            List<Entidades.Colaborador_Skill> lst = new List<Entidades.Colaborador_Skill>();

            sQuery = QueryDefault("", "", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>());

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<Entidades.Colaborador_Skill> ListarColaboradoresPorSkill(int skill_id)
        {
            List<Entidades.Colaborador_Skill> lst = new List<Entidades.Colaborador_Skill>();

            sQuery = QueryDefault("", " skill_id = @skill_id ", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("skill_id",skill_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<Entidades.Colaborador_Skill> ListarSkillsPorColaborador(int usu_id)
        {
            List<Entidades.Colaborador_Skill> lst = new List<Entidades.Colaborador_Skill>();

            sQuery = QueryDefault("", " usu_id = @usu_id ", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("usu_id",usu_id)
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }
            return lst;
        }

        public List<SIS_Usuario> ListarPorSkillCategoriaTexto(int skill, int categoria, string textoBusqueda = "")
        {
            List<SIS_Usuario> lst = new List<SIS_Usuario>();

            string _where = " 1=1 ";

            if (skill > 0)
                _where += " and colskill_skill_id=@skill ";


            if (categoria > 0)
                _where += " and usu_cat_id=@categoria ";

            if (textoBusqueda.Trim().Length > 0)
            {
                if (Resources.Repositorio.IsNumeric(textoBusqueda))
                    _where += " and usu_documento like @searchText ";
                else
                    _where += " and (usu_apellido like @textoBusqueda OR usu_nombre like @textoBusqueda) ";
            }

            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("skill",skill),
                new System.Data.SqlClient.SqlParameter("categoria",categoria),
                new System.Data.SqlClient.SqlParameter("textoBusqueda", "%" + textoBusqueda + "%")
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(MapearCompleto(row).Colaborador);
            }

            return lst;
        }

        public override Colaborador_Skill MapearCompleto(DataRow dr)
        {
            Entidades.Colaborador_Skill obj = MapearSimple(dr);
            obj.Colaborador = new Negocio.App.SIS_Usuarios(Token).Mapear(dr);
            return obj;
        }

        public override Colaborador_Skill ObjetoNuevo()
        {
            Entidades.Colaborador_Skill obj = new Colaborador_Skill();
            obj.colskill_id = 0;
            obj.IdEncriptado = Negocio.App.Security.EncriptarID(obj.colskill_id.ToString());
            return obj;
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override ObjectMessage Save(Colaborador_Skill Obj)
        {
            throw new NotImplementedException();
        }


        public ObjectMessage PuntajeActual(string skill_id, string usu_id)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                List<System.Data.SqlClient.SqlParameter> lstParam = new List<System.Data.SqlClient.SqlParameter>();
                lstParam.Add(new System.Data.SqlClient.SqlParameter("usu_id", usu_id));
                lstParam.Add(new System.Data.SqlClient.SqlParameter("skill_id", skill_id));

                DataTable dt = db.SQLSelect(QueryDefault("", " colskill_usu_id=@usu_id AND colskill_skill_id=@skill_id ", ""), lstParam);

                if (dt.Rows.Count > 0)
                {
                    oM.Success = true;
                    oM.ObjectRelation = Resources.Validaciones.valNULLINT(dt.Rows[0]["colskill_puntaje"]);              
                }
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
                oM.ObjectRelation = "-1";
            }

            return oM;
        }

        #endregion

    }
}