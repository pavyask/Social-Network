using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging;
using NuGet.Protocol.Plugins;
using SocialNetwork.Data;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class UsersController : Controller
    {
        private readonly SocialNetworkService _service;

        public UsersController(SocialNetworkService service)
        {
            _service = service;
        }

        // GET: Users
        public async Task<IActionResult> List()
        {
            if (IsCurentUserAdmin())
                return View(await _service.GetAllUsers());

            else return NotFound();
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (IsCurentUserAdmin())
                return View();

            else return NotFound();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Login,CreationDateTime")] User user)
        {
            if (IsCurentUserAdmin())
            {
                if (ModelState.IsValid)
                    _service.CreateUser(user);

                return View(user);
            }
            else return NotFound();
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string login)
        {
            if (IsCurentUserAdmin())
            {
                if (login == null)
                    return NotFound();

                var user = await _service.GetUserByLoginOrNull(login);
                if (user == null)
                    return NotFound();

                return View(user);
            }
            else return NotFound();
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string login)
        {
            if (IsCurentUserAdmin())
            {
                var userToDelete = await _service.GetUserByLoginOrNull(login);
                if (userToDelete != null)
                {
                    _service.RemoveUser(userToDelete);
                    if (userToDelete.Login == Request.Cookies["login"])
                        return Logout();
                }
                return RedirectToAction(nameof(List));
            }
            else return NotFound();
        }

        // POST: Users/Login
        [HttpPost]
        public IActionResult Login(string login)
        {
            if (!IsUserLoggedIn())
            {
                var user = _service.GetUserByLoginOrNull(login);
                if (user != null)
                {
                    Response.Cookies.Append("login", login);
                    return RedirectToAction("Index", "Friends");
                }
                return RedirectToAction("Index", "Home");
            }
            else return NotFound();
        }

        // POST: Users/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            if (IsUserLoggedIn())
            {
                if (Request.Cookies["login"] != null)
                    Response.Cookies.Delete("login");
                return RedirectToAction("Index", "Home");
            }
            else return NotFound();
        }

        [HttpPost]
        public IActionResult Init()
        {
            var users = new List<User>{
                new User("John", DateTime.Now.AddDays(-1)),
                new User("Alice", DateTime.Now.AddDays(-2)),
                new User("Bob", DateTime.Now.AddDays(-3)),
            };


            
            users[0].AddFriend(users[1]);
            users[1].AddFriend(users[0]); users[1].AddFriend(users[2]);
            users[2].AddFriend(users[1]); users[2].AddFriend(users[3]);

            _service.CreateUsers(users);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _service.RemoveAllExceptAdmin();
            return RedirectToAction(nameof(List));
        }

        private bool IsCurentUserAdmin()
        {
            if (Request.Cookies.TryGetValue("login", out string? login))
            {
                return login == "admin";
            }
            else return false;
        }

        private bool IsUserLoggedIn() => Request.Cookies.TryGetValue("login", out _);
    }
}
