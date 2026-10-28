using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Eventos.Web.Controllers
{
    public class InscripcionesController : Controller
    {
        private readonly InscripcionRepositorio _inscripcionRepo;
        private readonly EventoRepositorio _eventoRepo;
        private readonly AsistenteRepositorio _asistenteRepo;

        public InscripcionesController(InscripcionRepositorio inscripcionRepo, EventoRepositorio eventoRepo, AsistenteRepositorio asistenteRepo)
        {
            _inscripcionRepo = inscripcionRepo;
            _eventoRepo = eventoRepo;
            _asistenteRepo = asistenteRepo;
        }

        // GET: Inscripciones
        public async Task<IActionResult> Index()
        {
            var inscripciones = await _inscripcionRepo.ObtenerTodasAsync();
            return View(inscripciones);
        }

        // GET: Inscripciones/Create
        public async Task<IActionResult> Create()
        {
            var eventos = await _eventoRepo.ObtenerTodosAsync(null);
            var asistentes = await _asistenteRepo.ObtenerActivosAsync();

            ViewBag.Eventos = new SelectList(eventos, "Id", "Titulo");
            ViewBag.Asistentes = new SelectList(asistentes, "Id", "Nombres");
            return View();
        }

        // POST: Inscripciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inscripcion inscripcion)
        {
            var evento = await _eventoRepo.ObtenerPorIdAsync(inscripcion.EventoId);

            if (evento == null)
            {
                ModelState.AddModelError("", "El evento seleccionado no existe.");
            }
            else
            {
                // Validar si el evento ya pasó
                if (evento.Fecha < DateTime.Now.Date)
                {
                    ModelState.AddModelError("", "No se puede inscribir a un evento que ya pasó.");
                }

                // Validar aforo completo
                int confirmadas = await _inscripcionRepo.ContarConfirmadasAsync(inscripcion.EventoId);
                if (confirmadas >= evento.Aforo)
                {
                    ModelState.AddModelError("", "El aforo para este evento está completo.");
                }

                // Validar si ya está inscrito
                if (await _inscripcionRepo.EstaInscritoAsync(inscripcion.EventoId, inscripcion.AsistenteId))
                {
                    ModelState.AddModelError("", "El asistente ya se encuentra inscrito y confirmado en este evento.");
                }
            }

            if (ModelState.IsValid)
            {
                inscripcion.Estado = "Confirmada";
                inscripcion.FechaInscripcion = DateTime.Now;
                await _inscripcionRepo.CrearAsync(inscripcion);
                TempData["Mensaje"] = "Inscripción realizada con éxito.";
                return RedirectToAction(nameof(Index));
            }

            var eventos = await _eventoRepo.ObtenerTodosAsync(null);
            var asistentes = await _asistenteRepo.ObtenerActivosAsync();
            ViewBag.Eventos = new SelectList(eventos, "Id", "Titulo", inscripcion.EventoId);
            ViewBag.Asistentes = new SelectList(asistentes, "Id", "Nombres", inscripcion.AsistenteId);
            return View(inscripcion);
        }

        // POST: Inscripciones/Cancelar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            await _inscripcionRepo.CancelarAsync(id);
            TempData["Mensaje"] = "Inscripción cancelada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}