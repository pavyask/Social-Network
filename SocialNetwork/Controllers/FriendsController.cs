using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NuGet.Protocol.Plugins;
using SocialNetwork.Data;
using SocialNetwork.Models;
using System.Linq;
using System.Text;

namespace SocialNetwork.Controllers
{
    public class FriendsController : Controller
    {
        private readonly SocialNetworkService _service;
        public FriendsController(SocialNetworkService service)
        {
            //// constructor called every time when new view is returned?
            _service = service;
        }

        // GET: Friends
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUser();
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: Friends/List
        public async Task<IActionResult> List()
        {
            var user = await GetCurrentUser();
            if (user == null)
                return NotFound();

            else return Json(user.Friends);
        }

        // POST: Friends/Add/{login}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string login)
        {
            var currentUser = await GetCurrentUser();
            if (currentUser == null)
                return NotFound();

            var userToAdd = await _service.GetUserByLoginOrNull(login);
            if (userToAdd == null || userToAdd.Login == currentUser.Login)
                return Json(false);
            else
            {
                if (currentUser.IsUserFriend(userToAdd))
                    currentUser.AddFriend(userToAdd);

                return Json(true);
            }
        }

        // POST: Friends/Delete/{login}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string login)
        {
            var currentUser = await GetCurrentUser();
            if (currentUser == null)
                return NotFound();

            var friendToDelete = currentUser.GetFriendByLogin(login);
            if (friendToDelete == null)
                return Json(false);
            else
            {
                currentUser.RemoveFriend(friendToDelete);
                return Json(true);
            }
        }

        // POST: Friends/Export
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Export()
        {
            var currentUser = await GetCurrentUser();
            if (currentUser == null)
                return NotFound();

            var friendLogins = currentUser.GetFriendLogins();
            byte[] bytes;

            using (MemoryStream memoryStream = new MemoryStream())
            using (TextWriter tw = new StreamWriter(memoryStream))
            {
                foreach (var login in friendLogins)
                    tw.WriteLine(login);
                tw.Flush();
                bytes = memoryStream.GetBuffer();
            }

            return File(bytes, "text/plain", "friends.txt");
        }

        //POST: Friends/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var currentUser = await GetCurrentUser();
            if (currentUser == null)
                return NotFound();

            if (file != null)
            {
                using (TextReader tr = new StreamReader(file.OpenReadStream()))
                {
                    var friendLogins = tr.ReadToEnd().Split(Environment.NewLine);
                    currentUser.Friends.Clear();

                    foreach (var login in friendLogins)
                    {
                        var userToImport = await _service.GetUserByLoginOrNull(login);
                        if (userToImport != null)
                            currentUser.AddFriend(userToImport);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<User?> GetCurrentUser()
        {
            if (Request.Cookies.TryGetValue("login", out string? login))
                return await _service.GetUserByLoginOrNull(login);

            else return null;
        }
    }
}
