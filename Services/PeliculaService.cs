using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Dapper;
using Entidades;

namespace ImbaquingoAaronCine.Services
{
    internal class PeliculaService
    {
        private string connectionString;
        private SqliteConnection connection;

        public PeliculaService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "cine.db");
            connectionString = $"Data Source={dbPath};";
            connection = new SqliteConnection(connectionString);
            connection.Open();

            // Crear la tabla de películas si no existe
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Pelicula (
                    Id INTEGER PRIMARY KEY,
                    Titulo TEXT NOT NULL,
                    Duracion INTEGER NOT NULL,
                    Clasificacion TEXT NOT NULL
                )"
            );
        }

        // C-reate
        public Pelicula Create(int id, string titulo, int duracion, string clasificacion)
        {
            var nuevaPelicula = new Pelicula
            {
                Id = id,
                Titulo = titulo,
                Duracion = duracion,
                Clasificacion = clasificacion
            };

            var recordsAffected = connection.Execute(
                "INSERT INTO Pelicula (Id, Titulo, Duracion, Clasificacion) " +
                "VALUES (@Id, @Titulo, @Duracion, @Clasificacion)",
                nuevaPelicula);

            if (recordsAffected == 0)
                throw new Exception("ERROR: NO SE PUDO CREAR LA PELÍCULA.");

            return nuevaPelicula;
        }

        // R-ead by Id
        public Pelicula ReadById(int id)
        {
            var data = connection.Query<Pelicula>(
                "SELECT * FROM Pelicula WHERE Id = @IdPelicula",
                new { IdPelicula = id })
                .ToList();

            if (data.Count == 0)
                throw new Exception("ERROR: NO SE ENCONTRÓ NINGUNA PELÍCULA CON ESE ID.");

            return data[0];
        }

        // R-ead all
        public List<Pelicula> ReadAll()
        {
            var data = connection.Query<Pelicula>("SELECT * FROM Pelicula").ToList();

            if (data.Count == 0)
                throw new Exception("NO HAY PELÍCULAS REGISTRADAS EN LA BASE DE DATOS.");

            return data;
        }

        // U-pdate
        public void Update(int id, string titulo, int duracion, string clasificacion)
        {
            var recordsAffected = connection.Execute(
                "UPDATE Pelicula SET Titulo = @Titulo, Duracion = @Duracion, Clasificacion = @Clasificacion " +
                "WHERE Id = @Id",
                new { Id = id, Titulo = titulo, Duracion = duracion, Clasificacion = clasificacion });

            if (recordsAffected == 0)
                throw new Exception("ERROR: NO SE PUDO ACTUALIZAR. VERIFIQUE QUE EL ID EXISTA.");
        }

        // D-elete
        public void Delete(int id)
        {
            var recordsAffected = connection.Execute(
                "DELETE FROM Pelicula WHERE Id = @IdPelicula",
                new { IdPelicula = id });

            if (recordsAffected == 0)
                throw new Exception($"ERROR: NO SE PUDO ELIMINAR. LA PELÍCULA CON ID {id} NO EXISTE.");
        }
    }
}
