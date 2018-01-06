using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeoHelper.Models;
using GeoHelper.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace GeoHelper.Controllers
{
    public class PointsController : Controller
    {
        private readonly GeoHelperContext _context;
        private readonly ApplicationDbContext _contextApp;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public PointsController(GeoHelperContext context, ApplicationDbContext contextApp, ILogger<ObliczeniaController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextApp = contextApp;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Points
        public async Task<IActionResult> IndexCurrentProject()
        {
            String email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
            UsersProjects usersInProjects = (from proj in _context.UsersProjects
                                             where proj.user == email && proj.leading == true
                                             select proj).First();
            var points = (from point in _context.Point
                          where point.projectId == usersInProjects.projectId
                          select point).ToList();
            ViewBag.projectId = usersInProjects.projectId;
            return View( points);
        }
        // GET: Points
        public async Task<IActionResult> Index(int? id)
        {
            var points = (from point in _context.Point
                                  where point.projectId == id
                                  select point);
            ViewBag.projectId = id;
            return View(await points.ToListAsync());
        }

        // GET: Points/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Point
                .SingleOrDefaultAsync(m => m.ID == id);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // GET: Points/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Points/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,name,x,y,z,projectId")] Point point)
        {
            if (ModelState.IsValid)
            {
                _context.Add(point);
                await _context.SaveChangesAsync();
                String email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                UsersProjects usersInProjects = (from proj in _context.UsersProjects
                                                 where proj.user == email && proj.leading == true
                                                 select proj).First();
                if (usersInProjects.projectId == point.projectId)
                {
                    return RedirectToAction("IndexCurrentProject");
                }
                else
                {
                    return RedirectToAction("Index", new { id = point.projectId });
                }

            }
            return View(point);
        }
        // GET: Points/CreateCurrentProject
        public async Task<IActionResult> CreateCurrentProject()
        {
            String email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
            UsersProjects usersInProjects = (from proj in _context.UsersProjects
                                             where proj.user == email && proj.leading == true
                                             select proj).First();
            ViewBag.projectId =usersInProjects.projectId ;
            return View();
        }

        // POST: Points/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCurrentProject([Bind("ID,name,x,y,z,projectId")] Point point)
        {
            if (ModelState.IsValid)
            {
                _context.Add(point);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexCurrentProject));
            }
            return View(point);
        }
        // GET: Points/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Point.SingleOrDefaultAsync(m => m.ID == id);
            if (point == null)
            {
                return NotFound();
            }
            return View(point);
        }

        // POST: Points/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,name,x,y,z,projectId")] Point point)
        {
            if (id != point.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(point);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PointExists(point.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = point.projectId });
            }
            return View(point);
        }

        // GET: Points/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var point = await _context.Point
                .SingleOrDefaultAsync(m => m.ID == id);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // POST: Points/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var point = await _context.Point.SingleOrDefaultAsync(m => m.ID == id);
            _context.Point.Remove(point);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = point.projectId });
        }

        private bool PointExists(int id)
        {
            return _context.Point.Any(e => e.ID == id);
        }
    }
}
