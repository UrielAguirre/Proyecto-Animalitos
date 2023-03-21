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
    public class DPermisos
    {

        public bool GuardaPermisos(LPermisos parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("GuardaPermisos", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idGrupoPermisos", parametros.idGrupoPermisos);
                cmd.Parameters.AddWithValue("@idSucursal", parametros.idSucursal);
                cmd.Parameters.AddWithValue("@Modulo", parametros.Modulo);
                cmd.Parameters.AddWithValue("@control", parametros.Control);
                cmd.Parameters.AddWithValue("@Acceso", parametros.Acceso);
                cmd.Parameters.AddWithValue("@Usuario", parametros.Usuario);
                cmd.Parameters.AddWithValue("@UsuFecha", parametros.Usufecha);
                cmd.Parameters.AddWithValue("@Usuhora", parametros.Usuhora);
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
    }
}
