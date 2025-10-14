using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class EstabelecimentoController : Controller
    {
        private readonly IServicoEstabelecimento _servicoEstabelecimento;

        public EstabelecimentoController(IServicoEstabelecimento servicoEstabelecimento)
        {
            _servicoEstabelecimento = servicoEstabelecimento;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 7, string? filtro = null)
        {
            if (filtro == "") filtro = null;

            var estabelecimentos = await _servicoEstabelecimento.GetAllEstabelecimentos(pageNumber,
                                    pageSize, filtro);
            return View(estabelecimentos);
        }

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

        // GET: EstabelecimentoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EstabelecimentoController/Edit/5
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

        // GET: EstabelecimentoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EstabelecimentoController/Delete/5
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
