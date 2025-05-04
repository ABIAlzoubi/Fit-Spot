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
    public class AboutuspagesController : Controller
    {
        private readonly ModelContext _context;

        private readonly IWebHostEnvironment _webHostEnviroment;
        public AboutuspagesController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Aboutuspages
        public async Task<IActionResult> Index()
        {
            return _context.Aboutuspages != null ?
                        View(await _context.Aboutuspages.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Aboutuspages'  is null.");
        }

        // GET: Aboutuspages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutuspage == null)
            {
                return NotFound();
            }

            return View(aboutuspage);
        }

        // GET: Aboutuspages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aboutuspages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Headerpic,ImageFileHeaderPic,Headpic1,ImageFileHeadPic1,Headpic2,ImageFileHeadPic2,Aboutusheadertext,Aboutusparagraph")] Aboutuspage aboutuspage)
        {
            if (aboutuspage.ImageFileHeaderPic != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFileHeaderPic.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await aboutuspage.ImageFileHeaderPic.CopyToAsync(fileStream);
                }
                aboutuspage.Headerpic = fileName;
            }

            if (aboutuspage.ImageFileHeadPic1 != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFileHeadPic1.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await aboutuspage.ImageFileHeadPic1.CopyToAsync(fileStream);
                }
                aboutuspage.Headpic1 = fileName;
            }

            if (aboutuspage.ImageFileHeadPic2 != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFileHeadPic2.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await aboutuspage.ImageFileHeadPic2.CopyToAsync(fileStream);
                }
                aboutuspage.Headpic2 = fileName;
            }



            _context.Add(aboutuspage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Aboutuspages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages.FindAsync(id);
            if (aboutuspage == null)
            {
                return NotFound();
            }
            return View(aboutuspage);
        }

        // POST: Aboutuspages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Headerpic,ImageFileHeaderPic,Headpic1,ImageFileHeadPic1,Headpic2,ImageFileHeadPic2,Aboutusheadertext,Aboutusparagraph")] Aboutuspage aboutuspage)
        {
            if (id != aboutuspage.Id)
            {
                return NotFound();
            }

            var existingAboutuspage = await _context.Aboutuspages.FindAsync(id);
            if (existingAboutuspage == null)
            {
                return NotFound();
            }

            if (aboutuspage.ImageFileHeaderPic != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFileHeaderPic.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await aboutuspage.ImageFileHeaderPic.CopyToAsync(fileStream);
                }
                existingAboutuspage.Headerpic = fileName;
            }

            if (aboutuspage.ImageFileHeadPic1 != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFileHeadPic1.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await aboutuspage.ImageFileHeadPic1.CopyToAsync(fileStream);
                }
                existingAboutuspage.Headpic1 = fileName;
            }

            if (aboutuspage.ImageFileHeadPic2 != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + aboutuspage.ImageFileHeadPic2.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await aboutuspage.ImageFileHeadPic2.CopyToAsync(fileStream);
                }
                existingAboutuspage.Headpic2 = fileName;
            }

            existingAboutuspage.Aboutusheadertext = aboutuspage.Aboutusheadertext;
            existingAboutuspage.Aboutusparagraph = aboutuspage.Aboutusparagraph;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Staffs");
            }
            catch (DbUpdateConcurrencyException)
            {
                return View(aboutuspage);
            }
        }


        // GET: Aboutuspages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutuspages == null)
            {
                return NotFound();
            }

            var aboutuspage = await _context.Aboutuspages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutuspage == null)
            {
                return NotFound();
            }

            return View(aboutuspage);
        }

        // POST: Aboutuspages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutuspages == null)
            {
                return Problem("Entity set 'ModelContext.Aboutuspages'  is null.");
            }
            var aboutuspage = await _context.Aboutuspages.FindAsync(id);
            if (aboutuspage != null)
            {
                _context.Aboutuspages.Remove(aboutuspage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutuspageExists(decimal id)
        {
            return (_context.Aboutuspages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
