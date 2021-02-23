using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class DoubleSignatureRequest
    {
        public List<SignatureRequest> UserAsSender { get; set; }

        public List<SignatureRequest> UserAsReceiver { get; set; }
    }
}
