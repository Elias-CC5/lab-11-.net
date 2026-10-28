using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Eventos.Web.Controllers
{
    public class EventosController : Controller
    {
        private readonly EventoRepositorio _eventoRepo;
        private readonly CategoriaRepositorio _categoriaRepo;

        public EventosController(EventoRepositorio eventoRepo, CategoriaRepositorio categoriaRepo)
        {
            _eventoRepo = eventoRepo;
            _categoriaRepo = categoriaRepo;
        }

        // GET: Eventos
        public async Task<IActionResult> Index(int? categoriaId)
        {
            var categorias = await _categoriaRepo.ObtenerActivasAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", categoriaId);
            ViewBag.CategoriaSeleccionada = categoriaId;

            var eventos = await _eventoRepo.ObtenerTodosAsync(categoriaId);
            return View(eventos);
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var evento = await _eventoRepo.ObtenerPorIdAsync(id);
            if (evento == null) return NotFound();
            return View(evento);
        }

        // GET: Eventos/Create
        public async Task<IActionResult> Create()
        {
            var categorias = await _categoriaRepo.ObtenerActivasAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");
            return View();
        }

        // POST: Eventos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                await _eventoRepo.CrearAsync(evento);
                TempData["Mensaje"] = "Evento creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            var categorias = await _categoriaRepo.ObtenerActivasAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", evento.CategoriaId);
            return View(evento);
        }

        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var evento = await _eventoRepo.ObtenerPorIdAsync(id);
            if (evento == null) return NotFound();

            var categorias = await _categoriaRepo.ObtenerActivasAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", evento.CategoriaId);

            return View(evento);
        }

        // POST: Eventos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (id != evento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _eventoRepo.ActualizarAsync(evento);
                TempData["Mensaje"] = "Evento actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            var categorias = await _categoriaRepo.ObtenerActivasAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", evento.CategoriaId);

            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var evento = await _eventoRepo.ObtenerPorIdAsync(id);
            if (evento == null) return NotFound();
            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _eventoRepo.EliminarLogicoAsync(id);
            TempData["Mensaje"] = "Evento eliminado lógicamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}