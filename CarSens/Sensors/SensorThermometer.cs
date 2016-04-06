using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using UsbHid;
using UsbHid.USB.Classes.Messaging;
using System.Globalization;

namespace CarSens.Sensors
{
    class SensorThermometer : Sensor
    {
        private UsbHidDevice Device;
        private int deviceId = 1;
        private String id;
        private String name = "Thermometer";
        private String value;
        private float valueStack = 0;
        private int valueStackCount = 0;
        private SensorStatus status = SensorStatus.DISCONNECTED;
        private float floatVal;
        private int maxValue;
        private int minValue;
        private int sensorTimeOut = Program.sensorTimeout;

        public SensorStatus getStatus()
        {
            if (sensorTimeOut > 0)
            {
                sensorTimeOut--;
            }
            if (status == SensorStatus.BUSY)
            {
                if(this.sensorTimeOut == 0) return SensorStatus.FAILURE;
            }
            else if (this.getFloatValue() > this.getMaximumValue())
            {
                this.status = SensorStatus.MAXIMUMEXCEEDED;
            }
            else if (this.getFloatValue() < this.getMinimumValue())
            {
                this.status = SensorStatus.MINIMUMEXCEEDED;
            }
        
            return this.status;
        }

        public SensorThermometer()
        {
            this.value = "N/A";
            Device = new UsbHidDevice(0x16C0, 0x0480);
            Device.OnConnected += DeviceOnConnected;
            Device.OnDisConnected += DeviceOnDisConnected;
            Device.DataReceived += DeviceDataReceived;
            this.status = SensorStatus.BUSY;
            Device.Connect();
        }

        public String getValue()
        {
            if (this.status != SensorStatus.CONNECTED)
            {
                return this.value;
            }
            return this.value;
        }

        public String getAverageValue()
        {
            if (this.status != SensorStatus.CONNECTED)
            {
                return this.value;
            }
            return String.Format(CultureInfo.CurrentUICulture, "{0:0.00}", valueStack );
        }

        public SensorType getType()
        {
            return SensorType.THERMOMETER;
        }

        public string getName()
        {
            return this.name;
        }


        private void DeviceDataReceived(byte[] data)
        {
            AppendText(ByteArrayToString(data));
        }

        private void AppendText(string p)
        {
            if (id != null)
            {
                String[] bytes = p.Split(',');

                String deviceId = "";
                for (int i = 9; i <= 16; i++)
                {
                    deviceId += bytes[i] + ".";
                }
                deviceId = deviceId.Substring(0, deviceId.Length - 1);

                String temp = "";
                int multiplier = 0;


                temp = bytes[5];
                int decAgain = int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
                multiplier = int.Parse(bytes[6], System.Globalization.NumberStyles.HexNumber);
                float tFloat = decAgain + (256 * multiplier);
                tFloat = tFloat / 10;
                temp = tFloat.ToString();
                floatVal = tFloat;
                //String id = bytes[2];
                //int intId = Int32.Parse(id);
                if (deviceId == this.id)
                {
                    this.setValue(tFloat);
                }
            }

        }

        private void setValue(float value)
        {
            

            /*
             * D = (D*n + Xnew) / (n+1)
             * Thanks to Dr. Rainer Bültermann
             */
            valueStack = (valueStack * valueStackCount + value) / (++this.valueStackCount);
            

            this.value = value.ToString();
        }

        private void DeviceOnDisConnected()
        {
            this.value = "N/A";
            status = SensorStatus.DISCONNECTED;
        }

        private void DeviceOnConnected()
        {
            status = SensorStatus.CONNECTED;
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



        public void setName(string name)
        {
            this.name = name;
        }

        public void setDeviceIdentifier(string identifier)
        {
            this.id = identifier;
        }


        public void connect()
        {
            Device.Connect();
        }

        public void disconnect()
        {
            Device.Disconnect();
        }


        public string getIdentifier()
        {
            return this.id;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            name = reader.GetAttribute("name");
            deviceId = Int32.Parse(reader.GetAttribute("deviceId"));
            maxValue = Int32.Parse(reader.GetAttribute("maxValue"));
            minValue = Int32.Parse(reader.GetAttribute("minValue"));
            id = reader.GetAttribute("id");
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("deviceId", deviceId.ToString());
            writer.WriteAttributeString("id", id);
            writer.WriteAttributeString("minValue", minValue.ToString());
            writer.WriteAttributeString("maxValue", maxValue.ToString());
            writer.WriteAttributeString("name", name);
           
        }


        public int getMaximumValue()
        {
            return this.maxValue;
        }

        public int getMinimumValue()
        {
            return this.minValue;
        }

        public void setMaximumValue(int max)
        {
            this.maxValue = max;
        }

        public void setMinimumValue(int min)
        {
            this.minValue = min;
        }


        public float getFloatValue()
        {
            return this.floatVal;
        }


        public SensorUnit getUnit()
        {
            return SensorUnit.DEGREE;
        }

    }
}
