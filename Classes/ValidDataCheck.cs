using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Launcher0._2.Classes
{
    public class ValidDataCheck
    {
        public bool PassCheck(string pass)
        {
            Regex regex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,40}$");
            Match match = regex.Match(pass);

            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public bool EmailCheck(string mail)
        {
            Regex regex = new Regex("^(\\S+@\\S+\\.\\S+).{0,40}$");
            Match match = regex.Match(mail);

            if (match.Success)
            {
                return true;
            }
            return false;
        }
    }
}
