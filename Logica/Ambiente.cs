using Sorteo_de_Animalitos.Presentacion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorteo_de_Animalitos
{
    public class Ambiente
    {
        public static string var1;
        public static string var2;
        public static string var3;
        public static string var4;
        public static string var5;
        public static string var6;
        public static string var7;
        public static string var8;
        public static string var9;
        public static string var10;

        public static string Usuario = "";
        public static string Estacion = "";
        public static string Sucursal = "";
        public static string nombreEmpresa = "";
        public static byte[] Logo;

        public static bool candadoCerrado = false;
        public static bool usuarioPuedeCambiarPermisos = false;

        public bool validaExisteEnCatalogo(string clave, string tabla, string campo)
        {
            try
            {
                ConexionBD.Abrir();
                string sqlQuery = "Select * From " + tabla + " Where " + campo + " = '" + clave + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public void validaSoloDigitos(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        public bool validaPermisoEnControl()
        {
            return true;
        }
        public static int AsignaPermisos(string Modulo, object currentSender, string Tipo)
        {
            int permiso = 0;
            string nameControl = "";
            if (Tipo == "Button")
            {
                Button controlPulsado = currentSender as Button;
                nameControl = controlPulsado.Name;                
            }
            if (Tipo == "PictureBox")
            {
                PictureBox controlPulsado = currentSender as PictureBox;
                nameControl = controlPulsado.Name;
            }
            if (Tipo == "DataGrid")
            {
               nameControl = currentSender.ToString();
            }

            

            if (usuarioPuedeCambiarPermisos == true && candadoCerrado == true)
            {
                //if (Tipo == "Button")
                //{
                    frmPermisos forma = new frmPermisos(Modulo, nameControl);
                    forma.ShowDialog();
                    permiso = 2;
                //}
                 
                //if (Tipo == "DataGrid")
                //{
                //    frmPermisos forma = new frmPermisos(Modulo, nameControl);
                //    forma.ShowDialog();
                //    permiso = 2;
                //}
                return permiso;
            }
            else if (usuarioPuedeCambiarPermisos == true && candadoCerrado == false)
            {
                permiso = 1;
                return permiso;
            }
            else 
            { 
                try
                {
                    string strSQL = "Select p.Acceso From SIEPermisos As p Left Join SIEConfigUsuarios As u On p.idGrupoPermisos = u.GrupoPermisos Where u.idUsuario = '" + Ambiente.Usuario + "' And p.modulo = '" + Modulo  + "' And p.control = '" + nameControl + "'";
                    //MessageBox.Show(strSQL);
                    ConexionBD.Abrir();
                    SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                    SqlDataReader registro = cmd.ExecuteReader();

                    if (registro.Read())
                    {
                        if(Convert.ToInt32(registro["Acceso"].ToString()) == 1)
                        {
                            permiso = 1;
                        }                           
                        else
                        {
                            permiso = 0;
                            MessageBox.Show("Acceso denegado, solicite el permiso con el administrador del sistema.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        permiso = 0;
                        MessageBox.Show("Acceso denegado, solicite el permiso con el administrador del sistema.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return permiso;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + ex.StackTrace);
                    MessageBox.Show("Acceso denegado, solicite el permiso con el administrador del sistema.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                finally
                {
                    ConexionBD.Cerrar();
                }
            }
        }         
        public static void usuarioCambiaPermisos()
        {
            /* Valida si el usuario pertenece al grupo 0, para validar permisos
             * 
             */
            int cambiaPermisos = 0;
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select * From SIEConfigUsuarios Where idUsuario = '" + Ambiente.Usuario + "'";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registros = cmd.ExecuteReader();
                if (registros.Read())
                {
                    cambiaPermisos = Convert.ToInt32(registros["cambiaPermisos"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                ConexionBD.Cerrar();
            }
            if (cambiaPermisos == 1)
            {
                usuarioPuedeCambiarPermisos = true;
            }
            else{
                usuarioPuedeCambiarPermisos = false;
            }
        }
        public static void AsignaLogoEmpresa(PictureBox cajaLogo)
        {
            SqlDataReader registro;
            try
            {
                string strSQL = "Select * From SIEConfigGeneral";
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                registro = cmd.ExecuteReader();
                if (registro.Read())
                {                   
                    Ambiente.Logo = (byte[])registro["logo"];                   
                    try
                    {
                        byte[] b = Ambiente.Logo;
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(b);                       
                        cajaLogo.BackgroundImage = null;
                        cajaLogo.BackgroundImage = Image.FromStream(ms);
                    }
                    catch (Exception ex)
                    {
                        cajaLogo.BackgroundImage = null;
                        MessageBox.Show(ex.Message);
                                               
                    }
                }
                else
                {                   
                    try
                    {
                        cajaLogo.BackgroundImage = null;
                        cajaLogo.BackgroundImage = Properties.Resources.logoPrueba;
                    }
                    catch (Exception ex)
                    {
                        cajaLogo.BackgroundImage = null;
                        MessageBox.Show(ex.Message);
                    }
                }
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
        public static string AsignaLNombreEmpresa()
        {
            SqlDataReader registro;
            try
            {
                string strSQL = "Select * From SIEConfigGeneral";
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    Ambiente.nombreEmpresa = "" + registro["empresa"].ToString();   
                }
                else
                {
                    Ambiente.nombreEmpresa = "Empresa, S.A. de C.V.";                   
                }
                return Ambiente.nombreEmpresa;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
                return "";
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }
    }
}
