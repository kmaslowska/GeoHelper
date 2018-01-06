using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoHelper.Models;
using GeoHelper.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace GeoHelper.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoHelperContext _context;
        private readonly ApplicationDbContext _contextApp;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(GeoHelperContext context, ApplicationDbContext contextApp, ILogger<ObliczeniaController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextApp = contextApp;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
            String email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
            List<UsersProjects> userProjects = (from proj in _context.UsersProjects
                                             where proj.user == email 
                                             select proj).ToList();
                ViewBag.numberOfProjects = userProjects.Count;
            }
            else
            {
                ViewBag.numberOfProjects = 0;
            }


            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
