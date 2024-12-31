using ETickets.Models;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class RequestsCinemaController : Controller
    {
        private readonly IRequestsCinemaRepository requestsCinemaRepository;

        public RequestsCinemaController(IRequestsCinemaRepository requestsCinemaRepository)
        {
            this.requestsCinemaRepository = requestsCinemaRepository;
        }
        [HttpGet]
        public IActionResult CreateNewRequest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateNewRequest(RequestCinema requestCinema)
        {
            if (ModelState.IsValid)
            {
                requestsCinemaRepository.Create(requestCinema);
                TempData["success"] = "The request has been added successfully";
                return RedirectToAction("Index","Home");   
            }
            return View(requestCinema);
        }
    }
}
