using Sorteo_de_Animalitos.Datos;
using Sorteo_de_Animalitos.Presentacion;
using Sorteo_de_Animalitos.Presentacion.Formatos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.Reporting.Processing;
using Telerik.Reporting.Drawing;

namespace Sorteo_de_Animalitos
{
    public partial class controlVentas : UserControl
    {
        int desde = 1;
        int hasta = 10;
        private PrintDocument Documento;
        public controlVentas()
        {
            InitializeComponent();
            MuestraVentas(0);
        }

        private void MuestraVentas(int noVenta)
        {
            DataTable dt = new DataTable();
            DVentas funcion = new DVentas();
            funcion.MuestraVentas(ref dt, Convert.ToDateTime(dtFechaInicial.Value.ToString("yyyy-MM-dd")), Convert.ToDateTime(dtFechaFinal.Value.ToString("yyyy-MM-dd")), noVenta);
            dtgVentas.DataSource = dt;

            dtgVentas.Columns[0].Width = 70;
            dtgVentas.Columns[1].Width = 100;
            dtgVentas.Columns[2].Width = 100;
            dtgVentas.Columns[3].Visible = false;
            dtgVentas.Columns[4].Width = 300;
            dtgVentas.Columns[5].Width = 100;
            dtgVentas.Columns[6].Width = 150;
            dtgVentas.Columns[7].Width = 60;
            //dtgRegistros.Columns[2].Width = 100;

            // dtgVentas.Columns[5].Visible = false;
            //dtgVentas.Columns[3].DefaultCellStyle.Format = "hh:mm tt";
            dtgVentas.Columns[5].DefaultCellStyle.Format = "N";
            dtgVentas.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void MuestraPartidas(int nVenta)
        {
            DataTable dt = new DataTable();
            DVentas funcion = new DVentas();
            funcion.MuestraPartidas(ref dt, nVenta);
            dtgPartidas.DataSource = dt;

            dtgPartidas.Columns[0].Width = 100;
            dtgPartidas.Columns[1].Width = 200;
            dtgPartidas.Columns[2].Width = 100;
          
            dtgPartidas.Columns[2].DefaultCellStyle.Format = "N";
            dtgPartidas.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;   
        }

        private void dtFechaInicial_CloseUp(object sender, EventArgs e)
        {
            MuestraVentas(0);
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            MuestraVentas(0);
        }

        private void dtFechaInicial_ValueChanged(object sender, EventArgs e)
        {
            MuestraVentas(0);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            MuestraVentas(0);
        }
               
        private void dtgRegistros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex == dtgVentas.Columns["Imprimir"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name + "dtgRegistros", "Imprimir", "DataGrid") != 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea imprimir la venta?", "Enviar a impresora", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.OK)
                    {
                        ImprimeTicket(Convert.ToInt32(dtgVentas[8, dtgVentas.CurrentCell.RowIndex].Value.ToString()));
                    }
                }
            }

            if (e.ColumnIndex == dtgVentas.Columns["Imprimir"].Index)
            {
                MuestraPartidas(Convert.ToInt32(dtgVentas[8, dtgVentas.CurrentCell.RowIndex].Value.ToString()));
            }
        }
       
        private void btnVender_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                this.Controls.Clear();
                ControlVender control = new ControlVender();
                this.Controls.Add(control);
                control.Dock = DockStyle.Fill;
            }
        }

        private void ImprimeTicket(int nVenta)
        {
            VistaPreviaTicket(nVenta);
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
        private void VistaPreviaTicket( int nVenta)
        {
            Presentacion.Formatos.TicketVenta rpt = new Presentacion.Formatos.TicketVenta();
            DataTable dt = new DataTable();
            DVender funcion = new DVender();
            funcion.ImprimeTicketVenta(ref dt, nVenta, "");
            rpt.DataSource = dt;
            rpt.table1.DataSource = dt;
            reportViewer1.Report = rpt;
            reportViewer1.RefreshReport();
        }
        private void txtNoVenta_Leave(object sender, EventArgs e)
        {          
            try
            {
                if (!String.IsNullOrEmpty(txtNoVenta.Text))
                {                   
                    if (double.IsNaN(Convert.ToInt32(txtNoVenta.Text)) == false)
                    {                        
                        MuestraVentas(Convert.ToInt32(txtNoVenta.Text));
                    }
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message);                
            }            
        }
    
    }
}
