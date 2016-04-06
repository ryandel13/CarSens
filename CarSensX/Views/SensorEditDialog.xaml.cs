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
using CarSensX.Sensors;

namespace CarSensX.Views
{
    /// <summary>
    /// Interaktionslogik für SensorEditDialog.xaml
    /// </summary>
    public partial class SensorEditDialog : UserControl
    {
        private Sensor sensor;
        private MainWindow mainWindow;

        public SensorEditDialog(Sensor sensor, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.sensor = sensor;
            InitializeComponent();

            this.LblIdentifier.Content = sensor.getIdentifier();
            this.TxtMaxValue.Text = sensor.getMaximumValue().ToString();
            this.TxtMinValue.Text = sensor.getMinimumValue().ToString();
            this.TxtName.Text = sensor.getName();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.OpenWindow(new SensorConfig(mainWindow));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sensor.setName(TxtName.Text);
            sensor.setMaximumValue(Int32.Parse(TxtMaxValue.Text));
            sensor.setMinimumValue(Int32.Parse(TxtMinValue.Text));
            
            SensorManager.AddSensor(sensor);
            this.mainWindow.OpenWindow(new SensorConfig(mainWindow));
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            SensorManager.RemoveSensor(sensor);
            this.mainWindow.OpenWindow(new SensorConfig(mainWindow));
        }
 
    }
}
