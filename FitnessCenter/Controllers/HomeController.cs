using System.Diagnostics;
using FitnessCenter.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ModelContext _context;

        private readonly IWebHostEnvironment _webHostEnviroment;


        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _logger = logger;
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {

            ViewData["MemberId"] = HttpContext.Session.GetInt32("MemberID");


            var SharedInfo = _context.Sharedlayouts.Where(x => x.Id == 7).SingleOrDefault()!;

            HttpContext.Session.SetString("LogoPic", SharedInfo.Logo!);
            HttpContext.Session.SetString("FacebookLink", SharedInfo.Facebooklink!);
            HttpContext.Session.SetString("TwitterLink", SharedInfo!.Twitterlink!);
            HttpContext.Session.SetString("GitHub", SharedInfo.Githublink!);
            HttpContext.Session.SetString("PhoterParagraph", SharedInfo.Photerparagraph!);
            HttpContext.Session.SetString("HomeLocation", SharedInfo.Homelocation!);
            HttpContext.Session.SetString("CopyWrite", SharedInfo.Copywritestatement!);


            var Courses = _context.Workouts.ToList();
            var HomePage = _context.Homepages.Where(x => x.Id == 2).SingleOrDefault()!;
            var feedBacks = _context.Testimonials.Where(x => x.Approved == true).OrderByDescending(x => x.TestimonialsDate).Take(4).ToList();

            var HomePageInfo = Tuple.Create<IEnumerable<Workout>, IEnumerable<Testimonial>, IEnumerable<Homepage>>(Courses, feedBacks, new[] { HomePage });


            // ViewData
            return View(HomePageInfo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SingleCourse(int? CourseId)
        {
            var course_trainer = _context.TrainerWorkouts.ToList();
            var trainers = _context.Staff.Where(x => x.RoleId == 2).ToList();

            var headerInfo = _context.Aboutuspages.Where(x => x.Id == 1).SingleOrDefault()!;
            ViewBag.HeaderImage = headerInfo.Headerpic;


            //Getting Trainer Info
            var result = (from t in trainers
                          join c1 in course_trainer
                          on t.StaffId equals c1.TrinerId
                          where c1.WorkoutId == CourseId
                          select new { t.FirstName, t.LastName }).ToArray();

            ViewBag.TrainerInfo = result;


            //check if the user has enrolled with other course
            var userid = HttpContext.Session.GetInt32("MemberID");
            if (userid != null)
            {
                var userinfo = _context.Members.Where(x => x.MemberId == userid).SingleOrDefault()!;
                if (userinfo.WorkoutPlaneId == null)
                {
                    ViewBag.IsEnrolled = false;
                }
                else
                {
                    ViewBag.IsEnrolled = true;
                }
            }
            else
            {
                ViewBag.IsEnrolled = false;
            }


            var Single_Course = _context.Workouts.Where(x => x.WorkoutId == CourseId).SingleOrDefault();
            if (Single_Course != null)
            {
                return View(Single_Course);
            }
            return View();

        }



        public IActionResult Pricing()
        {
            var Courses = _context.Workouts.ToList();

            var headerInfo = _context.Aboutuspages.Where(x => x.Id == 1).SingleOrDefault()!;
            ViewBag.HeaderImage = headerInfo.Headerpic;

            //check if the user has enrolled with other course
            var userid = HttpContext.Session.GetInt32("MemberID");
            if (userid != null)
            {
                var userinfo = _context.Members.Where(x => x.MemberId == userid).SingleOrDefault()!;
                if (userinfo.WorkoutPlaneId == null)
                {
                    ViewBag.IsEnrolled = false;
                }
                else
                {
                    ViewBag.IsEnrolled = true;
                }
            }
            else
            {
                ViewBag.IsEnrolled = false;
            }


            return View(Courses);
        }


        public IActionResult Courses()
        {
            var headerInfo = _context.Aboutuspages.Where(x => x.Id == 1).SingleOrDefault()!;
            ViewBag.HeaderImage = headerInfo.Headerpic;

            var Courses = _context.Workouts.ToList();
            return View(Courses);
        }

        public IActionResult AboutUs()
        {
            var aboutUsPageInfo = _context.Aboutuspages.Where(x => x.Id == 1).SingleOrDefault();

            return View(aboutUsPageInfo);
        }


        public IActionResult TrainersView()
        {
            var headerInfo = _context.Aboutuspages.Where(x => x.Id == 1).SingleOrDefault()!;
            ViewBag.HeaderImage = headerInfo.Headerpic;

            var trrainer = _context.Staff.Where(x => x.RoleId == 2).ToList();
            return View(trrainer);
        }


        public IActionResult MemberProfile()
        {
            var HomePageInfo = _context.Homepages.Where(x => x.Id == 2).SingleOrDefault()!;
            ViewBag.MemberProgilebackGround = HomePageInfo.Mainpic;
            var userId = HttpContext.Session.GetInt32("MemberID");
            var memberInfo = _context.Members.Where(x => x.MemberId == userId).SingleOrDefault();
            return View(memberInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberProfile([Bind("MemberId,FirstName,LastName,Email,Image,ImageFile,Password")] MemberInfoUpdate member)
        {
            var HomePageInfo = _context.Homepages.Where(x => x.Id == 2).SingleOrDefault()!;
            ViewBag.MemberProgilebackGround = HomePageInfo.Mainpic;


            var userId = HttpContext.Session.GetInt32("MemberID");

            Member updatedValues = _context.Members.Where(x => x.MemberId == userId).SingleOrDefault()!;


            if (userId != member.MemberId)
            {
                return NotFound();
            }

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
                updatedValues.Image = member.Image;
                updatedValues.ImageFile = member.ImageFile!;

            }
            else {
                updatedValues.Image = updatedValues.Image;
                updatedValues.ImageFile = updatedValues.ImageFile!;
            }

            if (member.FirstName != null && member.LastName != null && member.Email != null && member.Password != null)
            {
                try
                {
                    updatedValues.FirstName = member.FirstName;
                    updatedValues.LastName = member.LastName;
                    updatedValues.Email = member.Email;
                    updatedValues.Password = member.Password;

                    _context.Update(updatedValues);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("WelcomeMember");

                }
                catch
                {
                    var memberInfo = _context.Members.Where(x => x.MemberId == userId).SingleOrDefault();
                    return View(memberInfo);
                }
            }
            else {
                var memberInfo = _context.Members.Where(x => x.MemberId == userId).SingleOrDefault();
                return View(memberInfo);
            }

        }


        public IActionResult WelcomeMember()
        {
            var userId = HttpContext.Session.GetInt32("MemberID");

            var memberInfo = _context.Members.Where(x => x.MemberId == userId).SingleOrDefault()!;
            var memberWorkout = _context.Workouts.Where(x => x.WorkoutId == memberInfo.WorkoutPlaneId).SingleOrDefault()!;
            var userTestemonials = _context.Testimonials.Where(x => x.MemberId == userId).ToList();

            var HomePageInfo = _context.Homepages.Where(x => x.Id == 2).SingleOrDefault()!;
            ViewBag.backGroundImage = HomePageInfo.Mainpic;

            var SharedInfo = _context.Sharedlayouts.Where(x => x.Id == 7).SingleOrDefault()!;
            HttpContext.Session.SetString("LogoPic", SharedInfo.Logo!);
            HttpContext.Session.SetString("FacebookLink", SharedInfo.Facebooklink!);
            HttpContext.Session.SetString("TwitterLink", SharedInfo!.Twitterlink!);
            HttpContext.Session.SetString("GitHub", SharedInfo.Githublink!);
            HttpContext.Session.SetString("PhoterParagraph", SharedInfo.Photerparagraph!);
            HttpContext.Session.SetString("HomeLocation", SharedInfo.Homelocation!);
            HttpContext.Session.SetString("CopyWrite", SharedInfo.Copywritestatement!);


            var userData = Tuple.Create<FitnessCenter.Models.Member, FitnessCenter.Models.Workout, IEnumerable<Testimonial>>(memberInfo, memberWorkout, userTestemonials);


            return View(userData);
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
