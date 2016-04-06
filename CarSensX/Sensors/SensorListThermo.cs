using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSensX.Components;
using System.Data;
using System.Threading;
using UsbHid;
using UsbHid.USB.Classes.Messaging;

namespace CarSensX.Sensors
{
    class SensorListThermo : SensorList
    {

        UsbHidDevice Device;
        DataTable set;
        int count = 0;
        Boolean updated = false;
        private Boolean connected = false;
        private Boolean finished = false;
        String[] ids;

        DataTable SensorList.getConnectedSensors()
        {
            return set;
        }


        public SensorListThermo(DataTable table)
        {
            Device = new UsbHidDevice(0x16C0, 0x0480);
            Device.DataReceived += DeviceDataReceived;
            connected = Device.Connect();

            set = table;
        }

        private void DeviceDataReceived(byte[] data)
        {
            AppendText(ByteArrayToString(data));
        }

        private void AppendText(string p)
        {
            Console.WriteLine(p);
            String[] bytes = p.Split(',');

            if(count == 0) {
                count = int.Parse(bytes[1], System.Globalization.NumberStyles.HexNumber);
                ids = new String[count];
            }
      
            int currentSensor = int.Parse(bytes[2], System.Globalization.NumberStyles.HexNumber);

            String deviceId = "";
            for (int i = 9; i <= 16; i++)
            {
                deviceId += bytes[i] + ".";
            }
            deviceId = deviceId.Substring(0, deviceId.Length - 1);
            if(!ids.Contains(deviceId)) 
            {
                  ids[currentSensor-1] = deviceId;
                  Sensor newSens = new SensorThermometer();
                  newSens.setDeviceIdentifier(deviceId);
                  newSens.disconnect();
                  DataRow row = set.Rows.Add();
                    row.ItemArray = new Object[] {SensorType.THERMOMETER, deviceId, newSens};
                    Console.WriteLine("Added " + deviceId);
            }
          
            if(set.Rows.Count == count) {
                Device.Disconnect();
                this.updated = true;
                Console.WriteLine("Disconnected");
            }
        }

        private static string ByteArrayToString(ICollection<byte> input)
        {
            var result = string.Empty;

            if (input != null && input.Count > 0)
            {
                var isFirst = true;
                foreach (var b in input)
                {
                    result += isFirst ? string.Empty : ",";
                    result += b.ToString("X2");
                    isFirst = false;
                }
            }
            return result;
        }

        public void initialize(System.ComponentModel.BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e)
        {
            FindThermSensors(worker);
        }

        private void FindThermSensors(System.ComponentModel.BackgroundWorker worker)
        {
            if (!connected)
            {
                this.finished = true;
                worker.ReportProgress(100);
                return;
            }
            int i = 0;
            while (!finished)
            {
                worker.ReportProgress(i++);
                if (ids != null)
                {
                    if (set.Rows.Count == ids.Length)
                    {
                        break;
                    }
                }
                if (i > 1000)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            this.finished = true;
            worker.ReportProgress(100);
        }

        public bool isFinished()
        {
            return finished;
        }
    }
}
