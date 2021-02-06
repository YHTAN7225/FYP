using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class UserInfo
    {
        public UserAccess UserAccess { get; set; }

        public string UserName { get; set; }

        public UserInfo(UserAccess UserAccess, string UserName) {
            this.UserAccess = UserAccess;
            this.UserName = UserName;
        }

        public UserInfo(){}
    }
}
