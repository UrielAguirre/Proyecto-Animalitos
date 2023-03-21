using Sorteo_de_Animalitos.Datos;
using Sorteo_de_Animalitos.Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.Reporting.Processing;
using Telerik.Reporting.Drawing;

namespace Sorteo_de_Animalitos.Presentacion
{
    public partial class frmCobroRapido : Form
    {
        int nVenta = Convert.ToInt32(Ambiente.var2);
        double cambio, iImporte, iPago1, iPago2, iPago3;
        string sCliente = "SYS";
        string sNombreCliente = "";
        private PrintDocument Documento;
        public frmCobroRapido()
        {
            InitializeComponent();            
            iImporte = Convert.ToDouble(Ambiente.var1);
            iPago1 = Convert.ToDouble(Ambiente.var1);
            iPago2 = 0;
            iPago3 = 0;

            txtImporte.Text = Convert.ToDouble(Ambiente.var1).ToString("N");
            txtIPago1.Text = Convert.ToDouble(Ambiente.var1).ToString("N");
            Ambiente.var1 = "";
            Ambiente.var2 = "";
                                    
            sNombreCliente = "" + Ambiente.var4;
            Ambiente.var4 = "";
            Ambiente.var5 = "";
        }

       
       
        private string DaFormato(double valor)
        {
            string valorResultante = "0.00";
            try
            {
                valorResultante = Convert.ToDouble(valor).ToString("N");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return valorResultante;
        }
       

        private void btnAtrasVentas_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        private void btnCobrar_Click(object sender, EventArgs e)
        {
            validaDatosCobro();
            EjecutaCobro();
        }
        private void validaDatosCobro()
        {

            if (txtIPago1.Text.Trim() == "")
            {
                txtIPago1.Text = "0";
            }
            if (txtIPago2.Text.Trim() == "")
            {
                txtIPago2.Text = "0";
            }
            if (txtIPago3.Text.Trim() == "")
            {
                txtIPago3.Text = "0";
            }
            if (txtCambio.Text.Trim() == "")
            {
                txtCambio.Text = "0";
            }
        }
        private void EjecutaCobro()
        {
            Ambiente.var1 = "";
            LCobroRapido parametros = new LCobroRapido();
            DCobroRapido funcion = new DCobroRapido();
            parametros.nVenta = nVenta;            
            parametros.sCliente = "SYS";
            parametros.nombreCliente = sNombreCliente;
            parametros.sFPago1 = txtPago1.Text.Trim();
            parametros.sFPago2 = txtPago2.Text.Trim();
            parametros.sFPago3 = txtPago3.Text.Trim();
            parametros.iImporte1 = Convert.ToDouble(txtIPago1.Text);
            parametros.iImporte2 = Convert.ToDouble(txtIPago2.Text);
            parametros.iImporte3 = Convert.ToDouble(txtIPago3.Text);
            if (Convert.ToDouble(txtCambio.Text) >= 0)
            {
                parametros.iCambio = Convert.ToDouble(txtCambio.Text);                
            }
            else
            {
                parametros.iCambio = 0;
                MessageBox.Show("Debe liquidar la cuenta total");
                return;
            }
            //MessageBox.Show("" + Convert.ToDouble(txtCambio.Text) * -1);

            parametros.Usuario = Ambiente.Usuario;
            parametros.Usufecha = DateTime.Now.ToString("yyyyMMdd");
            parametros.Usuhora = "00:00";
            parametros.Estacion = "ESTACION01";
            parametros.Sucursal = Ambiente.Sucursal;

            if (funcion.CobraVenta(parametros) == true)
            {
                ImprimeTicket();
                Ambiente.var1 = "true";
                Dispose();

            }
            else
            {
                MessageBox.Show("Error, no se confirmó la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ImprimeTicket()
        {
            VistaPreviaTicket();
            Documento = new PrintDocument();
            Documento.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            if (Documento.PrinterSettings.IsValid)
            {
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.PrinterName = "Microsoft Print to PDF";
                ReportProcessor reportProcesor = new ReportProcessor();
                reportProcesor.PrintReport(reportViewer1.ReportSource, printerSettings);
            }

        }

       
        private void VistaPreviaTicket()
        {
            Formatos.TicketVenta rpt = new Formatos.TicketVenta();
            DataTable dt = new DataTable();
            DVender funcion = new DVender();
            funcion.ImprimeTicketVenta(ref dt, nVenta, "");
            rpt.DataSource = dt;
            rpt.table1.DataSource = dt;
            reportViewer1.Report = rpt;
            reportViewer1.RefreshReport();
        }

        private void txtIPago1_TextChanged_1(object sender, EventArgs e)
        {
            calculaCambio();
        }

        private void txtIPago2_TextChanged(object sender, EventArgs e)
        {
            calculaCambio();
        }

        private void txtIPago3_TextChanged(object sender, EventArgs e)
        {
            calculaCambio();
        }

        private void btnAtrasVentas_Click_1(object sender, EventArgs e)
        {
            Ambiente.var1 = "";
            this.Dispose();
        }

        private void txtIPago1_Leave_1(object sender, EventArgs e)
        {
            if (txtIPago1.Text.Trim() == "")
            {
                iPago1 = 0;
            }
            else
            {
                iPago1 = Convert.ToDouble(txtIPago1.Text);
            }

            txtIPago1.Text = DaFormato(iPago1);
        }

        private void txtIPago2_Leave_1(object sender, EventArgs e)
        {
            if (txtIPago2.Text.Trim() == "")
            {
                iPago2 = 0;
            }
            else
            {
                iPago2 = Convert.ToDouble(txtIPago2.Text);
            }
            txtIPago2.Text = DaFormato(iPago2);
        }

        private void txtIPago3_Leave_1(object sender, EventArgs e)
        {
            if (txtIPago3.Text.Trim() == "")
            {
                iPago3 = 0;
            }
            else
            {
                iPago3 = Convert.ToDouble(txtIPago3.Text);
            }
            txtIPago3.Text = DaFormato(iPago3);
        }

        private void txtIPago3_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtIPago2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtIPago1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void calculaCambio()
        {
            try
            {
                cambio = (iPago1 + iPago2 + iPago3) - iImporte;
                txtCambio.Text = cambio.ToString("N");
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }
       
       
       

    }
}

