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
    public class DVender
    {

        public bool InsertarVenta(LVender parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("InsertarVenta", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Venta", parametros.Venta);
                cmd.Parameters.AddWithValue("@Idsorteo", parametros.idSorteo);
                cmd.Parameters.AddWithValue("@Sucursal", parametros.Sucursal);
                cmd.Parameters.AddWithValue("@Cliente", parametros.Cliente);
                cmd.Parameters.AddWithValue("@NombreCliente", parametros.nombreCliente);
                cmd.Parameters.AddWithValue("@Telefono", parametros.Telefono);
                cmd.Parameters.AddWithValue("@Fecha", parametros.Fecha);
                cmd.Parameters.AddWithValue("@Fecha_Venc", parametros.Fecha_venc);
                cmd.Parameters.AddWithValue("@No_Referen", parametros.No_Referen);
                cmd.Parameters.AddWithValue("@Importe", parametros.Importe);
                cmd.Parameters.AddWithValue("@Impuesto", parametros.Impuesto);
                cmd.Parameters.AddWithValue("@Estado", parametros.Estado);
                cmd.Parameters.AddWithValue("@Usuario", parametros.Usuario);
                cmd.Parameters.AddWithValue("@Usufecha", parametros.UsuFecha);
                cmd.Parameters.AddWithValue("@Usuhora", parametros.UsuHora);
                cmd.Parameters.AddWithValue("@Estacion", parametros.Estacion);
                cmd.Parameters.AddWithValue("@Producto", parametros.Producto);
                cmd.Parameters.AddWithValue("@Descrip", parametros.Descrip);
                cmd.Parameters.AddWithValue("@Cantidad", parametros.Cantidad);
                cmd.Parameters.AddWithValue("@Precio", parametros.Precio);
                cmd.Parameters.AddWithValue("@ImpuestoU", parametros.ImpuestoU);
                cmd.Parameters.AddWithValue("@DescuentoU", parametros.DescuentoU);

                SqlParameter idUVenta = new SqlParameter("@VentaNva", SqlDbType.Int);
                idUVenta.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(idUVenta);

                cmd.ExecuteNonQuery();
                cmd.ResetCommandTimeout();
                int idVenta = (int)idUVenta.Value;
                //MessageBox.Show(idVenta.ToString());
                Ambiente.var1 = idVenta.ToString();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.StackTrace);
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }

        }

        public void CalculaImportesVenta(int nVenta)
        {
            double iImporte = 0;
            double iImpuesto = 0;
            string strSQL = "Select IsNull(Sum(precio * cantidad),0) As i From SIEpartvta Where venta = " + nVenta;
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                iImporte = Convert.ToDouble(cmd.ExecuteScalar());
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.Cerrar();
            }

            strSQL = "Select IsNull(Sum(precio * cantidad * (impuesto/100)),0) As i From SIEpartvta Where venta = " + nVenta;
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                iImpuesto = Convert.ToDouble(cmd.ExecuteScalar());
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                ConexionBD.Cerrar();
            }

            strSQL = "Update SIEventas Set importe = " + iImporte + ", impuesto = " + iImpuesto + " Where venta = " + nVenta;
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                Convert.ToDouble(cmd.ExecuteScalar());
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        public void ImprimeTicketVenta(ref DataTable dt, int idVenta, string importeLetra)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("TicketVenta", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@idVenta", idVenta);
                da.SelectCommand.Parameters.AddWithValue("@totalconLetra", importeLetra);
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
    }
}
