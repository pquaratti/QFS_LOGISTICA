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
    public class PreguntasFrecuentes : NegocioBase<Entidades.Pregunta_Frecuente>
    {
        Negocio.PreguntasFrecuentesCategorias negocioPGFC;

        public PreguntasFrecuentes(Entidades.App.Token paramToken) : base("pgf_id", "pgf_activo", "Preguntas_Frecuentes", "pgf")
        {
            Token = paramToken;
            negocioPGFC = new PreguntasFrecuentesCategorias(Token);
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Pregunta_Frecuente Mapear(DataRow dr)
        {
            Entidades.Pregunta_Frecuente obj = MapearSimple(dr);
            obj.Categoria = negocioPGFC.MapearSimple(dr);
                     
            return obj;
        }

        public override Pregunta_Frecuente MapearCompleto(DataRow dr)
        {
            Entidades.Pregunta_Frecuente obj = Mapear(dr);
            return obj;
        }

        public override Pregunta_Frecuente MapearSimple(DataRow dr)
        {
            Entidades.Pregunta_Frecuente obj = new Entidades.Pregunta_Frecuente();
            obj.pgf_id = Resources.Validaciones.valNULLINT(dr["pgf_id"]);
            obj.pgf_titulo = Resources.Validaciones.valNULLString(dr["pgf_titulo"]);
            obj.pgf_contenido = Resources.Validaciones.valNULLString(dr["pgf_contenido"]);
            obj.pgf_activo = Resources.Validaciones.valNULLBool(dr["pgf_activo"]);
            obj.Categoria = new Entidades.Pregunta_Frecuente_Categoria()
            {
                pgfc_id = Resources.Validaciones.valNULLINT(dr["pgf_pgfc_id"])
            };

            return obj;
        }

        public override Pregunta_Frecuente ObjetoNuevo()
        {
            Entidades.Pregunta_Frecuente obj = new Entidades.Pregunta_Frecuente();
            return obj;
        }

        public override ObjectMessage Save(Entidades.Pregunta_Frecuente Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Preguntas_Frecuentes");

                row["pgf_id"] = Obj.pgf_id;
                row["pgf_titulo"] = Obj.pgf_titulo;
                row["pgf_contenido"] = Obj.pgf_contenido;
                row["pgf_pgfc_id"] = Obj.Categoria.pgfc_id;
                row["pgf_activo"] = Obj.pgf_activo;


                if (Obj.pgf_id.Equals(0))
                {
                    row["pgf_usu_id_creador"] = Token.UserID;
                    row["pgf_fec_creador"] = DateTime.Now;
                    row["pgf_usu_id_modificador"] = Token.UserID;
                    row["pgf_fec_modificador"] = DateTime.Now;
                    row["pgf_activo"] = 1;
                    Obj.pgf_id = db.SQLInsert(row, "pgf_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["pgf_usu_id_modificador"] = Token.UserID;
                    row["pgf_fec_modificador"] = DateTime.Now;
                    db.SQLUpdate(row, "pgf_id=@pgf_id", "pgf_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("pgf_id",Obj.pgf_id)
                    });
                    oM.Message = "Datos actualizados";
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

        public ObjectMessage DeletePreguntaFrecuente(int pgf_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();
            try
            {
                if (!(Convert.ToInt32(pgf_id) > 0))
                    throw new Exception("El ID ingresado no es válido.");
                oM = Delete(pgf_id);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }
            return oM;


        }
        

        public ObjectMessage ActivarDesactivar(int preguntaID, bool status)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                if (Convert.ToInt32(preguntaID) > 0)
                {
                    db.SQLExecuteNonQuery("UPDATE Preguntas_Frecuentes SET pgf_activo=@status WHERE pgf_id=@pgf_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("pgf_id",preguntaID),
                    new System.Data.SqlClient.SqlParameter("status",status)
                });
                }
                oM.Success = true;
                oM.Message = "OK";
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }
            return oM;
        }

        public List<Entidades.Pregunta_Frecuente> ListarPorCategoria(int categoriaID)
        {
            List<Entidades.Pregunta_Frecuente> lst = new List<Entidades.Pregunta_Frecuente>();

            string _where = " (pgf_pgfc_id=@pgfc_id or @pgfc_id=0) ";
          
            sQuery = QueryDefault("", _where, "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("pgfc_id",categoriaID),
            });

            foreach (DataRow row in dt_bus.Rows)
            {
                lst.Add(Mapear(row));
            }

            return lst;
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Preguntas_Frecuentes ";
            sQuery += " LEFT JOIN Preguntas_Frecuentes_Categorias ON pgfc_id=pgf_pgfc_id ";

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
      
    }
}
