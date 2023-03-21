using Sorteo_de_Animalitos.Datos;
using Sorteo_de_Animalitos.Logica;
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
    public partial class ControlConfiguracion : UserControl
    {
        bool CargaInicial = false;
        public ControlConfiguracion()
        {
            CargaInicial = true;
            InitializeComponent();

            chkDomingo.Checked = true;
            chkLunes.Checked = true;
            chkMartes.Checked = true;
            chkMiercoles.Checked = true;
            chkJueves.Checked = true;
            chkViernes.Checked = true;
            chkSabado.Checked = true;
            chkDomingo.Checked = true;

            dtHora.Value = Convert.ToDateTime("12:00");
            MuestraDatos();
            //llenaCombos();
            llenaCombosSuc1();
            llenaCombosUsuarios();

            MuestraConfigGral();
            MostrarSucursales();
            MostrarUsuarios();
            MuestraUsuarioSucursal();

            llenaCombosSucTicketConfig();

            CargaInicial = false;

            //MostrarConfigAvanzada();
        }

        //private void llenaCombos()
        //{
        //    try
        //    {
        //        ConexionBD.Abrir();
        //        SqlCommand cmd = new SqlCommand("Select Sucursal, Nombre From SIEConfigSucursales Order By nombre", ConexionBD.conectar);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();

        //        da.Fill(dt);

        //        DataRow fila = dt.NewRow();
        //        fila["Nombre"] = "Sucursales";
        //        dt.Rows.InsertAt(fila, 0);

        //        cbSucursal.ValueMember = "Sucursal";
        //        cbSucursal.DisplayMember = "Nombre";
        //        cbSucursal.DataSource = dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        ConexionBD.Cerrar();
        //    }
        //}
        private void llenaCombosSuc1()
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select Sucursal, Nombre From SIEConfigSucursales Order By nombre", ConexionBD.conectar);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                DataRow fila = dt.NewRow();
                fila["Nombre"] = "";
                dt.Rows.InsertAt(fila, 0);

                cmbSucursales.ValueMember = "Sucursal";
                cmbSucursales.DisplayMember = "Nombre";
                cmbSucursales.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }
        private void llenaCombosUsuarios()
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select idUsuario As Usuario, Nombre From SIEConfigUsuarios Order By nombre", ConexionBD.conectar);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                DataRow fila = dt.NewRow();
                fila["Nombre"] = "";
                dt.Rows.InsertAt(fila, 0);

                cmbUsuarios.ValueMember = "Usuario";
                cmbUsuarios.DisplayMember = "Nombre";
                cmbUsuarios.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }
           
        private void Guardar()
        {
            try
            {
                string strSQL = "";
                ConexionBD.Abrir();
                if (chkAutomatico.Checked == true)
                {
                    strSQL = "Update SIEConfigParam Set sorteoAutomatico = 1, minutos = " + txtMinutos.Text;
                }
                else
                {
                     strSQL = "Update SIEConfigParam Set sorteoAutomatico = 0, minutos = " + txtMinutos.Text;
                }
                
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                cmd.ExecuteNonQuery();
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
        private void GuardarHoras()
        {
            try
            {
                if (string.IsNullOrEmpty(txtPermisoSEGOB.Text))
                {
                    MessageBox.Show("Ingrese el permiso de la SEGOB");
                    return;
                }

                if (string.IsNullOrEmpty(txtImporteAnimalito.Text))
                {
                    MessageBox.Show("Ingrese el importe por animalito");
                    return;
                }

                int Lunes = 0, Martes = 0, Miercoles = 0, Jueves = 0, Viernes = 0, Sabado = 0, Domingo = 0;
                DateTime hora;
                if (chkLunes.Checked == true)
                {
                    Lunes = 1;
                }

                if (chkMartes.Checked == true)
                {
                    Martes = 1;
                }

                if (chkMiercoles.Checked == true)
                {
                    Miercoles = 1;
                }

                if (chkJueves.Checked == true)
                {
                    Jueves = 1;
                }

                if (chkViernes.Checked == true)
                {
                    Viernes = 1;
                }

                if (chkSabado.Checked == true)
                {
                    Sabado = 1;
                }

                if (chkDomingo.Checked == true)
                {
                    Domingo = 1;
                }

               
                LConfig parametros = new LConfig();
                DConfigSorteos funcion = new DConfigSorteos();

                //parametros.Minutos = Convert.ToInt32(txtMinutos.Text);
                parametros.Hora = dtHora.Value;
                parametros.Lunes = Lunes;
                parametros.Martes = Martes;
                parametros.Miercoles = Miercoles;
                parametros.Jueves = Jueves;
                parametros.Viernes = Viernes;
                parametros.Sabado = Sabado;
                parametros.Domingo = Domingo;
                parametros.Activo = 1;
                parametros.PermisoSEGOB = txtPermisoSEGOB.Text;
                parametros.ImporteAnimalitos = txtImporteAnimalito.Text;
                if (funcion.GuardaConfigSorteos(parametros) == true)
                {
                    txtPermisoSEGOB.Clear();
                    MuestraDatos();
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
        private void MuestraDatos()
        {
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select * From SIEConfigParam";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();

                while (registro.Read())
                {
                    txtMinutos.Text = registro["minutos"].ToString();
                    if(Convert.ToInt32(registro["sorteoAutomatico"].ToString()) == 1)
                    {
                        chkAutomatico.Checked = true;
                    }
                    else
                    {
                        chkAutomatico.Checked = false;
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

            DataTable dt = new DataTable();
            DConfigSorteos funcion = new DConfigSorteos();
            funcion.MostrarConfigSorteos(ref dt);
            dtgRegistros.DataSource = dt;
           
            dtgRegistros.Columns[0].Width = 80;
            dtgRegistros.Columns[1].Visible = false;
            dtgRegistros.Columns[2].Width = 80;
            dtgRegistros.Columns[3].Width = 80;
            dtgRegistros.Columns[4].Width = 80;
            dtgRegistros.Columns[5].Width = 80;
            dtgRegistros.Columns[6].Width = 80;
            dtgRegistros.Columns[7].Width = 80;
            dtgRegistros.Columns[8].Width = 80;
            dtgRegistros.Columns[9].Width = 80;
            dtgRegistros.Columns[10].Width = 200;
            dtgRegistros.Columns[11].Visible = false;


        }
        private void btnGuardaSucursal_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                if (validaSucursal() == true)
                {
                    GuardaSucursal();
                    MostrarSucursales();
                    txtSucursal.Clear();
                    txtNombreSuc.Clear();
                    txtSerieTicket.Clear();
                    llenaCombosSuc1();
                }
            }
        }
        private void GuardaSucursal()
        {
            LSucursales parametros = new LSucursales();
            DSucursales funcion = new DSucursales();

            parametros.Sucursal = txtSucursal.Text;
            parametros.nombre = txtNombreSuc.Text;
            parametros.serieTicket = txtSerieTicket.Text;
            if (funcion.GuardarSucursal(parametros) == true)
            {
                MessageBox.Show("Sucursal guardada correctamente");
                txtSucursal.Focus();
            }

        }
        private void LimpiaCajasSuc()
        {
            txtSucursal.Text = "";
            txtNombreSuc.Text = "";
            txtSucursal.Focus();
        }
        private void LimpiaCajasUsu()
        {
            txtUsuario.Text = "";
            txtNombreUsuario.Text = "";
            txtPass.Text = "";
            txtPassConf.Text = "";
            txtUsuario.Enabled = true;
            txtUsuario.Focus();
        }
        private void btnCancelarSuc_Click(object sender, EventArgs e)
        {
            LimpiaCajasSuc();
        }
        private void btnCancelarUsuarios_Click(object sender, EventArgs e)
        {
            LimpiaCajasUsu();
        }
        private void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                if (validaUsuario() == true)
                {
                    GuardaUsuario();
                    LimpiaCajasUsu();
                    MostrarUsuarios();
                }
            }
        }
        private void GuardaUsuario()
        {
            LUsuarios parametros = new LUsuarios();
            DUsuarios funcion = new DUsuarios();

            parametros.idUsuario = txtUsuario.Text;
            parametros.Nombre = txtNombreUsuario.Text;
            parametros.Pass = txtPass.Text;
            parametros.GrupoPermisos = Convert.ToInt32(txtGrupoPermisos.Text);
            if(chkPermisos.Checked == true)
            {
                parametros.CambiaPermisos = 1;
            }
            else
            {
                parametros.CambiaPermisos = 0;
            }
            parametros.Usuario = Ambiente.Usuario;
            parametros.UsuFecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            parametros.UsuHora = Convert.ToDateTime("00:00");//Convert.ToDateTime(DateTime.Now.ToString("mm:ss"));
            parametros.Estacion = Ambiente.Estacion;
            if (funcion.GuardarUsuario(parametros) == true)
            {
                MessageBox.Show("Usuario guardado correctamente", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsuario.Focus();
            }
        }
        private bool validaUsuario()
        {
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                MessageBox.Show("Ingrese el usuario");
                txtUsuario.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtNombreUsuario.Text))
            {
                MessageBox.Show("Ingrese el nombre");
                txtNombreUsuario.Focus();
                return false;
            }

            if (txtPass.Text != txtPassConf.Text) 
            {
                MessageBox.Show("La contraseña no coincide");
                txtPass.Focus();
                return false;
            }
            return true;
        }
        private bool validaSucursal()
        {
            if (string.IsNullOrEmpty(txtSucursal.Text))
            {
                MessageBox.Show("Ingrese la sucursal");
                txtSucursal.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtNombreSuc.Text))
            {
                MessageBox.Show("Ingrese el nombre de la sucursal");
                txtNombreSuc.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtSerieTicket.Text))
            {
                MessageBox.Show("Ingrese la serie para el ticket de venta");
                txtSerieTicket.Focus();
                return false;
            }

            return true;
        }
        private void MostrarSucursales()
        {
            DataTable dt = new DataTable();
            DSucursales funcion = new DSucursales();
            funcion.MostrarSucursales(ref dt);
            dtgSucursales.DataSource = dt;

            dtgSucursales.Columns[0].Width = 100;
            dtgSucursales.Columns[1].Width = 200;            

        }
        private void MostrarUsuarios()
        {
            DataTable dt = new DataTable();
            DUsuarios funcion = new DUsuarios();
            funcion.MostrarUsuarios(ref dt);
            dtgUsuarios.DataSource = dt;

            dtgUsuarios.Columns[0].Width = 50;
            dtgUsuarios.Columns[1].Width = 50;
            dtgUsuarios.Columns[2].Width = 100;
            dtgUsuarios.Columns[3].Width = 200;
            dtgUsuarios.Columns[4].Width = 180;
        }
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                GuardarHoras();
                MuestraDatos();
            }
            
        }
        private void btnGeneraSorteo_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                Sorteo funcion = new Sorteo();
                funcion.SorteosAutomaticos();
            }
        }
        private void dtgRegistros_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dtgRegistros.Columns["Eliminar"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name + "dtgRegistros", "Eliminar", "DataGrid") != 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea eliminar el sorteo?", "Elimina sorteo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.OK)
                    {
                        EliminarConfigSorteo();
                    }
                }
            }
            
        }
        private void EliminarConfigSorteo()
        {
            int idSorteo = Convert.ToInt32(dtgRegistros.SelectedCells[11].Value.ToString());
            //MessageBox.Show(idSorteo.ToString());
            //LSorteo parametros = new LSorteo();
            DSorteo funcion = new DSorteo();
            //parametros.idSorteo = idSorteo;
            if (funcion.EliminarConfigSorteo(idSorteo) == true)
            {
                MuestraDatos();
            }
        }
        private void btnGuardarConfAv_Click(object sender, EventArgs e)
        {
            //GuardaConfigAvanzada();
        }
        //private void GuardaConfigAvanzada()
        //{
        //    LConfiguracionAvanzada parametros = new LConfiguracionAvanzada();
        //    DConfiguracionAvanzada funcion = new DConfiguracionAvanzada();

        //    parametros.SerieTicket = txtSerie.Text;
           
        //    if (funcion.GuardarConfig(parametros) == true)
        //    {
        //        MessageBox.Show("Configuración guardada correctamente");
        //        MostrarConfigAvanzada();
        //        txtSerie.Focus();
        //    }
        //}

        //private void MostrarConfigAvanzada()
        //{
        //    try
        //    {
        //        ConexionBD.Abrir();
        //        SqlCommand cmd = new SqlCommand("Select * From SIEConfigAvanzada", ConexionBD.conectar);
        //        SqlDataReader registro = cmd.ExecuteReader();
        //        if (registro.Read())
        //        {
        //            txtSerie.Text = registro["serieTicket"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + " " + ex.StackTrace);
        //    }
        //    finally
        //    {
        //        ConexionBD.Cerrar();
        //    }
        //}
        private void btnAgregaUsuSuc_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                GuardaUsuarioSucursal();
            }
        }
        private void GuardaUsuarioSucursal()
        {
            if (validaUsuarioSucursal() == true)
            {
                LUsuariosSuc parametros = new LUsuariosSuc();
                DUsuariosSuc funcion = new DUsuariosSuc();
                parametros.Usuario = cmbUsuarios.SelectedValue.ToString();
                parametros.Sucursal = cmbSucursales.SelectedValue.ToString();
                if (funcion.GuardarUsuarioSuc(parametros) == true)
                {
                    MuestraUsuarioSucursal();
                }
            }
        }
        private void MuestraUsuarioSucursal()
        {
            DataTable dt = new DataTable();
            DUsuariosSuc funcion = new DUsuariosSuc();
            funcion.MostrarUsuariosSuc(ref dt);
            dtgUsuariosSuc.DataSource = dt;

            dtgUsuariosSuc.Columns[0].Width = 80;
            dtgUsuariosSuc.Columns[1].Width = 100;
            dtgUsuariosSuc.Columns[2].Width = 100;

        }
        private void dtgUsuariosSuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dtgRegistros.Columns["Eliminar"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name + "dtgUsuariosSuc", "Eliminar", "DataGrid") != 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea eliminar el registro?", "Elimina registro", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.OK)
                    {
                        EliminarUsuSucursal();
                    }
                }
            }
           
        }
        private void dtgSucursales_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dtgSucursales.Columns["EliminarSuc"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name + "dtgSucursales", "EliminarSuc", "DataGrid") != 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea eliminar el registro?", "Elimina registro", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.OK)
                    {
                        EliminarSucursal();
                    }
                }
            }
            
        }
        private void dtgUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dtgUsuarios.Columns["EliminaUsuario"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name + "dtgUsuarios", "EliminaUsuario", "DataGrid") != 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea eliminar el usuario?", "Elimina usuario", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.OK)
                    {
                        EliminarUsuario();
                    }
                }
            }

            if (e.ColumnIndex == dtgUsuarios.Columns["EditaUsuario"].Index)
            {
                if (Ambiente.AsignaPermisos(this.Name + "dtgUsuarios", "EditaUsuario", "DataGrid") != 0)
                {
                    DialogResult result = MessageBox.Show("¿Desea editar el usuario?", "Edita usuario", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.OK)
                    {
                        //MessageBox.Show(dtgUsuarios[3,dtgUsuarios.CurrentCell.RowIndex].Value.ToString());
                        EditarUsuario(dtgUsuarios[2, dtgUsuarios.CurrentCell.RowIndex].Value.ToString());
                        txtUsuario.Enabled = false;
                    }
                }
            }
            
        }
        private void EditarUsuario(string sUsuario)
        {
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select * From SIEConfigUsuarios Where idUsuario = '" + sUsuario  + "'";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();

                if (registro.Read())
                {
                    txtUsuario.Text = "" + registro["idUsuario"].ToString();
                    txtNombreUsuario.Text = "" + registro["Nombre"].ToString();
                    txtPass.Text = "" + registro["password"].ToString();
                    txtPassConf.Text = "" + registro["password"].ToString();
                    txtGrupoPermisos.Text = "" + registro["grupoPermisos"].ToString();
                    if(Convert.ToInt32(registro["cambiaPermisos"].ToString()) == 1)
                    {
                        chkPermisos.Checked = true;
                    }
                    else
                    {
                        chkPermisos.Checked = false;
                    }                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.ToString());

            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }
        private void EliminarSucursal()
        {            
            string idSucursal = dtgSucursales.SelectedCells[1].Value.ToString();

            LSucursales parametros = new LSucursales();
            DSucursales funcion = new DSucursales();            
            parametros.Sucursal = idSucursal;
            if (funcion.EliminarSucursal(parametros) == true)
            {
                MostrarSucursales();
            }
        }
        private void EliminarUsuario()
        {
            string idUsuario = dtgUsuarios.SelectedCells[2].Value.ToString();

            LUsuarios parametros = new LUsuarios();
            DUsuarios funcion = new DUsuarios();
            parametros.Usuario = idUsuario;
            if (funcion.EliminarUsuario(parametros) == true)
            {
                MostrarUsuarios();
            }
        }
        private void EliminarUsuSucursal()
        {
            string idUsuario = dtgUsuariosSuc.SelectedCells[1].Value.ToString();
            string idSucursal = dtgUsuariosSuc.SelectedCells[2].Value.ToString();
            
            LUsuariosSuc parametros = new LUsuariosSuc();
            DUsuariosSuc funcion = new DUsuariosSuc();
            parametros.Usuario = idUsuario;
            parametros.Sucursal = idSucursal;
            if (funcion.EliminarUsuarioSuc(parametros) == true)
            {
                MuestraUsuarioSucursal();
            }
        }
        private bool validaUsuarioSucursal()
        {
            if (string.IsNullOrEmpty(cmbUsuarios.SelectedValue.ToString()))
            {
                MessageBox.Show("Seleccione el usuario");                
                return false;
            }

            if (string.IsNullOrEmpty(cmbSucursales.SelectedValue.ToString()))
            {
                MessageBox.Show("Seleccione la sucursal");                
                return false;
            }

            return true;
        }
        private void chkAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            //Guardar();
        }
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                Guardar();
            }
        }     

        private void GuardaTicketConfig()
        {
            if (cbSucursalTicketConfig.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Seleccione una sucursal");
                return;
            }
            LTicket parametros = new LTicket();
            DTicket funcion = new DTicket();

            parametros.Encabezado = txtEncabezado.Text;
            parametros.PieDePagina = txtPieDePagina.Text;
            parametros.Sucursal = cbSucursalTicketConfig.SelectedValue.ToString();
            if (funcion.GuardarTicketConfig(parametros) == true)
            {
                MessageBox.Show("Configuración de ticket guardada correctamente");
                txtSucursal.Focus();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                GuardaTicketConfig();
            }
        }

        private void llenaCombosSucTicketConfig()
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select Sucursal, Nombre From SIEConfigSucursales Order By nombre", ConexionBD.conectar);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                DataRow fila = dt.NewRow();
                fila["Nombre"] = "";
                dt.Rows.InsertAt(fila, 0);

                cbSucursalTicketConfig.ValueMember = "Sucursal";
                cbSucursalTicketConfig.DisplayMember = "Nombre";
                cbSucursalTicketConfig.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }

        private void cbSucursalTicketConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if(CargaInicial == false)
            {
                LimpiaTicketconfig();
                if (cbSucursalTicketConfig.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Seleccione una sucursal");
                    return;
                }
                MuestraTicketconfig();
            }            
        }

        private void MuestraTicketconfig()
        {
           
            DataTable dt = new DataTable();
            DTicket funcion = new DTicket();
            funcion.MostrarConfigTicket(ref dt, cbSucursalTicketConfig.SelectedValue.ToString());
            //dtgUsuariosSuc.DataSource = dt;
            if (dt.Rows.Count>0)
            {
                txtEncabezado.Text = dt.Rows[0]["encabezado"].ToString();
                txtPieDePagina.Text = dt.Rows[0]["piePagina"].ToString();
                cbSucursalTicketConfig.SelectedValue = dt.Rows[0]["sucursal"].ToString();
            }
            

        }

        private void LimpiaTicketconfig()
        {
            txtEncabezado.Text = "";
            txtPieDePagina.Text = "";
        }

        private void txtGrupoPermisos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}

            //// solo 1 punto decimal
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void btnViewPass_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                if (txtPass.PasswordChar == '*')
                {
                    txtPass.PasswordChar = Convert.ToChar("\0");
                    txtPassConf.PasswordChar = Convert.ToChar("\0");
                }
                else
                {
                    txtPass.PasswordChar = Convert.ToChar("*");
                    txtPassConf.PasswordChar = Convert.ToChar("*");
                }
            }
        }

        private void pbLogo_DoubleClick(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "PictureBox") != 0)
            {
                dlg.InitialDirectory = "";
                dlg.Filter = "Imágenes|*.jpg;*.png";
                dlg.FilterIndex = 2;
                dlg.Title = "Cargador de imágenes";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pbLogo.BackgroundImage = null;
                    //pbFoto.Image = new Bitmap(dlg.FileName);
                    pbLogo.BackgroundImage = new Bitmap(dlg.FileName);
                }
            }
        }
        private void btnGuardaConfigGral_Click(object sender, EventArgs e)
        {
            GuardaConfigGral();
        }

        private void GuardaConfigGral()
        {

            LConfigGral parametros = new LConfigGral();
            DConfigGral funcion = new DConfigGral();

            parametros.nombreEmpresa = txtEmpresa.Text;
            if (chkValidaPermisos.Checked == true)
            {
                parametros.validaPermisos = 1;
            }
            else
            {
                parametros.validaPermisos = 0;
            }

            if (pbLogo.BackgroundImage == null)
            {
                pbLogo.BackgroundImage = Properties.Resources.logoPrueba;
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            pbLogo.BackgroundImage.Save(ms, pbLogo.BackgroundImage.RawFormat);
            parametros.Logo = ms.GetBuffer();
            if (funcion.GuardarConfigGral(parametros) == true)
            {
                MessageBox.Show("Configuración general guardada correctamente", "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information);               
            }
        }

        private void MuestraConfigGral()
        {
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select * From SIEConfigGeneral";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();

                while (registro.Read())
                {
                    txtEmpresa.Text = registro["empresa"].ToString();
                    if (Convert.ToInt32(registro["sorteoAutomatico"].ToString()) == 1)
                    {
                        chkAutomatico.Checked = true;
                    }
                    else
                    {
                        chkAutomatico.Checked = false;
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
    }
}


