using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Entidades
{
    public class Pelicula
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Duracion { get; set; }
        public string Clasificacion { get; set; }
    }
}
