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
    public class MembersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public MembersController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnviroment = webHostEnvironment;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Members.Include(m => m.Role).Include(m => m.WorkoutPlane);
            return View(await modelContext.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Role)
                .Include(m => m.WorkoutPlane)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            ViewData["WorkoutPlaneId"] = new SelectList(_context.Workouts, "WorkoutId", "WorkoutId");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,FirstName,LastName,Email,Image,ImageFile,JoinDate,WorkoutPlaneId,RoleId,Password")] Member newMember)
        {


            if (newMember.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + newMember.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await newMember.ImageFile.CopyToAsync(fileStream);
                }
                newMember.Image = fileName;
            }
            newMember.RoleId = 3;
            newMember.JoinDate = DateTime.Today;

            if (newMember.FirstName != null && newMember.LastName != null && newMember.WorkoutPlaneId != null && newMember.Email != null)
            {
                _context.Add(newMember);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(newMember);
        }






       //Admin Manage Members 
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", member.RoleId);
            ViewData["WorkoutPlaneId"] = new SelectList(_context.Workouts, "WorkoutId", "WorkoutId", member.WorkoutPlaneId);
            return View(member);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("MemberId,FirstName,LastName,Email,Image,ImageFile,JoinDate,WorkoutPlaneId,Password")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }
            member.RoleId = 3;
            var UpdatedData= _context.Members.Where(x=>x.MemberId==id).SingleOrDefault()!;


            if (member.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + member.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await member.ImageFile.CopyToAsync(fileStream);
                }
                member.Image = fileName;
                UpdatedData.Image = member.Image;
                UpdatedData.ImageFile = member.ImageFile;
            }
            if (member.FirstName != null && member.LastName != null && member.Email != null && member.Password != null && member.JoinDate != null && member.WorkoutPlaneId != null)
            {

                UpdatedData.FirstName = member.FirstName;
                UpdatedData.LastName = member.LastName;
                UpdatedData.Email = member.Email;
                UpdatedData.Password = member.Password;
                UpdatedData.WorkoutPlaneId = member.WorkoutPlaneId;
                UpdatedData.JoinDate = member.JoinDate;

                try
                {
                    _context.Update(UpdatedData);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View(member);
                }

            }
            else {
                return View(member);
            }
            
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Role)
                .Include(m => m.WorkoutPlane)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'ModelContext.Members'  is null.");
            }
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(decimal id)
        {
            return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
