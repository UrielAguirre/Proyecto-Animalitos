using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorteo_de_Animalitos.Presentacion
{
    public partial class frmImporte : Form
    {
        string cAnimalito;
        public frmImporte( string cAnimalito )
        {
            InitializeComponent();
            this.cAnimalito = cAnimalito;
            Importedisponibledisplay();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            Importedisponible();

        }

        private void Importedisponible()
        {
            Sorteo sorteo = new Sorteo();
            double impDisponible = sorteo.ImporteDisponible(cAnimalito);
            int idSorteo = sorteo.SorteoActivo(cAnimalito);

            if (Convert.ToDouble(txtImporte.Text) % 10 != 0)
            {
                MessageBox.Show("El importe debe ser múlitplos de 10");
                return;
            }
            if (Convert.ToDouble(txtImporte.Text) > impDisponible)
            {

                MessageBox.Show("El importe disponible es de " + impDisponible);
                return;
            }
            else
            {
                this.Text = "";
                Ambiente.var1 = txtImporte.Text;
                Ambiente.var2 = idSorteo.ToString();
                this.Dispose();
            }
        }

        private void Importedisponibledisplay()
        {
            Sorteo sorteo = new Sorteo();
            double impDisponible = sorteo.ImporteDisponible(cAnimalito);
            int idSorteo = sorteo.SorteoActivo(cAnimalito);
            
            this.Text = "Importe disponible: " + impDisponible.ToString("C");              
            
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Ambiente.var1 = "0";
            Ambiente.var2 = "0";
            this.Dispose();
        }
        
        private void txtImporte_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAceptar.PerformClick();
            }

            if(e.KeyCode == Keys.Escape)
            {
                btnCancelar.PerformClick();
            }
        }
    }
}
