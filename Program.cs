using Eventos.Web.Data;
using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar DbContext
builder.Services.AddDbContext<EventosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositorios
builder.Services.AddScoped<CategoriaRepositorio>();
builder.Services.AddScoped<EventoRepositorio>();
builder.Services.AddScoped<AsistenteRepositorio>();
builder.Services.AddScoped<InscripcionRepositorio>();

var app = builder.Build();

// Sembrado de datos iniciales
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EventosContext>();

    if (!await context.Categorias.AnyAsync())
    {
        context.Categorias.AddRange(
            new Categoria { Nombre = "Tecnología", Activo = true },
            new Categoria { Nombre = "Negocios", Activo = true },
            new Categoria { Nombre = "Innovación", Activo = true }
        );
        await context.SaveChangesAsync();
    }

    if (!await context.Eventos.AnyAsync())
    {
        var catId = context.Categorias.First().Id;
        context.Eventos.AddRange(
            new Evento { Titulo = "Congreso .NET 2026", Fecha = DateTime.Now.AddDays(10), Lugar = "Auditorio Principal", Aforo = 100, Precio = 50.00m, CategoriaId = catId, Activo = true, Modalidad = "Presencial" },
            new Evento { Titulo = "Workshop EF Core", Fecha = DateTime.Now.AddDays(15), Lugar = "Lab 02", Aforo = 30, Precio = 0.00m, CategoriaId = catId, Activo = true, Modalidad = "Virtual" },
            new Evento { Titulo = "Seminario Cloud", Fecha = DateTime.Now.AddDays(20), Lugar = "Auditorio B", Aforo = 150, Precio = 25.00m, CategoriaId = catId, Activo = true, Modalidad = "Presencial" },
            new Evento { Titulo = "Hackathon AI", Fecha = DateTime.Now.AddDays(25), Lugar = "Campus Central", Aforo = 200, Precio = 10.00m, CategoriaId = catId, Activo = true, Modalidad = "Presencial" }
        );
        await context.SaveChangesAsync();
    }

    if (!await context.Asistentes.AnyAsync())
    {
        context.Asistentes.AddRange(
            new Asistente { Nombres = "Juan Pérez", Dni = "12345678", Correo = "juan@mail.com", Telefono = "987654321", Activo = true },
            new Asistente { Nombres = "María Gómez", Dni = "87654321", Correo = "maria@mail.com", Telefono = "912345678", Activo = true },
            new Asistente { Nombres = "Carlos Ruiz", Dni = "11223344", Correo = "carlos@mail.com", Activo = true },
            new Asistente { Nombres = "Ana Torres", Dni = "44332211", Correo = "ana@mail.com", Telefono = "998877665", Activo = true },
            new Asistente { Nombres = "Luis Soto", Dni = "55667788", Correo = "luis@mail.com", Activo = true }
        );
        await context.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Habilitar archivos estáticos para que cargue el CSS y JS correctamente
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();