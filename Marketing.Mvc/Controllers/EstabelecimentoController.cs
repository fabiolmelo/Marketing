using System.Linq.Expressions;
using Marketing.Application.DTOs;
using Marketing.Application.Mappers;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketing.Mvc.Controllers
{
    public class EstabelecimentoController : Controller
    {
        private readonly IServicoEstabelecimento _servicoEstabelecimento;
        private readonly IServicoRede _servicoRede;

        public EstabelecimentoController(IServicoEstabelecimento servicoEstabelecimento, IServicoRede servicoRede)
        {
            _servicoEstabelecimento = servicoEstabelecimento;
            _servicoRede = servicoRede;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? pageNumber = 1, int? pageSize = 5, string? filtro = null)
        {
            ViewData["FiltroAtual"] = filtro?.ToUpper();
            var estabelecimentos = await _servicoEstabelecimento.GetAllEstabelecimentos(pageNumber,
                                    pageSize, filtro);
            return View(estabelecimentos);
        }

        public async Task<ActionResult> IndexP(int pageNumber = 1, int pageSize = 5, string? filtro = null)
        {
            return RedirectToAction("Index", new {pageNumber = pageNumber, pageSize = pageSize, filtro = filtro});
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
        public async Task<ActionResult> Edit(string id)
        {
            var estabelecimento = await _servicoEstabelecimento.GetByIdStringAsync(id);
            var redes = await _servicoRede.GetAllRedesAsync(1, 999999);
            ViewData["Redes"] = redes;
            if (estabelecimento == null) return BadRequest();
            return View(estabelecimento.MapToDto());
        }

        // POST: EstabelecimentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EstabelecimentoDto estabelecimentoDto)
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
