namespace ProyectoErifer
{
    partial class frmBusqueda
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtgBusqueda = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdAceptar = new System.Windows.Forms.Button();
            this.cmdCancelar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgBusqueda)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtgBusqueda);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(831, 340);
            this.panel1.TabIndex = 0;
            // 
            // dtgBusqueda
            // 
            this.dtgBusqueda.AllowUserToAddRows = false;
            this.dtgBusqueda.AllowUserToDeleteRows = false;
            this.dtgBusqueda.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dtgBusqueda.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgBusqueda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgBusqueda.Location = new System.Drawing.Point(0, 0);
            this.dtgBusqueda.Name = "dtgBusqueda";
            this.dtgBusqueda.ReadOnly = true;
            this.dtgBusqueda.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgBusqueda.Size = new System.Drawing.Size(831, 340);
            this.dtgBusqueda.TabIndex = 0;
            this.dtgBusqueda.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgBusqueda_CellDoubleClick);
            this.dtgBusqueda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtgBusqueda_KeyDown);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.panel2.Controls.Add(this.cmdAceptar);
            this.panel2.Controls.Add(this.cmdCancelar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 240);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(831, 100);
            this.panel2.TabIndex = 1;
            // 
            // cmdAceptar
            // 
            this.cmdAceptar.BackgroundImage = global::Sorteo_de_Animalitos.Properties.Resources.aprobar_1_2;
            this.cmdAceptar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cmdAceptar.FlatAppearance.BorderSize = 0;
            this.cmdAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdAceptar.Location = new System.Drawing.Point(578, 15);
            this.cmdAceptar.Name = "cmdAceptar";
            this.cmdAceptar.Size = new System.Drawing.Size(112, 73);
            this.cmdAceptar.TabIndex = 4;
            this.cmdAceptar.UseVisualStyleBackColor = true;
            this.cmdAceptar.Click += new System.EventHandler(this.cmdAceptar_Click);
            // 
            // cmdCancelar
            // 
            this.cmdCancelar.BackgroundImage = global::Sorteo_de_Animalitos.Properties.Resources.regreso128;
            this.cmdCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cmdCancelar.FlatAppearance.BorderSize = 0;
            this.cmdCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancelar.Location = new System.Drawing.Point(707, 15);
            this.cmdCancelar.Name = "cmdCancelar";
            this.cmdCancelar.Size = new System.Drawing.Size(112, 73);
            this.cmdCancelar.TabIndex = 3;
            this.cmdCancelar.UseVisualStyleBackColor = true;
            this.cmdCancelar.Click += new System.EventHandler(this.cmdCancelar_Click);
            // 
            // frmBusqueda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.ClientSize = new System.Drawing.Size(831, 340);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBusqueda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmBusqueda";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgBusqueda)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dtgBusqueda;
        private System.Windows.Forms.Button cmdAceptar;
        private System.Windows.Forms.Button cmdCancelar;
    }
}