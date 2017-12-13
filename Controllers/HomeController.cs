using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("Meals") == null)
            {
                HttpContext.Session.SetInt32("Meals", 3);
                HttpContext.Session.SetInt32("Happy", 20);
                HttpContext.Session.SetInt32("Full", 20);
                HttpContext.Session.SetInt32("Energy", 50);
                TempData["Image"] = "/img/egg.gif";                
            }
            if(HttpContext.Session.GetInt32("Happy") >= 100 && HttpContext.Session.GetInt32("Energy") >= 100 && HttpContext.Session.GetInt32("Full") >=100)
            {
                ViewBag.Win = "You did it! Tamagotchi has reached full potential.";
                TempData["Image"] = "/img/win.gif";
                
            }
            if(HttpContext.Session.GetInt32("Happy") <= 0 || HttpContext.Session.GetInt32("Full") <= 0){
                TempData["Image"] = "/img/rip.gif";
                ViewBag.Lose = "R.I.P. little Tamagotchi";
            }
            ViewBag.Meals = HttpContext.Session.GetInt32("Meals");
            ViewBag.Happy = HttpContext.Session.GetInt32("Happy");
            ViewBag.Full = HttpContext.Session.GetInt32("Full");
            ViewBag.Energy = HttpContext.Session.GetInt32("Energy");
            ViewBag.Outcome = TempData["Outcome"];
            ViewBag.Image = TempData["Image"];
            return View("Index");
        }
        [HttpPost]
        [Route("feed")]
        public IActionResult Feed()
        {
            if ((int)HttpContext.Session.GetInt32("Meals") < 1){
                TempData["Outcome"] = "You do not have any meals.";
                TempData["Image"] = "/img/default.gif";
                return RedirectToAction("Index");
                
            }
            Random rand = new Random();
            if (rand.Next(1,5) != 4){
                int fullness = rand.Next(5,11);
                int full = (int)HttpContext.Session.GetInt32("Full") + fullness;
                HttpContext.Session.SetInt32("Full", full);
                TempData["Outcome"] = "You fed Tamagotchi. Fullness +" + fullness;
                TempData["Image"] = "/img/eat.gif";
            }
            else{
                TempData["Outcome"] = "Tamagotchi is Angry! Throws meal away.";
                TempData["Image"] = "/img/angry.gif";
                
            }
            int meal = (int)HttpContext.Session.GetInt32("Meals") - 1;
            HttpContext.Session.SetInt32("Meals", meal);
            
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        [Route("play")]
        public IActionResult Play()
        {
            if ((int)HttpContext.Session.GetInt32("Energy") < 5){
                TempData["Outcome"] = "Needs 5 Energy.";
                TempData["Image"] = "/img/default.gif";                
                return RedirectToAction("Index");
            }
            Random rand = new Random();
            if (rand.Next(1,5) != 4){
                int happiness = rand.Next(5,11);
                int happy = (int)HttpContext.Session.GetInt32("Happy") + happiness;
                HttpContext.Session.SetInt32("Happy", happy);
                TempData["Image"] = "/img/ball.gif";                
                TempData["Outcome"] = "Played with Tamagotchi. Happiness +" + happiness;
            }
            else{
                TempData["Image"] = "/img/angry.gif";                
                TempData["Outcome"] = "Tamagotchi is upset! Bites your hand.";
            }
            int energy = (int)HttpContext.Session.GetInt32("Energy") - 5;
            HttpContext.Session.SetInt32("Energy", energy);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("work")]
        public IActionResult Work()
        {
            if ((int)HttpContext.Session.GetInt32("Energy") < 5){
                TempData["Outcome"] = "Needs 5 Energy.";
                TempData["Image"] = "/img/default.gif";                
                return RedirectToAction("Index");
            }
            Random rand = new Random();
            int meals = rand.Next(1,4);
            int meal = (int)HttpContext.Session.GetInt32("Meals") + meals;
            HttpContext.Session.SetInt32("Meals", meal);
            int energy = (int)HttpContext.Session.GetInt32("Energy") - 5;
            HttpContext.Session.SetInt32("Energy", energy);
            TempData["Image"] = "/img/buy.gif";            
            TempData["Outcome"] = "Finished working. Bought " + meals + " meals." ;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("sleep")]
        public IActionResult Sleep()
        {
            int happiness = (int)HttpContext.Session.GetInt32("Happy") - 5;
            HttpContext.Session.SetInt32("Happy", happiness);
            int fullness = (int)HttpContext.Session.GetInt32("Full") - 5;
            HttpContext.Session.SetInt32("Full", fullness);
            int energy = (int)HttpContext.Session.GetInt32("Energy") + 15;
            HttpContext.Session.SetInt32("Energy", energy);
            if (HttpContext.Session.GetInt32("Happy") < 0)
            {
                HttpContext.Session.SetInt32("Happy", 0);
            }
            if (HttpContext.Session.GetInt32("Full") < 0)
            {
                HttpContext.Session.SetInt32("Full", 0);
            }
            TempData["Image"] = "/img/sleep.gif";            
            TempData["Outcome"] = "Tamagotchi Slept. Energy + 15";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("restart")]
        public IActionResult Restart(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("hatch")]
        public IActionResult Hatch(){
            TempData["Image"] = "/img/love.gif";
            TempData["Outcome"] = "You hatched a Tamagotchi!!";
            return RedirectToAction("Index");
        }
    }
}