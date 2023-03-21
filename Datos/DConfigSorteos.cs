using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sorteo_de_Animalitos.Logica;

namespace Sorteo_de_Animalitos.Datos
{
    public class DConfigSorteos
    {
        public bool GuardaConfigSorteos( LConfig parametros)        {

            try
            {                               
                ConexionBD.Abrir();               

                SqlCommand cmd = new SqlCommand("GuardaConfigSorteos", ConexionBD.conectar);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //cmd.Parameters.AddWithValue("@Minutos", parametros.Minutos);
                cmd.Parameters.AddWithValue("@Hora", parametros.Hora);
                cmd.Parameters.AddWithValue("@Lunes", parametros.Lunes);
                cmd.Parameters.AddWithValue("@Martes", parametros.Martes);
                cmd.Parameters.AddWithValue("@Miercoles", parametros.Miercoles);
                cmd.Parameters.AddWithValue("@Jueves", parametros.Jueves);
                cmd.Parameters.AddWithValue("@Viernes", parametros.Viernes);
                cmd.Parameters.AddWithValue("@Sabado", parametros.Sabado);
                cmd.Parameters.AddWithValue("@Domingo", parametros.Domingo);
                cmd.Parameters.AddWithValue("@Activo", parametros.Activo);
                cmd.Parameters.AddWithValue ("@PermisoSEGOB", parametros.PermisoSEGOB);
                cmd.Parameters.AddWithValue("@ImporteMax", parametros.ImporteAnimalitos);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }


        }

        public void MostrarConfigSorteos(ref DataTable dt)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("mostrarConfigSorteos", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;               
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }
    }
}
