using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorteo_de_Animalitos.Logica
{
    public class LGanadores
    {
        public int IdSorteo { get; set; }
        public string Animalito { get; set; }
        public string NombreAnimalito { get; set; }
        public string Usuario { get; set; }
        public DateTime UsuFecha { get; set; }
        public DateTime UsuHora { get; set; }
        public string Estacion { get; set; }
        public string Sucursal { get; set; }
    }
}
