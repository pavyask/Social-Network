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
        //private readonly SocialNetworkContext _context;
        private readonly SocialNetworkData _context;

        public UsersController(SocialNetworkData context)
        {
            //// constructor called every time when new view is returned?
            _context = context;
        }

        // GET: Users
        public IActionResult List()
        {
            if (IsCurentUserAdmin())
                return View(_context.User.ToList());

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
                {
                    _context.User.Add(user);
                    return RedirectToAction(nameof(List));
                }
                return View(user);
            }
            else return NotFound();
        }

        // GET: Users/Delete/5
        public IActionResult Delete(string login)
        {
            if (IsCurentUserAdmin())
            {
                if (login == null || _context.User == null)
                    return NotFound();

                var user = _context.User.FirstOrDefault(m => m.Login == login);
                if (user == null)
                    return NotFound();

                return View(user);
            }
            else return NotFound();
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string login)
        {
            if (IsCurentUserAdmin())
            {
                var user = _context.User.FirstOrDefault(user => user.Login == login);
                if (user != null)
                {
                    _context.User.Remove(user);
                    if (user.Login == Request.Cookies["login"])
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
                var user = _context.User.FirstOrDefault(user => user.Login == login);
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
                new User("Sam", DateTime.Now.AddDays(-4)),
                new User("Mia", DateTime.Now.AddDays(-5)),
                new User("Zoe", DateTime.Now.AddDays(-6)),
            };

            users[0].Friends.Add(users[1]);
            users[1].Friends.Add(users[0]); users[1].Friends.Add(users[2]);
            users[2].Friends.Add(users[1]); users[2].Friends.Add(users[3]); users[2].Friends.Add(users[4]);
            users[3].Friends.Add(users[2]);
            users[4].Friends.Add(users[3]); users[4].Friends.Add(users[5]);
            users[5].Friends.Add(users[4]); users[5].Friends.Add(users[0]); users[5].Friends.Add(users[1]);

            _context.User.AddRange(users);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            (_context.User as List<User>)!.RemoveAll(user => user.Login != "admin");
            return RedirectToAction(nameof(List));
        }


        private bool UserExists(string login)
        {
            return _context.User.Any(e => e.Login == login);
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
