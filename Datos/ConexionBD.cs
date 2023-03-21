using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorteo_de_Animalitos
{
    public class ConexionBD
    {

        //public static string conexion = @"Data Source=.\SQLEXPRESS; Initial Catalog=SorteoAnimalitos20; Integrated Security=true;";
        public static string conexion = @"Data Source=.\MYBUSINESSPOS; Initial Catalog=Animalitos; Integrated Security=true;";

        public static SqlConnection conectar = new SqlConnection(conexion);

        public static void Abrir()
        {
            if (conectar.State == System.Data.ConnectionState.Closed)
            {
                conectar.Open();
            }
        }

        public static void Cerrar()
        {
            if (conectar.State == System.Data.ConnectionState.Open)
            {
                conectar.Close();
            }
        }
       
    }
}
