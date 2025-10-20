using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Infraestrutura.Repositorio.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class ConfiguracoesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfiguracoesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index(string? erro = null, string? sucesso = null)
        {
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            var config = await _unitOfWork.GetRepository<ConfiguracaoApp>().GetByIdAsync(1);
            if (config == null) throw new Exception("Configurações indisponíveis!");
            return View(config);
        }

        // POST: ContatoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ConfiguracaoApp configuracaoApp)
        {
            try
            {
                _unitOfWork.GetRepository<ConfiguracaoApp>().Update(configuracaoApp);
                await _unitOfWork.CommitAsync();
                return RedirectToAction("Index",
                    new { sucesso = "Configurações atualizadas com sucesso!" });
            }
            catch
            {
                return RedirectToAction("Index",
                    new { erro = "Erro atualizando cnfigurações!" });
            }
        }
    }
}