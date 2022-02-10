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
    public class Tutoriales : NegocioBase<Entidades.Tutorial>
    {
        Negocio.App.SIS_Acciones negocioACC;

        public Tutoriales(Entidades.App.Token paramToken) : base("tut_id", "tut_activo", "Tutoriales", "tut")
        {
            Token = paramToken;
            negocioACC = new Negocio.App.SIS_Acciones(Token);
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override Tutorial Mapear(DataRow dr)
        {
            Entidades.Tutorial obj = MapearSimple(dr);
            obj.Accion = negocioACC.MapearSimple(dr);
                     
            return obj;
        }

        public override Tutorial MapearCompleto(DataRow dr)
        {
            Entidades.Tutorial obj = Mapear(dr);
            return obj;
        }

        public override Tutorial MapearSimple(DataRow dr)
        {
            Entidades.Tutorial obj = new Entidades.Tutorial();
            obj.tut_id = Resources.Validaciones.valNULLINT(dr["tut_id"]);
            obj.tut_titulo = Resources.Validaciones.valNULLString(dr["tut_titulo"]);
            obj.tut_archivo = Resources.Validaciones.valNULLString(dr["tut_archivo"]);
            obj.tut_activo = Resources.Validaciones.valNULLBool(dr["tut_activo"]);
            obj.tut_descrip = Resources.Validaciones.valNULLString(dr["tut_descrip"]);
            obj.tut_icono = Resources.Validaciones.valNULLString(dr["tut_icono"]);
            //obj.tut_usu_id_creador = Resources.Validaciones.valNULLINT(dr["tut_usu_id_creador"]);
            //obj.tut_usu_id_modificador = Resources.Validaciones.valNULLINT(dr["tut_usu_id_modificador"]);
            //obj.tut_usu_id_baja = Resources.Validaciones.valNULLINT(dr["tut_usu_id_baja"]);
            //obj.tut_fec_creador = Convert.ToDateTime(dr["tut_fec_creador"]);
            //obj.tut_fec_modificador = Convert.ToDateTime(dr["tut_fec_creador"]);
            //obj.tut_fec_baja = Convert.ToDateTime(dr["tut_fec_baja"]);
            obj.Accion = new Entidades.App.SIS_Accion()
            {
                acc_id = Resources.Validaciones.valNULLINT(dr["tut_acc_id"])
            };

            return obj;
        }

        public override Tutorial ObjetoNuevo()
        {
            Entidades.Tutorial obj = new Entidades.Tutorial();
            return obj;
        }

        public override ObjectMessage Save(Entidades.Tutorial Obj)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                DataRow row = db.Estructura("Tutoriales");

                row["tut_titulo"] = Obj.tut_titulo;
                row["tut_archivo"] = Obj.tut_archivo;
                row["tut_activo"] = Obj.tut_activo;
                row["tut_descrip"] = Obj.tut_descrip;
                row["tut_acc_id"] = Obj.Accion.acc_id;
                row["tut_icono"] = Obj.tut_icono;

                if (Obj.tut_id.Equals(0))
                {                      
                    row["tut_usu_id_creador"] = Token.UserID;
                    row["tut_fec_creador"] = DateTime.Now;
                    row["tut_usu_id_mod"] = Token.UserID;
                    row["tut_fec_mod"] = DateTime.Now;
                    row["tut_activo"] = 1;
                    Obj.tut_id = db.SQLInsert(row, "tut_id").Valor;
                    oM.Message = "Datos ingresados";
                }
                else
                {
                    row["tut_usu_id_mod"] = Token.UserID;
                    row["tut_fec_mod"] = DateTime.Now;
                    db.SQLUpdate(row, "tut_id=@tut_id", "tut_id", new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter("tut_id",Obj.tut_id)
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

        public ObjectMessage ImportTutorial(string pathFile, string tut_id)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                if (Convert.ToInt32(tut_id) > 0)
                {
                    db.SQLExecuteNonQuery("UPDATE Tutoriales SET tut_archivo=@pathFile WHERE tut_id=@tut_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("pathFile",pathFile),
                    new System.Data.SqlClient.SqlParameter("tut_id",tut_id)
                });
                    oM.Message = "OK";
                    oM.Success = true;
                }
            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
            }

            return oM;
        }

        public ObjectMessage DeleteTutorial(int tut_id)
        {
            Entidades.App.ObjectMessage oM = new ObjectMessage();
            try
            {
                if (!(Convert.ToInt32(tut_id) > 0))
                    throw new Exception("El ID ingresado no es válido.");
                oM = Delete(tut_id);
            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }
            return oM;


        }
        

        public ObjectMessage ActivarDesactivar(int tutorialID, bool status)
        {
            ObjectMessage oM = new ObjectMessage();
            try
            {
                if (Convert.ToInt32(tutorialID) > 0)
                {
                    db.SQLExecuteNonQuery("UPDATE Tutoriales SET tut_activo=@status WHERE tut_id=@tut_id", new List<System.Data.SqlClient.SqlParameter>() {
                    new System.Data.SqlClient.SqlParameter("tut_id",tutorialID),
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

        public List<Entidades.Tutorial> ListarPorAccion(int acc_id)
        {
            sQuery = QueryDefault("", "", "");

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (acc_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "tut_acc_id", Value = acc_id });

            List<Entidades.Tutorial> tutoriales = new List<Entidades.Tutorial>();

            tutoriales = ListarConFiltros(paramsFilter);

            return tutoriales;
        }

        public List<Entidades.Tutorial> ListarPorAccionController(int acc_id, string controller)
        {
            sQuery = QueryDefault("", "", "");

            List<ObjectParameter> paramsFilter = new List<ObjectParameter>();

            if (acc_id > 0)
                paramsFilter.Add(new ObjectParameter() { Name = "tut_acc_id", Value = acc_id });
            paramsFilter.Add(new ObjectParameter() { Name = "acc_controller", Value = controller });

            List<Entidades.Tutorial> tutoriales = new List<Entidades.Tutorial>();

            tutoriales = ListarConFiltros(paramsFilter);

            return tutoriales;
        }


        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM Tutoriales ";
            sQuery += " LEFT JOIN SIS_Acciones ON acc_id=tut_acc_id ";

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
