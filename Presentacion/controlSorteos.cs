using Sorteo_de_Animalitos.Presentacion;
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
    public partial class controlSorteos : UserControl
    {
        int desde = 1;
        int hasta = 10;
        int idSorteo = 0, idRegistroVenta = 0, idPagado = 0;
        bool sorteoValido;
        string cAnimalitoActivo = "";
        public controlSorteos()
        {
            InitializeComponent();
            MuestraSorteos();

            var mensajeAyuda = new ToolTip();

            mensajeAyuda.SetToolTip(btnGeneraSorteo, "Nuevo sorteo");

            mensajeAyuda.SetToolTip(btnGanador, "Ingresa animalitos ganadores");

            mensajeAyuda.SetToolTip(btnPagar, "Paga tikets ganadores");
        }

        private void btnGeneraSorteo_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {

                if (String.IsNullOrEmpty(txtPermisoSEGOB.Text) == true)
                {
                    MessageBox.Show("Ingrese el permiso SEGOB");
                    txtPermisoSEGOB.Focus();
                    return;
                }

                if (String.IsNullOrEmpty(txtImporteAnimalitos.Text))
                {
                    MessageBox.Show("Ingrese el importe por animalito");
                    txtImporteAnimalitos.Focus();
                    return;
                }
                if (MessageBox.Show("¿Confirma la creación de un nuevo sorteo?", "Sorteos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                if (MessageBox.Show("Los sorteos anteriores quedarán inactivos, ¿desea continuar?", "Sorteos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                DateTime fechaSorteo;
                DateTime horaSorteo;

                fechaSorteo = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                horaSorteo = Convert.ToDateTime(dtHora.Value.ToString("hh:ss"));

                Sorteo miSorteo = new Sorteo();
                miSorteo.NuevoSorteo(fechaSorteo, horaSorteo, txtPermisoSEGOB.Text, Convert.ToDouble(txtImporteAnimalitos.Text));
                MuestraSorteos();
            }
        }

        private void MuestraSorteos()
        {
            DataTable dt = new DataTable();
            DSorteo funcion = new DSorteo();
            funcion.MostrarSorteos(ref dt, desde, hasta, Convert.ToDateTime(dtFechaInicial.Value.ToString("yyyy-MM-dd")), Convert.ToDateTime(dtFechaFinal.Value.ToString("yyyy-MM-dd")));
            dtgRegistros.DataSource = dt;

            dtgRegistros.Columns[0].Visible = false;
            dtgRegistros.Columns[1].Visible = false;
            dtgRegistros.Columns[2].Width = 100;
            dtgRegistros.Columns[3].Width = 100;
            dtgRegistros.Columns[4].Width = 200;
            dtgRegistros.Columns[5].Width = 100;
            dtgRegistros.Columns[6].Visible = false;
            //dtgRegistros.Columns[5].Visible = false;
            //dtgRegistros.Columns[6].Visible = false;

            //dtgRegistros.Columns[5].Visible = false;
            dtgRegistros.Columns[3].DefaultCellStyle.Format = "hh:mm tt";            
        }

        private void marcaSorteosConfirmados()
        {           
            for (int i = 0; i < dtgRegistros.Rows.Count; i++){
            //if(nCol == 5)
            //{
            //    if (Convert.ToInt32(dtgRegistros.Rows[nRow].Cells[5].Value) == 1)
            //    {
            //        dtgRegistros.Rows[nRow].Cells[nCol].Value = "*";
            //    }
            //    else
            //    {
            //        dtgRegistros.Rows[nRow].Cells[5].Value = "";
            //    }
            //}

           
                if (Convert.ToInt32(dtgRegistros.Rows[i].Cells[6].Value) == 1)
                {
                    dtgRegistros.Rows[i].Cells[2].Style.BackColor = Color.Orange;
                }
            
            }           
            
        }

        private void marcarAnimalesGanadores()
        {
            for (int i = 0; i < dtgDetalle.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtgDetalle.Rows[i].Cells["esGanador"].Value) == 1)
                {
                    //MessageBox.Show(dtgRegistros.Rows[i].Cells["PermisoSEGOB"].Value.ToString());
                    //dtgRegistros.Rows[i].DefaultCellStyle.BackColor = Color.DarkOrange;
                    dtgDetalle.Rows[i].Cells[0].Style.BackColor = Color.Orange;
                }
            }
        }

        private void MuestraSorteosDet(int idSorteo)
        {
            DataTable dt = new DataTable();
            DSorteo funcion = new DSorteo();
            funcion.MostrarSorteosDet(ref dt, idSorteo);
            dtgDetalle.DataSource = dt;

            dtgDetalle.Columns[0].Width = 100;
            dtgDetalle.Columns[1].Width = 100;
            dtgDetalle.Columns[2].Width = 100;
            dtgDetalle.Columns[3].Width = 80;
            dtgDetalle.Columns[4].Width = 100;
            dtgDetalle.Columns[5].Visible = false;
            //dtgDetalle.Columns[5].Width = 100;
            //dtgDetalle.Columns[6].Width = 100;
            //dtgDetalle.Columns[7].Width = 100;
            //dtgRegistros.Columns[2].Width = 100;

            // dtgDetalle.Columns[5].Visible = false;
            //dtgDetalle.Columns[1].DefaultCellStyle.Format = "hh:mm tt";
            dtgDetalle.Columns[2].DefaultCellStyle.Format = "N";
            dtgDetalle.Columns[3].DefaultCellStyle.Format = "N";
            dtgDetalle.Columns[4].DefaultCellStyle.Format = "N";

        }

        private void MuestraSorteosMovs(int idSorteo, string cAnimalito)
        {
            DataTable dt = new DataTable();
            DSorteo funcion = new DSorteo();
            funcion.MostrarSorteosMovs(ref dt, idSorteo, cAnimalito);
            dtgMovs.DataSource = dt;

            dtgMovs.Columns[0].Width = 200;
            dtgMovs.Columns[1].Width = 100;
            dtgMovs.Columns[2].Width = 100;
            dtgMovs.Columns[3].Width = 80;
            dtgMovs.Columns[4].Width = 100;
            dtgMovs.Columns[6].Width = 100;
            //dtgDetalle.Columns[5].Width = 100;
            //dtgDetalle.Columns[6].Width = 100;
            //dtgDetalle.Columns[7].Width = 100;
            //dtgRegistros.Columns[2].Width = 100;

            // dtgDetalle.Columns[5].Visible = false;
            //dtgDetalle.Columns[1].DefaultCellStyle.Format = "hh:mm tt";
            dtgMovs.Columns[1].DefaultCellStyle.Format = "N";
            dtgMovs.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtgMovs.Columns[2].DefaultCellStyle.Format = "N";
            dtgMovs.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtgMovs.Columns[5].DefaultCellStyle.Format = "hh:mm tt";
            // dtgMovs.Columns[3].DefaultCellStyle.Format = "N";
            // dtgMovs.Columns[4].DefaultCellStyle.Format = "N";

        }

        private void dtFechaInicial_CloseUp(object sender, EventArgs e)
        {
            MuestraSorteos();
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            MuestraSorteos();
        }

        private void dtFechaInicial_ValueChanged(object sender, EventArgs e)
        {
            MuestraSorteos();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            MuestraSorteos();
        }

        private void dtgRegistros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("" + dtgRegistros.SelectedCells[6].Value);
            MuestraSorteosMovs(0, "");
            idSorteo = Convert.ToInt32(dtgRegistros.SelectedCells[7].Value);
            MuestraSorteosDet(idSorteo);            
        }
        private void dtgDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("" + Convert.ToInt32(dtgRegistros.SelectedCells[6].Value));
            //MessageBox.Show(dtgMovs.SelectedCells[0].Value.ToString());
            cAnimalitoActivo = dtgDetalle.SelectedCells[0].Value.ToString();
            MuestraSorteosMovs(idSorteo, cAnimalitoActivo);
            if (Convert.ToInt32(dtgDetalle.SelectedCells[5].Value.ToString()) == 1)
            {
                btnPagar.Enabled = true;
            }
            else
            {
                btnPagar.Enabled = false;
            }
        }

        private void btnGanador_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                if (idSorteo == 0)
                {
                    MessageBox.Show("Seleccione un sorteo");
                    return;
                }
                ValidaSorteo();
                if (sorteoValido == true)
                {
                    frmGanadores formulario = new frmGanadores(idSorteo);
                    formulario.ShowDialog();
                    idSorteo = 0;
                    MuestraSorteos();
                    MuestraSorteosDet(idSorteo);
                    MuestraSorteosMovs(idSorteo, "");
                }
            }
        }

        private void ValidaSorteo()
        {
            try
            {
                sorteoValido = true;
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select * From SIESorteos Where eliminado = 0 And idSorteo = " + idSorteo, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    if(Convert.ToInt32(registro["activo"].ToString()) == 1)
                    {
                        MessageBox.Show("Este sorteo continúa activo, no puede ingresar ganadores");
                        sorteoValido = false;
                        return;
                    }
                    if (Convert.ToInt32(registro["ganadoresConfirmados"].ToString()) == 1)
                    {
                        MessageBox.Show("Ya se registraron los ganadores a este sorteo");
                        sorteoValido = false;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Este sorteo no existe");
                    sorteoValido = false;
                    return;
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

        private void dtgRegistros_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           // MessageBox.Show(e.ColumnIndex.ToString());
            //if (e.ColumnIndex == 5 || e.ColumnIndex == 6)
            //{                
                marcaSorteosConfirmados();
            //}            
        }

        private void dtgDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            marcarAnimalesGanadores();
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(idRegistroVenta.ToString());
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                if (idPagado == 0)
                {
                    PagarAGanador(idRegistroVenta);
                    MuestraSorteosMovs(idSorteo, cAnimalitoActivo);
                }
                else
                {
                    MessageBox.Show("Este registro ya ha sido pagado");
                }
            }
        }

        private void txtImporteAnimalitos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void PagarAGanador( int id)
        {
            try
            {               
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Update SIESorteosMovs Set importePagado = importe * 2 Where id = " + id, ConexionBD.conectar);
                cmd.ExecuteScalar();
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

        private void dtgMovs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idRegistroVenta = Convert.ToInt32(dtgMovs.SelectedCells[7].Value);
            idPagado = Convert.ToInt32(dtgMovs.SelectedCells[2].Value);
        }
    }
}
