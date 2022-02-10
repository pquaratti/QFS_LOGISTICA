using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Negocio.App
{
    public class MailManager
    {
        public static Entidades.App.ObjectMessage SendMessage(string MailDestino, string subjectMail, string PathTemplate, List<Entidades.App.ObjectParameter> fieldsTemplate, List<Entidades.App.MailAttachment> attachmentsFile = null)
        {
            Entidades.App.ObjectMessage oM = new Entidades.App.ObjectMessage();

            try
            {
                string cuerpoMail = "";

                if (PathTemplate.Length > 0)
                {
                    using (StreamReader reader = new StreamReader(PathTemplate))
                    {
                        cuerpoMail = reader.ReadToEnd();
                    }

                    foreach (var itemField in fieldsTemplate)
                    {
                        cuerpoMail = cuerpoMail.Replace("{" + itemField.Name + "}", itemField.Value.ToString());
                    }

                }
                else
                {
                    cuerpoMail = "MAIL TEST";
                }


                using (MailMessage mail = new MailMessage(Resources.Repositorio.MailOrigen().Trim(), MailDestino))
                {
                    SmtpClient client = new SmtpClient();
                    client.Host = Resources.Repositorio.MailHost().Trim();
                    client.Port = Convert.ToInt32(Resources.Repositorio.MailPort());
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = Resources.Repositorio.MailSSL();
                    client.Timeout = 10000;
                    client.Credentials = new System.Net.NetworkCredential(Resources.Repositorio.MailCredentialsUser().Trim(), Resources.Repositorio.MailCredentialsPassword().Trim());
                    mail.Subject = subjectMail;
                    mail.IsBodyHtml = true;
                    mail.Body = cuerpoMail;

                    if (attachmentsFile != null)
                    {
                        if (attachmentsFile.Count > 0)
                        {
                            foreach (var itemAttach in attachmentsFile)
                            {
                                itemAttach.FileFullPath = Resources.Repositorio.PathArchivosMail() + itemAttach.FileName;
                                mail.Attachments.Add(new System.Net.Mail.Attachment(itemAttach.FileFullPath));
                            }
                        }
                    }

                    client.Send(mail);

                    oM.Success = true;
                    oM.Message = "OK";
                }

            }
            catch (SmtpException ex)
            {
                oM.Success = false;
                oM.Message = ex.Message;
            }

            return oM;
        }

        //public static void RegisterMailBatch(string to, string subject, string templateName, Entidades.App.Token token, List<Entidades.App.ObjectParameter> fieldsTemplate, List<Entidades.App.MailAttachment> attachmentsFiles)
        //{
        //    string _paramsJSONMetaData = Resources.Repositorio.JSONSerialize(fieldsTemplate);
        //    string _paramJSONAttachments = Resources.Repositorio.JSONSerialize(attachmentsFiles);

        //    string sQuery = "INSERT Process_Batch_Email (pbe_to,pbe_subject,pbe_templateName, pbe_objectParameters, pbe_fec_creado,pbe_objectAttachments) ";
        //    sQuery += "      VALUES (@pbe_to,@pbe_subject,@pbe_templateName,@pbe_objectParameters,@pbe_fec_creado,@pbe_objectAttachments) ";

        //    dbSQL.SQLExecuteNonQuery(sQuery, new List<System.Data.SqlClient.SqlParameter>()
        //    {
        //       new System.Data.SqlClient.SqlParameter("pbe_to",to),
        //       new System.Data.SqlClient.SqlParameter("pbe_subject",subject),
        //       new System.Data.SqlClient.SqlParameter("pbe_templateName",templateName),
        //       new System.Data.SqlClient.SqlParameter("pbe_objectParameters",_paramsJSONMetaData),
        //       new System.Data.SqlClient.SqlParameter("pbe_objectAttachments",_paramJSONAttachments),
        //       new System.Data.SqlClient.SqlParameter("pbe_fec_creado",DateTime.Now)
        //    });

        //}

        //public void ExecuteBatch()
        //{
        //    string sQuery = "SELECT * FROM Process_Batch_Email WHERE pbe_fec_enviado is null order by pbe_fec_creado ";

        //    DataTable dt = dbSQL.SQLSelect(sQuery);

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        var _id = Convert.ToInt32(row["pbe_id"]);
        //        string _mailDestino = row["pbe_to"].ToString();
        //        string _subjectMail = row["pbe_subject"].ToString();
        //        string _templateName = row["pbe_templateName"].ToString();
        //        List<Entidades.App.ObjectParameter> _fieldsTemplate = JsonConvert.DeserializeObject<List<Entidades.App.ObjectParameter>>(row["pbe_objectParameters"].ToString());
        //        List<Entidades.App.MailAttachment> _attachmentsFiles = JsonConvert.DeserializeObject<List<Entidades.App.MailAttachment>>(row["pbe_objectAttachments"].ToString());

        //        Entidades.App.ObjectMessage oM = SendMessage(_mailDestino, _subjectMail, _templateName, _fieldsTemplate, _attachmentsFiles);

        //        if (oM.Success)
        //        {
        //            dbSQL.SQLExecuteNonQuery("UPDATE Process_Batch_Email SET pbe_fec_enviado=GETDATE() WHERE pbe_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
        //                new System.Data.SqlClient.SqlParameter("id",_id)
        //            });
        //        }
        //        else
        //        {
        //            dbSQL.SQLExecuteNonQuery("UPDATE Process_Batch_Email SET pbe_error_message=@error WHERE pbe_id=@id", new List<System.Data.SqlClient.SqlParameter>() {
        //                new System.Data.SqlClient.SqlParameter("id",_id),
        //                new System.Data.SqlClient.SqlParameter("error",oM.Message)
        //            });
        //        }
        //    }
        //}

    }
}
