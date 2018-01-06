using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeoHelper.Models;
using Microsoft.AspNetCore.Identity;
using GeoHelper.Data;
using Microsoft.Extensions.Logging;

namespace GeoHelper.Controllers
{
    public class UsersProjectsController : Controller
    {
        private readonly GeoHelperContext _context;
        private readonly ApplicationDbContext _contextApp;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersProjectsController(GeoHelperContext context, ApplicationDbContext contextApp, ILogger<ObliczeniaController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextApp = contextApp;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: UsersProjects
        public async Task<IActionResult> Index(int? id)
        {

            if (!id.HasValue)
            {
                ViewBag.isAll = true;
                string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;

                List<Project> projekty = (from proj in _context.Project
                                          where proj.owner == email
                                          select proj).ToList();
                List<UsersProjects> usersInProjektList = new List<UsersProjects>();
                for (int i = 0; i < projekty.Count; i++)
                {
                    List<UsersProjects> usersInProjekt = (from user in _context.UsersProjects
                                                          where user.projectId == projekty[i].ID
                                                          select user).ToList();
                    usersInProjektList.AddRange(usersInProjekt);
                }

                return View(usersInProjektList);
            }
            else
            {
                ViewBag.isAll = false;
                ViewBag.projectId = id;
                var usersInProjekt = (from user in _context.UsersProjects
                            where user.projectId == id
                            select user);
            return View(await usersInProjekt.ToListAsync());
            }

        }

        // GET: UsersProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersProjects = await _context.UsersProjects
                .SingleOrDefaultAsync(m => m.ID == id);
            if (usersProjects == null)
            {
                return NotFound();
            }

            return View(usersProjects);
        }

        // GET: UsersProjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UsersProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,projectId,user")] UsersProjects usersProjects)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersProjects);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = usersProjects.projectId });
            }
            return View(usersProjects);
        }

        // GET: UsersProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersProjects = await _context.UsersProjects.SingleOrDefaultAsync(m => m.ID == id);
            if (usersProjects == null)
            {
                return NotFound();
            }
            return View(usersProjects);
        }

        // POST: UsersProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,projectId,user")] UsersProjects usersProjects)
        {
            if (id != usersProjects.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersProjects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersProjectsExists(usersProjects.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = usersProjects.projectId });
            }
            return View(usersProjects);
        }

        // GET: UsersProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersProjects = await _context.UsersProjects
                .SingleOrDefaultAsync(m => m.ID == id);
            if (usersProjects == null)
            {
                return NotFound();
            }

            return View(usersProjects);
        }

        // POST: UsersProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usersProjects = await _context.UsersProjects.SingleOrDefaultAsync(m => m.ID == id);
            _context.UsersProjects.Remove(usersProjects);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = usersProjects.projectId });
        }

        private bool UsersProjectsExists(int id)
        {
            return _context.UsersProjects.Any(e => e.ID == id);
        }
    }
}
