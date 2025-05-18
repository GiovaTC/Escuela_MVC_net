using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EscuelaMVC.Models;
using EscuelaMVC.DAO;
using System.Reflection.Metadata.Ecma335;

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

        // POST : /Estudiante/Crear
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
