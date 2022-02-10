using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class SIS_Usuario_Password_Recovery : EntidadBase
    {
        public int upr_id { get; set; }
        public DateTime upr_fec_ini { get; set; }
        public Entidades.App.SIS_Usuario Usuario { get; set; }
        public string upr_mail { get; set; }
        public DateTime upr_fec_fin { get; set; }
        public string upr_verify_code { get; set; }


        [Display(Name = "Contraseña Nueva")]
        [Required(ErrorMessage = "Campo requerido")]
        public string upr_new_password { get; set; }

        public string code1 { get; set; }
        public string code2 { get; set; }
        public string code3 { get; set; }
        public string code4 { get; set; }
        public string code5 { get; set; }
        public string code6 { get; set; }
        public string codeFull { get; set; }
        public string recoveryTokenID { get; set; }

        [Display(Name = "Contraseña Nueva (Confirmación)")]
        [Required(ErrorMessage = "Campo requerido")]
        public string passwordNewConfirm { get; set; }

        public string customMessageError { get; set; }

        public string LeyendaEnvioCodigo { get; set; }

        public string PathTemplateMailCode { get; set; }

        public string MailTemplatePath { get; set; }
        public SIS_Usuario_Password_Recovery()
        {
            this.customMessageError = "";
        }

    }
}
