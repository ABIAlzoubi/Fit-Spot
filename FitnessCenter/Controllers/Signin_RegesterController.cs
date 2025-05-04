using FitnessCenter.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FitnessCenter.Controllers
{
    public class Signin_RegesterController : Controller
    {

        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public Signin_RegesterController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnviroment = webHostEnvironment;
        }


        public IActionResult Signin()
        {
            var picture = _context.Loginpics.Where(x => x.Id == 1).SingleOrDefault()!;
            
            
            ViewBag.loginPic = picture.Loginimagepath;
            return View();
        }

        [HttpPost]
        public IActionResult Signin([Bind("Email,Password")] Member userSignin)
        {
            var auth = _context.Members.Where(x => x.Email == userSignin.Email && x.Password == userSignin.Password && x.RoleId == 3).SingleOrDefault();
            var picture = _context.Loginpics.Where(x => x.Id == 1).SingleOrDefault()!;
            ViewBag.loginPic = picture.Loginimagepath;



            if (auth != null)
            {

                //Member
                HttpContext.Session.SetInt32("MemberID", (int)auth.MemberId);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("WelcomeMember", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                ModelState.AddModelError("", "Fill all Fields Please");
                
            }

            return View();

        }


        public IActionResult StaffSignin()
        {
            var picture =  _context.Loginpics.Where(x => x.Id == 1).SingleOrDefault()!;
            ViewBag.loginPic = picture.Loginimagepath;

            return View();
        }

        [HttpPost]
        public IActionResult StaffSignin([Bind("Email,Password")] Staff StaffSignin)
        {

            var auth = _context.Staff.Where(x => x.Email == StaffSignin.Email && x.Password == StaffSignin.Password).SingleOrDefault();
            var picture = _context.Loginpics.Where(x => x.Id == 1).SingleOrDefault()!;
            ViewBag.loginPic = picture.Loginimagepath;
            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 1://Admin
                        HttpContext.Session.SetString("AdminName", auth.FirstName!);
                        HttpContext.Session.SetInt32("AdminID", (int)auth.StaffId);
                        return RedirectToAction("Index", "Staffs");

                    case 2://Trainer 
                        HttpContext.Session.SetString("TrainerFirstName", auth.FirstName!);
                        HttpContext.Session.SetInt32("TrainerID", (int)auth.StaffId);
                        return RedirectToAction("TrainerIndex", "Staffs");
                }

            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                ModelState.AddModelError("", "Fill all Fields Please");
            }

            return View();
        }





        public IActionResult Regester(int? courseid)
        {
            TempData["CourseId"] = courseid;


            var CardInfo = _context.Cridtcards.Where(x => x.CardId == 2).SingleOrDefault();
            var CoursePrice = _context.Workouts.Where(x => x.WorkoutId == courseid).SingleOrDefault();
            var price = CoursePrice.Price;
            ViewBag.CardNumber = CardInfo.CardNumber;
            ViewBag.CardCvv = CardInfo.CodeCvv;
            ViewBag.Expir = CardInfo.Expirationdate.Value.ToShortDateString();
            ViewBag.Price = price;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regester([Bind("MemberId,FirstName,LastName,Email,Image,ImageFile,JoinDate,WorkoutPlaneId,RoleId,Password")] Member newMember)
        {

            if (ModelState.IsValid)
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
                newMember.WorkoutPlaneId = (int)TempData["CourseId"]!;
                newMember.JoinDate = DateTime.Today;


                _context.Add(newMember);
                await _context.SaveChangesAsync();



                //Adding the process to Payment 
                var price = _context.Workouts.Where(x => x.WorkoutId == (int)TempData["CourseId"]!).SingleOrDefault();
                var Amount = price.Price;

                var x = new Payment();
                x.Amount = Amount;
                x.PaymentDate = DateTime.Today;
                x.MemberId = newMember.MemberId;
                _context.Add(x);
                await _context.SaveChangesAsync();


                return RedirectToAction("Signin");
            }
            else
            {
                ModelState.AddModelError("", "Fill all Fields Please");
            }
            return View(newMember);
        }










        public IActionResult CardRegester()
        {

            return View();
        }



        //Update the card

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CardRegester([Bind("CardId,CardNumber,Expirationdate,CodeCvv,Cardbalance")] Cridtcard newCard, decimal id = 2)
        {
            if (id == 2)
            {
                var cardInfo = _context.Cridtcards.Where(x => x.CardId == 2).SingleOrDefault()!;

                var CourseId = (int)TempData["CourseId"]!;
                var CoursePrice = _context.Workouts.Where(x => x.WorkoutId == CourseId).SingleOrDefault()!;
                var Price = (int)CoursePrice.Price!;
                var currentBlanance = (int)cardInfo.Cardbalance!;
                var balnce = currentBlanance - Price;
                
                cardInfo.Cardbalance = balnce;

                if (cardInfo.Cardbalance > Price)
                {

                    try
                    {
                        TempData["showButton"] = "true";
                        _context.Update(cardInfo);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Regester", new { courseid = CourseId });

                    }
                    catch
                    {
                        TempData["showButton"] = "False";
                        return RedirectToAction("Regester", new { courseid = CourseId });
                    }
                }
                else
                {
                    TempData["showButton"] = "NOT";
                    return RedirectToAction("Regester", new { courseid = CourseId });
                }

            }
            else { return NotFound(); }
        }





        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Signin");
        }



    }
}
