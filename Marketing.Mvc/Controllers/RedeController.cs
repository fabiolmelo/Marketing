using System.Linq.Expressions;
using Marketing.Application.DTOs;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketing.Mvc.Controllers
{
    public class RedeController : Controller
    {
        private readonly IServicoRede _servicoRede;

        public RedeController(IServicoRede servicoRede)
        {
            _servicoRede = servicoRede;
        }

        public async Task<ActionResult> Index(int? pageNumber, int? pageSize, string? filtro)
        {
            ViewData["FiltroAtual"] = filtro?.ToUpper();
            Expression<Func<Rede, bool>>? filtros = null;
            if (filtro != null && !filtro.IsNullOrEmpty())
            {
                filtros = x => x.Nome.Contains(filtro.ToUpper());
            }
            var redes = await _servicoRede.GetAllRedesAsync(pageNumber ?? 1, pageSize ?? 5, filtros);
            return View(redes);
        }
        
        [HttpPost]
        public async Task<ActionResult> IndexP(int? pageNumber, int? pageSize, string? filtro)
        {
            return RedirectToAction("Index", new {pageNumber = pageNumber, pageSize = pageSize, filtro = filtro});
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

        public async Task<ActionResult> UploadLogo(string id, string? erro = null, string? sucesso = null)
        {
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            var rede = await _servicoRede.GetByIdStringAsync(id);
            if (rede == null) return NotFound();
            var redeDto = new RedeDto()
            {
                Nome = rede.Nome,
                DataCadastro = rede.DataCadastro,
                Logo = rede.Logo 
            };
            return View(redeDto);
        }

        // POST: RedeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadLogo([FromForm] RedeDto redeDto)
        {
            try
            {
                if (redeDto.ArquivoLogo == null || redeDto.ArquivoLogo.Length == 0)
                {
                    return RedirectToAction("UploadLogo", new { id = redeDto.Nome, erro = "Nenhum arquivo foi selecionado!" });
                }
                var extensao = Path.GetExtension(redeDto.ArquivoLogo.FileName);
                if (extensao.ToLower() != ".png") return RedirectToAction("UploadLogo", new { id = redeDto.Nome, erro = "O arquivo selecionado não é uma imagem PNG!" });
                var rede = new Rede(redeDto.Nome);
                rede.DataCadastro = redeDto.DataCadastro;
                var filePath = Path.Combine("DadosApp", "Logos", redeDto.ArquivoLogo.FileName);
                try
                {
                    using (FileStream filestream = System.IO.File.Create(filePath))
                    {
                        await redeDto.ArquivoLogo.CopyToAsync(filestream);
                        filestream.Flush();
                    }
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    string base64String = Convert.ToBase64String(imageBytes);
                    rede.Logo = base64String;
                    _servicoRede.Update(rede);
                    await _servicoRede.CommitAsync();
                }
                catch (System.Exception)
                {
                    return RedirectToAction("UploadLogo", new { id = redeDto.Nome, erro = "Erro ao processar imagem!" });
                }
                return RedirectToAction("UploadLogo", new { id = redeDto.Nome, sucesso = "Rede atualizada com sucesso!" });
            }
            catch
            {
                return RedirectToAction("Index", new { id = redeDto.Nome, erro = "Erro ao atualizar Rede" });
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
