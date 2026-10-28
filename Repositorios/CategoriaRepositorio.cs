using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventos.Web.Repositorios
{
    public class CategoriaRepositorio
    {
        private readonly EventosContext _context;
        public CategoriaRepositorio(EventosContext context) => _context = context;

        public async Task<List<Categoria>> ObtenerActivasAsync() =>
            await _context.Categorias.Where(c => c.Activo).AsNoTracking().ToListAsync();
    }
}