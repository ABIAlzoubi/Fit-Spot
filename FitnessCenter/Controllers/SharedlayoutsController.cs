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
    public class SharedlayoutsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public SharedlayoutsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Sharedlayouts
        public async Task<IActionResult> Index()
        {
            return _context.Sharedlayouts != null ?
                        View(await _context.Sharedlayouts.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Sharedlayouts'  is null.");
        }

        // GET: Sharedlayouts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Sharedlayouts == null)
            {
                return NotFound();
            }

            var sharedlayout = await _context.Sharedlayouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sharedlayout == null)
            {
                return NotFound();
            }

            return View(sharedlayout);
        }

        // GET: Sharedlayouts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sharedlayouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Logo,ImageFile,Facebooklink,Twitterlink,Githublink,Photerparagraph,Homelocation,Copywritestatement")] Sharedlayout sharedlayout)
        {
            if (sharedlayout.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + sharedlayout.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await sharedlayout.ImageFile.CopyToAsync(fileStream);
                }
                sharedlayout.Logo = fileName;
            }
            if (sharedlayout.Photerparagraph!.Length > 350)
            {

                sharedlayout.Photerparagraph = sharedlayout.Photerparagraph.Substring(0, 340);
            }


            _context.Add(sharedlayout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            return View(sharedlayout);
        }

        // GET: Sharedlayouts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Sharedlayouts == null)
            {
                return NotFound();
            }

            var sharedlayout = await _context.Sharedlayouts.FindAsync(id);
            if (sharedlayout == null)
            {
                return NotFound();
            }
            return View(sharedlayout);
        }

        // POST: Sharedlayouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Logo,ImageFile,Facebooklink,Twitterlink,Githublink,Photerparagraph,Homelocation,Copywritestatement")] Sharedlayout sharedlayout)
        {
            if (id != sharedlayout.Id)
            {
                return NotFound();
            }

            // Detach any existing tracked entity with the same Id to avoid conflicts
            var trackedEntity = _context.Sharedlayouts.Local.FirstOrDefault(e => e.Id == id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            // Fetch the existing entity from the database
            var existingSharedlayout = await _context.Sharedlayouts.FindAsync(id);
            if (existingSharedlayout == null)
            {
                return NotFound();
            }

            // Update only the properties that need changes
            if (sharedlayout.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + sharedlayout.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await sharedlayout.ImageFile.CopyToAsync(fileStream);
                }

                existingSharedlayout.Logo = fileName;
            }

            // Update other properties
            existingSharedlayout.Facebooklink = sharedlayout.Facebooklink;
            existingSharedlayout.Twitterlink = sharedlayout.Twitterlink;
            existingSharedlayout.Githublink = sharedlayout.Githublink;
            existingSharedlayout.Photerparagraph = sharedlayout.Photerparagraph;
            existingSharedlayout.Homelocation = sharedlayout.Homelocation;
            existingSharedlayout.Copywritestatement = sharedlayout.Copywritestatement;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Staffs");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Concurrency error: {ex.Message}");
                return View(sharedlayout);
            }
        }


        // GET: Sharedlayouts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Sharedlayouts == null)
            {
                return NotFound();
            }

            var sharedlayout = await _context.Sharedlayouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sharedlayout == null)
            {
                return NotFound();
            }

            return View(sharedlayout);
        }

        // POST: Sharedlayouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Sharedlayouts == null)
            {
                return Problem("Entity set 'ModelContext.Sharedlayouts'  is null.");
            }
            var sharedlayout = await _context.Sharedlayouts.FindAsync(id);
            if (sharedlayout != null)
            {
                _context.Sharedlayouts.Remove(sharedlayout);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SharedlayoutExists(decimal id)
        {
            return (_context.Sharedlayouts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
