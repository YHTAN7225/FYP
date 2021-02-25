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
            this.FileList = "";
        }

        public void AddFileList(string fileId) {
            if ((this.FileList != null) && (this.FileList != ""))
            {
                if (!this.FileList.Contains(fileId))
                {
                        this.FileList = this.FileList + "|" + fileId;
                }
                else
                {
                    List<string> list = FileList.Split("|").ToList();
                    list.Remove(fileId);

                    string newList = "";
                    foreach (var item in list)
                    {
                        if (newList == "")
                        {
                            newList = item;
                        }
                        else
                        {
                            newList = newList + "|" + item;
                        }
                    }
                    this.FileList = newList + "|" + fileId;
                }
            }
            else {
                this.FileList = fileId;
            }
        }

        public void RemoveFileAccess(string FileId) {
            List<string> list = this.FileList.Split("|").ToList();
            this.FileList = "";

            foreach (var item in list) {
                if (item != FileId) {
                    AddFileList(item);
                }
            }
        }
    }
}
