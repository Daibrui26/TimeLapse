using TimelapseAPI.Repositories;
using TimelapseAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Controllers
builder.Services.AddControllers();

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 CORS - DEBE IR AQUÍ, ANTES DE Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy => policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// 🔹 Repositories (ADO.NET)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICapsulaRepository, CapsulaRepository>();
builder.Services.AddScoped<IAmistadRepository, AmistadRepository>();
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<IContenidoRepository, ContenidoRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();
builder.Services.AddScoped<IUsuarioCapsulaRepository, UsuarioCapsulaRepository>();

// 🔹 Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICapsulaService, CapsulaService>();
builder.Services.AddScoped<IAmistadService, AmistadService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();
builder.Services.AddScoped<IContenidoService, ContenidoService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IUsuarioCapsulaService, UsuarioCapsulaService>();

// AQUÍ SE CONSTRUYE LA APP
var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ CORS - DEBE IR ANTES DE HTTPS Y AUTORIZACIÓN
app.UseCors("AllowVueApp");

app.UseHttpsRedirection();
app.UseAuthorization();

// 🔹 Controllers routing
app.MapControllers();

app.Run();