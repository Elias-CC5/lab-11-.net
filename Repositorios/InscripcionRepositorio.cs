using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventos.Web.Repositorios
{
    public class InscripcionRepositorio
    {
        private readonly EventosContext _context;
        public InscripcionRepositorio(EventosContext context) => _context = context;

        public async Task<List<Inscripcion>> ObtenerTodasAsync() =>
            await _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.Asistente)
                .AsNoTracking()
                .ToListAsync();

        public async Task<bool> EstaInscritoAsync(int eventoId, int asistenteId) =>
            await _context.Inscripciones
                .AnyAsync(i => i.EventoId == eventoId && i.AsistenteId == asistenteId && i.Estado == "Confirmada");

        public async Task<int> ContarConfirmadasAsync(int eventoId) =>
            await _context.Inscripciones
                .CountAsync(i => i.EventoId == eventoId && i.Estado == "Confirmada");

        public async Task CrearAsync(Inscripcion inscripcion)
        {
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task CancelarAsync(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion != null)
            {
                inscripcion.Estado = "Cancelada";
                await _context.SaveChangesAsync();
            }
        }
    }
}