using Marketing.Application.DTOs;
using Marketing.Application.Mappers;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marketing.Mvc.Controllers
{
    public class EstabelecimentoController : Controller
    {
        private readonly IServicoEstabelecimento _servicoEstabelecimento;
        private readonly IServicoRede _servicoRede;
        private readonly IServicoReceitaFederal _servicoReceitaFederal;

        public EstabelecimentoController(IServicoEstabelecimento servicoEstabelecimento, IServicoRede servicoRede, IServicoReceitaFederal servicoReceitaFederal)
        {
            _servicoEstabelecimento = servicoEstabelecimento;
            _servicoRede = servicoRede;
            _servicoReceitaFederal = servicoReceitaFederal;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? pageNumber = 1, int? pageSize = 5, string? filtro = null)
        {
            ViewData["FiltroAtual"] = filtro?.ToUpper();
            var estabelecimentos = await _servicoEstabelecimento.GetAllEstabelecimentos(pageNumber,
                                    pageSize, filtro);
            return View(estabelecimentos);
        }

        public ActionResult IndexP(int pageNumber = 1, int pageSize = 5, string? filtro = null)
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
        public async Task<ActionResult> Edit(string id, string? erro = null, string? sucesso = null)
        {
             ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            var estabelecimento = await _servicoEstabelecimento.GetByIdStringAsync(id);
            var redes = await _servicoRede.GetAllRedesAsync(1, 999999);
            var selectList = new SelectList(redes.Dados, "Nome", "Nome", estabelecimento?.RedeNome);
            ViewBag.Redes = selectList;
            if (estabelecimento == null) return BadRequest();
            var estabelecimentoDto = estabelecimento.MapToDto();
            return View(estabelecimentoDto);
        }


        public async Task<ActionResult> AtualizarDadosCadastraisViaReceitaFederal(string id)
        {
            if (await _servicoEstabelecimento.AtualizarDadosCadastraisViaReceitaFederal(id, true))
            {
                return RedirectToAction("Edit",
                    new { sucesso = "Dados atualizados via Receita Federal.", id = id });
            }
            else
            {
                return RedirectToAction("Edit",
                    new { erro = "Erro. Processamento não efetuado!", id = id });
            }
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
