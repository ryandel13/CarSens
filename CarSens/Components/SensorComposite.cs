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
    /// Composite for Displaying a Single Sensor.
    /// </summary>
    public partial class SensorComposite : UserControl
    {

        System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();

        private Sensor sensor;
        private SensorStatus status;
        private Boolean chartEnabled = false;
        private Boolean hasFailed = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sensor"></param>
        public SensorComposite(Sensor sensor)
        {
            InitializeComponent();
            if (sensor != null)
            {
                this.lblName.Text = sensor.getName();
                status = sensor.getStatus();
                update.Enabled = true;
                switch(sensor.getType()) {
                    case SensorType.GPS: pictureBox1.Image = global::CarSens.Properties.Resources.iconGPS;
                        break;
                    case SensorType.VOLTMETER: pictureBox1.Image = global::CarSens.Properties.Resources.iconVoltage30;
                        break;
                    default: pictureBox1.Image = global::CarSens.Properties.Resources.thermometer30;
                        break;
                }
            }
            this.sensor = sensor;
            chart1.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chart1.ChartAreas[0].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.ChartAreas[0].BackColor = Color.Transparent;
            chart1.BackColor = Color.Transparent;
            chart1.Series.Add(series);

            chart1.Hide();
        }

        /// <summary>
        /// Setter for the Name.
        /// Delegates through to the underlying sensor.
        /// </summary>
        /// <param name="name"></param>
        public void setName(String name)
        {
            sensor.setName(name);
        }

        /// <summary>
        /// Update Tick for updating the sensor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Tick(object sender, EventArgs e)
        {
            if (sensor != null)
            {
                Boolean hasFailedC = false;
                SensorStatus cStatus = sensor.getStatus();

                if (cStatus == SensorStatus.BUSY)
                {
                    this.lblAverageValue.Text = "";
                    this.lblValue.Text = "";
                    return;
                }
                else if (cStatus == SensorStatus.CONNECTED || cStatus == SensorStatus.MAXIMUMEXCEEDED || cStatus == SensorStatus.MINIMUMEXCEEDED)
                {
                    this.lblName.Text = sensor.getName();
                    this.lblValue.Text = sensor.getValue() + " " + sensor.getUnit().ToDescription();
                    this.lblAverageValue.Text = sensor.getAverageValue() + " " + sensor.getUnit().ToDescription();
                    this.pictureBox2.Hide();
                }
                else if (cStatus == SensorStatus.FAILURE && !hasFailed)
                {
                    hasFailedC = true;
                }

                series.Color = Color.Red;
                series.Points.Add(new double[] {sensor.getFloatValue()});
                if (series.Points.Count > 100)
                {
                    series.Points.RemoveAt(0);
                }
                if ((cStatus != status) || hasFailedC)
                {
                    status = sensor.getStatus();
                    switch (status)
                    {
                        case SensorStatus.CONNECTED:
                            {
                                new Notification("Info", "Sensor has been reconnected");
                                this.pictureBox2.Hide();
                                this.BackColor = Color.LightGray;
                            }
                            break;
                        case SensorStatus.DISCONNECTED:
                            {
                                new Notification("Error", "Sensor has been disconnected");
                                this.lblValue.Text = "N/A";
                                this.lblAverageValue.Text = "N/A";
                                this.BackColor = Color.DarkRed;
                            }
                            break;
                        case SensorStatus.FAILURE:
                            {
                                
                                this.lblValue.Text = "N/A";
                                this.lblAverageValue.Text = "N/A";
                                this.pictureBox2.Hide();
                                new Notification("Fatal", "Fatal error");
                                this.BackColor = Color.DarkRed;
                                this.hasFailed = true;
                            }
                            break;
                        case SensorStatus.MAXIMUMEXCEEDED:
                            {
                                new Notification("Error", "Sensor " + sensor.getName() + " value over defined maximum");
                                this.BackColor = Color.DarkRed;
                            }
                            break;
                        case SensorStatus.MINIMUMEXCEEDED:
                            {
                                new Notification("Error", "Sensor " + sensor.getName() + " value below defined minimum");
                                this.BackColor = Color.DarkRed;
                            }
                            break;
                        
                        default: ;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// On Click EventHandler.
        /// Will toggle between value and chart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SensorComposite_Load(object sender, EventArgs e)
        {
            if (chartEnabled)
            {
                chart1.Hide();
                this.lblAverage.Show();
            }
            else
            {
                chart1.Show();
                this.lblAverage.Hide();
            }
            chartEnabled = !chartEnabled;
        }
    }
}