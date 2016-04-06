namespace CarSens
{
    partial class SensorView
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
            this.pnlSensorContainer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlSensorContainer
            // 
            this.pnlSensorContainer.AutoSize = true;
            this.pnlSensorContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSensorContainer.Location = new System.Drawing.Point(6, 45);
            this.pnlSensorContainer.Name = "pnlSensorContainer";
            this.pnlSensorContainer.Size = new System.Drawing.Size(0, 0);
            this.pnlSensorContainer.TabIndex = 2;
            // 
            // SensorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlSensorContainer);
            this.Name = "SensorView";
            this.Size = new System.Drawing.Size(1024, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSensorContainer;


    }
}
