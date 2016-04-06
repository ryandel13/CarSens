using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CarSensX.Sensors
{
    class SensorVolt : Sensor
    {

        private float value = 0f;
        private String name = "Voltmeter";
        private String deviceId;
        private int stackNum = 0;
        private SensorStatus status = SensorStatus.DISCONNECTED;
        private float valueStack = 0;
        private int valueStackCount = 0;
        private int sensorTimeOut = MainWindow.sensorTimeout;

        private int maxValue;
        private int minValue;

        public SensorVolt()
        {
            this.status = SensorStatus.BUSY;
            if (SensorListVolt.VOLTDEV == null)
            {
                SensorListVolt.INIT();
            }
        }

        public string getValue()
        {
            return String.Format(CultureInfo.CurrentUICulture, "{0:0.00}", this.getFloatValue());
        }

        public string getAverageValue()
        {
            return String.Format(CultureInfo.CurrentUICulture, "{0:0.00}", this.valueStack); ;
        }

        public SensorType getType()
        {
            return SensorType.VOLTMETER;
        }

        public string getName()
        {
            return this.name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setDeviceIdentifier(string identifier)
        {
            this.deviceId = identifier;
            String[] ids = identifier.Split(':');
            this.stackNum = Int32.Parse(ids[2]);
        }

        public void connect()
        {
            SensorListVolt.INIT();
            if (SensorListVolt.connected)
            {
                this.status = SensorStatus.CONNECTED;
            }
        }

        public void disconnect()
        {
           
        }

        public SensorStatus getStatus()
        {
            if (sensorTimeOut > 0)
            {
                sensorTimeOut--;
            }
            if (this.status == SensorStatus.BUSY)
            {
                this.status = SensorStatus.FAILURE;
            }
            return this.status;
        }

        public string getIdentifier()
        {
            return this.deviceId;
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
            if (SensorListVolt.VOLTDEV.GetMeasuredValue() != 0)
            {
                if (this.status == SensorStatus.CONNECTED)
                {
                    this.status = SensorStatus.DISCONNECTED;
                }
                else
                {
                    this.status = SensorStatus.FAILURE;
                }
            }
            else
            {
                this.status = SensorStatus.CONNECTED;
            }
            float val = 0;
            switch (this.stackNum)
            {
                case 0: val = (float)SensorListVolt.VOLTDEV.ADCValue1; break;
                case 1: val = (float)SensorListVolt.VOLTDEV.ADCValue2; break;
                case 2: val = (float)SensorListVolt.VOLTDEV.ADCValue3; break;
                case 3: val = (float)SensorListVolt.VOLTDEV.ADCValue4; break;
                default: val = 0; break;
            }
            setValue(val);

            return this.value;
        }

        private void setValue(float value)
        {
            if (this.status == SensorStatus.CONNECTED)
            {
                /*
                  * D = (D*n + Xnew) / (n+1)
                  * Thanks to Dr. Rainer Bültermann
                  */
                valueStack = (valueStack * valueStackCount + value) / (++this.valueStackCount);

            }
            this.value = value;
        }

        public SensorUnit getUnit()
        {
            return SensorUnit.VOLT;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            name = reader.GetAttribute("name");
            deviceId = reader.GetAttribute("deviceId");
            maxValue = Int32.Parse(reader.GetAttribute("maxValue"));
            minValue = Int32.Parse(reader.GetAttribute("minValue"));
            
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("deviceId", deviceId.ToString());
            writer.WriteAttributeString("minValue", minValue.ToString());
            writer.WriteAttributeString("maxValue", maxValue.ToString());
            writer.WriteAttributeString("name", name);

        }
    }
}
