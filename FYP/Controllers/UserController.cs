using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    public class UserController : Controller
    {
        private readonly FYPContext _context;
        private UserManager<IdentityUser> _userManager;
        private Storage _storage;
        private Security _security;
        private Constant _constant;

        public UserController(FYPContext context, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._storage = new Storage();
            this._security = new Security();
            this._constant = new Constant();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateLink()
        {
            return View();
        }

        public IActionResult GenerateLinkButton()
        {
            LinkStatus LinkStatus= new LinkStatus(_userManager.GetUserId(User));
            _context.LinkStatus.Add(LinkStatus);
            _context.SaveChanges();
            TempData["Link"] = _constant.GeneratedLinkURL(LinkStatus.LinkId); 
            return RedirectToAction("GenerateLink", "User");
        }

        public IActionResult Sign()
        {
            return View();
        }

        public IActionResult Files()
        {
            UserAccess ua = _context.UserAccess.Where(x => x.UserId.Equals(_userManager.GetUserId(User))).First();
            List<string> FileList = new List<string>();

            foreach (var item in ua.FileList.Split("|")) {
                FileList.Add(_security.Decrypt(item));
            }
            return View(_storage.GetFileListBasedOnUser(ua.AdminId, FileList));
        }

        [HttpGet]
        public IActionResult Download(string FileName)
        {
            UserAccess ua = _context.UserAccess.Where(x => x.UserId.Equals(_userManager.GetUserId(User))).First();
            var file = _storage.GetSpecificFile(ua.AdminId, FileName);

            return File(file.OpenReadAsync().Result, file.Properties.ContentType, file.Name);
        
        }
           

        public IActionResult Share()
        {
            return View();
        }

        public IActionResult RequestFile()
        {
            return View();
        }
    }


}
