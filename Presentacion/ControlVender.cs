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
    public partial class ControlVender : UserControl
    {
        string btnNombre = "";
        int nVenta = 0;
        private object panelPrincipal;

        public ControlVender()
        {
            InitializeComponent();
            TraeDatosSorteActual();
            CreaBotones();
            LlenaGrid();
        }
                
        private void CreaBotones()
        {
            try
            {
                int cCuantosPorRen = 5;
                int numeroButton = 0;               

                ConexionBD.Abrir();
                String strSQL = "Select a.* From SIESorteos As s Left Join SIESorteosDet As sd On s.idSorteo = sd.idSorteo Left Join SIEAnimalitos As a On sd.animalito = a.animalito Where (sd.importeMaximo - sd.importeVendido) > 0 And s.activo = 1 Order By s.idsorteo Desc, a.nombre";
                                
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registros = cmd.ExecuteReader();

                while (registros.Read())
                {                    
                        Button btn = new Button();
                        btn.Visible = true;
                        btn.Width = 120;
                        btn.Height = 120;

                        //Label lbl = new Label();
                        //lbl.Visible = true;
                        //lbl.Width = 80;
                        //lbl.Height = 10;

                        byte[] b = ((Byte[])registros["foto"]);
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                        btn.BackgroundImage = null;
                        btn.BackgroundImage = Image.FromStream(ms);
                        btn.BackgroundImageLayout = ImageLayout.Zoom;

                    //btn.Name = "button" + numeroButton;
                    btn.Name = "button" + registros["animalito"].ToString();
                       // btn.Text = btn + registros["animalito"].ToString();

                    //lbl.Name = "lbl" + numeroButton;
                    //lbl.Text = btn + registros["animalito"].ToString();
                    //btn.Font.;
                    //btn.TextAlign = ContentAlignment.;

                    btn.Click += Btn_Click;

                        panel1.Controls.Add(btn);

                        numeroButton++;  
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
        private void Btn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            // MessageBox.Show(sender.ToString());
            //essageBox.Show(sender.);
            Button botonSeleccionado = new Button();
            botonSeleccionado = (Button) sender;

            string cAnimalito = "";
            cAnimalito = botonSeleccionado.Name.Substring(6, botonSeleccionado.Name.Length - 6);
            //MessageBox.Show(cAnimalito);
            // MessageBox.Show(cAnimalito);

            VenderAnimalito(cAnimalito);
        }
        private void InsertarVenta( string cArticulo, string cDescrip, double cImporte, int idSorteo )
        {
            LVender parametros = new LVender();
            DVender funcion = new DVender();
            parametros.Venta = nVenta;
            parametros.idSorteo = idSorteo;
            parametros.Sucursal = Ambiente.Sucursal;
            parametros.Cliente = "SYS";
            parametros.nombreCliente= txtNombreCliente.Text;
            parametros.Telefono = txtTelefono.Text;
            parametros.Fecha = DateTime.Now;
            parametros.Fecha_venc = DateTime.Now;
            parametros.No_Referen = 0;
            parametros.Importe = Convert.ToDouble(txtTotal.Text) + cImporte;
            parametros.Impuesto = 0;
            parametros.Estado = "PE";       
            parametros.Usuario = "SUP";
            parametros.UsuFecha = DateTime.Now;
            parametros.UsuHora = Convert.ToDateTime("00:00");
            parametros.Estacion = "ESTACION01";
            parametros.Producto = cArticulo;
            parametros.Descrip = cDescrip;
            parametros.Precio = Convert.ToDouble(cImporte);
            parametros.Cantidad = 1;
            parametros.ImpuestoU = 0;
            parametros.DescuentoU = 0;

            if (funcion.InsertarVenta(parametros) == true)
            {
                nVenta = Convert.ToInt32(Ambiente.var1);
                //LimpiarPanelVender();
                LlenaGrid();
                Ambiente.var1 = "";

                //MostrarMedico();
                //PanelMedicosReg.Visible = false;
                //MessageBox.Show("Médico guardado correctamente", "Guarda registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error, no se guardó la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LlenaGrid()
        {
            try
            {
                //MessageBox.Show(nVenta.ToString());
                string strSQL = "";
                strSQL = strSQL + "Select Descrip As 'Descripción', Cantidad, Precio, (precio * cantidad) As Importe, id From SIEpartvta Where venta = " + nVenta;
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter(strSQL, ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dtgProductos.DataSource = dt;

                dtgProductos.Columns[0].Width = 150;
                dtgProductos.Columns[1].Width = 100;
                dtgProductos.Columns[2].Width = 100;
                dtgProductos.Columns[3].Width = 100;                
                dtgProductos.Columns[4].Visible = false;

                dtgProductos.Columns[2].DefaultCellStyle.Format = "N";
                dtgProductos.Columns[3].DefaultCellStyle.Format = "N";

                //dtgProductos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgProductos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgProductos.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("Select Sum(cantidad * precio) As i From SIEpartvta Where venta = " + nVenta, ConexionBD.conectar);

                txtTotal.Text = cmd.ExecuteScalar().ToString();
                if (txtTotal.Text.Trim() == "")
                {
                    txtTotal.Text = "0.00";
                }
                else
                {
                    txtTotal.Text = DaFormato(Convert.ToDouble(txtTotal.Text));
                }

                ConexionBD.Abrir();
                SqlCommand cmd1 = new SqlCommand("Select estado From SIEventas Where venta = " + nVenta, ConexionBD.conectar);
                SqlDataReader reader = cmd1.ExecuteReader();

                if (reader.Read())
                {
                    lblEstado.Text = "" + reader["Estado"].ToString();
                }
                else
                {
                    lblEstado.Text = "PE";
                }

                if (lblEstado.Text == "CO")
                {
                    
                    btnVender.Enabled = false;
                    dtgProductos.Enabled = false;
                    panel1.Enabled = false;
                }
                else
                {
                    btnVender.Enabled = true;
                    dtgProductos.Enabled = true;
                    panel1.Enabled = true;

                }
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

        private void btnVender_Click(object sender, EventArgs e)
        {

            //if (String.IsNullOrEmpty(txtNombreCliente.Text))
            //{
            //    MessageBox.Show("Ingrese el nombre del cliente");
            //    txtNombreCliente.Focus();
            //    return;
            //}

            //if (String.IsNullOrEmpty(txtTelefono.Text))
            //{
            //    MessageBox.Show("Ingrese el teléfono del cliente");
            //    txtTelefono.Focus();
            //    return;
            //}
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                if (Convert.ToDouble(txtTotal.Text) == 0)
                {
                    MessageBox.Show("No puede cobrar una venta sin partidas","Cobro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Ambiente.var1 = txtTotal.Text;
                Ambiente.var2 = nVenta.ToString();
                Ambiente.var4 = txtNombreCliente.Text;
                frmCobroRapido cobro = new frmCobroRapido();
                cobro.ShowDialog();
                if (Ambiente.var1 == "true")
                {
                    Ambiente.var1 = "";
                    nuevaVenta();
                }
                txtAnimalito.Focus();
            }
            
        }
                
        private void nuevaVenta()
        {
            nVenta = 0;
            LlenaGrid();
            txtNombreCliente.Text = "";
            txtTelefono.Text = "";
            txtTotal.Text = "0.00";
            txtNombreCliente.Focus();

            TraeDatosSorteActual();
        }
        private string NombreAnimalito(string cAnimalito)
        {
            string nombreAnimalito = "";
            try
            {
                
                string strSQL = "Select nombre From SIEAnimalitos Where animalito = '" + cAnimalito  + "'";
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                nombreAnimalito = cmd.ExecuteScalar().ToString();

                return nombreAnimalito;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
                return nombreAnimalito;
            }
            finally
            {
                ConexionBD.Cerrar();
            }

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

        private void dtgProductos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
               // DialogResult respuesta = MessageBox.Show("¿Desea eliminar el artículo seleccionado?", "Elimina registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
               // if (respuesta.Equals(DialogResult.Yes))
               // {
                    eliminaRegistro(Convert.ToInt32(dtgProductos.SelectedCells[4].Value));
               // }
            }
        }

        private void eliminaRegistro(int id)
        {
            try
            {
                string strSQL = "";
                strSQL = strSQL + "Delete From siepartvta Where id = " + id;

                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                cmd.ExecuteNonQuery();
                DVender funcion = new DVender();
                funcion.CalculaImportesVenta(nVenta);
                LlenaGrid();

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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            nuevaVenta();
        }

        private void btnAgregaAnimalito_Click(object sender, EventArgs e)
        {
            if (Ambiente.AsignaPermisos(this.Name, sender, "Button") != 0)
            {
                VenderAnimalito(txtAnimalito.Text);
                txtAnimalito.Clear();
                txtAnimalito.Focus();
            }
        }

        private void VenderAnimalito( string cAnimalito )
        {
            //if (String.IsNullOrEmpty(txtNombreCliente.Text))
            //{
            //    MessageBox.Show("Ingrese el nombre del cliente");
            //    txtNombreCliente.Focus();
            //    return;
            //}

            //if (String.IsNullOrEmpty(txtTelefono.Text))
            //{
            //    MessageBox.Show("Ingrese el teléfono del cliente");
            //    txtTelefono.Focus();
            //    return;
            //}
            

            frmImporte formImp = new frmImporte(cAnimalito);
            formImp.ShowDialog();
            double ImportexAnimalito = Convert.ToDouble(Ambiente.var1);
            Ambiente.var1 = "";
            int IdSorteo = Convert.ToInt32(Ambiente.var2);
            Ambiente.var2 = "";

            if (IdSorteo > 0 && ImportexAnimalito > 0)
            {
                InsertarVenta(cAnimalito, NombreAnimalito(cAnimalito), ImportexAnimalito, IdSorteo);
            }
            txtAnimalito.Focus();
        }

        private void TraeDatosSorteActual()
        {
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select * From SIESorteos Where activo = 1 Order by idSorteo Desc";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();

                if (registro.Read())
                {
                    txtSorteo.Text = "Sorteo: " + registro["idSorteo"].ToString() + " Fecha: " + registro["fecha"].ToString();
                    txtSEGOB.Text = "" + registro["PermisoSEGOB"].ToString();
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
    

        private void txtAnimalito_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnVender.PerformClick();
            }

            if (e.KeyCode == Keys.Down)
            {
                string cCondition = "";
                if (!String.IsNullOrEmpty(txtAnimalito.Text))
                {
                    cCondition = cCondition + " And (animalito like '%" + txtAnimalito.Text + "%' Or nombre like '%" + txtAnimalito.Text + "%') ";
                }
                string strSQL = "Select Animalito, Nombre From SIEAnimalitos Where animalito <> '' " + cCondition;
                frmBusqueda busqueda = new frmBusqueda(strSQL, this.Location.X + txtAnimalito.Location.X + 175, this.Location.Y + txtAnimalito.Location.Y + 200);
                busqueda.ShowDialog();
                txtAnimalito.Text = busqueda.cCodigoSeleccionado;
            }
        }
    }
          
    
}
