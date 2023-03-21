using Sorteo_de_Animalitos.Datos;
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
    public partial class ControlFormatoTicket : UserControl
    {
        int idVenta;
        public ControlFormatoTicket( int idventa)
        {
            InitializeComponent();
            idVenta = idventa;
            //Imprimeticket();
        }

        private void Imprimeticket()
        {
                Reportes.ReporteSorteo rpt = new Reportes.ReporteSorteo();
                DataTable dt = new DataTable();
                DVender funcion = new DVender();
                funcion.ImprimeTicketVenta(ref dt, idVenta, "");
                rpt.DataSource = dt;
                rpt.table1.DataSource = dt;
                reportViewer1.Report = rpt;
                reportViewer1.RefreshReport();                       
        }

       
    }
}
