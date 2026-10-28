using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventos.Web.Repositorios
{
    public class AsistenteRepositorio
    {
        private readonly EventosContext _context;

        public AsistenteRepositorio(EventosContext context)
        {
            _context = context;
        }

        public async Task<List<Asistente>> ObtenerActivosAsync() =>
            await _context.Asistentes.Where(a => a.Activo).OrderBy(a => a.Nombres).AsNoTracking().ToListAsync();

        public async Task<bool> ExisteDniAsync(string dni) =>
            await _context.Asistentes.AnyAsync(a => a.Dni == dni);

        public async Task<bool> ExisteCorreoAsync(string correo) =>
            await _context.Asistentes.AnyAsync(a => a.Correo == correo);

        public async Task CrearAsync(Asistente asistente)
        {
            _context.Asistentes.Add(asistente);
            await _context.SaveChangesAsync();
        }
    }
}