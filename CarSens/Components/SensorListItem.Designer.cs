namespace CarSens.Components
{
    partial class SensorListItem
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSensorId = new System.Windows.Forms.Label();
            this.lblSensorName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSensorId
            // 
            this.lblSensorId.AutoSize = true;
            this.lblSensorId.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorId.Location = new System.Drawing.Point(312, 21);
            this.lblSensorId.Name = "lblSensorId";
            this.lblSensorId.Size = new System.Drawing.Size(47, 13);
            this.lblSensorId.TabIndex = 2;
            this.lblSensorId.Text = "label1";
            // 
            // lblSensorName
            // 
            this.lblSensorName.AutoSize = true;
            this.lblSensorName.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSensorName.Location = new System.Drawing.Point(62, 17);
            this.lblSensorName.Name = "lblSensorName";
            this.lblSensorName.Size = new System.Drawing.Size(120, 18);
            this.lblSensorName.TabIndex = 0;
            this.lblSensorName.Text = "SensorName";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 45);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(570, 51);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // SensorListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblSensorId);
            this.Controls.Add(this.lblSensorName);
            this.Name = "SensorListItem";
            this.Size = new System.Drawing.Size(570, 51);
            this.MouseEnter += new System.EventHandler(this.onmouseIn);
            this.MouseLeave += new System.EventHandler(this.onmouseOut);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void onmouseIn(object sender, System.EventArgs e)
        {
            this.tmpColor = this.BackColor;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
             
        }

        private void onmouseOut(object sender, System.EventArgs e)
        {
            this.BackColor = this.tmpColor;
        }

        #endregion

        private System.Windows.Forms.Label lblSensorId;
        private System.Windows.Forms.Label lblSensorName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;



        public System.Drawing.Color tmpColor { get; set; }
    }
}
