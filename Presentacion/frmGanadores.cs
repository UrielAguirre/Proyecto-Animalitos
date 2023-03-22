using ProyectoErifer;
using Sorteo_de_Animalitos.Datos;
using Sorteo_de_Animalitos.Logica;
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
    public partial class frmGanadores : Form
    {
        int idSorteo;
        public frmGanadores( int idSorteo)
        {
            InitializeComponent();
            this.idSorteo = idSorteo;
            this.Text = "Captura de ganadores del sorteo " + idSorteo;
            MuestraGanador();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            confirmaGanadores();
        }

        private void confirmaGanadores()
        {
            LGanadores parametros = new LGanadores();
            DGanadores funcion = new DGanadores();
            parametros.IdSorteo = idSorteo;            
            if (funcion.ConfirmaGanadores(parametros) == true)
            {
                Dispose();
            }
            else
            {
                MessageBox.Show("Error, no se confirmaron los ganadores", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        private void guardaGanador()
        {
            LGanadores parametros = new LGanadores();
            DGanadores funcion = new DGanadores();
            parametros.IdSorteo = idSorteo;
            parametros.Animalito = txtAnimalito.Text;
            parametros.NombreAnimalito = txtNombreAnimalito.Text;
            parametros.Usuario = Ambiente.Usuario;
            parametros.UsuFecha = DateTime.Now;
            parametros.UsuHora = Convert.ToDateTime("00:00");
            parametros.Estacion = Ambiente.Estacion;
            parametros.Sucursal = Ambiente.Sucursal;
            if (funcion.GuardarGanadores(parametros) == true)
            {
                MuestraGanador();
            }
            else
            {
                MessageBox.Show("Error, no se guardó el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MuestraGanador()
        {
            DataTable dt = new DataTable();
            DGanadores funcion = new DGanadores();
            funcion.MostrarGanadores(ref dt, idSorteo);
            dtgRegistros.DataSource = dt;

            dtgRegistros.Columns[0].Width = 50;
            dtgRegistros.Columns[1].Width = 100;
            dtgRegistros.Columns[2].Width = 200;
            dtgRegistros.Columns[3].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtAnimalito.Text) == true)
            {
                MessageBox.Show("Ingrese un animalito correcto");
                txtAnimalito.Focus();
                return;
            }
            guardaGanador();
        }

        private void txtAnimalito_Leave(object sender, EventArgs e)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select * From SIEAnimalitos Where animalito = '"  + txtAnimalito.Text + "'", ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    txtNombreAnimalito.Text = registro["nombre"].ToString();
                }
                else
                {
                    txtNombreAnimalito.Text = "";
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

        private void dtgRegistros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dtgRegistros.Columns["Eliminar"].Index)
            {
                DialogResult result = MessageBox.Show("¿Desea eliminar el animalito?", "Elimina animalito", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.OK)
                {
                    EliminaAnimalito(Convert.ToInt32(dtgRegistros.SelectedCells[3].Value.ToString()));
                    MuestraGanador();
                }
            }
        }

        private void EliminaAnimalito( int idReg)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Delete From SIEGanadores Where id = " + idReg, ConexionBD.conectar);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {


            }
            finally
            {
                ConexionBD.Cerrar();
            }

        }

        private void btnCancelar_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                btnCancelar.PerformClick();
            }
        }

        private void txtAnimalito_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down)
            {
                string cCondition = "";
                if (!String.IsNullOrEmpty(txtAnimalito.Text))
                {
                    cCondition = cCondition + " And animalito like '%" + txtAnimalito.Text + "%'";
                }
                string strSQL = "Select Animalito, Nombre From SIEAnimalitos Where animalito <> '' " + cCondition;
                frmBusqueda busqueda = new frmBusqueda(strSQL, this.Location.X + txtAnimalito.Location.X + 13, this.Location.Y + txtAnimalito.Location.Y + 80);
                busqueda.ShowDialog();
                txtAnimalito.Text = busqueda.cCodigoSeleccionado;
            }
        }
    }
}
