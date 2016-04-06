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
    /// Displays a Single Sensor as a list Item.
    /// Should use the same underlying components as the Sensor Composite.
    /// </summary>
    public partial class SensorListItem : UserControl
    {
        private Sensor sens;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sensor">Sensor</param>
        public SensorListItem(Sensor sensor)
        {
            InitializeComponent();
            this.sens = sensor;
            this.lblSensorName.Text = sensor.getName();
            this.lblSensorId.Text = sensor.getIdentifier();
            this.setType(sensor.getType());
        }

        /// <summary>
        /// Sets the Type.
        /// This is used to determine the correct Icon.
        /// </summary>
        /// <param name="type"></param>
        private void setType(SensorType type)
        {
            switch (type)
            {
                case SensorType.THERMOMETER: pictureBox1.Image = global::CarSens.Properties.Resources.thermometer30;
                    break;

                case SensorType.GPS: pictureBox1.Image = global::CarSens.Properties.Resources.iconGPS;
                    break;

                case SensorType.VOLTMETER: pictureBox1.Image = global::CarSens.Properties.Resources.iconVoltage;
                    break;
            }
        }

        /// <summary>
        /// Getter for the SensorDelegate.
        /// </summary>
        /// <returns></returns>
        internal Sensor getSensor()
        {
            return this.sens;
        }

        /// <summary>
        /// Setter for the Status.
        /// </summary>
        /// <param name="status"></param>
        public void setStatus(SensorStatus status)
        {
            switch (status)
            {
                case SensorStatus.AVAILABLE:
                    {
                        this.BackColor = System.Drawing.Color.LightGray;
                    }
                    break;
                case SensorStatus.CONNECTED:
                    {
                        this.BackColor = System.Drawing.Color.LightGreen;
                    }
                    break;
                case SensorStatus.DISCONNECTED:
                    {
                        this.BackColor = Color.Red;
                    } break;
                default: ; break;
            }
        }
    }
}
