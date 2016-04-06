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

namespace CarSensX.Components
{
    /// <summary>
    /// Interaktionslogik für SensorListItem.xaml
    /// </summary>
    public partial class SensorListItem : UserControl
    {
        private Sensors.Sensor sensor;

        public SensorListItem()
        {
            InitializeComponent();
        }

        public SensorListItem(Sensors.Sensor sensor)
        {
            InitializeComponent();
            this.sensor = sensor;

            this.LblName.Content = sensor.getName();
            this.LblIdentifier.Content = sensor.getIdentifier();
        }

        public Sensor getSensor()
        {
            return this.sensor;
        }
    }
}
