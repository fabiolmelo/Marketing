using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Marketing.Mvc.Controllers
{
    [AllowAnonymous]
    public class PrivacyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }        
    }
}