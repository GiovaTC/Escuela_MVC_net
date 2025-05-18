# Escuela_MVC_net

# 🧩 Estructura del proyecto (ASP.NET MVC)

Puedes crear un nuevo proyecto en Visual Studio 2022 como:

Archivo > Nuevo > Proyecto > Aplicación web ASP.NET (.NET Framework)
→ Elegir plantilla "MVC"
→ Nombre: EscuelaMVC

---

# 🛢 Script SQL (MySQL)

```sql
CREATE DATABASE escuela;
USE escuela;

CREATE TABLE cursos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100)
);

CREATE TABLE profesores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100),
    especialidad VARCHAR(100)
);

CREATE TABLE estudiantes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100),
    edad INT,
    id_curso INT,
    FOREIGN KEY (id_curso) REFERENCES cursos(id)
);

-- Crear la base de datos
CREATE DATABASE IF NOT EXISTS escuela;
USE escuela;

-- Crear tabla cursos
CREATE TABLE IF NOT EXISTS cursos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

-- Crear tabla profesores
CREATE TABLE IF NOT EXISTS profesores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    especialidad VARCHAR(100) NOT NULL
);

-- Crear tabla estudiantes
CREATE TABLE IF NOT EXISTS estudiantes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    edad INT NOT NULL,
    id_curso INT NOT NULL,
    FOREIGN KEY (id_curso) REFERENCES cursos(id)
);

-- Insertar datos en cursos
INSERT INTO cursos (nombre) VALUES
('Matemáticas'),
('Ciencias'),
('Lengua'),
('Historia');

-- Insertar datos en profesores
INSERT INTO profesores (nombre, especialidad) VALUES
('Ana Pérez', 'Matemáticas'),
('Luis Gómez', 'Ciencias'),
('Carla Ruiz', 'Lengua'),
('Pedro Martínez', 'Historia');

-- Insertar datos en estudiantes
INSERT INTO estudiantes (nombre, edad, id_curso) VALUES
('Juan López', 15, 1),
('María Torres', 14, 2),
('Carlos Ramírez', 16, 1),
('Lucía Sánchez', 13, 3),
('David Herrera', 15, 4);


🔗 Conexión a MySQL
Instala el paquete NuGet: MySql.Data

📄 appsettings.json
json

{
  "ConnectionStrings": {
    "MySqlConnection": "server=localhost;database=escuela;uid=root;pwd=tu_contraseña;"
  }
}

📁 Modelo: Estudiante.cs
Ubicación: Models/Estudiante.cs

csharp

namespace EscuelaMVC.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int IdCurso { get; set; }
    }
}

📁 DAO: EstudianteDAO.cs
Ubicación: Models/EstudianteDAO.cs

csharp

using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace EscuelaMVC.Models
{
    public class EstudianteDAO
    {
        private readonly string connectionString;

        public EstudianteDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MySqlConnection");
        }

        public List<Estudiante> ObtenerTodos()
        {
            var lista = new List<Estudiante>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM estudiantes";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Estudiante
                        {
                            Id = reader.GetInt32("id"),
                            Nombre = reader.GetString("nombre"),
                            Edad = reader.GetInt32("edad"),
                            IdCurso = reader.GetInt32("id_curso")
                        });
                    }
                }
            }
            return lista;
        }

        public Estudiante ObtenerPorId(int id)
        {
            Estudiante estudiante = null;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM estudiantes WHERE id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            estudiante = new Estudiante
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Edad = reader.GetInt32("edad"),
                                IdCurso = reader.GetInt32("id_curso")
                            };
                        }
                    }
                }
            }
            return estudiante;
        }

        public void Insertar(Estudiante estudiante)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO estudiantes (nombre, edad, id_curso) VALUES (@nombre, @edad, @id_curso)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", estudiante.Nombre);
                    cmd.Parameters.AddWithValue("@edad", estudiante.Edad);
                    cmd.Parameters.AddWithValue("@id_curso", estudiante.IdCurso);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Estudiante estudiante)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE estudiantes SET nombre = @nombre, edad = @edad, id_curso = @id_curso WHERE id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", estudiante.Nombre);
                    cmd.Parameters.AddWithValue("@edad", estudiante.Edad);
                    cmd.Parameters.AddWithValue("@id_curso", estudiante.IdCurso);
                    cmd.Parameters.AddWithValue("@id", estudiante.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM estudiantes WHERE id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


🧷 Inyectar EstudianteDAO en el controlador
csharp

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EscuelaMVC.Models;

namespace EscuelaMVC.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly EstudianteDAO dao;

        public EstudianteController(IConfiguration config)
        {
            dao = new EstudianteDAO(config);
        }

        // GET: /Estudiante/
        public IActionResult Index()
        {
            var lista = dao.ObtenerTodos();
            return View(lista);
        }

        // GET: /Estudiante/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Estudiante/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                dao.Insertar(estudiante);
                return RedirectToAction("Index");
            }
            return View(estudiante);
        }

        // GET: /Estudiante/Editar/5
        public IActionResult Editar(int id)
        {
            var estudiante = dao.ObtenerPorId(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        // POST: /Estudiante/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                dao.Actualizar(estudiante);
                return RedirectToAction("Index");
            }
            return View(estudiante);
        }

        // GET: /Estudiante/Eliminar/5
        public IActionResult Eliminar(int id)
        {
            var estudiante = dao.ObtenerPorId(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }

        // POST: /Estudiante/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarEliminar(int id)
        {
            dao.Eliminar(id);
            return RedirectToAction("Index");
        }

        // GET: /Estudiante/Detalle/5
        public IActionResult Detalle(int id)
        {
            var estudiante = dao.ObtenerPorId(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            return View(estudiante);
        }
    }
}


🧪 En Program.cs

var builder = WebApplication.CreateBuilder(args);

// Agrega acceso a appsettings.json
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.Run();

🖼 Vistas
Ubicación: Views/Estudiante/

✅ Index.cshtml
html

@model List<EscuelaMVC.Models.Estudiante>

@{
    ViewData["Title"] = "Listado de Estudiantes";
}

<h2>@ViewData["Title"]</h2>

<p>
    <a asp-action="Crear" class="btn btn-success">Nuevo Estudiante</a>
</p>

<table class="table table-bordered">
    <thead>
        <tr>
            <th>Nombre</th>
            <th>Edad</th>
            <th>ID Curso</th>
            <th>Acciones</th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model)
{
        <tr>
            <td>@item.Nombre</td>
            <td>@item.Edad</td>
            <td>@item.IdCurso</td>
            <td>
                <a asp-action="Detalle" asp-route-id="@item.Id" class="btn btn-info btn-sm">Detalle</a>
                <a asp-action="Editar" asp-route-id="@item.Id" class="btn btn-primary btn-sm">Editar</a>
                <a asp-action="Eliminar" asp-route-id="@item.Id" class="btn btn-danger btn-sm">Eliminar</a>
            </td>
        </tr>
}
    </tbody>
</table>

✅ Crear.cshtml
html

@model EscuelaMVC.Models.Estudiante

@{
    ViewData["Title"] = "Crear Estudiante";
}

<h2>@ViewData["Title"]</h2>

<form asp-action="Crear">
    <div class="form-group">
        <label asp-for="Nombre"></label>
        <input asp-for="Nombre" class="form-control" />
    </div>
    <div class="form-group">
        <label asp-for="Edad"></label>
        <input asp-for="Edad" class="form-control" />
    </div>
    <div class="form-group">
        <label asp-for="IdCurso"></label>
        <input asp-for="IdCurso" class="form-control" />
    </div>
    <button type="submit" class="btn btn-primary">Crear</button>
    <a asp-action="Index" class="btn btn-secondary">Volver</a>
</form>

✅ Editar.cshtml
html

@model EscuelaMVC.Models.Estudiante

@{
    ViewData["Title"] = "Editar Estudiante";
}

<h2>@ViewData["Title"]</h2>

<form asp-action="Editar">
    <input type="hidden" asp-for="Id" />
    <div class="form-group">
        <label asp-for="Nombre"></label>
        <input asp-for="Nombre" class="form-control" />
    </div>
    <div class="form-group">
        <label asp-for="Edad"></label>
        <input asp-for="Edad" class="form-control" />
    </div>
    <div class="form-group">
        <label asp-for="IdCurso"></label>
        <input asp-for="IdCurso" class="form-control" />
    </div>
    <button type="submit" class="btn btn-primary">Guardar</button>
    <a asp-action="Index" class="btn btn-secondary">Volver</a>
</form>

✅ Eliminar.cshtml
html

@model EscuelaMVC.Models.Estudiante

@{
    ViewData["Title"] = "Eliminar Estudiante";
}

<h2>@ViewData["Title"]</h2>

<h4>¿Estás seguro de eliminar este estudiante?</h4>
<div>
    <p><strong>Nombre:</strong> @Model.Nombre</p>
    <p><strong>Edad:</strong> @Model.Edad</p>
    <p><strong>ID Curso:</strong> @Model.IdCurso</p>
</div>

<form asp-action="ConfirmarEliminar">
    <input type="hidden" asp-for="Id" />
    <button type="submit" class="btn btn-danger">Eliminar</button>
    <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
</form>

✅ Detalle.cshtml
html

@model EscuelaMVC.Models.Estudiante

@{
    ViewData["Title"] = "Detalle del Estudiante";
}

<h2>@ViewData["Title"]</h2>

<div>
    <p><strong>Nombre:</strong> @Model.Nombre</p>
    <p><strong>Edad:</strong> @Model.Edad</p>
    <p><strong>ID Curso:</strong> @Model.IdCurso</p>
</div>

<a asp-action="Index" class="btn btn-secondary">Volver</a>

🎁 Bonus
Puedes agregar Bootstrap vía CDN en tu _Layout.cshtml para un diseño más limpio:

html

<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/
