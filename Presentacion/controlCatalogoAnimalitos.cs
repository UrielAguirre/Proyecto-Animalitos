using ProyectoErifer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorteo_de_Animalitos
{
    public partial class controlCatalogoAnimalitos : UserControl
    {
        int desde = 1;
        int hasta = 10000;
        public controlCatalogoAnimalitos()
        {
            InitializeComponent();
            
            txtAnimalito.Focus();
            MostrarAnimalitos();            
        }               
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                GuardarAimalito();
                txtAnimalito.Enabled = true;
            }
        }
        private void GuardarAimalito()
        {
            if(txtAnimalito.Text.Length != 3)
            {
                MessageBox.Show("El código del animalito debe tener un largo de 3 caracteres", "Inserta animalito", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtAnimalito.Focus();
                return;
            }
            LAnimales parametros = new LAnimales();
            DAnimales funcion = new DAnimales();
            parametros.Animalito = txtAnimalito.Text;
            parametros.AnimalNombre = txtNombre.Text;

            if (pbFoto.BackgroundImage == null)
            {
                pbFoto.BackgroundImage = Properties.Resources.logoPrueba;
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            pbFoto.BackgroundImage.Save(ms, pbFoto.BackgroundImage.RawFormat);
            parametros.Foto = ms.GetBuffer();

            parametros.Usuario = Ambiente.Usuario;
            parametros.UsuFecha = DateTime.Now;
            parametros.UsuHora = Convert.ToDateTime("00:00");
            parametros.Estacion = "ESTACION01";

            if (funcion.GuardarAnimalito(parametros) == true)
            {
                //ReinciarPaginado();
                LimpiaCajas();
                MostrarAnimalitos();
                txtAnimalito.Focus();
                //PanelPacientesReg.Visible = false;
                //MessageBox.Show("Producto guardado correctamente", "Guarda registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error, no se guardó el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }
        private void MostrarAnimalitos()
        {
            DataTable dt = new DataTable();
            DAnimales funcion = new DAnimales();
            funcion.MostrarAnimalitos(ref dt, desde, hasta);
            dtgRegistros.DataSource = dt;
           
            dtgRegistros.Columns[0].Width = 50;
            dtgRegistros.Columns[1].Width = 50;
            dtgRegistros.Columns[2].Width = 100;
            dtgRegistros.Columns[3].Width = 350;
            dtgRegistros.Columns[4].Visible = false;
            dtgRegistros.Columns[5].Visible = false;

            dtgRegistros.Columns[4].DefaultCellStyle.Format = "N";
            dtgRegistros.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dtgRegistros.Columns[2].Width = 100;
        }       
        private void ModificaAnimalito()
        {
            //PanelPaginado.Visible = false;
            // PanelReg.Visible = true;
            // PanelBusqueda.Visible = false;
            // dtgRegistros.Visible = false;
            //Limpiar();

            txtAnimalito.Enabled = false;
            txtNombre.Focus();

            txtAnimalito.Text = dtgRegistros.SelectedCells[2].Value.ToString();
            txtNombre.Text = dtgRegistros.SelectedCells[3].Value.ToString();           
            
            try
            {                
                byte[] b = ((Byte[])dtgRegistros.SelectedCells[5].Value);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                //pbFoto.Image = Image.FromStream(ms);
                pbFoto.BackgroundImage = null;
                pbFoto.BackgroundImage = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                pbFoto.BackgroundImage = null;
                MessageBox.Show(ex.Message);
            }
        }
        private void EliminarAnimalito()
        {
            string idAnimalito = dtgRegistros.SelectedCells[2].Value.ToString();
            LAnimales parametros = new LAnimales();
            DAnimales funcion = new DAnimales();
            parametros.Animalito = idAnimalito;
            if (funcion.EliminarAnimalito(parametros) == true)
            {
                MostrarAnimalitos();
            }
        }
        private void dtgRegistros_CellClick(object sender, DataGridViewCellEventArgs e)
        {           
            if (e.ColumnIndex == dtgRegistros.Columns["Eliminar"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name, "Eliminar", "DataGrid") != 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea eliminar el animalito?", "Elimina animalito", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.OK)
                    {
                        EliminarAnimalito();
                    }
                }
            }
            
            if (e.ColumnIndex == dtgRegistros.Columns["Editar"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name, "Editar", "DataGrid") != 0)
                {
                    ModificaAnimalito();
                }
            }
        }
        private void LimpiaCajas()
        {
            txtAnimalito.Clear();
            txtNombre.Clear();            
            pbFoto.BackgroundImage = null; 
        }
        private void pbFoto_DoubleClick(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "PictureBox") != 0)
            {
                dlg.InitialDirectory = "";
                dlg.Filter = "Imágenes|*.jpg;*.png";
                dlg.FilterIndex = 2;
                dlg.Title = "Cargador de imágenes";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pbFoto.BackgroundImage = null;
                    //pbFoto.Image = new Bitmap(dlg.FileName);
                    pbFoto.BackgroundImage = new Bitmap(dlg.FileName);
                }
            }
        }
        private void txtImporte_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAdd.PerformClick();
            }
        }

        private void txtAnimalito_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                string cCondition = "";
                if (!String.IsNullOrEmpty(txtAnimalito.Text))
                {
                    cCondition = cCondition + " And (animalito like '%" + txtAnimalito.Text + "%' Or nombre like '%" + txtAnimalito.Text + "%') ";
                }
                string strSQL = "Select Animalito, Nombre From SIEAnimalitos Where animalito <> '' " + cCondition;
                frmBusqueda busqueda = new frmBusqueda(strSQL, this.Location.X + txtAnimalito.Location.X + 175, this.Location.Y + txtAnimalito.Location.Y + 200);
                busqueda.ShowDialog();
                txtAnimalito.Text = busqueda.cCodigoSeleccionado;
            }
        }

        private void MuestraAnimalito()
        {
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select * From SIEAnimalitos Where animalito = '" + txtAnimalito.Text + "'";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    txtNombre.Text = "" + registro["nombre"].ToString();
                }
                else
                {
                    txtNombre.Text = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message + " " + ex.StackTrace);
                
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }

        private void txtAnimalito_Leave(object sender, EventArgs e)
        {
            MuestraAnimalito();
        }

        private void txtAnimalito_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }
    }
}
