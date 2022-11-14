using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Controllers
{
    public class ErrorController : Controller
    {
        [ActionName("NotFound")]
        public IActionResult CustomNotFound()
        {
            return View();
        }
    }
}
