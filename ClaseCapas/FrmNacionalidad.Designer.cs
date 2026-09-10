namespace ClaseCapas
{
    partial class FrmNacionalidad
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescNacionalidad = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIdNacionalidad = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnEditarNacionalidad = new System.Windows.Forms.Button();
            this.btnEliminarNacionalidad = new System.Windows.Forms.Button();
            this.btnAgregarNacionalidad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nacionalidad";
            // 
            // txtDescNacionalidad
            // 
            this.txtDescNacionalidad.Location = new System.Drawing.Point(226, 66);
            this.txtDescNacionalidad.Name = "txtDescNacionalidad";
            this.txtDescNacionalidad.Size = new System.Drawing.Size(100, 22);
            this.txtDescNacionalidad.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "ID";
            // 
            // txtIdNacionalidad
            // 
            this.txtIdNacionalidad.Enabled = false;
            this.txtIdNacionalidad.Location = new System.Drawing.Point(226, 26);
            this.txtIdNacionalidad.Name = "txtIdNacionalidad";
            this.txtIdNacionalidad.Size = new System.Drawing.Size(100, 22);
            this.txtIdNacionalidad.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(399, 26);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnEditarNacionalidad
            // 
            this.btnEditarNacionalidad.Location = new System.Drawing.Point(206, 106);
            this.btnEditarNacionalidad.Name = "btnEditarNacionalidad";
            this.btnEditarNacionalidad.Size = new System.Drawing.Size(75, 23);
            this.btnEditarNacionalidad.TabIndex = 30;
            this.btnEditarNacionalidad.Text = "Editar";
            this.btnEditarNacionalidad.UseVisualStyleBackColor = true;
            this.btnEditarNacionalidad.Click += new System.EventHandler(this.btnEditarNacionalidad_Click);
            // 
            // btnEliminarNacionalidad
            // 
            this.btnEliminarNacionalidad.Location = new System.Drawing.Point(287, 106);
            this.btnEliminarNacionalidad.Name = "btnEliminarNacionalidad";
            this.btnEliminarNacionalidad.Size = new System.Drawing.Size(75, 23);
            this.btnEliminarNacionalidad.TabIndex = 29;
            this.btnEliminarNacionalidad.Text = "Eliminar";
            this.btnEliminarNacionalidad.UseVisualStyleBackColor = true;
            this.btnEliminarNacionalidad.Click += new System.EventHandler(this.btnEliminarNacionalidad_Click);
            // 
            // btnAgregarNacionalidad
            // 
            this.btnAgregarNacionalidad.Location = new System.Drawing.Point(125, 106);
            this.btnAgregarNacionalidad.Name = "btnAgregarNacionalidad";
            this.btnAgregarNacionalidad.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarNacionalidad.TabIndex = 28;
            this.btnAgregarNacionalidad.Text = "Agregar";
            this.btnAgregarNacionalidad.UseVisualStyleBackColor = true;
            this.btnAgregarNacionalidad.Click += new System.EventHandler(this.btnAgregarNacionalidad_Click);
            // 
            // FrmNacionalidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEditarNacionalidad);
            this.Controls.Add(this.btnEliminarNacionalidad);
            this.Controls.Add(this.btnAgregarNacionalidad);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtIdNacionalidad);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescNacionalidad);
            this.Controls.Add(this.label1);
            this.Name = "FrmNacionalidad";
            this.Text = "FrmNacionalidad";
            this.Load += new System.EventHandler(this.FrmNacionalidad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescNacionalidad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIdNacionalidad;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnEditarNacionalidad;
        private System.Windows.Forms.Button btnEliminarNacionalidad;
        private System.Windows.Forms.Button btnAgregarNacionalidad;
    }
}