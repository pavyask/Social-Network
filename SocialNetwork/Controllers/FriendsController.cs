using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Data;
using SocialNetwork.Models;
using System.Linq;

namespace SocialNetwork.Controllers
{
    public class FriendsController : Controller
    {
        private readonly SocialNetworkData _context;

        public FriendsController(SocialNetworkData context)
        {
            //// constructor called every time when new view is returned?
            _context = context;
        }

        // GET: Friends
        public IActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: Friends/List
        public IActionResult List()
        {
            var user = GetCurrentUser();
            if (user == null)
                return NotFound();

            else return Json(user.Friends);
        }

        // POST: Friends/Add/{login}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(string login)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return NotFound();

            var userToAdd = _context.User.FirstOrDefault(user => user.Login == login);
            if (userToAdd == null || userToAdd.Login == currentUser.Login)
                return Json(false);
            else
            {
                if (currentUser.Friends.FirstOrDefault(user => user.Login == userToAdd.Login) == null)
                    currentUser.Friends.Add(userToAdd);

                return Json(true);
            }
        }

        // POST: Friends/Delete/{login}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string login)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return NotFound();

            var userToDelete = currentUser.Friends.FirstOrDefault(user => user.Login == login);
            if (userToDelete == null)
                return Json(false);
            else
            {
                currentUser.Friends.Remove(userToDelete);
                return Json(true);
            }
        }

        private User? GetCurrentUser()
        {
            if (Request.Cookies.TryGetValue("login", out string? login))
                return _context.User.FirstOrDefault(user => user.Login == login);

            else return null;
        }
    }
}
