using Sorteo_de_Animalitos.Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorteo_de_Animalitos.Datos
{
    public class DConfigGral
    {

        public bool GuardarConfigGral(LConfigGral parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("GuardaConfigGeneral", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombreEmpresa", parametros.nombreEmpresa);
                cmd.Parameters.AddWithValue("@Logo", parametros.Logo);
                cmd.Parameters.AddWithValue("@ValidaPermisos", parametros.validaPermisos);               
                cmd.ExecuteNonQuery();
                //cmd.ResetCommandTimeout();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + " " + ex.Message);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }

        }

       
    }
}
