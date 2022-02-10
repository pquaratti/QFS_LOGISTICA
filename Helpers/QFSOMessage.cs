using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public class QFSOMessage
    {
        #region ### Properties ###

        public QFSEnum.StateMessage State;
        public int Valor;
        public string TextMessage;
        public object ObjectResponse;

        #endregion

        #region Functions & Methods

        public QFSOMessage()
        {
            this.State = QFSEnum.StateMessage.PreInit;
            this.Valor = 0;
            this.TextMessage = "";
            this.ObjectResponse = null;
        }

        #endregion


    }
}
