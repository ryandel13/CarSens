using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CarSensX.Sensors;

namespace CarSensX.Components
{
    /// <summary>
    /// Interaktionslogik für SensorComposite.xaml
    /// </summary>
    public partial class SensorComposite : UserControl
    {
        private Sensor sensor;
        private SensorStatus status;
        private Boolean chartEnabled = false;
        private Boolean hasFailed = false;
        private DispatcherTimer myDispatcherTimer;

        public SensorComposite(Sensor sensor)
        {
            InitializeComponent();

            myDispatcherTimer = new DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 100 Milliseconds 
            myDispatcherTimer.Tick += new EventHandler(update_Tick);
            

            if (sensor != null)
            {
                this.lblName.Content = sensor.getName();
                status = sensor.getStatus();
                //update.Enabled = true;
                switch (sensor.getType())
                {
                    case SensorType.GPS:
                        imgIcon.Source = Static.getBitmapSourceFromBitmap(global::CarSensX.Properties.Resources.iconGPS);
                        break;
                    case SensorType.VOLTMETER: imgIcon.Source = Static.getBitmapSourceFromBitmap(global::CarSensX.Properties.Resources.iconVoltage30);
                        break;
                    default: imgIcon.Source = Static.getBitmapSourceFromBitmap(global::CarSensX.Properties.Resources.thermometer30);
                        break;
                }
            }
            this.sensor = sensor;
            myDispatcherTimer.Start();
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
                    this.lblAvgValue.Content = "";
                    this.lblValue.Content = "";
                    return;
                }
                else if (cStatus == SensorStatus.CONNECTED || cStatus == SensorStatus.MAXIMUMEXCEEDED || cStatus == SensorStatus.MINIMUMEXCEEDED)
                {
                    this.lblName.Content = sensor.getName();
                    this.lblValue.Content = sensor.getValue() + " " + sensor.getUnit().ToDescription();
                    this.lblAvgValue.Content = sensor.getAverageValue() + " " + sensor.getUnit().ToDescription();
                    //this.pictureBox2.Hide();
                }
                else if (cStatus == SensorStatus.FAILURE && !hasFailed)
                {
                    hasFailedC = true;
                }

                //series.Color = Color.Red;
                //series.Points.Add(new double[] { sensor.getFloatValue() });
                //if (series.Points.Count > 100)
                //{
                //    series.Points.RemoveAt(0);
                //}
                if ((cStatus != status) || hasFailedC)
                {
                    status = sensor.getStatus();
                    switch (status)
                    {
                        case SensorStatus.CONNECTED:
                            {
                                new Notification("Info", "Sensor has been reconnected");
                                //this.pictureBox2.Hide();
                                //this.BackColor = Color.LightGray;
                            }
                            break;
                        case SensorStatus.DISCONNECTED:
                            {
                                new Notification("Error", "Sensor has been disconnected");
                                this.lblValue.Content = "N/A";
                                this.lblAvgValue.Content = "N/A";
                                //this.BackColor = Color.DarkRed;
                            }
                            break;
                        case SensorStatus.FAILURE:
                            {

                                this.lblValue.Content = "N/A";
                                this.lblAvgValue.Content = "N/A";
                                //this.pictureBox2.Hide();
                                //new Notification("Fatal", "Fatal error");
                                //this.BackColor = Color.DarkRed;
                                this.hasFailed = true;
                            }
                            break;
                        case SensorStatus.MAXIMUMEXCEEDED:
                            {
                                new Notification("Error", "Sensor " + sensor.getName() + " value over defined maximum");
                                //this.BackColor = Color.DarkRed;
                            }
                            break;
                        case SensorStatus.MINIMUMEXCEEDED:
                            {
                                new Notification("Error", "Sensor " + sensor.getName() + " value below defined minimum");
                                //this.BackColor = Color.DarkRed;
                            }
                            break;

                        default: ;
                            break;
                    }
                }
            }
        }
    }
}
