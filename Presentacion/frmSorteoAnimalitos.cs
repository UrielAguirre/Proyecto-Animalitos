﻿using Sorteo_de_Animalitos.Presentacion;
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

namespace Sorteo_de_Animalitos
{
    public partial class frmSorteoAnimalitos : Form
    {
        public frmSorteoAnimalitos()
        {
            InitializeComponent();
            lblUsuario.Text = "Usuario: " + Ambiente.Usuario;
            lblSucursal.Text = "Sucursal: " + Ambiente.Sucursal;

            Ambiente.usuarioCambiaPermisos();
            if (Ambiente.usuarioPuedeCambiarPermisos == false)
            {
                pbCandado.Visible = false;
            }

            Sorteo funcion = new Sorteo();
            funcion.SorteosAutomaticos();

            lblEmpresa.Text = Ambiente.AsignaLNombreEmpresa();
            Ambiente.AsignaLogoEmpresa(pbLogo); 

        }

        private void AsignaLogoEmpresa()
        {
            SqlDataReader registro;
            try
            {
                string strSQL = "Select * From SIEConfigGeneral";
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                registro = cmd.ExecuteReader();
                if (registro.Read())
                {
                    Ambiente.nombreEmpresa = "" + registro["empresa"].ToString();
                    Ambiente.Logo = (byte[])registro["logo"];
                    lblEmpresa.Text = Ambiente.nombreEmpresa; 
                try
                    {
                        byte[] b = Ambiente.Logo;
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                        //pbFoto.Image = Image.FromStream(ms);
                        pbLogo.BackgroundImage = null;
                        pbLogo.BackgroundImage = Image.FromStream(ms);
                    }
                    catch (Exception ex)
                    {
                        pbLogo.BackgroundImage = null;
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    Ambiente.nombreEmpresa = "Empresa, S.A. de C.V.";
                    lblEmpresa.Text = Ambiente.nombreEmpresa;
                    try
                    {
                        //System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                        //pbFoto.Image = Image.FromStream(ms);
                        pbLogo.BackgroundImage = null;
                        pbLogo.BackgroundImage = Properties.Resources.logoPrueba;
                    }
                    catch (Exception ex)
                    {
                        pbLogo.BackgroundImage = null;
                        MessageBox.Show(ex.Message);
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
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnVender_Click_1(object sender, EventArgs e)
        {
            //panelPrincipal.Controls.Clear();
            //ControlVender control = new ControlVender();
            //control.Dock = DockStyle.Fill;
            //panelPrincipal.Controls.Add(control);
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                panelPrincipal.Controls.Clear();
                controlVentas control = new controlVentas();
                control.Dock = DockStyle.Fill;
                panelPrincipal.Controls.Add(control);
            }
        }
        private void btnSorteos_Click_1(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                panelPrincipal.Controls.Clear();
                controlSorteos control = new controlSorteos();
                control.Dock = DockStyle.Fill;
                panelPrincipal.Controls.Add(control);
            }
        }
        private void btnCatalogo_Click_1(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                panelPrincipal.Controls.Clear();
                controlCatalogoAnimalitos control = new controlCatalogoAnimalitos();
                control.Dock = DockStyle.Fill;
                panelPrincipal.Controls.Add(control);
            }
        }              
        private void frmSorteoAnimalitos_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void btnReportes_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                panelPrincipal.Controls.Clear();
                ControlReporteSorteosPeriodo control = new ControlReporteSorteosPeriodo();
                control.Dock = DockStyle.Fill;
                panelPrincipal.Controls.Add(control);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Sorteo funcion = new Sorteo();
            funcion.SorteosAutomaticos();
        }
        private void btnConfiguracion_Click_1(object sender, EventArgs e)
        {                        
            if(Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                panelPrincipal.Controls.Clear();
                ControlConfiguracion control = new ControlConfiguracion();
                control.Dock = DockStyle.Fill;
                panelPrincipal.Controls.Add(control);
            }            
        }                  
        private void pbCandado_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           //essageBox.Show("" + Image.FromFile(pbCandado.Tag.ToString()));
            if (Ambiente.candadoCerrado==false)
            {
                pbCandado.BackgroundImage = null;
                pbCandado.BackgroundImage = Properties.Resources.CandadoCerrado32;
                pbCandado.Refresh();
                pbCandado.Visible = true;
                //MessageBox.Show("SI");
                Ambiente.candadoCerrado = true;
            }
            else
            {
                pbCandado.BackgroundImage = null;
                pbCandado.BackgroundImage = Properties.Resources.CandadoAbierto32;
                pbCandado.Refresh();
                pbCandado.Visible = true;
                Ambiente.candadoCerrado = false;
                //MessageBox.Show("ELSE");
            }
        }
    }
} 
