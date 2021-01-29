using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class AdminAccess
    {
        [Key]
        public string AdminId { get; set; }

        public string OrganizationName { get; set; }

        public string FileList { get; set; }

        public string UserList { get; set; }

        public AdminAccess(string AdminId, string OrganizationName)
        {
            this.AdminId = AdminId;
            this.OrganizationName = OrganizationName;
        }
    }
}
