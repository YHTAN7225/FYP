using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class ApprovalRequest
    {
        [Key]
        public string ApproveId { get; set; }

        public DateTime TimeStamp { get; set; }

        public string SenderUserId { get; set; }

        public string ReceiverUserId { get; set; }

        public string AdminId { get; set; }

        public string FileId { get; set; }

        public string ApprovalStatus { get; set; }

        public string SignatureId { get; set; }

        public ApprovalRequest(string SenderUserId, string ReceiverUserId, string AdminId, string FileId) { 
            this.ApproveId = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.Now;
            this.ApprovalStatus = "false";
            this.SenderUserId = SenderUserId;
            this.ReceiverUserId = ReceiverUserId;
            this.AdminId = AdminId;
            this.FileId = FileId;
        }

        public ApprovalRequest(string SenderUserId, string ReceiverUserId, string AdminId, string FileId, string SignatureId)
        {
            this.ApproveId = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.Now;
            this.ApprovalStatus = "false";
            this.SenderUserId = SenderUserId;
            this.ReceiverUserId = ReceiverUserId;
            this.AdminId = AdminId;
            this.FileId = FileId;
            this.SignatureId = SignatureId;
        }
    }   
}
