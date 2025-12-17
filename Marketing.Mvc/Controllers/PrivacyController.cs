using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class PrivacyController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }
        
    }
}