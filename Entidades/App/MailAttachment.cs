using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.App
{
    public class MailAttachment
    {
        public string FileName { get; set; }
        public string FileNameOriginal { get; set; }
        public string FileFullPath { get; set; }

        public MailAttachment(string FileNameOriginal)
        {
            this.FileName = FileNameOriginal;
        }

    }
}
