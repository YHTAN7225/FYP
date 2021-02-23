using FYP.Data;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Controllers
{
    public class StartUp
    {
        private readonly FYPContext _context;

        public StartUp(FYPContext context)
        {
            _context = context;
        }

        public void ClearExpiredLink(){
            List<LinkStatus> LinkToBeDeleted = new List<LinkStatus>();
            List<LinkStatus> LinkStatusList = _context.LinkStatus.ToList();

            foreach (var item in LinkStatusList) {
                if (!item.IsValid()) {
                    LinkToBeDeleted.Add(item);
                }
            }
            _context.LinkStatus.RemoveRange();
            _context.SaveChangesAsync().Wait();
        }
    }
}
