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
using CarSensX.Components;
using CarSensX.Sensors;

namespace CarSensX.Views
{
    /// <summary>
    /// Interaktionslogik für SensorView.xaml
    /// </summary>
    public partial class SensorView : UserControl
    {
        public SensorView()
        {
            InitializeComponent();

            this.UpdateUI();
        }

        public void UpdateUI()
        {
            this.Panel.Children.Clear();
            this.Panel.RowDefinitions.Clear();

            Sensor[] sensors = SensorManager.getAllSensors();
            int col = 0;
            int row = 0;
            foreach (Sensor s in sensors)
            {
                SensorComposite sComposite = new SensorComposite(s);
                this.Panel.Children.Add(sComposite);
                Grid.SetColumn(sComposite, col);
                Grid.SetRow(sComposite, row);
                col++;
                if (col == Panel.ColumnDefinitions.Count)
                {
                    RowDefinition rDef = new RowDefinition();
                    this.Panel.RowDefinitions.Add(new RowDefinition());
                    this.Panel.Height = (sComposite.Height) * row;
                    row++;
                    col = 0;
                }
            }
        }
    }
}
