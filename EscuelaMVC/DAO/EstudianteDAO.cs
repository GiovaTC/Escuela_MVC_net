using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using EscuelaMVC.Models;

namespace EscuelaMVC.DAO
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
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("nombre"),
                            Edad = reader.GetInt32("edad"),
                            IdCurso = reader.GetInt32("id_curso")
                        });
                    }
                }

            }
            return lista;
        }

        // Métodos Insertar, Actualizar, Eliminar también usan connectionString...
    }
}
