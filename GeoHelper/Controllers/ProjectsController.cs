using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeoHelper.Models;
using GeoHelper.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace GeoHelper.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly GeoHelperContext _context;
        private readonly ApplicationDbContext _contextApp;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(GeoHelperContext context, ApplicationDbContext contextApp, ILogger<ObliczeniaController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextApp = contextApp;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
            _logger.LogDebug(message: "email----------------------------------------------------------------------------------------------"+email);
            var projekty= (from proj in _context.Project
                           where proj.owner == email 
                           select proj);
            return View(await projekty.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .SingleOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,owner,name,description,frameOfReference")] Project project)
        {
            if (ModelState.IsValid)
            {
                String email= (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                project.owner = email;
                _context.Add(project);
                await _context.SaveChangesAsync();
                //usuniecie flagi projektu wiodacego
                UsersProjects usersInProjects= (from proj in _context.UsersProjects
                                                where proj.user == email && proj.leading==true
                                                select proj).First();
                if (usersInProjects != null)
                {
                    usersInProjects.leading = false;
                    _context.Update(usersInProjects);
                    await _context.SaveChangesAsync();
                }
                
                Project projectAfterSaver = (from proj in _context.Project
                                             where proj.owner == email && proj.name==project.name && proj.description==project.description
                                             select proj).First();
                //dodanie użytkownika tworzącego jako kolaboranta i ustawienia projektu jako główny
                UsersProjects userProject = new UsersProjects();
                userProject.projectId = projectAfterSaver.ID;
                userProject.user = projectAfterSaver.owner;
                userProject.leading = true;
                _context.Add(userProject);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.SingleOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,owner,name,description,frameOfReference")] Project project)
        {
            if (id != project.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .SingleOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.SingleOrDefaultAsync(m => m.ID == id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }
    }
}
