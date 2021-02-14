using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class RequestInfo
    {
        public string ApproveId { get; set; }
        public DateTime TimeStamp{ get; set; }
        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public string FileId { get; set; }
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public string FileName { get; set; }
    }
}
