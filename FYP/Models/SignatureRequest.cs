using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class SignatureRequest
    {
        [Key]
        public string  SignatureId { get; set; }
        public DateTime TimeStamp{ get; set; }
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public string SignatureStatus { get; set; }

        public SignatureRequest() {
            this.SignatureId = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.Now;
        }

        public Boolean IsSigned() {
            if (this.SignatureStatus.ToLower() == "true") {
                return true;
            }
            return false;
        }
    }
}
