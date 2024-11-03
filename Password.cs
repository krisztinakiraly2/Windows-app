using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApp
{
    internal class Password
    {
        String website;
        String emailOrUsername;
        String password;

        public Password() { }
        public Password (String website, String emailOrUsername, String password)
        {
            this.website = website;
            this.emailOrUsername = emailOrUsername;
            this.password = password;
        }

        public string GetWebsite()
        {
            return website;
        }

        public string GetEmailOrUsername()
        {
            return emailOrUsername;
        }

        public string GetPassword()
        {
            return password;
        }
    }
}
