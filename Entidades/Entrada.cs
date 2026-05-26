using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Entidades
{
    public class Entrada
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed] 
        public int PeliculaId { get; set; }

        public int CantidadAsientos { get; set; }
        public string FechaFuncion { get; set; } 
        public double TotalPagado { get; set; }
    }
}
