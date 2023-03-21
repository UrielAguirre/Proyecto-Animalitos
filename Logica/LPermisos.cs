using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorteo_de_Animalitos.Logica
{
    public class LPermisos
    {
        public int idGrupoPermisos{get; set;}

        public string idSucursal { get; set; }

        public string Modulo { get; set; }
        public string Control { get; set; }

        public int Acceso { get; set; }
        public string Usuario { get; set; }

        public DateTime Usufecha { get; set; }

        public DateTime Usuhora { get; set; }

        public string Estacion { get; set; }

        public string Sucursal { get; set; }

    }
}
