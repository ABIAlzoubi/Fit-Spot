using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Models;

namespace FitnessCenter.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials









        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.Member);
            return View(await modelContext.ToListAsync());
        }








        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimoniaDetail = _context.Testimonials.Where(x=>x.TestemonialId == id).SingleOrDefault()!;
            var MembersLdetail = _context.Members.Where(x=>x.MemberId== testimoniaDetail!.MemberId).SingleOrDefault()!;

            var details = new MembersTestimonealsClass();
            details.TestimonialText = testimoniaDetail.TestimonialsText;
            details.TestimonialDate = testimoniaDetail.TestimonialsDate;
            details.Approved = testimoniaDetail.Approved;
            details.MemberFirstName = MembersLdetail.FirstName;
            details.MemberLastName= MembersLdetail.LastName;


            return View(details);
        }









        // GET: Testimonials/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "MemberId");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestemonialId,TestimonialsText,TestimonialsDate,MemberId,Approved")] Testimonial testimonial)
        {

            testimonial.MemberId = HttpContext.Session.GetInt32("MemberID");
            testimonial.TestimonialsDate = DateTime.Today;
            testimonial.Approved = false;
            if (testimonial.TestimonialsText != null)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                TempData["Testimonial"] = "your feedback submitted successfully!";
                return RedirectToAction("WelcomeMember", "Home");
            }
            return View(testimonial);

        }
















        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", testimonial.MemberId);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("TestemonialId,TestimonialsText,TestimonialsDate,MemberId,Approved")] Testimonial testimonial)
        {
            if (id != testimonial.TestemonialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.TestemonialId))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "MemberId", testimonial.MemberId);
            return View(testimonial);
        }
















        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Member)
                .FirstOrDefaultAsync(m => m.TestemonialId == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.TestemonialId == id)).GetValueOrDefault();
        }
    }
}
