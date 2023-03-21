using Sorteo_de_Animalitos.Logica;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorteo_de_Animalitos.Datos
{
    public class DCobroRapido
    {
        public bool CobraVenta(LCobroRapido parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("CobroRapido", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@nVenta", parametros.nVenta);                
                cmd.Parameters.AddWithValue("@No_Referen", 0);
                cmd.Parameters.AddWithValue("@Fecha", DateTime.Now.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@Fecha_venc", DateTime.Now.ToString("yyyy/MM/dd"));
                cmd.Parameters.AddWithValue("@sCliente", parametros.sCliente);
                cmd.Parameters.AddWithValue("@nombreCliente", parametros.nombreCliente);
                cmd.Parameters.AddWithValue("@sFPago1", parametros.sFPago1);
                cmd.Parameters.AddWithValue("@iImporte1", parametros.iImporte1);
                cmd.Parameters.AddWithValue("@sFPago2", parametros.sFPago2);
                cmd.Parameters.AddWithValue("@iImporte2", parametros.iImporte2);
                cmd.Parameters.AddWithValue("@sFPago3", parametros.sFPago3);
                cmd.Parameters.AddWithValue("@iImporte3", parametros.iImporte3);
                cmd.Parameters.AddWithValue("@iCambio", parametros.iCambio);               
                cmd.Parameters.AddWithValue("@Usuario", parametros.Usuario);
                cmd.Parameters.AddWithValue("@Usufecha", parametros.Usufecha);
                cmd.Parameters.AddWithValue("@Usuhora", parametros.Usuhora);
                cmd.Parameters.AddWithValue("@Estacion", parametros.Estacion);
                cmd.Parameters.AddWithValue("@Sucursal", parametros.Sucursal);

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }


        }

    }
}
