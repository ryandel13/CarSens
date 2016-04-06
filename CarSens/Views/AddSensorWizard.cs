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

namespace CarSens.Views
{
    /// <summary>
    /// This class should be removed.
    /// Everything should be run via the SensorManagerClass.
    /// </summary>
    [Obsolete]
    public partial class AddSensorWizard : UserControl
    {

        DataSet set = new DataSet();
        int count = 0;
        CarSens.Sensors.SensorList therms;
        CarSens.Sensors.SensorList gpss;
        CarSens.Sensors.SensorList voltslist;

        /// <summary>
        /// Initializer for a new Wizard.
        /// </summary>
        public AddSensorWizard(SensorListItem sli)
        {
            InitializeComponent();

            //SensorListItem sli = (SensorListItem)sender;
            Sensor selSens = sli.getSensor();

            EditSensor editSens = new EditSensor(selSens);
            editSens.Location = new System.Drawing.Point(0, 0);
            this.Controls.Clear();
            this.Controls.Add(editSens);

            editSens.VisibleChanged += new System.EventHandler(this.proceed);

            //DataColumn type = new DataColumn();
            //DataColumn name = new DataColumn();
            //DataColumn sensor = new DataColumn();

            //type.DataType = Type.GetType("CarSens.Sensors.SensorType");
            //sensor.DataType = Type.GetType("CarSens.Sensors.Sensor");

            //DataTable termo = new DataTable("Thermo");
            //termo.Columns.Add(type);
            //termo.Columns.Add(name);
            //termo.Columns.Add(sensor);

            //therms = new SensorListThermo(termo);

            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //termo.WriteXmlSchema(stream, true);


            //DataTable gps = new DataTable();
            //stream.Position = 0;
            //gps.ReadXmlSchema(stream);
            //gps.TableName = "GPS";

            //gpss = new SensorListGPS(gps);

            //DataTable volts = new DataTable();
            //stream.Position = 0;
            //volts.ReadXmlSchema(stream);
            //volts.TableName = "VOLTS";

            //voltslist = new SensorListVolt(volts);

            //timer1.Interval = 40;
            //timer1.Enabled = true;
            //updateSensors.Interval = 4000;
            //updateSensors.Enabled = true;
        }

        /// <summary>
        /// Timer for loading the Sensors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            set.Tables.Clear(); 
            set.Tables.Add(therms.getConnectedSensors());
            set.Tables.Add(gpss.getConnectedSensors());
            set.Tables.Add(voltslist.getConnectedSensors());

            foreach(DataTable lists in set.Tables) {
                foreach (DataRow row in lists.Rows)
                {
                    Sensor sens = (Sensor)row.ItemArray[2];
                }
            }

            lblLoadingSensors.Hide();
            progressBar.Hide();
            updateSensors.Enabled = false;
            updateSensorList();
        }

        /// <summary>
        /// Updates the Sensor list.
        /// </summary>
        private void updateSensorList()
        {
            int top = 0;
            int newItems = 0;

            foreach (DataTable lists in set.Tables)
            {
                foreach (DataRow row in lists.Rows)
                {
                    Sensor[] senses = SensorManager.getAllSensors();
                    Sensor sens = (Sensor)row.ItemArray[2];
                    Boolean inList = false;
                    foreach (Sensor sin in senses)
                    {
                        if (sin.getIdentifier().Equals(sens.getIdentifier()))
                        {
                            inList = true;
                        }
                    }
                    if (!inList)
                    {
                        SensorListItem listItem = new SensorListItem(sens);

                        listItem.Click += new System.EventHandler(this.startWizard);

                        listItem.Location = new System.Drawing.Point(0, top);
                        top += listItem.Size.Height + 10;
                        this.panel2.Controls.Add(listItem);

                        newItems++;
                    }
                }
            }

            if (newItems == 0)
            {
                Label nothing = new Label();
                nothing.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                nothing.ForeColor = Color.White;
                nothing.Text = "No unassigned Sensors found";
                nothing.Width = 250;
                nothing.Location = new System.Drawing.Point(0, top);
                this.Controls.Add(nothing);
            }
            
        }

        /// <summary>
        /// Starts the Wizard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startWizard(object sender, EventArgs e)
        {
            if (sender.GetType() == Type.GetType("CarSens.Components.SensorListItem"))
            {
                SensorListItem sli = (SensorListItem)sender;
                Sensor selSens = sli.getSensor();
                
                EditSensor editSens = new EditSensor(selSens);
                editSens.Location = new System.Drawing.Point(0, 0);
                this.Controls.Clear();
                this.Controls.Add(editSens);

                editSens.VisibleChanged += new System.EventHandler(this.proceed);
            }
        }

        /// <summary>
        /// This will hide the Wizard.
        /// Hide will trigger the Sensor adding by implementation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proceed(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Click on the cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// tick for the progress bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (progressBar.Value < 100)
            {
                progressBar.Value += 1;
            }
        }
    }
}
