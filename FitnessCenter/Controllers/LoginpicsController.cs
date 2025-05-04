using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Models;
using Microsoft.AspNetCore.Hosting;

namespace FitnessCenter.Controllers
{
    public class LoginpicsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public LoginpicsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Loginpics
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Loginpics.Include(l => l.Admin);
            return View(await modelContext.ToListAsync());
        }

        // GET: Loginpics/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Loginpics == null)
            {
                return NotFound();
            }

            var loginpic = await _context.Loginpics
                .Include(l => l.Admin)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loginpic == null)
            {
                return NotFound();
            }

            return View(loginpic);
        }

        // GET: Loginpics/Create
        public IActionResult Create()
        {
            ViewData["Adminid"] = new SelectList(_context.Staff, "StaffId", "StaffId");
            return View();
        }

        // POST: Loginpics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Loginimagepath,ImageFile,Adminid")] Loginpic loginpic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loginpic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Adminid"] = new SelectList(_context.Staff, "StaffId", "StaffId", loginpic.Adminid);
            return View(loginpic);
        }

        // GET: Loginpics/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Loginpics == null)
            {
                return NotFound();
            }


            var loginpic = await _context.Loginpics.FindAsync(id);
            if (loginpic == null)
            {
                return NotFound();
            }
            ViewData["Adminid"] = new SelectList(_context.Staff, "StaffId", "StaffId", loginpic.Adminid);
            return View(loginpic);
        }

        // POST: Loginpics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Loginimagepath,ImageFile,Adminid")] Loginpic loginpic)
        {
            if (id != loginpic.Id)
            {
                return NotFound();
            }

            
            var trackedEntity = _context.Loginpics.Local.FirstOrDefault(e => e.Id == id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            
            var existingLoginpic = await _context.Loginpics.FindAsync(id);
            if (existingLoginpic == null)
            {
                return NotFound();
            }

            
            if (loginpic.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + loginpic.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await loginpic.ImageFile.CopyToAsync(fileStream);
                }
                existingLoginpic.Loginimagepath = fileName; 
            }

            existingLoginpic.Adminid =1;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Staffs");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return View(loginpic);
            }
        }


        // GET: Loginpics/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Loginpics == null)
            {
                return NotFound();
            }

            var loginpic = await _context.Loginpics
                .Include(l => l.Admin)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loginpic == null)
            {
                return NotFound();
            }

            return View(loginpic);
        }

        // POST: Loginpics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Loginpics == null)
            {
                return Problem("Entity set 'ModelContext.Loginpics'  is null.");
            }
            var loginpic = await _context.Loginpics.FindAsync(id);
            if (loginpic != null)
            {
                _context.Loginpics.Remove(loginpic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoginpicExists(decimal id)
        {
            return (_context.Loginpics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
