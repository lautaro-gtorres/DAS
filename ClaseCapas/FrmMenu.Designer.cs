namespace ClaseCapas
{
    partial class FrmMenu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.personaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nacionalidadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.profesionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buscarPersonaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.personaToolStripMenuItem,
            this.nacionalidadToolStripMenuItem,
            this.profesionesToolStripMenuItem,
            this.consultasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1006, 28);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // personaToolStripMenuItem
            // 
            this.personaToolStripMenuItem.Name = "personaToolStripMenuItem";
            this.personaToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.personaToolStripMenuItem.Text = "Persona";
            this.personaToolStripMenuItem.Click += new System.EventHandler(this.personaToolStripMenuItem_Click);
            // 
            // nacionalidadToolStripMenuItem
            // 
            this.nacionalidadToolStripMenuItem.Name = "nacionalidadToolStripMenuItem";
            this.nacionalidadToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.nacionalidadToolStripMenuItem.Text = "Nacionalidad";
            this.nacionalidadToolStripMenuItem.Click += new System.EventHandler(this.nacionalidadToolStripMenuItem_Click);
            // 
            // profesionesToolStripMenuItem
            // 
            this.profesionesToolStripMenuItem.Name = "profesionesToolStripMenuItem";
            this.profesionesToolStripMenuItem.Size = new System.Drawing.Size(99, 24);
            this.profesionesToolStripMenuItem.Text = "Profesiones";
            this.profesionesToolStripMenuItem.Click += new System.EventHandler(this.profesionesToolStripMenuItem_Click);
            // 
            // consultasToolStripMenuItem
            // 
            this.consultasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buscarPersonaToolStripMenuItem,
            this.resumenToolStripMenuItem});
            this.consultasToolStripMenuItem.Name = "consultasToolStripMenuItem";
            this.consultasToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.consultasToolStripMenuItem.Text = "Consultas";
            // 
            // buscarPersonaToolStripMenuItem
            // 
            this.buscarPersonaToolStripMenuItem.Name = "buscarPersonaToolStripMenuItem";
            this.buscarPersonaToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.buscarPersonaToolStripMenuItem.Text = "Buscar Persona";
            this.buscarPersonaToolStripMenuItem.Click += new System.EventHandler(this.buscarPersonaToolStripMenuItem_Click);
            // 
            // resumenToolStripMenuItem
            // 
            this.resumenToolStripMenuItem.Name = "resumenToolStripMenuItem";
            this.resumenToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.resumenToolStripMenuItem.Text = "Resumen";
            this.resumenToolStripMenuItem.Click += new System.EventHandler(this.resumenToolStripMenuItem_Click);
            // 
            // FrmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 450);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMenu";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem personaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nacionalidadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem profesionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buscarPersonaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumenToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}

