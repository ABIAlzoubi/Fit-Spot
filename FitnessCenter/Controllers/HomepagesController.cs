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
    public class HomepagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public HomepagesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Homepages
        public async Task<IActionResult> Index()
        {
            return _context.Homepages != null ?
                        View(await _context.Homepages.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Homepages'  is null.");
        }

        // GET: Homepages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // GET: Homepages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homepages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mainpic,ImageFileMainPic,Mainstatement,Joinuspic,ImageFileJoinUs,Joinusparagraph,Discountpic,ImageFileDiscount,Discountheader,Feedbackpic,ImageFileFeedback")] Homepage homepage)
        {

            if (homepage.ImageFileMainPic != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileMainPic.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileMainPic.CopyToAsync(fileStream);
                }
                homepage.Mainpic = fileName;
            }

            if (homepage.ImageFileJoinUs != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileJoinUs.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileJoinUs.CopyToAsync(fileStream);
                }
                homepage.Joinuspic = fileName;
            }

            if (homepage.ImageFileDiscount != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileDiscount.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileDiscount.CopyToAsync(fileStream);
                }
                homepage.Discountpic = fileName;
            }

            if (homepage.ImageFileFeedback != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileFeedback.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileFeedback.CopyToAsync(fileStream);
                }
                homepage.Feedbackpic = fileName;
            }



            _context.Add(homepage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Homepages/Edit/5
        public async Task<IActionResult> Edit(decimal? Pageid)
        {
            if (Pageid == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages.FindAsync(Pageid);
            if (homepage == null)
            {
                return NotFound();
            }
            return View(homepage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Mainpic,ImageFileMainPic,Mainstatement,Joinuspic,ImageFileJoinUs,Joinusparagraph,Discountpic,ImageFileDiscount,Discountheader,Feedbackpic,ImageFileFeedback")] Homepage homepage)
        {
            if (id != homepage.Id)
            {
                return NotFound();
            }

            // Ensure no duplicate tracked entities
            var trackedEntity = _context.Homepages.Local.FirstOrDefault(e => e.Id == id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            // Fetch the entity to update
            var existingHomepage = await _context.Homepages.FindAsync(id);
            if (existingHomepage == null)
            {
                return NotFound();
            }

            // Update properties only if files are uploaded
            if (homepage.ImageFileMainPic != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileMainPic.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileMainPic.CopyToAsync(fileStream);
                }
                existingHomepage.Mainpic = fileName;
            }

            if (homepage.ImageFileJoinUs != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileJoinUs.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileJoinUs.CopyToAsync(fileStream);
                }
                existingHomepage.Joinuspic = fileName;
            }

            if (homepage.ImageFileDiscount != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileDiscount.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileDiscount.CopyToAsync(fileStream);
                }
                existingHomepage.Discountpic = fileName;
            }

            if (homepage.ImageFileFeedback != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + homepage.ImageFileFeedback.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await homepage.ImageFileFeedback.CopyToAsync(fileStream);
                }
                existingHomepage.Feedbackpic = fileName;
            }

            // Update other fields
            existingHomepage.Mainstatement = homepage.Mainstatement;
            existingHomepage.Joinusparagraph = homepage.Joinusparagraph;
            existingHomepage.Discountheader = homepage.Discountheader;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Staffs");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log exception for debugging
                Console.WriteLine(ex.Message);
                return View(homepage);
            }
        }


        // GET: Homepages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homepages == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // POST: Homepages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homepages == null)
            {
                return Problem("Entity set 'ModelContext.Homepages'  is null.");
            }
            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage != null)
            {
                _context.Homepages.Remove(homepage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepageExists(decimal id)
        {
            return (_context.Homepages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
