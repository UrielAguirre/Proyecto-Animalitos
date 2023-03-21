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
    public class DGanadores    
    {
        public bool GuardarGanadores(LGanadores parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("GanadoresSorteo", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue ("@idSorteo", parametros.IdSorteo);
                cmd.Parameters.AddWithValue("@Animalito", parametros.Animalito);
                cmd.Parameters.AddWithValue("@nombreAnimalito", parametros.NombreAnimalito);
                cmd.Parameters.AddWithValue("@Usuario", parametros.Usuario);
                cmd.Parameters.AddWithValue("@UsuFecha", parametros.UsuFecha);
                cmd.Parameters.AddWithValue("@Usuhora", parametros.UsuHora);
                cmd.Parameters.AddWithValue("@Estacion", parametros.Estacion);
                cmd.Parameters.AddWithValue("@Sucursal", parametros.Sucursal);
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

        public void MostrarGanadores(ref DataTable dt, int idSorteo)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("MuestraGanadores", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@idSorteo", idSorteo);
                
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


        public bool ConfirmaGanadores(LGanadores parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("ConfirmaGanadores", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idSorteo", parametros.IdSorteo);               
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

