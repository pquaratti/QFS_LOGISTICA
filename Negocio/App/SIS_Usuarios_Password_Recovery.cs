using Entidades.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.App
{
    public class SIS_Usuarios_Password_Recovery : NegocioBase<Entidades.App.SIS_Usuario_Password_Recovery>
    {
        Negocio.App.SIS_Usuarios negocioUSU;

        public SIS_Usuarios_Password_Recovery()
        {
            negocioUSU = new SIS_Usuarios();
        }

        public override List<DLLObject> ListarDLL(bool agregaDefault = false)
        {
            throw new NotImplementedException();
        }

        public override SIS_Usuario_Password_Recovery Mapear(DataRow dr)
        {
            Entidades.App.SIS_Usuario_Password_Recovery obj = MapearSimple(dr);
            obj.Usuario = negocioUSU.MapearSimple(dr);
            return obj;
        }

        public override SIS_Usuario_Password_Recovery MapearCompleto(DataRow dr)
        {
            return Mapear(dr);
        }

        public override SIS_Usuario_Password_Recovery MapearSimple(DataRow dr)
        {
            Entidades.App.SIS_Usuario_Password_Recovery obj = new SIS_Usuario_Password_Recovery();
            obj.upr_id = Resources.Validaciones.valNULLINT(dr["upr_id"]);
            obj.upr_fec_ini = Resources.Validaciones.valNULLDateTime(dr["upr_fec_ini"]);
            obj.Usuario = new SIS_Usuario() { usu_id = Resources.Validaciones.valNULLINT(dr["upr_usu_id"]) };
            obj.upr_mail = Resources.Validaciones.valNULLString(dr["upr_mail"]);
            obj.upr_verify_code = Resources.Validaciones.valNULLString(dr["upr_verify_code"]);
            obj.upr_new_password = Resources.Validaciones.valNULLString(dr["upr_new_password"]);
            obj.recoveryTokenID = Resources.Validaciones.valNULLString(dr["upr_recovery_token"]);

            return obj;
        }

        public override SIS_Usuario_Password_Recovery ObjetoNuevo()
        {
            throw new NotImplementedException();
        }

        public override ObjectMessage Save(SIS_Usuario_Password_Recovery Obj)
        {
            throw new NotImplementedException();
        }

        protected override string QueryDefault(string sTOP, string sWHERE, string sOrderBy)
        {
            sQuery = "  SELECT * FROM SIS_Usuarios_Password_Recovery ";
            sQuery += " LEFT JOIN SIS_Usuarios on usu_id=upr_usu_id ";

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

        public ObjectMessage SolicitarRecupero(Entidades.App.SIS_Usuario unUsuario, string pathMailCode)
        {
            ObjectMessage oM = new ObjectMessage();
            Negocio.App.SIS_Usuarios negocioUSU = new SIS_Usuarios();

            try
            {
                Entidades.App.SIS_Usuario_Password_Recovery recoveryData = new SIS_Usuario_Password_Recovery();
                recoveryData.Usuario = negocioUSU.ObtenerUsuarioParaRecoveryPassword(unUsuario.usu_nickname, unUsuario.usu_mail);

                if (recoveryData.Usuario.usu_id.Equals(0))
                    throw new Exception("Los datos ingresados son incorrectos. Verifique los datos ingresados e intente nuevamente.");

                SetRandomVerifyCode(recoveryData);

                DataRow row = db.Estructura("SIS_Usuarios_Password_Recovery");
                row["upr_fec_ini"] = DateTime.Now;
                row["upr_usu_id"] = recoveryData.Usuario.usu_id;
                row["upr_mail"] = recoveryData.Usuario.usu_mail;
                row["upr_verify_code"] = recoveryData.codeFull;
                row["upr_recovery_token"] = recoveryData.recoveryTokenID;

                recoveryData.upr_id = db.SQLInsert(row, "upr_id").Valor;

                oM.Success = true;
                oM.Message = "OK";
                oM.ObjectRelation = Negocio.App.Security.EncriptarBasico(recoveryData.recoveryTokenID);

                recoveryData.PathTemplateMailCode = pathMailCode;
                SendRecoveryMail(recoveryData);

            }
            catch (Exception ex)
            {
                oM.Success = false;
                oM.Message = "Ocurrió un inconveniente al realizar la recuperación";
            }

            return oM;
        }

        public ObjectMessage ValidarChallenge(Entidades.App.SIS_Usuario_Password_Recovery unChallenge)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                unChallenge.codeFull = unChallenge.code1.ToString();
                unChallenge.codeFull += unChallenge.code2.ToString();
                unChallenge.codeFull += unChallenge.code3.ToString();
                unChallenge.codeFull += unChallenge.code4.ToString();
                unChallenge.codeFull += unChallenge.code5.ToString();
                unChallenge.codeFull += unChallenge.code6.ToString();

                sQuery = "  SELECT * FROM SIS_Usuarios_Password_Recovery ";
                sQuery += " WHERE upr_verify_code=@code and upr_recovery_token=@token ";

                DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("code", unChallenge.codeFull),
                    new System.Data.SqlClient.SqlParameter("token", Negocio.App.Security.DesencriptarBasico(unChallenge.recoveryTokenID))
                });

                if (dt_bus.Rows.Count > 0)
                {
                    oM.Message = "OK";
                    oM.Success = true;
                }
                else
                    throw new Exception("Error Challenge");

            }
            catch (Exception ex)
            {
                oM.Message = "Error de validación de Challenge. Vuelva a ingresar los datos solicitados.";
                oM.Success = false;
            }

            return oM;
        }


        public ObjectMessage RealizarCambioPassword(Entidades.App.SIS_Usuario_Password_Recovery recoveryChallenge)
        {
            ObjectMessage oM = new ObjectMessage();

            try
            {
                Negocio.App.SIS_Usuarios negocioUSU = new SIS_Usuarios();

                if (recoveryChallenge.upr_new_password != recoveryChallenge.passwordNewConfirm)
                    throw new Exception("Las contraseñas ingresadas no coinciden");

                recoveryChallenge.recoveryTokenID = Negocio.App.Security.DesencriptarBasico(recoveryChallenge.recoveryTokenID);

                Entidades.App.SIS_Usuario_Password_Recovery challengeVigente = ObtenerPorRecoveryToken(recoveryChallenge.recoveryTokenID);

                if (challengeVigente.Usuario.usu_id > 0)
                {
                    oM = negocioUSU.ActualizarPasswordDirect(challengeVigente.Usuario.usu_id.ToString(), recoveryChallenge.upr_new_password);

                    if (oM.Success == false)
                        throw new Exception(oM.Message);

                    challengeVigente.upr_new_password = Negocio.App.Security.Encriptar(recoveryChallenge.upr_new_password);
                    challengeVigente.MailTemplatePath = recoveryChallenge.MailTemplatePath;
                    
                    CloseRecoveryChallenge(challengeVigente);
                    SendRecoveryPasswordNotification(challengeVigente);
                }
                else
                    throw new Exception("Los datos ingresados no son válidos");

                oM.Success = true;
                oM.Message = "OK";

            }
            catch (Exception ex)
            {
                oM.Message = ex.Message;
                oM.Success = false;
               
            }

            return oM;
        }


        public void SetRandomVerifyCode(Entidades.App.SIS_Usuario_Password_Recovery recoveryData)
        {
            recoveryData.codeFull = Resources.Repositorio.RandomCode(100000, 999999);
            recoveryData.recoveryTokenID = Guid.NewGuid().ToString();
        }

        public void SendRecoveryMail(Entidades.App.SIS_Usuario_Password_Recovery recoveryData)
        {
            List<ObjectParameter> oParams = new List<ObjectParameter>();

            oParams.Add(new ObjectParameter() { Name = "verifyCode", Value = recoveryData.codeFull });
            oParams.Add(new ObjectParameter() { Name = "nombreUsuario", Value = recoveryData.Usuario.usu_nombre + " " + recoveryData.Usuario.usu_apellido });

            MailManager.SendMessage(recoveryData.Usuario.usu_mail, "Código de Verificación", recoveryData.PathTemplateMailCode, oParams);
        }

        public void CloseRecoveryChallenge(Entidades.App.SIS_Usuario_Password_Recovery recoveryData)
        {
            sQuery = "UPDATE SIS_Usuarios_Password_Recovery SET upr_fec_fin=@fecha, upr_new_password=@password WHERE upr_id=@id";

            db.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
            {
                new System.Data.SqlClient.SqlParameter("fecha", DateTime.Now),
                new System.Data.SqlClient.SqlParameter("password", recoveryData.upr_new_password),
                new System.Data.SqlClient.SqlParameter("id", recoveryData.upr_id)
            });

        }

        public Entidades.App.SIS_Usuario_Password_Recovery ObtenerPorRecoveryToken(string recoveryTokenID)
        {
            Entidades.App.SIS_Usuario_Password_Recovery obj = new SIS_Usuario_Password_Recovery();

            sQuery = QueryDefault("", "upr_recovery_token=@token", "");

            DataTable dt_bus = db.SQLSelect(sQuery, new List<System.Data.SqlClient.SqlParameter>() {
                new System.Data.SqlClient.SqlParameter("token", recoveryTokenID)
            });

            if (dt_bus.Rows.Count > 0)
                obj = Mapear(dt_bus.Rows[0]);
            
            return obj;
        }

        public void SendRecoveryPasswordNotification(Entidades.App.SIS_Usuario_Password_Recovery recoveryData)
        {
            List<ObjectParameter> oParams = new List<ObjectParameter>();

            oParams.Add(new ObjectParameter() { Name = "nombreUsuario", Value = recoveryData.Usuario.usu_nombre + " " + recoveryData.Usuario.usu_apellido });

            MailManager.SendMessage(recoveryData.Usuario.usu_mail, "Contraseña modificada exitosamente", recoveryData.MailTemplatePath, oParams);

        }

    }
}
