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

namespace CarSens.Components
{
    /// <summary>
    /// EditComponent to be shown in the Wizard.
    /// </summary>
    public partial class EditSensor : UserControl
    {

        Sensor delSensor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sensor"></param>
        public EditSensor(Sensor sensor)
        {
            InitializeComponent();

            txtMinValue.Text = sensor.getMinimumValue().ToString();
            txtMaxValue.Text = sensor.getMaximumValue().ToString(); ;
            txtName.Text = sensor.getName(); ;
            txtType.Text = sensor.getType().ToString();
            txtIdentifier.Text = sensor.getIdentifier();
            delSensor = sensor;
        }

        /// <summary>
        /// Saves the Changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            delSensor.setName(txtName.Text);
            try
            {
                delSensor.setMaximumValue(Int32.Parse(txtMaxValue.Text));
                delSensor.setMinimumValue(Int32.Parse(txtMinValue.Text));
                SensorManager.AddSensor(delSensor);
                this.Hide();
            }
            catch (FormatException ice)
            {
                new Notification("Error", "Maximum or Minimum is not a valid number");
            }
        }

        /// <summary>
        /// Cancels wizard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            SensorManager.RemoveSensor(this.delSensor);
            this.Hide();
        }
    }
}
