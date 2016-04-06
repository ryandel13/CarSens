using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaktionslogik für SensorConfig.xaml
    /// </summary>
    public partial class SensorConfig : UserControl
    {
        private MainWindow mainWindow;

        DataSet set = new DataSet();
        CarSensX.Sensors.SensorList therms;
        CarSensX.Sensors.SensorList gpss;
        CarSensX.Sensors.SensorList voltslist;

        DataTable gps;
        DataTable termo;
        DataTable volts;

        DataTable assigned;

        BackgroundWorker gpsFinderWorker = new BackgroundWorker();
        BackgroundWorker thermFinderWorker = new BackgroundWorker();

        private void backGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            if (progress > 100)
            {
                progress = 100;
            }
            progressBar.Value = progress;
        }

        private void backGroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (therms.isFinished() && gpss.isFinished() && voltslist.isFinished())
            {
               this.updateUI();
            }
        }

        private void backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            CarSensX.Sensors.SensorList sList = (CarSensX.Sensors.SensorList)e.Argument;
            sList.initialize(worker, e);
        }

        public SensorConfig(MainWindow mainWindow)
        {
            gpsFinderWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
            gpsFinderWorker.ProgressChanged += new ProgressChangedEventHandler(backGroundWorker_ProgressChanged);
            gpsFinderWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_Completed);
            gpsFinderWorker.WorkerReportsProgress = true;

            thermFinderWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
            thermFinderWorker.ProgressChanged += new ProgressChangedEventHandler(backGroundWorker_ProgressChanged);
            thermFinderWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_Completed);
            thermFinderWorker.WorkerReportsProgress = true;

            this.mainWindow = mainWindow;
            InitializeComponent();
            GenerateStructure();
            //Sensor[] sensors = SensorManager.getAllSensors();
            //foreach (Sensor sensor in sensors)
            //{
            //    SensorListItem slItem = new SensorListItem(sensor);
            //    this.Panel.Children.Add(slItem);

            //    slItem.MouseLeftButtonUp += new MouseButtonEventHandler(SensorListItemCLick);
            //}
        }

        private void SensorListItemCLick(object sender, MouseButtonEventArgs e)
        {
            SensorListItem slItem = (SensorListItem)sender;
            mainWindow.OpenWindow(new SensorEditDialog(slItem.getSensor(), this.mainWindow));
        }

        private void GenerateStructure()
        {
            DataColumn type = new DataColumn();
            DataColumn name = new DataColumn();
            DataColumn sensor = new DataColumn();


            type.DataType = Type.GetType("CarSensX.Sensors.SensorType");
            sensor.DataType = Type.GetType("CarSensX.Sensors.Sensor");


            termo = new DataTable("Thermo");
            termo.Columns.Add(type);
            termo.Columns.Add(name);
            termo.Columns.Add(sensor);

            therms = new SensorListThermo(termo);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            termo.WriteXmlSchema(stream, true);


            gps = new DataTable();
            stream.Position = 0;
            gps.ReadXmlSchema(stream);
            gps.TableName = "GPS";

            gpss = new SensorListGPS(gps);
            

            volts = new DataTable();
            stream.Position = 0;
            volts.ReadXmlSchema(stream);
            volts.TableName = "VOLTS";

            voltslist = new SensorListVolt(volts);

            this.updateSensorTables();
            thermFinderWorker.RunWorkerAsync(therms);
            gpsFinderWorker.RunWorkerAsync(gpss);
        }

        private void updateSensorTables()
        {
            DataColumn name2 = new DataColumn();
            DataColumn sensor2 = new DataColumn();
            DataColumn connected = new DataColumn();

            sensor2.DataType = Type.GetType("CarSensX.Sensors.Sensor");
            connected.DataType = Type.GetType("System.Boolean");

            Sensor[] senses = SensorManager.getAllSensors();
            assigned = new DataTable();
            assigned.TableName = "AssignedSensors";
            assigned.Columns.Add(name2);
            assigned.Columns.Add(connected);
            assigned.Columns.Add(sensor2);
            set.Tables.Add(assigned);
            assigned.PrimaryKey = new DataColumn[] { name2 };
            foreach (Sensor s in senses)
            {
                assigned.Rows.Add(new Object[] { s.getIdentifier(), false, s });
            }
        }

        /// <summary>
        /// Updates the List.
        /// </summary>
        private void updateSensorList()
        {
            double top = 0;
            foreach (DataTable table in set.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    Sensor[] senses = SensorManager.getAllSensors();
                    Sensor sens = (Sensor)row.ItemArray[2];
                    Boolean inList = false;
                    SensorStatus status = SensorStatus.AVAILABLE;



                    //Check if Sensor is in CurrentlyConnectedList
                    foreach (Sensor sin in senses)
                    {


                        if (sin.getIdentifier().Equals(sens.getIdentifier()))
                        {
                            inList = true;
                            break;
                        }
                    }
                    if (!inList)
                    {
                        status = SensorStatus.AVAILABLE;
                    }
                    else
                    {

                        DataRow found = assigned.Rows.Find(sens.getIdentifier());
                        object[] array = found.ItemArray;
                        array[1] = true;
                        found.ItemArray = array;
                        sens = (Sensor)array[2];
                        status = SensorStatus.CONNECTED;
                    }
                    SensorListItem listItem = new SensorListItem(sens);
                    //listItem.setStatus(status);
                    //listItem.Location = new System.Drawing.Point(0, top);
                    top += listItem.Height + 10;
                    listItem.MouseLeftButtonUp += new MouseButtonEventHandler(SensorListItemCLick);
                    //listItem.MouseClick += new MouseEventHandler(ClickListItem);
                    this.Panel.Children.Add(listItem);
                }
            }
            foreach (DataRow row in assigned.Rows)
            {
                if (!(Boolean)row.ItemArray[1])
                {
                    Sensor sens = (Sensor)row.ItemArray[2];
                    SensorListItem listItem = new SensorListItem(sens);
                    listItem.MouseLeftButtonUp += new MouseButtonEventHandler(SensorListItemCLick);
                    //listItem.Location = new System.Drawing.Point(0, top);
                    top += listItem.Height + 10;
                    Panel.Children.Add(listItem);
                    //listItem.setStatus(SensorStatus.CONNECTED);
                }
            }

            if (Panel.Children.Count == 0)
            {
                SensorMock mock = new SensorMock();
                mock.setName("No Sensors found");
                mock.setDeviceIdentifier("");
                Panel.Children.Add(new SensorListItem(mock));
            }
        }

        private void updateUI()
        {
            set.Tables.Clear();
            set.Tables.Add(therms.getConnectedSensors());
            set.Tables.Add(gpss.getConnectedSensors());
            set.Tables.Add(voltslist.getConnectedSensors());


            //foreach (DataTable lists in set.Tables)
            //{
            //    foreach (DataRow row in lists.Rows)
            //    {
            //        try
            //        {
            //            Sensor sens = (Sensor)row.ItemArray[2];
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    }
            //}
            this.updateSensorList();
            progressBar.Visibility = Visibility.Hidden;
        }
    }
}
