using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Controllers
{
    public class Security
    {
        public string Decrypt(string text)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(text);
                decrypted = ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public string Encrypt(string text)
        {
            byte[] b = ASCIIEncoding.ASCII.GetBytes(text);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
    }
}
