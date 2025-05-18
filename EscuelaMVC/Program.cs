var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor (MVC)
builder.Services.AddControllersWithViews();

// Construcción de la app
var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Ruta por defecto: controlador Estudiante, acción Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Estudiante}/{action=Index}/{id?}");

app.Run();

