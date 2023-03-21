using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorteo_de_Animalitos.Logica
{
    public class LVender
    {
        public int Venta { get; set; }

        public int idSorteo { get; set; }
        public string Sucursal { get; set; }
        public string Cliente { get; set; }
        public string nombreCliente { get; set; }

        public string Telefono { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Fecha_venc { get; set; }
        public int No_Referen { get; set; }
        public double Importe { get; set; }
        public double Impuesto { get; set; }
        public string Estado { get; set; }        
        public string Usuario { get; set; }
        public DateTime UsuFecha { get; set; }
        public DateTime UsuHora { get; set; }
        public string Estacion { get; set; }
        public string Producto { get; set; }
        public string Descrip { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public double ImpuestoU { get; set; }
        public double DescuentoU { get; set; }

    }
}
