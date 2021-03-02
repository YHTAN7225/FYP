using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FYP.Models;
using FYP.Data;

namespace FYP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FYPContext _context;
        private readonly Storage _storage;
        private readonly Security _security;

        public HomeController(FYPContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
            this._storage = new Storage();
            this._security = new Security();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult FileSubmissionResult()
        {
            return View();
        }


        public IActionResult FileSubmissionViaLinkView(string LinkId) 
        {
            LinkStatus LinkStatus = null;

            try
            {
                LinkStatus = _context.LinkStatus.Where(x => x.LinkId.Equals(LinkId)).First();
            }
            catch (Exception){
                return NotFound();
            }

            if (LinkStatus.IsValid())
            {
                var User = _context.Users.Where(x => x.Id.Equals(LinkStatus.UserId)).First();
                TempData["UserName"] = User.UserName;
                TempData["UserId"] = User.Id;
                TempData["LinkId"] = LinkId;
                return View();
            }
            else {
                return BadRequest();
            }
            
        }

        public IActionResult FileSubmissionViaLinkResult(LinkFileViewModel model, string UserId, string LinkId)
        {
            Boolean success = true;
            AdminAccess AdminAccess = _context.AdminAccess.Where(x => x.UserList.Contains(UserId)).First();
            UserAccess UserAccess = _context.UserAccess.Where(x => x.UserId.Equals(UserId)).First();

            success = _storage.UploadFile(AdminAccess.AdminId, model.Files);
            UserAccess.AddFileList(_security.Encrypt(model.Files.FileName));

            Notification notif = new Notification
            {
                ActionName = "LINK_UPLOAD",
                PrimaryUserName = _context.Users.Where(x => x.Id.Equals(UserId)).First().UserName,
                SecondaryUserName = _context.Users.Where(x => x.Id.Equals(_context.UserAccess.Where(x => x.UserId.Equals(UserId)).First().AdminId)).First().UserName,
                FileName = model.Files.FileName
            };
            _context.Notification.Add(notif);
            _context.SaveChangesAsync().Wait();
            
            LinkStatus LinkStatus = _context.LinkStatus.Where(x => x.LinkId.Equals(LinkId)).First();
            LinkStatus.Submitted = "true";    
            _context.SaveChangesAsync();

            if (success) {
                TempData["FileSubmissionViaLinkReturnMessage"] = "File has been Uploaded Successfully!";
                return RedirectToAction("FileSubmissionResult", "Home");
            }
            else {
                TempData["FileSubmissionViaLinkReturnMessage"] = "Error when uploading file, please contact the person in charge!";
                return RedirectToAction("FileSubmissionResult", "Home");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
