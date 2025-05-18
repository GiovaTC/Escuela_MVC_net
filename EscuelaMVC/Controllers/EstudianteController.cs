using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EscuelaMVC.Models;
using EscuelaMVC.DAO;

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

/*        // POST : /Estudiante/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                dao.Insertar(estudiante);
                return RedirectToAction("Index");
            }
        }*/
    }
}
