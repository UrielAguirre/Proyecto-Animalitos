 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Sorteo_de_Animalitos
{
    public class Sorteo
    {

        public Sorteo() { 
        
        
        }

        private bool sorteoAutomHabilitado()
        {

            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select sorteoAutomatico From SIEConfigParam";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();

                if (registro.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }

        }

        public void SorteosAutomaticos()
        {
            try
            {
                if (sorteoAutomHabilitado() == false)
                {
                    return;
                }
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("SorteosAutomaticosNVO", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                //cmd.ResetCommandTimeout();                
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
        public void SorteosAutomaticosAnt()
        {
            try
            {
                int diaDeLaSemana = (int) DateTime.Now.DayOfWeek;
                string cCondition = "";
                DateTime horaSorteo;

                switch (diaDeLaSemana)
                {
                    case 1:
                        cCondition = " And Lunes = 1 ";
                        break;
                    case 2:
                        cCondition = " And Martes = 1 ";
                        break;
                    case 3:
                        cCondition = " And Miercoles = 1 ";
                        break;
                    case 4:
                        cCondition = " And Jueves = 1 ";
                        break;
                    case 5:
                        cCondition = " And Viernes = 1 ";
                        break;
                    case 6:
                        cCondition = " And Sabado = 1 ";
                        break;
                    case 7:
                        cCondition = " And Domingo = 1 ";
                        break;                  
                    
                }

                string strSQL = "Select * From SIEConfigSorteos Where id > 0 " + cCondition  + "Order By hora desc";
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registros = cmd.ExecuteReader();

                while (registros.Read())
                {
                    horaSorteo = Convert.ToDateTime(registros["hora"].ToString());

                    SqlCommand cmd1 = new SqlCommand("Select * From SIESorteos Where hora <= Format(GetDate(), 'hh:mm') And fecha = GetDate() And hora = " + horaSorteo, ConexionBD.conectar);
                    SqlDataReader registro1 = cmd1.ExecuteReader();

                    if (registro1.Read())
                    {
                        MessageBox.Show("sisisis");

                    }
                    if (DateTime.Now.ToShortTimeString() != DateTime.Now.ToShortTimeString())
                    {
                        
                    }
                    registro1.Close();
                    cmd1.Dispose();
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
        public void NuevoSorteo(DateTime fecha, DateTime hora, string PermisoSEGOB, double importeAnimalitos)
        {
            int idSorteo = 0;
            DateTime Fecha = fecha;
            DateTime Hora = hora;  
            DateTime UsuFecha = DateTime.Now;
            DateTime UsuHora = Convert.ToDateTime("00:00");            

            LSorteo parametros = new LSorteo();
            DSorteo funcion = new DSorteo();
            parametros.Fecha = Fecha;
            parametros.Hora = Hora;
            parametros.PermisoSQGOB = PermisoSEGOB;
            parametros.importeAnimalitos = importeAnimalitos;
            parametros.Usuario = Ambiente.Usuario;
            parametros.UsuFecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            parametros.UsuHora = Convert.ToDateTime(DateTime.Now.ToString("hh:mm"));
            parametros.Sucursal = Ambiente.Sucursal;

            if (funcion.GeneraSorteo(parametros) == true)
            {
                //ReinciarPaginado();
                //MostrarProducto();
                //PanelPacientesReg.Visible = false;
                MessageBox.Show("Sorteo generado correctamente", "Guarda registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error, no se generó el sorteo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public double ImporteDisponible(string cAnimalito)
        {
            double importeVendido = 0, importeMax = 0, importeDisp = 0;
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select sd.importeVendido, sd.importeMaximo From SIESorteos As s Left Join SIESorteosdet As sd On s.idSorteo = sd.idSorteo Where s.eliminado = 0 And s.Activo = 1 And sd.animalito = '" + cAnimalito + "' Order By s.idSorteo desc";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();
                //MessageBox.Show("" + cAnimalito);
                if (registro.Read())
                {
                   // MessageBox.Show("" + registro["importeVendido"].ToString() + " " + registro["importeMaximo"].ToString());
                    importeVendido = Convert.ToDouble(registro["importeVendido"]);
                    importeMax = Convert.ToDouble(registro["importeMaximo"]);
                    importeDisp = importeMax - importeVendido;
                }
                return importeDisp;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
                return importeDisp;
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }

        public int SorteoActivo(string cAnimalito)
        {
            int idSorteo=0;
            try
            {
                ConexionBD.Abrir();
                string strSQL = "Select s.idSorteo From SIESorteos As s Left Join SIESorteosdet As sd On s.idSorteo = sd.idSorteo Where s.Activo = 1 And sd.animalito = '" + cAnimalito + "' Order By s.idSorteo desc";
                SqlCommand cmd = new SqlCommand(strSQL, ConexionBD.conectar);
                SqlDataReader registro = cmd.ExecuteReader();

                if (registro.Read())
                {
                    idSorteo = Convert.ToInt32(registro["idSorteo"]);                   
                }
                return idSorteo;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
                return idSorteo;
            }
            finally
            {
                ConexionBD.Cerrar();
            }

        }
    }
    public class LSorteo
    {
       public int idSorteo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string PermisoSQGOB { get; set; }
        public double importeAnimalitos { get; set; }
        public string Usuario { get; set; }
        public DateTime UsuFecha { get; set; }
        public DateTime UsuHora { get; set; }
        public string Sucursal { get; set; }
    }

    public class DSorteo
    {
        public bool GeneraSorteo( LSorteo parametros)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("GeneraSorteo", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSorteo", parametros.idSorteo);
                cmd.Parameters.AddWithValue("@Fecha", parametros.Fecha);
                cmd.Parameters.AddWithValue("@Hora", parametros.Hora);
                cmd.Parameters.AddWithValue("@PermisoSEGOB", parametros.PermisoSQGOB);
                cmd.Parameters.AddWithValue("@ImporteMaximo", parametros.importeAnimalitos);
                cmd.Parameters.AddWithValue("@Usuario", parametros.Usuario);
                cmd.Parameters.AddWithValue("@Usufecha", parametros.UsuFecha);
                cmd.Parameters.AddWithValue("@Usuhora", parametros.UsuHora);
                cmd.Parameters.AddWithValue("@Sucursal", parametros.Sucursal);
                cmd.ExecuteNonQuery();
                //cmd.ResetCommandTimeout();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + " " + ex.Message);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }

        public void MostrarSorteos(ref DataTable dt, int desde, int hasta, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("mostrarSorteos", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Desde", desde);
                da.SelectCommand.Parameters.AddWithValue("@Hasta", hasta);
                da.SelectCommand.Parameters.AddWithValue("@FechaIni", fechaIni);
                da.SelectCommand.Parameters.AddWithValue("@FechaFin", fechaFin);
                da.Fill(dt);

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

        public void MostrarSorteosDet(ref DataTable dt, int idSorteo)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("mostrarSorteosDet", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@IdSorteo", idSorteo);
                
                da.Fill(dt);

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

        public void MostrarSorteosMovs(ref DataTable dt, int idSorteo, string cAnimalito)
        {
            try
            {
                ConexionBD.Abrir();
                SqlDataAdapter da = new SqlDataAdapter("mostrarSorteosMovs", ConexionBD.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@IdSorteo", idSorteo);
                da.SelectCommand.Parameters.AddWithValue("@cAnimalito", cAnimalito);

                da.Fill(dt);
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
        public bool EliminarSorteo(int id)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("EliminarSorteo", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSorteo", id);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }

        public bool EliminarConfigSorteo(int id)
        {
            try
            {
                ConexionBD.Abrir();
                SqlCommand cmd = new SqlCommand("EliminarConfigSorteo", ConexionBD.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSorteo", id);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                return false;
            }
            finally
            {
                ConexionBD.Cerrar();
            }
        }

    }


}
