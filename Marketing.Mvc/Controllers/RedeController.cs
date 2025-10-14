using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class RedeController : Controller
    {
        private readonly IServicoRede _servicoRede;

        public RedeController(IServicoRede servicoRede)
        {
            _servicoRede = servicoRede;
        }

        public async Task<ActionResult> Index(int? pageNumber, int? pageSize)
        {
            var redes = await _servicoRede.GetAllRedesAsync(pageNumber ?? 1, pageSize ?? 8); 
            return View(redes);
        }

        // GET: RedeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RedeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RedeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RedeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RedeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RedeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RedeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
