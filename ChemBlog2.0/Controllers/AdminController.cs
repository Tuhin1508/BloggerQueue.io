using ChemBlog2._0.Data;
using ChemBlog2._0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace ChemBlog2._0.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ApplicationDBContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        public AdminController(ApplicationDBContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._db = db;
            this._userManager = userManager;
            this._signinManager = signInManager;
        }
        public IActionResult editor()
        {
            List<AdminPanelEntity> objList = _db.AddArticles.ToList();
            if(objList.Count==0)
            {
                return View();
            }
            return View(objList);
        }
        public IActionResult AdminPanel()
        {
            ViewBag.Id = UserRegId.RegId;
            return View();
        }
        [HttpPost]
        public IActionResult AdminPanel(string content, AdminPanelEntity adminPanelEntity)
        {
                var UserId=_userManager.GetUserId(User);
                adminPanelEntity.UserRegId = UserId;
                var buttonNames = Request.Form.Select(x => x.Key).ToList();
                string BtnName = buttonNames[buttonNames.Count() - 2];
                adminPanelEntity.EditorData = content.ToString();
                if (BtnName == "Preview")
                {
                    ViewBag.title = content.ToString();
                    return View();
                }
                _db.AddArticles.Add(adminPanelEntity);
                _db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("PublishedArticles");
            return View();
        }
        public IActionResult PublishedArticles()
        {
            List<AdminPanelEntity> objList = _db.AddArticles.ToList();
            List<AdminPanelEntity> UserSpecificobjList = new List<AdminPanelEntity>();
            foreach(var item in objList)
            {
                if(item.UserRegId == _userManager.GetUserId(User))
                {
                    UserSpecificobjList.Add(item);
                }
            }
            if(UserSpecificobjList!=null)
            {
                return View(UserSpecificobjList);
            }
            return View();
        }
        public IActionResult EditOrDelete()
        {   
            return View();
        }
        [HttpPost]
        public IActionResult EditOrDelete(string content,AdminPanelEntity adminPanelEntity)
        {
                bool checkPresentInDataBase = true;
                List<AdminPanelEntity> objList = _db.AddArticles.ToList();
                var buttonNames = Request.Form.Select(x => x.Key).ToList();
                AdminPanelEntity _adminPanelEntity = new AdminPanelEntity();
                foreach(var buttonName in buttonNames)
                {
                    string ButtonNameRegex = buttonName.Replace('+', ' ').Replace('_',' ').Trim();
                     ButtonNameRegex = Regex.Replace(ButtonNameRegex, "[0-9]", "" );
                    switch (ButtonNameRegex.Trim())
                    {
                        case "Edit":
                            {
                                string BtnName = buttonNames[0];
                                var SplitedStr = BtnName.Split('+');
                                int indexOfItem = Int16.Parse(SplitedStr[1]);
                                foreach (var item in objList)
                                {
                                    if (item.Id == indexOfItem)
                                    {
                                        _adminPanelEntity = item;
                                        //EditArticle(adminPanelEntity);
                                        break;
                                    }
                                }
                                return View(_adminPanelEntity);
                                break;
                            }
                        case "Delete":
                            {
                                string BtnName = buttonNames[0];
                                var SplitedStr = BtnName.Split('+');
                                int indexOfItem = Int16.Parse(SplitedStr[1]);
                                if (_db.AddArticles.Count() != 0)
                                {
                                    var Data = _db.AddArticles.Where(x => x.Id == indexOfItem).FirstOrDefault();
                                    if (Data != null)
                                    {
                                        _db.Remove(Data);
                                        _db.SaveChanges();
                                    }
                                }
                                return RedirectToAction("PublishedArticles");
                                break;
                            }
                        case "Submit":
                            {
                                string BtnName = buttonNames[4];
                                var SplitedStr = BtnName.Split('+');
                                int indexOfItem = Int16.Parse(SplitedStr[1]);
                                if (_db.AddArticles.Count() != 0)
                                {
                                    var Data = _db.AddArticles.Where(x => x.Id ==indexOfItem).FirstOrDefault();
                                    if (Data != null)
                                    {
                                        Data.Heading = adminPanelEntity.Heading;
                                        Data.ShortDescription = adminPanelEntity.ShortDescription;
                                        Data.ImagePath = adminPanelEntity.ImagePath;
                                        Data.EditorData = content.ToString();
                                        _db.Entry(Data).State = EntityState.Modified;
                                        _db.SaveChanges();
                                    }
                                }
                                return RedirectToAction("PublishedArticles");
                                break;
                            }
                        case "Preview":
                            {
                                break;
                            }
                    }
                }
            return View();
        }
        public IActionResult DeleteArticle(AdminPanelEntity adminPanelEntity)
        {
            //ViewBag.Heading = adminPanelEntity.Heading;
            //ViewBag.ShortDescription = adminPanelEntity.ShortDescription;
            ViewBag.EditorData = adminPanelEntity.EditorData.ToString();
            return View();
        }
        public IActionResult ReadArticle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ReadArticle(AdminPanelEntity adminPanelEntity)
        {
            bool checkPresentInDataBase = true;
            List<AdminPanelEntity> objList = _db.AddArticles.ToList();
            var buttonNames = Request.Form.Select(x => x.Key).ToList();
            AdminPanelEntity _adminPanelEntity = new AdminPanelEntity();
            foreach (var buttonName in buttonNames)
            {
                string ButtonNameRegex = buttonName.Replace('+', ' ').Replace('_', ' ').Trim();
                ButtonNameRegex = Regex.Replace(ButtonNameRegex, "[0-9]", "");
                switch (ButtonNameRegex.Trim())
                {
                    case "Read":
                        {
                            var SplitedStr = buttonNames[0].Split('+');
                            int indexOfItem = Int16.Parse(SplitedStr[1]);
                            if (_db.AddArticles.Count() != 0)
                            {
                                var Data = _db.AddArticles.Where(x => x.Id == indexOfItem).FirstOrDefault();
                                if (Data != null)
                                {
                                    try
                                    {
                                        ViewBag.Title = Data.EditorData.ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            return View(objList);
                            break;
                        }
                }
                break;
            }
            return null;
        }
    }   
}
