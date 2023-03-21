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
    public partial class ControlReporteSorteosPeriodo : UserControl
    {
        public ControlReporteSorteosPeriodo()
        {
            InitializeComponent();
        }

        private void ReporteDeSorteosPeriodo()
        {
            Reportes.ReporteSorteo rpt = new Reportes.ReporteSorteo();
            DataTable dt = new DataTable();
            DSorteos funcion = new DSorteos();
            funcion.mostrarSorteosPorPeriodo(ref dt, dtFechaInicial.Value, dtFechaFinal.Value);
            rpt.DataSource = dt;
            rpt.table1.DataSource = dt;
            reportViewer1.Report = rpt;
            reportViewer1.RefreshReport();

        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            ReporteDeSorteosPeriodo();
        }
    }
}
