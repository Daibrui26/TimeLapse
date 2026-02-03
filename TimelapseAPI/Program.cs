using TimelapseAPI.Repositories;
using TimelapseAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//
// 🔹 Controllers
//
builder.Services.AddControllers();

//
// 🔹 Swagger
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// 🔹 Repositories (ADO.NET)
//
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICapsulaRepository, CapsulaRepository>();
builder.Services.AddScoped<IAmistadRepository, AmistadRepository>();
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<IContenidoRepository, ContenidoRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<IUsuarioCapsulaRepository, UsuarioCapsulaRepository>();

//
// 🔹 Services
//
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICapsulaService, CapsulaService>();
builder.Services.AddScoped<IAmistadService, AmistadService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();
builder.Services.AddScoped<IContenidoService, ContenidoService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IUsuarioCapsulaService, UsuarioCapsulaService>();

var app = builder.Build();

//
// 🔹 Middleware
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//
// 🔹 Controllers routing
//
app.MapControllers();

app.Run();