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

    }
}
