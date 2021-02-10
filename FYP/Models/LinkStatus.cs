using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class LinkStatus
    {
        [Key]
        public string LinkId { get; set; }

        public DateTime GeneratedTime { get; set; }

        public string UserId { get; set; }

        public string Submitted { get; set; }

        public Boolean IsValid() {
            DateTime CurrentTime = DateTime.Now;
            TimeSpan TimeSpan = CurrentTime - this.GeneratedTime;

            if (TimeSpan.Minutes <= 1440)
            {
                if (this.Submitted == "false")
                {
                    return true;
                }
                else { 
                    return false;
                }            
            }
            else {
                return false;
            }
        }

        public LinkStatus(string UserId) {
            this.LinkId = Guid.NewGuid().ToString();
            this.GeneratedTime = DateTime.Now;
            this.UserId = UserId;
            Submitted = "false";
        }
    }
}
