using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics.Metrics;

namespace FitnessCenter.Controllers
{
    public class StaffsController : Controller
    {
        private readonly ModelContext _context;

        private readonly IWebHostEnvironment _webHostEnviroment;
        public StaffsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Staffs
        public IActionResult Index()
        {

            ViewBag.TrainersCount = _context.Staff.Where(x => x.RoleId == 2).Count();
            ViewBag.WorkOutsCount = _context.Workouts.Count();
            ViewBag.MembersCount = _context.Members.Count();
            ViewBag.ApprovedTestimonialCount = _context.Testimonials.Where(x => x.Approved == true).Count();
            ViewBag.UnapprovedTestimonialCount = _context.Testimonials.Where(x => x.Approved == false).Count();
            ViewBag.MembersNotes = _context.Contacts.Count();



            var Members1 = _context.Members.ToList();
            var Workout2 = _context.Workouts.ToList();



            //count hte last week date
            var lastWeekdate = DateTime.Now.AddDays(-7);
            //var RecentjoindUsers = _context.Members.Where(x => x.JoinDate > lastWeekdate).Take(3).ToList();

            //Getting the Most Populer workouts with user Count for each
            var workoutMemberCounts = Workout2.Select(Workout2 => new AdminDashDataClass
            {
                WorkoutId = Workout2.WorkoutId,
                WorkoutName = Workout2.WorkoutName!,
                Shift = Workout2.Shift!,
                Duration = (int)Workout2.WorkoutDuration!,
                MembersCount = Members1.Count(member => member.WorkoutPlaneId == Workout2.WorkoutId),
                RecentJoin = _context.Members.OrderByDescending(x => x.JoinDate).Take(3).ToList()
            }).OrderByDescending(x => x.MembersCount).Take(3).ToList();




            return View(workoutMemberCounts);
        }





        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Staff == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Role)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }









        // Admin Create New Trainer
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,FirstName,LastName,Email,Image,ImageFile,JoinDate,RoleId,Password")] Staff staff)
        {
            if (ModelState.IsValid)
            {

                if (staff.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + staff.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await staff.ImageFile.CopyToAsync(fileStream);
                    }
                    staff.Image = fileName;
                }
                staff.RoleId = 2;
                staff.JoinDate = DateTime.Now;



                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageTrainers");
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", staff.RoleId);
            return View(staff);

        }






        //Admin Update Trainer
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Staff == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", staff.RoleId);
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("StaffId,FirstName,LastName,Email,Image,ImageFile,JoinDate,RoleId,Password")] Staff staff)
        {
            if (id != staff.StaffId)
            {
                return NotFound();
            }

            staff.RoleId = 2;
            var UpdatedData = _context.Staff.Where(x => x.StaffId == id).SingleOrDefault()!;



            if (staff.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + staff.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await staff.ImageFile.CopyToAsync(fileStream);
                }
                staff.Image = fileName;
                UpdatedData.Image = staff.Image;
                UpdatedData.ImageFile = staff.ImageFile;
            }
            else
            {
                UpdatedData.Image = UpdatedData.Image;
                UpdatedData.ImageFile = UpdatedData.ImageFile;
            }


            if (staff.FirstName != null && staff.LastName != null && staff.Password != null && staff.Email != null)
            {
                UpdatedData.FirstName = staff.FirstName;
                UpdatedData.LastName = staff.LastName;
                UpdatedData.Email = staff.Email;
                UpdatedData.JoinDate = staff.JoinDate;
                UpdatedData.Password = staff.Password;

                try
                {
                    _context.Update(UpdatedData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ManageTrainers");
            }
            else
            {

                ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", staff.RoleId);
                return View(staff);
            }
        }






        //Update Admin Profile

        public IActionResult UpdateAdmin()
        {

            var Adminid = HttpContext.Session.GetInt32("AdminID");
            var staff = _context.Staff.Where(x => x.StaffId == Adminid).SingleOrDefault();

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdmin([Bind("StaffId,FirstName,LastName,Email,Image,ImageFile,JoinDate,RoleId,Password")] Staff staff)
        {


            staff.RoleId = 1;
            var id = HttpContext.Session.GetInt32("AdminID");
            var UpdatedData = _context.Staff.Where(x => x.StaffId == id).SingleOrDefault()!;

            if (staff.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + staff.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await staff.ImageFile.CopyToAsync(fileStream);
                }
                staff.Image = fileName;
                UpdatedData.Image = staff.Image;
                UpdatedData.ImageFile = staff.ImageFile;
            }
            else
            {
                UpdatedData.Image = UpdatedData.Image;
                UpdatedData.ImageFile = UpdatedData.ImageFile;
            }

            if (staff.FirstName != null && staff.LastName != null && staff.Password != null && staff.Email != null)
            {
                UpdatedData.FirstName = staff.FirstName;
                UpdatedData.LastName = staff.LastName;
                UpdatedData.Email = staff.Email;
                UpdatedData.JoinDate = UpdatedData.JoinDate;
                UpdatedData.Password = staff.Password;

                try
                {
                    _context.Update(UpdatedData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Staffs");
            }
            else
            {
                ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", staff.RoleId);
                return View(staff);

            }
        }



        //Trainer Update his Profile
        public IActionResult UpdateTrainer()
        {

            var TrainerId = HttpContext.Session.GetInt32("TrainerID");
            var staff = _context.Staff.Where(x => x.StaffId == TrainerId).SingleOrDefault();

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrainer([Bind("StaffId,FirstName,LastName,Email,Image,ImageFile,JoinDate,RoleId,Password")] Staff staff)
        {


            staff.RoleId = 2;

            var id = HttpContext.Session.GetInt32("TrainerID");
            var UpdatedData = _context.Staff.Where(x => x.StaffId == id).SingleOrDefault()!;

            if (staff.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + staff.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await staff.ImageFile.CopyToAsync(fileStream);
                }
                staff.Image = fileName;
                UpdatedData.Image = staff.Image;
                UpdatedData.ImageFile = staff.ImageFile;
            }
            else
            {
                UpdatedData.Image = UpdatedData.Image;
                UpdatedData.ImageFile = UpdatedData.ImageFile;
            }

            if (staff.FirstName != null && staff.LastName != null && staff.Password != null && staff.Email != null)
            {
                UpdatedData.FirstName = staff.FirstName;
                UpdatedData.LastName = staff.LastName;
                UpdatedData.Email = staff.Email;
                UpdatedData.JoinDate = UpdatedData.JoinDate;
                UpdatedData.Password = staff.Password;
                try
                {
                    _context.Update(UpdatedData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.StaffId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("TrainerIndex", "Staffs");
            }
            else
            {
                ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", staff.RoleId);
                return View(staff);
            }
        }









        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Staff == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Role)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Staff == null)
            {
                return Problem("Entity set 'ModelContext.Staff'  is null.");
            }
            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(decimal id)
        {
            return (_context.Staff?.Any(e => e.StaffId == id)).GetValueOrDefault();
        }












        public IActionResult ManageTrainers()
        {
            var Trainers = _context.Staff.Where(x => x.RoleId == 2);
            return View(Trainers);
        }



        public IActionResult AboutUsMails()
        {
            var Mails = _context.Contacts.ToList();
            if (Mails == null)
            {
                return NotFound();
            }
            return View(Mails);
        }





        public IActionResult Search()
        {
            var memberList = _context.Members.ToList();
            var workoutList = _context.Workouts.ToList();
            var result = (from m in memberList
                          join w in workoutList
                          on m.WorkoutPlaneId equals w.WorkoutId
                          select new MemberWorkoutClass
                          {
                              Name = m.FirstName + " " + m.LastName,
                              Email = m.Email!,
                              WorkoutName = w.WorkoutName!,
                              WorkoutPrice = w.Price,
                              JoinDate = m.JoinDate
                          });

            return View(result);
        }
        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate)
        {
            var memberList = _context.Members.ToList();
            var workoutList = _context.Workouts.ToList();

            var result = (from m in memberList
                          join w in workoutList
                          on m.WorkoutPlaneId equals w.WorkoutId
                          select new MemberWorkoutClass
                          {
                              Name = m.FirstName + " " + m.LastName,
                              Email = m.Email!,
                              WorkoutName = w.WorkoutName!,
                              WorkoutPrice = w.Price,
                              JoinDate = m.JoinDate!.Value.Date
                          }).ToList();


            if (startDate == null && endDate == null)
            {
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {
                result = result.Where(x => x.JoinDate >= startDate).ToList();
                return View(result);
            }
            else if (startDate == null && endDate != null)
            {
                result = result.Where(x => x.JoinDate <= endDate).ToList();
                return View(result);
            }
            else
            {
                result = result.Where(x => x.JoinDate >= startDate && x.JoinDate <= endDate).ToList();

                return View(result);

            }
        }





















        public IActionResult Report()
        {
            var memberList = _context.Members.ToList();
            var workoutList = _context.Workouts.ToList();
            var result = (from m in memberList
                          join w in workoutList
                          on m.WorkoutPlaneId equals w.WorkoutId
                          select new MemberWorkoutClass
                          {
                              Name = m.FirstName + " " + m.LastName,
                              Email = m.Email!,
                              WorkoutName = w.WorkoutName!,
                              WorkoutPrice = w.Price,
                              JoinDate = m.JoinDate
                          });

            return View(result);
        }

        [HttpPost]
        public IActionResult Report(int startDate=2025)
        {

            var monthlySubscriptions = _context.Members
                                         .Where(sub => sub.JoinDate.Value.Year == startDate)
                                         .GroupBy(sub => sub.JoinDate.Value.Month)
                                         .Select(g => new
                                         {
                                             Month = g.Key,
                                             Count = g.Count()
                                         })
                                          .OrderBy(g => g.Month)
                                          .ToList();


            var annualSubscriptions = _context.Members
                .GroupBy(sub => sub.JoinDate.Value.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ToList();


            var years = annualSubscriptions.Select(y => y.Year.ToString()).ToList();
            var yearCounts = annualSubscriptions.Select(y => y.Count).ToList();
            var months = monthlySubscriptions.Select(m => System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.Month)).ToList();
            var counts = monthlySubscriptions.Select(m => m.Count).ToList();

            ViewBag.Years = years;
            ViewBag.AnnualCounts = yearCounts;
            ViewBag.Months = months;
            ViewBag.MonthlyCounts = counts;



            var memberList = _context.Members.ToList();
            var workoutList = _context.Workouts.ToList();
            var result = (from m in memberList
                          join w in workoutList
                          on m.WorkoutPlaneId equals w.WorkoutId
                          where m.JoinDate.Value.Year==startDate
                          select new MemberWorkoutClass
                          {
                              Name = m.FirstName + " " + m.LastName,
                              Email = m.Email!,
                              WorkoutName = w.WorkoutName!,
                              WorkoutPrice = w.Price,
                              JoinDate=m.JoinDate
                          });

            return View(result);




        }


        public IActionResult chart()
        {

            var monthlySubscriptions = _context.Members
                                         .Where(sub => sub.JoinDate.Value.Year == DateTime.Now.Year)
                                         .GroupBy(sub => sub.JoinDate.Value.Month)
                                         .Select(g => new
                                         {
                                             Month = g.Key,
                                             Count = g.Count()
                                         })
                                          .OrderBy(g => g.Month)
                                          .ToList();


            var annualSubscriptions = _context.Members
                .GroupBy(sub => sub.JoinDate.Value.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ToList();


            var years = annualSubscriptions.Select(y => y.Year.ToString()).ToList();
            var yearCounts = annualSubscriptions.Select(y => y.Count).ToList();
            var months = monthlySubscriptions.Select(m => System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.Month)).ToList();
            var counts = monthlySubscriptions.Select(m => m.Count).ToList();

            ViewBag.Years = years;
            ViewBag.AnnualCounts = yearCounts;
            ViewBag.Months = months;
            ViewBag.MonthlyCounts = counts;



            return View();
        }


        public IActionResult TrainerIndex()
        {

            var Workouts = _context.Workouts.ToList();
            var TrainerWorkoutList = _context.TrainerWorkouts.ToList();

            var TrainerWorkoutsList = (from w in Workouts
                                       join tw in TrainerWorkoutList
                                       on w.WorkoutId equals tw.WorkoutId
                                       where tw.TrinerId == HttpContext.Session.GetInt32("TrainerID")
                                       select new TrainerIndexInfoClass
                                       {
                                           WorkoutName = w.WorkoutName!,
                                           WorkoutDurashion = w.WorkoutDuration,
                                           WorkoutShift = w.Shift!,
                                           WorkoutImage = w.Image!,
                                           MembersInClass = _context.Members.Where(x => x.WorkoutPlaneId == w.WorkoutId).ToList()
                                       }).ToList();



            return View(TrainerWorkoutsList);
        }


        public IActionResult TrainerShowMembers()
        {

            var membersList = _context.Members.ToList();
            return View(membersList);
        }





    }
}
