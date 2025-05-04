using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Models;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;

namespace FitnessCenter.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public WorkoutsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnviroment = webHostEnvironment;
        }

        // GET: Workouts
        public IActionResult Index()
        {
            var TrainerWorkoutList = _context.TrainerWorkouts.ToList();
            var workoutList =  _context.Workouts.ToList();

            var TrainerWorkouts = (from w in workoutList
                                  join tw in TrainerWorkoutList
                                  on w.WorkoutId equals tw.WorkoutId
                                  where tw.TrinerId == HttpContext.Session.GetInt32("TrainerID")
                                  select new Workout
                                   {
                                       WorkoutId = w.WorkoutId,
                                       WorkoutName = w.WorkoutName,
                                       WorkoutDuration = w.WorkoutDuration,
                                       Shift = w.Shift,
                                       Image = w.Image,
                                   }).ToList();

            return View(TrainerWorkouts);
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkoutId,WorkoutName,WorkoutDuration,Shift,Price,Image,ImageFile")] Workout workout)
        {

            if (workout.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + workout.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await workout.ImageFile.CopyToAsync(fileStream);
                }
                workout.Image = fileName;
            }
            if (workout.WorkoutDuration != null && workout.WorkoutName != null &&workout.Shift!=null && workout.Price!=null)
            {
                _context.Add(workout);
                await _context.SaveChangesAsync();

                var TrainerWorkout=new TrainerWorkout();
                TrainerWorkout.TrinerId = HttpContext.Session.GetInt32("TrainerID");
                TrainerWorkout.WorkoutId = workout.WorkoutId;
                _context.Add(TrainerWorkout);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(workout);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            return View(workout);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("WorkoutId,WorkoutName,WorkoutDuration,Shift,Price,Image,ImageFile")] Workout workout)
        {



            if (id != workout.WorkoutId)
            {
                return NotFound();
            }

            var UpdatedData=_context.Workouts.Where(x=>x.WorkoutId==id).SingleOrDefault()!;


            if (workout.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + workout.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await workout.ImageFile.CopyToAsync(fileStream);
                }
                workout.Image = fileName;
                UpdatedData.Image = workout.Image;
                UpdatedData.ImageFile=workout.ImageFile;
            }

            if (workout.WorkoutDuration != null && workout.WorkoutName != null && workout.Shift != null && workout.Price != null)
            {
                UpdatedData.WorkoutDuration = workout.WorkoutDuration;
                UpdatedData.WorkoutName = workout.WorkoutName;
                UpdatedData.Shift = workout.Shift;
                UpdatedData.Price= workout.Price;
                try
                {
                    _context.Update(UpdatedData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.WorkoutId))
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
            return View(workout);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Workouts == null)
            {
                return Problem("Entity set 'ModelContext.Workouts'  is null.");
            }
            var workout = await _context.Workouts.FindAsync(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(decimal id)
        {
            return (_context.Workouts?.Any(e => e.WorkoutId == id)).GetValueOrDefault();
        }
    }
}
