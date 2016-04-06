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
using CarSens.Views;
using System.Xml;

namespace CarSens.Components
{
    /// <summary>
    /// Component to show all Connected and Disconnected Sensors.
    /// </summary>
    public partial class SensorList : UserControl
    {
        DataSet set = new DataSet();
        CarSens.Sensors.SensorList therms;
        CarSens.Sensors.SensorList gpss;
        CarSens.Sensors.SensorList voltslist;

        DataTable gps;
        DataTable termo;
        DataTable volts;

        DataTable assigned;

        BackgroundWorker bWorker = new BackgroundWorker();

        private void backGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
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
            CarSens.Sensors.SensorList sList = (CarSens.Sensors.SensorList)e.Argument;
            sList.initialize(worker, e);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SensorList()
        {
            

            InitializeComponent();
            
            panel1.Hide();
            bWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
            bWorker.ProgressChanged += new ProgressChangedEventHandler(backGroundWorker_ProgressChanged);
            bWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_Completed);
            bWorker.WorkerReportsProgress = true;
            btnClose.Hide();

            DataColumn type = new DataColumn();
            DataColumn name = new DataColumn();
            DataColumn sensor = new DataColumn();
            

            type.DataType = Type.GetType("CarSens.Sensors.SensorType");
            sensor.DataType = Type.GetType("CarSens.Sensors.Sensor");
            

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
            bWorker.RunWorkerAsync(gpss);

            volts = new DataTable();
            stream.Position = 0;
            volts.ReadXmlSchema(stream);
            volts.TableName = "VOLTS";

            voltslist = new SensorListVolt(volts);

            //Sensor[] senses = SensorManager.getAllSensors();
            //assigned = new DataTable();
            //assigned.TableName = "AssignedSensors";
            //assigned.Columns.Add(name2);
            //assigned.Columns.Add(connected);
            //assigned.Columns.Add(sensor2);
            //set.Tables.Add(assigned);
            //assigned.PrimaryKey = new DataColumn[] {name2};
            //foreach (Sensor s in senses)
            //{
            //    assigned.Rows.Add(new Object[] { s.getIdentifier(), false, s });
            //}

            //loadList.Interval = 4000;
            //loadList.Enabled = true;
            this.updateSensorTables();

        }

        private void updateSensorTables()
        {
            DataColumn name2 = new DataColumn();
            DataColumn sensor2 = new DataColumn();
            DataColumn connected = new DataColumn();

            sensor2.DataType = Type.GetType("CarSens.Sensors.Sensor");
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
            int top = 0;
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
                    listItem.setStatus(status);
                    listItem.Location = new System.Drawing.Point(0, top);
                    top += listItem.Size.Height + 10;
                    listItem.MouseClick += new MouseEventHandler(ClickListItem);
                    panel1.Controls.Add(listItem);
                }
            }
                foreach (DataRow row in assigned.Rows)
                {
                    if (!(Boolean)row.ItemArray[1])
                    {
                        Sensor sens = (Sensor)row.ItemArray[2];
                        SensorListItem listItem = new SensorListItem(sens);
                        listItem.Location = new System.Drawing.Point(0, top);
                        top += listItem.Size.Height + 10;
                        panel1.Controls.Add(listItem);
                        listItem.setStatus(SensorStatus.CONNECTED);
                    }
                }

                panel1.Show();
            btnClose.Show();
        }

        private void ClickListItem(object sender, MouseEventArgs e)
        {
            AddSensorWizard addWizard = new AddSensorWizard((SensorListItem)sender);
            btnClose.Hide();
            panel1.Hide();
            this.Controls.Add(addWizard);
            addWizard.VisibleChanged += new EventHandler(LeaveWizard);
        }

        private void LeaveWizard(object sender, EventArgs e)
        {
            AddSensorWizard wiz = (AddSensorWizard)sender;
            if (wiz.Visible == false)
            {
                panel1.Controls.Clear();
                panel1.Show();
                btnClose.Show();
                updateSensorTables();
                this.updateUI();
                this.updateSensorList();
            }
        }

        /// <summary>
        /// Tick for loading the list.
        /// This will mainly give the sensors some time to connect and push the data to the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadList_Tick(object sender, EventArgs e)
        {
            //updateUI();
        }

        private void updateUI()
        {
            set.Tables.Clear();
            set.Tables.Add(therms.getConnectedSensors());
            set.Tables.Add(gpss.getConnectedSensors());
            set.Tables.Add(voltslist.getConnectedSensors());


            foreach (DataTable lists in set.Tables)
            {
                foreach (DataRow row in lists.Rows)
                {
                    try
                    {
                        Sensor sens = (Sensor)row.ItemArray[2];
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            this.updateSensorList();
            progressBar.Hide();
            loadList.Enabled = false;
        }

        /// <summary>
        /// Fills the progressbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fillProgress_Tick(object sender, EventArgs e)
        {
            //if (progressBar.Value < 100)
            //{
            //    progressBar.Value += 1;
            //}
        }

        /// <summary>
        /// Closes the Wizard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
