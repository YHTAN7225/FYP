﻿using System;
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

            if (ua.FileList != null) {
                foreach (var item in ua.FileList.Split("|"))
                {
                    FileList.Add(_security.Decrypt(item));
                }
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
           

        public IActionResult Share(string FileName)
        {
            if (FileName == null) {
                return BadRequest();
            }
            List<UserInfo> UserInfoList = new List<UserInfo>();
            AdminAccess AdminAccess = _context.AdminAccess.Where(x => x.UserList.Contains(_userManager.GetUserId(User))).First();
            List<UserAccess> UserAccessList = _context.UserAccess.Where(x => x.AdminId.Equals(AdminAccess.AdminId)).ToList();
            foreach (var item in UserAccessList) {
                if (item.UserId != _userManager.GetUserId(User)) {
                    if (item.FileList == null) {
                        UserInfoList.Add(new UserInfo(item, _context.Users.Where(x => x.Id.Equals(item.UserId)).First().UserName));
                    } else if (!item.FileList.Contains(_security.Encrypt(FileName))) {
                        UserInfoList.Add(new UserInfo(item, _context.Users.Where(x => x.Id.Equals(item.UserId)).First().UserName));
                    }
                }
            }

            TempData["FileId"] = _security.Encrypt(FileName);

            return View(UserInfoList);
        }

        public IActionResult ShareAction(string UserId, string AdminId, string FileId, string ReceiverId) {
            ApprovalRequest NewApprovalRequest =  new ApprovalRequest(UserId, ReceiverId, AdminId, FileId);
            List<ApprovalRequest> ApprovalRequestsList = _context.ApprovalRequest.ToList();

            foreach (var item in ApprovalRequestsList) {
                if ((item.SenderUserId == UserId)
                    && (item.ReceiverUserId == ReceiverId)
                    && (item.FileId == FileId))
                {
                    TempData["ShareActionReturnMessage"] = "The same request has already been made, please wait for admin to approve!";
                    return RedirectToAction("Files", "User");
                }
            }

            _context.ApprovalRequest.Add(NewApprovalRequest);
            var result = _context.SaveChangesAsync();
            result.Wait();
            if (result.IsCompletedSuccessfully) {
                TempData["ShareActionReturnMessage"] = "Successfully send request to admin!";
            }
            else {
                TempData["ShareActionReturnMessage"] = "Error when sending request!";
            }
            return RedirectToAction("Files", "User");
        }

        public IActionResult RequestFile()
        {
            return View();
        }
    }
}
