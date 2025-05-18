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

        public Estudiante ObtenerPorId(int id)
        {
            Estudiante estudiante = null;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM estudiantes WHERE id = @id";
                using (var cmd =  new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using( var reader = cmd.ExecuteReader())
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
                using ( var cmd = new MySqlCommand(sql, conn))
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

    }
}
