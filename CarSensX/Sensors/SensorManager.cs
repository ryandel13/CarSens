using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Xml;
using CarSensX.Components;

namespace CarSensX.Sensors
{
    class SensorManager
    {

        private static DataSet sensorSet;
        private static DataTable table;
        private static bool initialized = false;

        public SensorManager()
        {

        }


        private static void initialize()
        {
            global::CarSensX.Properties.Settings.Default.Reload();
            if (initialized)
            {
                return;
            }
            if (global::CarSensX.Properties.Settings.Default.SensorSetXML.Equals(""))
            {
                sensorSet = new DataSet();
                table = new DataTable("SensorList");
                DataColumn colIds = new DataColumn("SensorsIds", Type.GetType("System.String"));
                DataColumn col = new DataColumn("Sensors", Type.GetType("CarSensX.Sensors.Sensor"));
                sensorSet.Tables.Add(table);
                table.Columns.Add(colIds);
                table.PrimaryKey = new DataColumn[] { colIds };
                table.Columns.Add(col);
                initialized = true;
            }
            if (!global::CarSensX.Properties.Settings.Default.SensorSetXML.Equals(""))
            {
                try
                {
                    StringReader reader = new StringReader(global::CarSensX.Properties.Settings.Default.SensorSetXML);
                    sensorSet = new DataSet();
                    table = new DataTable("SensorList");
                    DataColumn colIds = new DataColumn("SensorsIds", Type.GetType("System.String"));
                    DataColumn col = new DataColumn("Sensors", Type.GetType("CarSensX.Sensors.Sensor"));
                    sensorSet.Tables.Add(table);
                    table.Columns.Add(colIds);
                   
                    table.PrimaryKey = new DataColumn[] {colIds};
                    table.Columns.Add(col);
                    sensorSet.ReadXml(reader);
                    table = sensorSet.Tables[0];
                    initialized = true;
                }
                catch (XmlException ex)
                {
                    new Notification("Fatal", "Error on ínitiliazing Application");
                }
            }
            
        }

        public static void AddSensor(Sensor sensor)
        {
            SensorManager.initialize();
            DataRow existing = table.Rows.Find(sensor.getIdentifier());
            if(existing != null) {
                table.Rows.Remove(existing);
            }
            table.Rows.Add(new Object[] { sensor.getIdentifier(), sensor });

            persist();
        }

        public static void RemoveSensor(Sensor sensor)
        {
            SensorManager.initialize();
            DataRow sens = table.Rows.Find(sensor.getIdentifier());
            table.Rows.Remove(sens);
            //foreach (DataRow row in SensorManager.table.Rows)
            //{
            //    String sens = (String)row.ItemArray[0];
            //    if (sens.Equals(sensor.getIdentifier()))
            //    {
            //        sensor.disconnect();
            //        SensorManager.table.Rows.Remove(row);
            //    }
            //}

            persist();
        }

        public static Sensor[] getAllSensors()
        {
            SensorManager.initialize();
            

            int mocks = global::CarSensX.Properties.Settings.Default.MockSensors;
            Sensor[] sens = new Sensor[table.Rows.Count + mocks];
            int i = 0;
            foreach (DataRow row in SensorManager.table.Rows)
            {
                sens[i] = (Sensor)row.ItemArray[1];
                i++;
            }
            for (i = i; i < sens.Length; i++)
            {
                sens[i] = new SensorMock();
                sens[i].setName("MockSensor" + i);
            }
            return sens;
        }

        private static void persist()
        {
           // if (table.Rows.Count > 0)
            {
                StringWriter writer = new StringWriter();
                sensorSet.WriteXml(writer);
                global::CarSensX.Properties.Settings.Default.SensorSetXML = writer.ToString();
                global::CarSensX.Properties.Settings.Default.Save();
            }
        }

        private static void disconnectAll() {
            Sensor[] sens = SensorManager.getAllSensors();
            foreach (Sensor sen in sens)
            {
                sen.disconnect();
            }
        }

        public static void clear()
        {
            SensorManager.disconnectAll();
            table.Rows.Clear();
            global::CarSensX.Properties.Settings.Default.SensorSetXML = "";
            global::CarSensX.Properties.Settings.Default.Save();
        }
    }
}
