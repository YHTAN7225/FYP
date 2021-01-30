using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class UserAccess
    {
        [Key]
        public string UserId { get; set; }

        public string AdminId{ get; set; }

        public string OrganizationName { get; set; }

        public string FileList { get; set; }

        public UserAccess(string UserId, string AdminId, string OrganizationName) {
            this.UserId = UserId;
            this.AdminId = AdminId;
            this.OrganizationName = OrganizationName;
        }
    }
}
