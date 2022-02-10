using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public class QFSEnum
    {
        public enum Client
        {
            THUNDER, 
            OCHENTA,
            FASSIGONZALEZ,
            SANOFI,
            EMERGENCIAS
        }

        public enum DBProvider
        {
            SQL,
            MySQL,
            MongoDB
        }

        public enum CryptoProvider
        {
            DES,
            TripleDES,
            RC2,
            Rijndael
        }

        public enum CryptoAction
        {
            Encrypt,
            Desencrypt
        }

        public enum StateMessage
        {
            Success = 1,
            Error = 2,
            Warning = 3,
            Exception = 4,
            PreInit = 5,
            NotSet = 6

        }

        public enum AlertMessageOptions
        {
            Success = 1,
            Warning = 2,
            Info = 3,
            Danger = 4
        }

    }
}
