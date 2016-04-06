using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO.Ports;
namespace CarSensX.Sensors
{
    class SensorListGPS : SensorList
    {
        DataTable set;
        private bool finished = false;

        public System.Data.DataTable getConnectedSensors()
        {
            return set;
        }

        public SensorListGPS(DataTable table)
        {
            set = table;
        }

        public void initialize(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e) {
            FindCOMSensors(worker);
        }

        private void FindCOMSensors(System.ComponentModel.BackgroundWorker worker)
        {
            String[] ports = SerialPort.GetPortNames();
            int i = 0;
            foreach (String port in ports)
            {
                i++;
                Sensor newSens = new SensorGPS();
                String id = port + ",4800,8";
                newSens.setDeviceIdentifier(id);
                newSens.connect();
                System.Threading.Thread.Sleep(1500);
                if (newSens.getStatus() != SensorStatus.FAILURE)
                {
                    DataRow row = set.Rows.Add();
                    row.ItemArray = new Object[] { SensorType.GPS, id, newSens };
                }
                newSens.disconnect();
                decimal y = (decimal)i / (decimal)ports.Length;
                decimal x = Math.Round(y * 100);
                worker.ReportProgress((int)x);
            }
            this.finished = true;
            worker.ReportProgress(100);
        }

        public bool isFinished()
        {
            return this.finished;
        }
    }

   
}
