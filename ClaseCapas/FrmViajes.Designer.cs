namespace ClaseCapas
{
    partial class FrmViajes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnPasajeros = new System.Windows.Forms.Button();
            this.btnReportes = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPasajero = new System.Windows.Forms.ComboBox();
            this.btnNuevoPasajero = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbVehiculo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.txtDestino = new ClaseCapas.Controles.CampoTexto();
            this.txtKm = new ClaseCapas.Controles.CampoTexto();
            this.txtImporte = new ClaseCapas.Controles.CampoTexto();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLiberar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // btnPasajeros
            //
            this.btnPasajeros.Location = new System.Drawing.Point(12, 12);
            this.btnPasajeros.Name = "btnPasajeros";
            this.btnPasajeros.Size = new System.Drawing.Size(130, 25);
            this.btnPasajeros.TabIndex = 0;
            this.btnPasajeros.Text = "ABM Pasajeros";
            this.btnPasajeros.UseVisualStyleBackColor = true;
            this.btnPasajeros.Click += new System.EventHandler(this.btnPasajeros_Click);
            //
            // btnReportes
            //
            this.btnReportes.Location = new System.Drawing.Point(148, 12);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(130, 25);
            this.btnReportes.TabIndex = 1;
            this.btnReportes.Text = "Reportes";
            this.btnReportes.UseVisualStyleBackColor = true;
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Pasajero";
            //
            // cmbPasajero
            //
            this.cmbPasajero.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPasajero.Location = new System.Drawing.Point(12, 66);
            this.cmbPasajero.Name = "cmbPasajero";
            this.cmbPasajero.Size = new System.Drawing.Size(200, 21);
            this.cmbPasajero.TabIndex = 3;
            //
            // btnNuevoPasajero
            //
            this.btnNuevoPasajero.Location = new System.Drawing.Point(218, 65);
            this.btnNuevoPasajero.Name = "btnNuevoPasajero";
            this.btnNuevoPasajero.Size = new System.Drawing.Size(60, 23);
            this.btnNuevoPasajero.TabIndex = 4;
            this.btnNuevoPasajero.Text = "Nuevo";
            this.btnNuevoPasajero.UseVisualStyleBackColor = true;
            this.btnNuevoPasajero.Click += new System.EventHandler(this.btnNuevoPasajero_Click);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Vehículo disponible";
            //
            // cmbVehiculo
            //
            this.cmbVehiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVehiculo.Location = new System.Drawing.Point(12, 112);
            this.cmbVehiculo.Name = "cmbVehiculo";
            this.cmbVehiculo.Size = new System.Drawing.Size(266, 21);
            this.cmbVehiculo.TabIndex = 6;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Fecha";
            //
            // dtpFecha
            //
            this.dtpFecha.Location = new System.Drawing.Point(12, 158);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(266, 20);
            this.dtpFecha.TabIndex = 8;
            //
            // txtDestino
            //
            this.txtDestino.Location = new System.Drawing.Point(12, 186);
            this.txtDestino.Name = "txtDestino";
            this.txtDestino.Requerido = true;
            this.txtDestino.Size = new System.Drawing.Size(220, 40);
            this.txtDestino.TabIndex = 9;
            this.txtDestino.Texto = "";
            this.txtDestino.Titulo = "Destino";
            //
            // txtKm
            //
            this.txtKm.Location = new System.Drawing.Point(12, 232);
            this.txtKm.Name = "txtKm";
            this.txtKm.Requerido = true;
            this.txtKm.Size = new System.Drawing.Size(220, 40);
            this.txtKm.SoloNumeros = true;
            this.txtKm.TabIndex = 10;
            this.txtKm.Texto = "";
            this.txtKm.Titulo = "Kilómetros";
            this.txtKm.TextoCambiado += new System.EventHandler(this.txtKm_TextoCambiado);
            //
            // txtImporte
            //
            this.txtImporte.Location = new System.Drawing.Point(12, 278);
            this.txtImporte.Name = "txtImporte";
            this.txtImporte.Size = new System.Drawing.Size(220, 40);
            this.txtImporte.SoloLectura = true;
            this.txtImporte.TabIndex = 11;
            this.txtImporte.TabStop = false;
            this.txtImporte.Texto = "";
            this.txtImporte.Titulo = "Importe ($9500 cada 30 km)";
            //
            // btnRegistrar
            //
            this.btnRegistrar.Location = new System.Drawing.Point(12, 326);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(220, 30);
            this.btnRegistrar.TabIndex = 12;
            this.btnRegistrar.Text = "Registrar viaje";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            //
            // dataGridView1
            //
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(300, 50);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(560, 268);
            this.dataGridView1.TabIndex = 13;
            //
            // btnLiberar
            //
            this.btnLiberar.Location = new System.Drawing.Point(300, 326);
            this.btnLiberar.Name = "btnLiberar";
            this.btnLiberar.Size = new System.Drawing.Size(250, 30);
            this.btnLiberar.TabIndex = 14;
            this.btnLiberar.Text = "Liberar vehículo del viaje seleccionado";
            this.btnLiberar.UseVisualStyleBackColor = true;
            this.btnLiberar.Click += new System.EventHandler(this.btnLiberar_Click);
            //
            // FrmViajes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 370);
            this.Controls.Add(this.btnLiberar);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.txtImporte);
            this.Controls.Add(this.txtKm);
            this.Controls.Add(this.txtDestino);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbVehiculo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnNuevoPasajero);
            this.Controls.Add(this.cmbPasajero);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReportes);
            this.Controls.Add(this.btnPasajeros);
            this.Name = "FrmViajes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Radio Taxi - Viajes";
            this.Load += new System.EventHandler(this.FrmViajes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnPasajeros;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPasajero;
        private System.Windows.Forms.Button btnNuevoPasajero;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbVehiculo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private Controles.CampoTexto txtDestino;
        private Controles.CampoTexto txtKm;
        private Controles.CampoTexto txtImporte;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLiberar;
    }
}
