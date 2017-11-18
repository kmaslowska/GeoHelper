using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeoHelper.Models;
using Microsoft.Extensions.Logging;

namespace GeoHelper.Controllers
{
    public class ObliczeniaController : Controller
    {
        private readonly ILogger _logger;
        private readonly GeoHelperContext _context;


        public ObliczeniaController(GeoHelperContext context, ILogger<ObliczeniaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Obliczenia
        public async Task<IActionResult> Index()
        {
            return View(await _context.Point.ToListAsync());
        }

        // GET: Obliczenia/Details/5
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

        // GET: Obliczenia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Obliczenia/Create
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
                return RedirectToAction(nameof(Index));
            }
            return View(point);
        }

        // GET: Obliczenia/Edit/5
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

        // POST: Obliczenia/Edit/5
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
                return RedirectToAction(nameof(Index));
            }
            return View(point);
        }

        // GET: Obliczenia/Delete/5
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

        // POST: Obliczenia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var point = await _context.Point.SingleOrDefaultAsync(m => m.ID == id);
            _context.Point.Remove(point);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Obliczenia/Odleglosci
        public IActionResult Odleglosci()
        {
            _logger.LogDebug(message: "get-------------------------------------------------------------------------------------------------");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odleglosci_wynik(OdleglosciViewModel odleglosci)
        {
            _logger.LogDebug(message: "odleglosci wynik------------------------------------------------------------------------------------------------Weszło odległości");
            OdleglosciViewModel odleglosciViewModel = odleglosci;
            double roznicaX = odleglosciViewModel.x2 - odleglosciViewModel.x1;
            double roznicaY = odleglosciViewModel.y2 - odleglosciViewModel.y1;
            double roznicaZ = odleglosciViewModel.z2 - odleglosciViewModel.z1;
            odleglosciViewModel.score = Math.Sqrt(roznicaX*roznicaX+roznicaY*roznicaY+roznicaZ*roznicaZ);

            return View(odleglosciViewModel);
        }

        private bool PointExists(int id)
        {
            return _context.Point.Any(e => e.ID == id);
        }
    }
}
