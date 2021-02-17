﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.File;

namespace FYP.Controllers
{
    public class AdminController : Controller
    {
        private readonly FYPContext _context;
        private UserManager<IdentityUser> _userManager;
        private Storage _storage;
        private Security _security;

        public AdminController(FYPContext context, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._storage = new Storage();
            this._security = new Security();
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Approve()
        {
            List<ApprovalRequest> ApprovalRequestList = _context.ApprovalRequest.Where(x => x.AdminId.Equals(_userManager.GetUserId(User))).ToList();
            List<RequestInfo> RequestInfoList = new List<RequestInfo>();

            foreach (var item in ApprovalRequestList) { 
                RequestInfoList.Add(new RequestInfo {
                    ApproveId = item.ApproveId,
                    TimeStamp = item.TimeStamp,
                    SenderUserId = item.SenderUserId,
                    ReceiverUserId = item.ReceiverUserId,
                    FileId = item.FileId,
                    SenderUserName = _context.Users.Where(x => x.Id.Equals(item.SenderUserId)).First().UserName,
                    ReceiverUserName = _context.Users.Where(x => x.Id.Equals(item.ReceiverUserId)).First().UserName,
                    FileName = _security.Decrypt(item.FileId)
                }); 
            }
            return View(RequestInfoList);
        }

        public IActionResult ApproveRequest(string ApproveId) 
        {
            ApprovalRequest ApprovalRequest = _context.ApprovalRequest.Where(x => x.ApproveId.Equals(ApproveId)).First();
            UserAccess UserAccess = _context.UserAccess.Where(x => x.UserId.Equals(ApprovalRequest.ReceiverUserId)).First();
            UserAccess.AddFileList(ApprovalRequest.FileId);
            var ApproveResult = _context.SaveChangesAsync();
            ApproveResult.Wait();

            if (ApproveResult.IsCompletedSuccessfully) {
                TempData["ApproveRequestReturnMessage"] = "The request has been approved!";
                _context.ApprovalRequest.Remove(ApprovalRequest);
                var RemoveResult = _context.SaveChangesAsync();
                if (!RemoveResult.IsCompletedSuccessfully) {
                    TempData["ApproveRequestReturnMessage"] = "The request has been approved, but database is not updated!";
                }
            }
            else {
                TempData["ApproveRequestReturnMessage"] = "Error when approving request!";
            }

            return RedirectToAction("Approve", "Admin");
        }

        public IActionResult RejectRequest(string ApproveId)
        {
            ApprovalRequest ApprovalRequest = _context.ApprovalRequest.Where(x => x.ApproveId.Equals(ApproveId)).First();
            _context.ApprovalRequest.Remove(ApprovalRequest);
            var result = _context.SaveChangesAsync();
            result.Wait();
            if (result.IsCompletedSuccessfully) {
                TempData["RejectRequestReturnMessage"] = "Request has been rejected successfully!";
            }
            else {
                TempData["RejectRequestReturnMessage"] = "Error when rejecting request!";
            }

            return RedirectToAction("Approve", "Admin");
        }

        [HttpPost]
        public IActionResult ApproveAllRequest(string IdList)
        {
            List<string> ApproveIdList = IdList.Split("|").ToList();

            bool AllSuccess = true;
            List<string> ErrorId = new List<string>();
            List<ApprovalRequest> ApprovalRequestList = new List<ApprovalRequest>();
            foreach (var item in ApproveIdList) {
                ApprovalRequestList.Add(_context.ApprovalRequest.Where(x => x.ApproveId.Equals(item)).First()); 
            }

            foreach (var item in ApprovalRequestList) {
                UserAccess UserAccess = _context.UserAccess.Where(x => x.UserId.Equals(item.ReceiverUserId)).First();
                UserAccess.AddFileList(item.FileId);
                var ApproveResult = _context.SaveChangesAsync();
                ApproveResult.Wait();
                if (ApproveResult.IsCompletedSuccessfully) {
                    AllSuccess = ApproveResult.IsCompletedSuccessfully;
                    _context.ApprovalRequest.Remove(item);
                    var RemoveResult = _context.SaveChangesAsync();
                    RemoveResult.Wait();
                    if (!RemoveResult.IsCompletedSuccessfully)
                    {
                        AllSuccess = RemoveResult.IsCompletedSuccessfully;
                        ErrorId.Add(item.ApproveId);
                    }
                }
                else {
                    AllSuccess = ApproveResult.IsCompletedSuccessfully;
                    ErrorId.Add(item.ApproveId);
                }
            }

            if (AllSuccess)
            {
                TempData["ApproveAllRequestReturnMessage"] = "All requests has been approved successfully!";
            }
            else {
                TempData["ApproveAllRequestReturnMessage"] = "Error when approving some request, please check recent activities!";
            }

            return RedirectToAction("Approve", "Admin");
        }

        public IActionResult RejectAllRequest(string IdList)
        {
            List<string> ApproveIdList = IdList.Split("|").ToList();

            bool AllSuccess = true;
            List<string> ErrorId = new List<string>();
            List<ApprovalRequest> ApprovalRequestList = new List<ApprovalRequest>();
            foreach (var item in ApproveIdList)
            {
                ApprovalRequestList.Add(_context.ApprovalRequest.Where(x => x.ApproveId.Equals(item)).First());
            }

            foreach (var item in ApprovalRequestList) {
                _context.ApprovalRequest.Remove(item);
                var result = _context.SaveChangesAsync();
                result.Wait();
                if(!result.IsCompletedSuccessfully) {
                    AllSuccess = result.IsCompletedSuccessfully;
                    ErrorId.Add(item.ApproveId);
                }
            }

            if (AllSuccess)
            {
                TempData["RejectAllRequestReturnMessage"] = "All requests has been rejected successfully!";
            }
            else
            {
                TempData["RejectAllRequestReturnMessage"] = "Error when rejecting some request, please check recent activities!";
            }

            return RedirectToAction("Approve", "Admin");
        }

        public IActionResult Files()
        {
            List<RetrievedFileViewModel> ModelList = new List<RetrievedFileViewModel>();
            var FileList = _storage.GetFileList(_userManager.GetUserId(User)).Result;

            foreach (var item in FileList)
            {
                RetrievedFileViewModel model = new RetrievedFileViewModel();
                model.RetrievedFile = item;
                ModelList.Add(model);
            }

            return View(ModelList);
        }

        private List<UserInfo> GetUserList() {
            List<UserAccess> UserAccessList = _context.UserAccess.Where(x => x.AdminId.Equals(_userManager.GetUserId(User))).ToList();
            List<UserInfo> UserInfoList = new List<UserInfo>();

            foreach (var item in UserAccessList)
            {
                string username = _context.Users.Where(x => x.Id.Equals(item.UserId)).First().UserName;
                UserInfo userInfo = new UserInfo(item, username);
                UserInfoList.Add(userInfo);
            }

            return UserInfoList;
        }

        [ActionName("ViewUser")]
        public IActionResult ViewUser()
        {
            return View(GetUserList());
        }

        [ActionName("DeleteUser")]
        public IActionResult DeleteUser(string UserId)
        {
            IdentityUser UserToBeDelete = _context.Users.Where(x => x.Id.Equals(UserId)).First();
            UserAccess UserAccessToBeDelete = _context.UserAccess.Where(x => x.UserId.Equals(UserId)).First();

            _context.Users.Remove(UserToBeDelete);
            _context.SaveChangesAsync();
            _context.UserAccess.Remove(UserAccessToBeDelete);
            var result = _context.SaveChangesAsync();
            result.Wait();

            if (result.IsCompletedSuccessfully)
            {
                TempData["DeleteUserReturnMessage"] = "Successfully deleted this user!";
            }
            else {
                TempData["DeleteUserReturnMessage"] = "Error when deleting this user!";
            }
            
            return RedirectToAction("ViewUser", "Admin");
        }



        [ActionName("Upload")]
        public ActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(FileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                foreach (var item in model.Files)
                {
                    if (!_storage.UploadFile(_userManager.GetUserId(User), item))
                    {
                        ViewBag.ReturnMessage = "Error Uploading File!";
                    }
                }
            }

            if (ViewBag.UploadReturnMessage == null)
            {
                ViewBag.UploadReturnMessage = "Upload Successful!";
            }
            return View();
        }

        [ActionName("CreateUser")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CreateUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUserAsync(NewUserViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                model.Admin = _userManager.GetUserId(User);
                var newUser = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true }; // email is confrimed by default
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");
                    AdminAccess ac = _context.AdminAccess.Where(x => x.AdminId.Equals(_userManager.GetUserId(User))).First();
                    if (ac.UserList == null)
                    {
                        ac.UserList = newUser.Id;
                    }
                    else
                    {
                        ac.UserList = ac.UserList + "|" + newUser.Id;
                    }

                    _context.AdminAccess.Update(ac);
                    UserAccess ua = new UserAccess(newUser.Id, _userManager.GetUserId(User), ac.OrganizationName);
                    _context.UserAccess.Add(ua);
                    _context.SaveChanges();
                    ViewBag.CreateUserReturnMessage = "A new user has been created!";
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        [ActionName("Delete")]
        public ActionResult Delete(string FileName)
        {
            if (FileName == null)
            {
                return BadRequest();
            }
            else
            {
                RetrievedFileViewModel model = new RetrievedFileViewModel();
                model.RetrievedFile = _storage.GetSpecificFile(_userManager.GetUserId(User), FileName);
                return View(model);
            }
        }

        [HttpPost]
        [ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(string FileName)
        {
            if (FileName == null)
            {
                return BadRequest();
            }
            else
            {

                if (_storage.DeleteFile(_userManager.GetUserId(User), FileName))
                {
                    ViewBag.DeleteFileReturnMessage = "File deleted successfully!";
                }
                else
                {
                    ViewBag.DeleteFileReturnMessage = "Error when deleting file!";
                }
                return RedirectToAction("Index", "Admin");
            }

        }

        [ActionName("Authorize")]
        public ActionResult Authorize(string FileName)
        {
            if (FileName == null)
            {
                return BadRequest();
            }

            List<UserAccess> UserAccessList = _context.UserAccess.Where(x => x.AdminId.Equals(_userManager.GetUserId(User))).ToList();
            List<UserInfo> UserInfoList = new List<UserInfo>();

            foreach (var item in UserAccessList)
            {
                string username = _context.Users.Where(x => x.Id.Equals(item.UserId)).First().UserName;
                UserInfo userInfo = new UserInfo(item, username);
                UserInfoList.Add(userInfo);
            }
            TempData["FileName"] = FileName;
            return View(UserInfoList);
        }


        [ActionName("AuthorizeAction")]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorizeAction(string UserId, string FileName)
        {
            UserAccess UserAccess = _context.UserAccess.Where(x => x.UserId.Equals(UserId)).First();
            if (UserAccess.FileList == null)
            {
                UserAccess.FileList = _security.Encrypt(FileName);
            }
            else {
                string[] FileList = UserAccess.FileList.Split("|");
                if (FileList.Contains(_security.Encrypt(FileName)))
                {
                    TempData["AuthorizeReturnMessage"] = "Selected user already have access to this file!";
                    return RedirectToAction("Files", "Admin");
                }
                else {
                    UserAccess.FileList = UserAccess.FileList + "|" + _security.Encrypt(FileName);
                }
            }
            _context.UserAccess.Update(UserAccess);
            var result = _context.SaveChangesAsync();
            result.Wait();

            if (result.IsCompletedSuccessfully)
            {
                TempData["AuthorizeReturnMessage"] = "Successfully authorize file to selected user!";
            }
            else {
                TempData["AuthorizeReturnMessage"] = "Error when authorizing!";
            }
            return RedirectToAction("Files", "Admin");
        }

    }
}
