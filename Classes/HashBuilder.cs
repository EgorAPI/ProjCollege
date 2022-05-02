using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Launcher0._2.Classes
{
    public class HashBuilder
    {
        public string GetHash(string pas)
        {
            byte[] tmpPas;
            byte[] tmpHash;

            tmpPas = ASCIIEncoding.ASCII.GetBytes(pas);

            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpPas);

            var bilder = new StringBuilder(tmpHash.Length * 2);

            for (int i = 0; i < tmpHash.Length; i++)
            {
                bilder.Append(tmpHash[i].ToString("X2"));
            }

            return bilder.ToString();
        }
    }
}
