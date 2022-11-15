using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        public IActionResult Change(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}