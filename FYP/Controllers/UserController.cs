using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Files()
        {
            return View();
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
