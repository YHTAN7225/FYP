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

        public string SenderUserId { get; set; }

        public string ReceiverUserId { get; set; }

        public DateTime TimeStamp { get; set; }

        public Notification(string ActionName, string SenderUserId, string ReceiverUserId) {
            this.NotificationId = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.Now;
            this.ActionName = ActionName;
            this.SenderUserId = SenderUserId;
            this.ReceiverUserId = ReceiverUserId;
        }

    }
}
