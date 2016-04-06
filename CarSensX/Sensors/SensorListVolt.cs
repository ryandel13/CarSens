using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using USB_DeviceLib;

namespace CarSensX.Sensors
{

    class SensorListVolt : SensorList
    {
        DataTable set;
        private bool finished = true;

        public static bool connected = false;
        public static USBM VOLTDEV;

        public System.Data.DataTable getConnectedSensors()
        {
            return set;
        }

        public SensorListVolt(DataTable table)
        {
            set = table;

            if (SensorListVolt.VOLTDEV == null)
            {
                SensorListVolt.INIT();
            }

            //if (SensorListVolt.VOLTDEV. == 0)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        DataRow row = set.Rows.Add();
            //        Sensor newSens = new SensorVolt();
            //        String id = "VOLT:0:" + i;
            //        newSens.setDeviceIdentifier(id);
            //        row.ItemArray = new Object[] { SensorType.GPS, id, newSens };
            //    }
            //}

        }

        internal static void INIT()
        {
            if (!connected)
            {
                SensorListVolt.VOLTDEV = new USBM();
                if (SensorListVolt.VOLTDEV.OpenDevice() == 0)
                {
                    SensorListVolt.connected = true;
                }
            }
        }

        public void initialize(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
        {
            INIT();
        }

        public bool isFinished()
        {
            return true;
        }
    }
}
