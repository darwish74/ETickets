using ETickets.Models;
using ETickets.Repository;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles="Admin")]
        public IActionResult Index()
        {
            var Requests = requestsCinemaRepository.Get();
            return View(Requests);
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
                requestsCinemaRepository.Commit();  
                TempData["success"] = "The request has been added successfully";
                return RedirectToAction("Index","Home");   
            }
            return View(requestCinema);
        }
    }
}
