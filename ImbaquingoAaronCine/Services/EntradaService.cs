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
    internal class EntradaService
    {
        private string connectionString;
        private SqliteConnection connection;

        public EntradaService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "cine.db");
            connectionString = $"Data Source={dbPath};";
            connection = new SqliteConnection(connectionString);
            connection.Open();

            // Crear la tabla de entradas si no existe
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Entrada (
                    Id INTEGER PRIMARY KEY,
                    PeliculaId INTEGER NOT NULL,
                    CantidadAsientos INTEGER NOT NULL,
                    FechaFuncion TEXT NOT NULL,
                    TotalPagado REAL NOT NULL
                )"
            );
        }

        // C-reate
        public Entrada Create(int id, int peliculaId, int cantidadAsientos, DateTime fechaFuncion, double totalPagado)
        {
            var nuevaEntrada = new Entrada
            {
                Id = id,
                PeliculaId = peliculaId,
                CantidadAsientos = cantidadAsientos,
                FechaFuncion = fechaFuncion.ToString("dd/MM/yyyy HH:mm"),
                TotalPagado = totalPagado
            };

            var recordsAffected = connection.Execute(
                "INSERT INTO Entrada (Id, PeliculaId, CantidadAsientos, FechaFuncion, TotalPagado) " +
                "VALUES (@Id, @PeliculaId, @CantidadAsientos, @FechaFuncion, @TotalPagado)",
                nuevaEntrada);

            if (recordsAffected == 0)
                throw new Exception("ERROR: NO SE PUDO REGISTRAR LA ENTRADA.");

            return nuevaEntrada;
        }

        // R-ead by Id
        public Entrada ReadById(int id)
        {
            var data = connection.Query<Entrada>(
                "SELECT * FROM Entrada WHERE Id = @IdEntrada",
                new { IdEntrada = id })
                .ToList();

            if (data.Count == 0)
                throw new Exception("ERROR: NO SE ENCONTRÓ NINGUNA ENTRADA CON ESE ID.");

            return data[0];
        }

        // R-ead all
        public List<Entrada> ReadAll()
        {
            var data = connection.Query<Entrada>("SELECT * FROM Entrada").ToList();

            if (data.Count == 0)
                throw new Exception("NO HAY ENTRADAS REGISTRADAS EN LA BASE DE DATOS.");

            return data;
        }

        // R-ead por Pelicula (Especial para la vista Detalle)
        public List<Entrada> ReadByPeliculaId(int peliculaId)
        {
            var data = connection.Query<Entrada>(
                "SELECT * FROM Entrada WHERE PeliculaId = @IdPeli",
                new { IdPeli = peliculaId }).ToList();

            return data;
        }

        // U-pdate
        public void Update(int id, int peliculaId, int cantidadAsientos, DateTime fechaFuncion, double totalPagado)
        {
            var recordsAffected = connection.Execute(
                "UPDATE Entrada SET PeliculaId = @PeliculaId, CantidadAsientos = @CantidadAsientos, " +
                "FechaFuncion = @FechaFuncion, TotalPagado = @TotalPagado " +
                "WHERE Id = @Id",
                new { Id = id, PeliculaId = peliculaId, CantidadAsientos = cantidadAsientos, FechaFuncion = fechaFuncion.ToString("dd/MM/yyyy HH:mm"), TotalPagado = totalPagado });

            if (recordsAffected == 0)
                throw new Exception("ERROR: NO SE PUDO ACTUALIZAR. VERIFIQUE QUE EL ID EXISTA.");
        }

        // D-elete
        public void Delete(int id)
        {
            var recordsAffected = connection.Execute(
                "DELETE FROM Entrada WHERE Id = @IdEntrada",
                new { IdEntrada = id });

            if (recordsAffected == 0)
                throw new Exception($"ERROR: NO SE PUDO ELIMINAR. LA ENTRADA CON ID {id} NO EXISTE.");
        }
    }
}
