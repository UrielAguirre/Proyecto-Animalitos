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
    public class DVentas
    {

        public void MuestraVentas(ref DataTable dt, DateTime fechaIni, DateTime fechaFin, int noVenta)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("mostrarVentas", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@FechaIni", fechaIni);
                da.SelectCommand.Parameters.AddWithValue("@FechaFin", fechaFin);
                da.SelectCommand.Parameters.AddWithValue("@noVenta", noVenta);
                da.Fill(dt);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                ConexionBD.Cerrar();
            }

        }

        public void MuestraPartidas(ref DataTable dt, int nVenta)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("mostrarPartidas", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@nVenta", nVenta);
                
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.HelpLink + " " + ex.Source);
                //MessageBox.Show("" + ex.InnerException);
                //MessageBox.Show(ex.Message);
                //MessageBox.Show(ex.StackTrace);
                //MessageBox.Show(ex.Source);
               
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }

        public bool CancelaVentas(LVentas parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("CancelaVenta", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nVenta", parametros.nVenta);                
                cmd.Parameters.AddWithValue("@Fecha", parametros.UsuFecha);
                cmd.Parameters.AddWithValue("@Usuario", parametros.Usuario);
                cmd.Parameters.AddWithValue("@Usufecha", parametros.UsuFecha);
                cmd.Parameters.AddWithValue("@Usuhora", parametros.UsuHora);
                cmd.Parameters.AddWithValue("@Estacion", parametros.Estacion);
                cmd.Parameters.AddWithValue("@Sucursal", parametros.Sucursal);
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


        public bool ValidaCancelaVentas(int nVenta)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select * From SIEVentas Where estado = 'CA' And venta = " + nVenta, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    MessageBox.Show("La venta ya fue cancelada", "Cancelación de venta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                } 
                              
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

    }
}
