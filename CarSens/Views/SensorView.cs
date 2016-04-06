using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CarSens.Components;
using CarSens.Sensors;
using System.Runtime.InteropServices;

namespace CarSens
{
    /// <summary>
    /// View Component for the mainView.
    /// </summary>
    public partial class SensorView : UserControl
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SensorView()
        {
            InitializeComponent();
            update();
        }

        /// <summary>
        /// Updates the current Sensors in viewspace.
        /// </summary>
        internal void update()
        {
            this.pnlSensorContainer.Controls.Clear();
            Sensor[] sens = SensorManager.getAllSensors();
            int i = 0;
            int leftOffset = 0;
            int topOffset = 0;
            int rightOffset = 250;
            foreach (Sensor s in sens)
            {
                SensorComposite sensor = new SensorComposite(s);
                s.connect();
                sensor.Top = topOffset;
                sensor.Left = leftOffset + (i * (sensor.Width + 20));
                this.pnlSensorContainer.Controls.Add(sensor);
                i++;
                if (i * (sensor.Width + 20) > (this.Width - rightOffset))
                {
                    leftOffset = 0;
                    topOffset += sensor.Height + 20;
                    i = 0;
                }

            }
        }
    }
}
