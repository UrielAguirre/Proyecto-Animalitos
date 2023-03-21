using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sorteo_de_Animalitos.Logica;

namespace Sorteo_de_Animalitos.Datos
{
    public class DSucursales
    {
        public bool GuardarSucursal(LSucursales parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("GuardaSucursal", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sucursal", parametros.Sucursal);
                cmd.Parameters.AddWithValue("@Nombre", parametros.nombre);
                cmd.Parameters.AddWithValue("@SerieTicket", parametros.serieTicket);
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

        public void MostrarSucursales(ref DataTable dt)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("mostrarSucursales", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //da.SelectCommand.Parameters.AddWithValue("@Desde", desde);
                //da.SelectCommand.Parameters.AddWithValue("@Hasta", hasta);
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

        public bool EliminarSucursal(LSucursales parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("EliminarSucursal", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@Sucursal", parametros.Sucursal);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();


            }
        }

    }
}
