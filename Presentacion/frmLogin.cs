using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorteo_de_Animalitos.Presentacion
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            llenaCombos();
        }

        private void llenaCombos()
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select Sucursal, Nombre From SIEConfigSucursales Order By nombre", ConexionBD.conectar);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                DataRow fila = dt.NewRow();
                fila["Nombre"] = "Sucursales";
                dt.Rows.InsertAt(fila, 0);

                cbSucursal.ValueMember = "Sucursal";
                cbSucursal.DisplayMember = "Nombre";
                cbSucursal.DataSource = dt;

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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(ValidaUsuario() == true && ValidaSucursal() == true)
            {
                Ambiente.Usuario = txtUsuario.Text.ToUpper();
                Ambiente.Sucursal = cbSucursal.SelectedValue.ToString().ToUpper();
                frmSorteoAnimalitos formaInicial = new frmSorteoAnimalitos();
                formaInicial.Show();                
                this.Visible=false;
                formaInicial.Visible = true;
                
            }
        }

        private bool ValidaUsuario()
        {
            SqlDataReader registro;
            try
            {
                string strSQL = "Select * From SIEConfigUsuarios Where idUsuario = '" + txtUsuario.Text + "'";
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    if (registro["idUsuario"].ToString().ToUpper() == txtUsuario.Text.ToUpper() && registro["password"].ToString() == txtPass.Text)
                    {
                        registro.Close();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("El usuario o la contraseña es incorrecta");
                        registro.Close();
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("El usuario no existe");
                    return false;
                }
                
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

        private bool ValidaSucursal()
        {
            SqlDataReader registro;
            try
            {
                string strSQL = "Select * From SIEConfigUsuSuc Where usuario = '" + txtUsuario.Text  + "' And sucursal = '" + cbSucursal.SelectedValue + "'";
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                registro = cmd.ExecuteReader();
                if (registro.Read())       
                {                    
                    return true;                    
                }
                else
                {
                    MessageBox.Show("El usuario no tiene permiso para entrar a ésta sucursal");
                    
                    return false;
                }
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ValidaUsuario() == true && ValidaSucursal() == true)
                {
                    Ambiente.Usuario = txtUsuario.Text.ToUpper();
                    Ambiente.Sucursal = cbSucursal.SelectedValue.ToString().ToUpper();
                    frmSorteoAnimalitos formaInicial = new frmSorteoAnimalitos();
                    formaInicial.Show();
                    this.Visible = false;
                    formaInicial.Visible = true;

                }
            }
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAceptar.PerformClick();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAceptar.PerformClick();
            }
        }

        private void cbSucursal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAceptar.PerformClick();
            }
        }
    }
}
