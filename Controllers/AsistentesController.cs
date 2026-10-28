

using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eventos.Web.Controllers
{
    public class AsistentesController : Controller
    {
        private readonly AsistenteRepositorio _asistenteRepo;

        public AsistentesController(AsistenteRepositorio asistenteRepo)
        {
            _asistenteRepo = asistenteRepo;
        }

        // GET: Asistentes
        public async Task<IActionResult> Index()
        {
            var asistentes = await _asistenteRepo.ObtenerActivosAsync();
            return View(asistentes);
        }

        // GET: Asistentes/Create
        public IActionResult Create() => View();

        // POST: Asistentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Asistente asistente)
        {
            if (await _asistenteRepo.ExisteDniAsync(asistente.Dni))
            {
                ModelState.AddModelError("Dni", "Ya existe un asistente registrado con este DNI.");
            }

            if (await _asistenteRepo.ExisteCorreoAsync(asistente.Correo))
            {
                ModelState.AddModelError("Correo", "Ya existe un asistente registrado con este correo.");
            }

            if (ModelState.IsValid)
            {
                await _asistenteRepo.CrearAsync(asistente);
                TempData["Mensaje"] = "Asistente registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(asistente);
        }
    }
}