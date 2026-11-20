namespace ClaseCapas
{
    partial class FrmReportes
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
            this.lblCantidad = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnXml25000 = new System.Windows.Forms.Button();
            this.btnXml20 = new System.Windows.Forms.Button();
            this.cmbVehiculo = new System.Windows.Forms.ComboBox();
            this.btnPorVehiculo = new System.Windows.Forms.Button();
            this.lblGrilla = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            //
            // lblCantidad
            //
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(12, 15);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(104, 13);
            this.lblCantidad.TabIndex = 0;
            this.lblCantidad.Text = "Cantidad de viajes: -";
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(250, 15);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(102, 13);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total de los viajes: -";
            //
            // btnXml25000
            //
            this.btnXml25000.Location = new System.Drawing.Point(12, 40);
            this.btnXml25000.Name = "btnXml25000";
            this.btnXml25000.Size = new System.Drawing.Size(220, 25);
            this.btnXml25000.TabIndex = 2;
            this.btnXml25000.Text = "XML viajes de más de $25000";
            this.btnXml25000.UseVisualStyleBackColor = true;
            this.btnXml25000.Click += new System.EventHandler(this.btnXml25000_Click);
            //
            // btnXml20
            //
            this.btnXml20.Location = new System.Drawing.Point(238, 40);
            this.btnXml20.Name = "btnXml20";
            this.btnXml20.Size = new System.Drawing.Size(220, 25);
            this.btnXml20.TabIndex = 3;
            this.btnXml20.Text = "XML viajes de hasta 20 km";
            this.btnXml20.UseVisualStyleBackColor = true;
            this.btnXml20.Click += new System.EventHandler(this.btnXml20_Click);
            //
            // cmbVehiculo
            //
            this.cmbVehiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVehiculo.Location = new System.Drawing.Point(12, 76);
            this.cmbVehiculo.Name = "cmbVehiculo";
            this.cmbVehiculo.Size = new System.Drawing.Size(220, 21);
            this.cmbVehiculo.TabIndex = 4;
            //
            // btnPorVehiculo
            //
            this.btnPorVehiculo.Location = new System.Drawing.Point(238, 74);
            this.btnPorVehiculo.Name = "btnPorVehiculo";
            this.btnPorVehiculo.Size = new System.Drawing.Size(220, 25);
            this.btnPorVehiculo.TabIndex = 5;
            this.btnPorVehiculo.Text = "Ver viajes del vehículo";
            this.btnPorVehiculo.UseVisualStyleBackColor = true;
            this.btnPorVehiculo.Click += new System.EventHandler(this.btnPorVehiculo_Click);
            //
            // lblGrilla
            //
            this.lblGrilla.AutoSize = true;
            this.lblGrilla.Location = new System.Drawing.Point(12, 110);
            this.lblGrilla.Name = "lblGrilla";
            this.lblGrilla.Size = new System.Drawing.Size(0, 13);
            this.lblGrilla.TabIndex = 6;
            //
            // dataGridView1
            //
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 128);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(640, 250);
            this.dataGridView1.TabIndex = 7;
            //
            // FrmReportes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 390);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblGrilla);
            this.Controls.Add(this.btnPorVehiculo);
            this.Controls.Add(this.cmbVehiculo);
            this.Controls.Add(this.btnXml20);
            this.Controls.Add(this.btnXml25000);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblCantidad);
            this.Name = "FrmReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.FrmReportes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnXml25000;
        private System.Windows.Forms.Button btnXml20;
        private System.Windows.Forms.ComboBox cmbVehiculo;
        private System.Windows.Forms.Button btnPorVehiculo;
        private System.Windows.Forms.Label lblGrilla;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
