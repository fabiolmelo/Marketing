using System.Linq.Expressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class ContatoController : Controller
    {
        private readonly IServicoContato _servicoContato;

        public ContatoController(IServicoContato servicoContato)
        {
            _servicoContato = servicoContato;
        }

        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 5, string? filtro = null)
        {
            Expression<Func<Contato, bool>>? filtroContato = null;
            if (filtro != null) 
            {
                filtroContato = x => (x.Nome != null && x.Nome.Contains(filtro))
                                                              || (x.Email != null && x.Email.Contains(filtro))  
                                                              || (x.Telefone != null && x.Telefone.Contains(filtro));
            }
            var contatos = await _servicoContato.GetAllAsync(pageNumber, pageSize, filtroContato);
            return View(contatos);
        }

        // GET: ContatoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContatoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContatoController/Create
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

        // GET: ContatoController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var contato = await _servicoContato.GetByIdStringAsync(id);
            if (contato == null) return NotFound();
            return View(contato);
        }

        // POST: ContatoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id,
                    [Bind("Telefone,Nome,AceitaMensagem,DataAceite,RecusaMensagem,DataRecusa,Email,Token,UltimaCompetenciaEnviada")] Contato contato)
        {
            try
            {
                _servicoContato.Update(contato);
                await _servicoContato.CommitAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ContatoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContatoController/Delete/5
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
