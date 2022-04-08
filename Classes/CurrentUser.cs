using Launcher0._2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher0._2.Classes
{
    public class CurrentUser
    {
        private User user;

        public void SetCurrentUser(User user)
        {
            this.user = user;
        }

        public User GetCurrentUser()
        {
            return this.user;
        }
    }
}
