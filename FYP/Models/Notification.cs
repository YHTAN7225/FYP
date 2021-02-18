using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class Notification
    {
        [Key]
        public string NotificationId { get; set; }

        public string ActionName { get; set; }

        public string PrimaryUserId { get; set; }

        public string SecondaryUserId { get; set; }

        public string FileId { get; set; }

        public DateTime TimeStamp { get; set; }

        public Notification() {
            this.NotificationId = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.Now;
        }

    }
}
