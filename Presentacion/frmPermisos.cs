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
    public partial class frmPermisos : Form
    {
        string Modulo;
        string Control;
        public frmPermisos(string modulo, string control)
        {
            InitializeComponent();
            Modulo = modulo;
            Control = control;

            MuestraPermisos(Modulo, Control);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {         
            foreach(Control controlActual in this.Controls)
            {
                if(controlActual is CheckBox)
                {
                    CheckBox chkActual;
                    chkActual = (CheckBox)controlActual;

                    string nombreControl = chkActual.Name;
                    int largo = nombreControl.Length;
                   // MessageBox.Show("" + nombreControl.Length);
                    //MessageBox.Show(nombreControl + " " + nombreControl.Substring(10, 1));
                    int numeroControl = Convert.ToInt32(nombreControl.Substring(8, largo - 8));
                    if (chkActual.Checked==true)
                    {   
                        GuardaPermiso(numeroControl, "", 1);
                    }
                    else
                    {
                        GuardaPermiso(numeroControl, "", 0);
                    }
                }
            }           
        }

        private void GuardaPermiso(int GrupoP, string idSuc, int Acceso)
        {
            LPermisos parametros = new LPermisos();
            DPermisos funcion = new DPermisos();

            parametros.idGrupoPermisos = GrupoP;
            parametros.idSucursal = idSuc;
            parametros.Modulo = Modulo;
            parametros.Control = Control;
            parametros.Acceso = Acceso;
            parametros.Usuario = Ambiente.Usuario;
            parametros.Usufecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            parametros.Usuhora = Convert.ToDateTime("00:00");//Convert.ToDateTime(DateTime.Now.ToString("mm:ss"));
            parametros.Estacion = Ambiente.Estacion;
            parametros.Sucursal = Ambiente.Sucursal;
            if (funcion.GuardaPermisos(parametros) == true)
            {
                this.Dispose();               
            }
        }

        private void MuestraPermisos( string Modulo, string cControl)
        {
            try
            {         
                foreach (Control controlActual in this.Controls)
                {
                    if (controlActual is CheckBox)
                    {
                        CheckBox chkActual;
                        chkActual = (CheckBox)controlActual;

                        string nombreControl = chkActual.Name;
                        int largo = nombreControl.Length;
                        // MessageBox.Show("" + nombreControl.Length);
                        //MessageBox.Show(nombreControl + " " + nombreControl.Substring(10, 1));
                        int numeroControl = Convert.ToInt32(nombreControl.Substring(8, largo - 8));

                        ConexionBD.Abrir();
                        string strSQL = "Select acceso From SIEPermisos Where idGrupoPermisos = " + numeroControl + " And modulo = '" + Modulo + "' And control = '" + cControl + "' Order By idGrupoPermisos";
                        SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                        SqlDataReader registros = cmd.ExecuteReader();
                        if (registros.Read())
                        {
                            if (Convert.ToInt32(registros["acceso"].ToString())==1)
                            {
                                chkActual.Checked = true;
                            }
                            else
                            {
                                chkActual.Checked = false;
                            }
                        }
                        else
                        {
                            chkActual.Checked = false;
                        }
                        ConexionBD.Cerrar();                        
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
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
