namespace ClaseCapas
{
    partial class FrmProfesion
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
            this.btnEditarProfesion = new System.Windows.Forms.Button();
            this.btnEliminarProfesion = new System.Windows.Forms.Button();
            this.btnAgregarProfesion = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtIdProfesion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescProfesion = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEditarProfesion
            // 
            this.btnEditarProfesion.Location = new System.Drawing.Point(168, 120);
            this.btnEditarProfesion.Name = "btnEditarProfesion";
            this.btnEditarProfesion.Size = new System.Drawing.Size(75, 23);
            this.btnEditarProfesion.TabIndex = 38;
            this.btnEditarProfesion.Text = "Editar";
            this.btnEditarProfesion.UseVisualStyleBackColor = true;
            this.btnEditarProfesion.Click += new System.EventHandler(this.btnEditarProfesion_Click);
            // 
            // btnEliminarProfesion
            // 
            this.btnEliminarProfesion.Location = new System.Drawing.Point(249, 120);
            this.btnEliminarProfesion.Name = "btnEliminarProfesion";
            this.btnEliminarProfesion.Size = new System.Drawing.Size(75, 23);
            this.btnEliminarProfesion.TabIndex = 37;
            this.btnEliminarProfesion.Text = "Eliminar";
            this.btnEliminarProfesion.UseVisualStyleBackColor = true;
            this.btnEliminarProfesion.Click += new System.EventHandler(this.btnEliminarProfesion_Click);
            // 
            // btnAgregarProfesion
            // 
            this.btnAgregarProfesion.Location = new System.Drawing.Point(87, 120);
            this.btnAgregarProfesion.Name = "btnAgregarProfesion";
            this.btnAgregarProfesion.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarProfesion.TabIndex = 36;
            this.btnAgregarProfesion.Text = "Agregar";
            this.btnAgregarProfesion.UseVisualStyleBackColor = true;
            this.btnAgregarProfesion.Click += new System.EventHandler(this.btnAgregarProfesion_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(22, 174);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(550, 229);
            this.dataGridView1.TabIndex = 35;
            // 
            // txtIdProfesion
            // 
            this.txtIdProfesion.Enabled = false;
            this.txtIdProfesion.Location = new System.Drawing.Point(188, 40);
            this.txtIdProfesion.Name = "txtIdProfesion";
            this.txtIdProfesion.Size = new System.Drawing.Size(100, 22);
            this.txtIdProfesion.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 33;
            this.label2.Text = "ID";
            // 
            // txtDescProfesion
            // 
            this.txtDescProfesion.Location = new System.Drawing.Point(188, 80);
            this.txtDescProfesion.Name = "txtDescProfesion";
            this.txtDescProfesion.Size = new System.Drawing.Size(100, 22);
            this.txtDescProfesion.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 31;
            this.label1.Text = "Profesion";
            // 
            // FrmProfesion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEditarProfesion);
            this.Controls.Add(this.btnEliminarProfesion);
            this.Controls.Add(this.btnAgregarProfesion);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtIdProfesion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescProfesion);
            this.Controls.Add(this.label1);
            this.Name = "FrmProfesion";
            this.Text = "FrmProfesion";
            this.Load += new System.EventHandler(this.FrmProfesion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEditarProfesion;
        private System.Windows.Forms.Button btnEliminarProfesion;
        private System.Windows.Forms.Button btnAgregarProfesion;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtIdProfesion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescProfesion;
        private System.Windows.Forms.Label label1;
    }
}