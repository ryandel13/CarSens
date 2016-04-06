using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarSens.Sensors;

namespace CarSens.Views
{
    /// <summary>
    /// View Component for the Edit Window
    /// </summary>
    [Obsolete]
    public partial class EditView : UserControl
    {
        
        private Form main;
        public EditView(Form main)
        {
            this.main = main;
            InitializeComponent();
        }

        /// <summary>
        /// Opens dialogue to replace the background.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "JPEGs (*.jpg) | *.jpg";
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Replaces the background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            String fileName = openFileDialog1.FileName;
            Image img = Image.FromFile(fileName);
            global::CarSens.Properties.Settings.Default.BackgroundImage = fileName;
            global::CarSens.Properties.Settings.Default.Save();
            main.BackgroundImage = img;
        }

        /// <summary>
        /// Opens the Add Sensor Wizard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSensor_Click(object sender, EventArgs e)
        {
            //AddSensorWizard wiz = new AddSensorWizard();
            //int posLeft = (Program.appWidth / 2) - (wiz.Width / 2);
            //int posTop = (Program.appHeight / 2) - (wiz.Height / 2);
            //wiz.Location = new System.Drawing.Point(posLeft, posTop);
            //wiz.BringToFront();
            //this.panel2.Controls.Add(wiz);
            //this.panelControls.Hide();
            //wiz.VisibleChanged += new System.EventHandler(hideAction);
        }

        /// <summary>
        /// Clears the list of connected Sensors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            SensorManager.clear();
        }

        /// <summary>
        /// hides the left Controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hideAction(Object sender, EventArgs e)
        {
            this.panelControls.Show();
        }

        /// <summary>
        /// Opens the SensorList.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sensorList_Click(object sender, EventArgs e)
        {
            CarSens.Components.SensorList wiz = new CarSens.Components.SensorList();
            int posLeft = (Program.appWidth / 2) - (wiz.Width / 2);
            int posTop = (Program.appHeight / 2) - (wiz.Height / 2);
            wiz.Location = new System.Drawing.Point(posLeft, posTop);
            wiz.BringToFront();
            this.panel2.Controls.Add(wiz);
            this.panelControls.Hide();
            wiz.VisibleChanged += new System.EventHandler(hideAction);
        }
    }
}
