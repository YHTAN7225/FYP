using FYP.Controllers;
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

        public void AddFileList(string fileId) {
            Security security = new Security();
            List<string> list = FileList.Split("|").ToList();

            foreach (var item in list)
            {
                if (fileId != security.Decrypt(item))
                {
                    if (this.FileList == null)
                    {
                        this.FileList = item;
                    }
                    else
                    {
                        this.FileList = this.FileList + "|" + item;
                    }
                }

            }

            if (this.FileList == null)
            {
                this.FileList = fileId;
            }
            else {
                this.FileList = this.FileList + "|" + fileId;            
            }
        }

    }
}
