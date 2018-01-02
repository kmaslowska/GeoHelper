using GeoHelper.Data;
using GeoHelper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoHelper.Controllers
{
    public class ObliczeniaController : Controller
    {
        private readonly ApplicationDbContext _contextApp;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GeoHelperContext _context;


        public ObliczeniaController(GeoHelperContext context, ApplicationDbContext contextApp, ILogger<ObliczeniaController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _contextApp = contextApp;
            _logger = logger;
            _userManager = userManager;
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
        public async Task<IActionResult> Odleglosci()
        {
            OdleglosciViewModel odleglosciViewModel = new OdleglosciViewModel();
            if (User.Identity.IsAuthenticated)
            {
                string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                UsersProjects leadingProject = (from proj in _context.UsersProjects
                                where proj.user == email && proj.leading==true
                                select proj).First();
                odleglosciViewModel.pointList1= (from point in _context.Point
                                                 where point.projectId==leadingProject.projectId
                                                 select point).ToList();
                odleglosciViewModel.pointList2 = (from point in _context.Point
                                                  where point.projectId == leadingProject.projectId
                                                  select point).ToList();
            }
            _logger.LogDebug(message: "get-------------------------------------------------------------------------------------------------");
            return View(odleglosciViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odleglosci_wynik(OdleglosciViewModel odleglosci)
        {
            _logger.LogDebug(message: "odleglosci wynik------------------------------------------------------------------------------------------------Weszło odległości");
            OdleglosciViewModel odleglosciViewModel = odleglosci;
            if (odleglosci.selectedId1 != 0)
            {
                odleglosciViewModel.x1 = (from point in _context.Point
                                                  where point.ID == odleglosci.selectedId1
                                                  select point).First().x;
                odleglosciViewModel.y1 = (from point in _context.Point
                                          where point.ID == odleglosci.selectedId1
                                          select point).First().y;
                odleglosciViewModel.name1 = (from point in _context.Point
                                          where point.ID == odleglosci.selectedId1
                                          select point).First().name;
            }
            if (odleglosci.selectedId2 != 0)
            {
                odleglosciViewModel.x2 = (from point in _context.Point
                                          where point.ID == odleglosci.selectedId2
                                          select point).First().x;
                odleglosciViewModel.y2 = (from point in _context.Point
                                          where point.ID == odleglosci.selectedId2
                                          select point).First().y;
                odleglosciViewModel.name2 = (from point in _context.Point
                                             where point.ID == odleglosci.selectedId2
                                             select point).First().name;
            }
            odleglosciViewModel.obliczDlugosc();

            return View(odleglosciViewModel);
        }
        // GET: Obliczenia/Azymut
        public async Task<IActionResult> Azymut()
        {
            AzymutViewModel azymutViewModel = new AzymutViewModel();
            if (User.Identity.IsAuthenticated)
            {
                string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                UsersProjects leadingProject = (from proj in _context.UsersProjects
                                                where proj.user == email && proj.leading == true
                                                select proj).First();
                azymutViewModel.pointList = (from point in _context.Point
                                                  where point.projectId == leadingProject.projectId
                                                  select point).ToList();
            }
            _logger.LogDebug(message: "get-------------------------------------------------------------------------------------------------");
            return View(azymutViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Azymut_wynik(AzymutViewModel azymut)
        {
            _logger.LogDebug(message: "odleglosci wynik------------------------------------------------------------------------------------------------Weszło odległości");
            AzymutViewModel azymutViewModel = azymut;
            if (azymut.selectedId1 != 0)
            {
                azymutViewModel.x1 = (from point in _context.Point
                                          where point.ID == azymut.selectedId1
                                          select point).First().x;
                azymutViewModel.y1 = (from point in _context.Point
                                          where point.ID == azymut.selectedId1
                                          select point).First().y;
                azymutViewModel.name1 = (from point in _context.Point
                                             where point.ID == azymut.selectedId1
                                             select point).First().name;
            }
            if (azymut.selectedId2 != 0)
            {
                azymutViewModel.x2 = (from point in _context.Point
                                          where point.ID == azymut.selectedId2
                                          select point).First().x;
                azymutViewModel.y2 = (from point in _context.Point
                                          where point.ID == azymut.selectedId2
                                          select point).First().y;
                azymutViewModel.name2 = (from point in _context.Point
                                             where point.ID == azymut.selectedId2
                                             select point).First().name;
            }
            azymutViewModel.obliczAzymut();

            return View(azymutViewModel);
        }
        // GET: Obliczenia/KatPoziomy
        public async Task<IActionResult> KatPoziomy()
        {
            KatPoziomyViewModel katPoziomyViewModel = new KatPoziomyViewModel();
            if (User.Identity.IsAuthenticated)
            {
                string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                UsersProjects leadingProject = (from proj in _context.UsersProjects
                                                where proj.user == email && proj.leading == true
                                                select proj).First();
                katPoziomyViewModel.pointList = (from point in _context.Point
                                              where point.projectId == leadingProject.projectId
                                              select point).ToList();
            }
            _logger.LogDebug(message: "get-------------------------------------------------------------------------------------------------");
            return View(katPoziomyViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KatPoziomy_wynik(KatPoziomyViewModel katPoziomy)
        {
            _logger.LogDebug(message: "odleglosci wynik------------------------------------------------------------------------------------------------Weszło odległości");
            KatPoziomyViewModel katPoziomyViewModel = katPoziomy;
            if (katPoziomy.selectedId1 != 0)
            {
                katPoziomyViewModel.x1 = (from point in _context.Point
                                      where point.ID == katPoziomy.selectedId1
                                      select point).First().x;
                katPoziomyViewModel.y1 = (from point in _context.Point
                                      where point.ID == katPoziomy.selectedId1
                                      select point).First().y;
                katPoziomyViewModel.name1 = (from point in _context.Point
                                         where point.ID == katPoziomy.selectedId1
                                         select point).First().name;
            }
            if (katPoziomy.selectedId2 != 0)
            {
                katPoziomyViewModel.x2 = (from point in _context.Point
                                      where point.ID == katPoziomy.selectedId2
                                      select point).First().x;
                katPoziomyViewModel.y2 = (from point in _context.Point
                                      where point.ID == katPoziomy.selectedId2
                                      select point).First().y;
                katPoziomyViewModel.name2 = (from point in _context.Point
                                         where point.ID == katPoziomy.selectedId2
                                         select point).First().name;
            }
            if (katPoziomy.selectedId3 != 0)
            {
                katPoziomyViewModel.x3 = (from point in _context.Point
                                          where point.ID == katPoziomy.selectedId2
                                          select point).First().x;
                katPoziomyViewModel.y3 = (from point in _context.Point
                                          where point.ID == katPoziomy.selectedId2
                                          select point).First().y;
                katPoziomyViewModel.name3 = (from point in _context.Point
                                             where point.ID == katPoziomy.selectedId2
                                             select point).First().name;
            }
            katPoziomyViewModel.obliczKatPoziomy();

            return View(katPoziomyViewModel);
        }
        // GET: Obliczenia/ZmianaMiarKatowych
        public IActionResult ZmianaMiarKatowych()
        {
            ZmianaMiarKatowychViewModel zmianaMiarKatowychViewModel = new ZmianaMiarKatowychViewModel();
            zmianaMiarKatowychViewModel.typeOfChange = new List<SelectListItem>
             {
                    new SelectListItem {Text = "stopnie->grady", Value = "stopnie->grady"},
                    new SelectListItem {Text = "grady->stopnie", Value = "grady->stopnie"},
                    new SelectListItem {Text = "stopnie->radiany", Value = "stopnie->radiany"},
                    new SelectListItem {Text = "radiany->stopnie", Value = "radiany->stopnie"},
                    new SelectListItem {Text = "grady->radiany", Value = "grady->radiany"},
                    new SelectListItem {Text = "radiany->grady", Value = "radiany->grady"}
            };

            return View(zmianaMiarKatowychViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZmianaMiarKatowych_wynik(ZmianaMiarKatowychViewModel zmianaMiarKatowych)
        {

            ZmianaMiarKatowychViewModel zmianaMiarKatowychViewModel = zmianaMiarKatowych;
            zmianaMiarKatowychViewModel.zamienMiary();

            return View(zmianaMiarKatowychViewModel);
        }

        // GET: Obliczenia/PunktyNaProstej
        public async Task<IActionResult> PunktyNaProstej()
        {
            PunktyNaProstejViewModel punktyNaProstejViewModel = new PunktyNaProstejViewModel();
            if (User.Identity.IsAuthenticated)
            {
                string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                UsersProjects leadingProject = (from proj in _context.UsersProjects
                                                where proj.user == email && proj.leading == true
                                                select proj).First();
                punktyNaProstejViewModel.pointList1 = (from point in _context.Point
                                                  where point.projectId == leadingProject.projectId
                                                  select point).ToList();
                punktyNaProstejViewModel.pointList2 = (from point in _context.Point
                                                  where point.projectId == leadingProject.projectId
                                                  select point).ToList();
            }
            return View(punktyNaProstejViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PunktyNaProstej_wynik(PunktyNaProstejViewModel punktyNaProstej)
        {
            PunktyNaProstejViewModel punktyNaProstejViewModel = punktyNaProstej;
            if (punktyNaProstej.selectedId1 != 0)
            {
                punktyNaProstejViewModel.x1 = (from point in _context.Point
                                          where point.ID == punktyNaProstej.selectedId1
                                          select point).First().x;
                punktyNaProstejViewModel.y1 = (from point in _context.Point
                                          where point.ID == punktyNaProstej.selectedId1
                                          select point).First().y;
                punktyNaProstejViewModel.name1 = (from point in _context.Point
                                             where point.ID == punktyNaProstej.selectedId1
                                             select point).First().name;
            }
            if (punktyNaProstej.selectedId2 != 0)
            {
                punktyNaProstejViewModel.x2 = (from point in _context.Point
                                          where point.ID == punktyNaProstej.selectedId2
                                          select point).First().x;
                punktyNaProstejViewModel.y2 = (from point in _context.Point
                                          where point.ID == punktyNaProstej.selectedId2
                                          select point).First().y;
                punktyNaProstejViewModel.name2 = (from point in _context.Point
                                             where point.ID == punktyNaProstej.selectedId2
                                             select point).First().name;
            }
            punktyNaProstejViewModel.obliczPunktNaProstej();

            return View(punktyNaProstejViewModel);
        }
        // GET: Obliczenia/MetodaBiegunowa
        public async Task<IActionResult> MetodaBiegunowa()
        {
            BiegunowaViewModel biegunowaViewModel = new BiegunowaViewModel();
            if (User.Identity.IsAuthenticated)
            {
                string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                UsersProjects leadingProject = (from proj in _context.UsersProjects
                                                where proj.user == email && proj.leading == true
                                                select proj).First();
                biegunowaViewModel.pointList = (from point in _context.Point
                                                 where point.projectId == leadingProject.projectId
                                                 select point).ToList();
            }
            _logger.LogDebug(message: "get-------------------------------------------------------------------------------------------------");
            return View(biegunowaViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MetodaBiegunowa_wynik(BiegunowaViewModel biegunowa)
        {
            _logger.LogDebug(message: "odleglosci wynik------------------------------------------------------------------------------------------------Weszło odległości");
            BiegunowaViewModel biegunowaViewModel = biegunowa;
            if (biegunowa.selectedId1 != 0)
            {
                biegunowaViewModel.x1 = (from point in _context.Point
                                          where point.ID == biegunowa.selectedId1
                                          select point).First().x;
                biegunowaViewModel.y1 = (from point in _context.Point
                                          where point.ID == biegunowa.selectedId1
                                          select point).First().y;
                biegunowaViewModel.name1 = (from point in _context.Point
                                             where point.ID == biegunowa.selectedId1
                                             select point).First().name;
            }
            if (biegunowa.selectedId2 != 0)
            {
                biegunowaViewModel.x2 = (from point in _context.Point
                                          where point.ID == biegunowa.selectedId2
                                          select point).First().x;
                biegunowaViewModel.y2 = (from point in _context.Point
                                          where point.ID == biegunowa.selectedId2
                                          select point).First().y;
                biegunowaViewModel.name2 = (from point in _context.Point
                                             where point.ID == biegunowa.selectedId2
                                             select point).First().name;
            }
            if (biegunowa.selectedId3 != 0)
            {
                biegunowaViewModel.x3 = (from point in _context.Point
                                          where point.ID == biegunowa.selectedId2
                                          select point).First().x;
                biegunowaViewModel.y3 = (from point in _context.Point
                                          where point.ID == biegunowa.selectedId2
                                          select point).First().y;
                biegunowaViewModel.name3 = (from point in _context.Point
                                             where point.ID == biegunowa.selectedId2
                                             select point).First().name;
            }
            biegunowaViewModel.obliczWspolrzednePunktu();

            return View(biegunowaViewModel);
        }
        // GET: Obliczenia/DomiaryProstokątne
        public async Task<IActionResult> DomiaryProstokatne()
        {
            DomiaryViewModel domiaryViewModel = new DomiaryViewModel();
            if (User.Identity.IsAuthenticated)
            {
                string email = (await _userManager.GetUserAsync(HttpContext.User))?.Email;
                UsersProjects leadingProject = (from proj in _context.UsersProjects
                                                where proj.user == email && proj.leading == true
                                                select proj).First();
                domiaryViewModel.pointList = (from point in _context.Point
                                                where point.projectId == leadingProject.projectId
                                                select point).ToList();
            }
            domiaryViewModel.typeOfPoint= new List<SelectListItem>
             {
                    new SelectListItem {Text = "prawe", Value = "prawe"},
                    new SelectListItem {Text = "lewe", Value = "lewe"},

            };
            _logger.LogDebug(message: "get-------------------------------------------------------------------------------------------------");
            return View(domiaryViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DomiaryProstokatne_wynik(DomiaryViewModel domiary)
        {
            _logger.LogDebug(message: "domiary wynik------------------------------------------------------------------------------------------------Weszło domiary post");
            DomiaryViewModel domiaryViewModel = domiary;
            if (domiary.selectedId1 != 0)
            {
                domiaryViewModel.x1 = (from point in _context.Point
                                         where point.ID == domiary.selectedId1
                                         select point).First().x;
                domiaryViewModel.y1 = (from point in _context.Point
                                         where point.ID == domiary.selectedId1
                                         select point).First().y;
                domiaryViewModel.name1 = (from point in _context.Point
                                            where point.ID == domiary.selectedId1
                                            select point).First().name;
            }
            if (domiary.selectedId2 != 0)
            {
                domiaryViewModel.x2 = (from point in _context.Point
                                         where point.ID == domiary.selectedId2
                                         select point).First().x;
                domiaryViewModel.y2 = (from point in _context.Point
                                         where point.ID == domiary.selectedId2
                                         select point).First().y;
                domiaryViewModel.name2 = (from point in _context.Point
                                            where point.ID == domiary.selectedId2
                                            select point).First().name;
            }
            domiaryViewModel.obliczWspolrzednePunktu();

            return View(domiaryViewModel);
        }

        private bool PointExists(int id)
        {
            return _context.Point.Any(e => e.ID == id);
        }
    }
}
