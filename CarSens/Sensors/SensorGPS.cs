using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using NMEA;
using System.IO.Ports;
using System.Globalization;
using CarSens.Components;

namespace CarSens.Sensors
{
    class SensorGPS : Sensor
    {
        private SerialPort port;
        private float value = 0f;
        private String name = "GPS - Speed";
        private String deviceId;
        private float avg;
        private SensorStatus status = SensorStatus.DISCONNECTED;
        private String inString = "";
        private Boolean fix = false;
        private float valueStack = 0;
        private int valueStackCount = 0;
        private int sensorTimeOut = Program.sensorTimeout;

        public SensorGPS()
        {
            if (deviceId != null)
            {
                String[] deviceIdbuff = this.deviceId.Split(',');
                port = new SerialPort(deviceIdbuff[0], Int32.Parse(deviceIdbuff[1]), Parity.None, Int32.Parse(deviceIdbuff[2]), StopBits.One);
                port.DataReceived += DataReceivedEvent;
            }
        }

        public string getValue()
        {
            return String.Format(CultureInfo.CurrentUICulture, "{0:0}", this.value);
        }

        public string getAverageValue()
        {
            return String.Format(CultureInfo.CurrentUICulture, "{0:0.00}", this.valueStack); ;
        }

        private void setValue(float value)
        {
            /*
              * D = (D*n + Xnew) / (n+1)
              * Thanks to Dr. Rainer Bültermann
              */
            valueStack = (valueStack * valueStackCount + value) / (++this.valueStackCount);


            this.value = value;
        }

        public SensorType getType()
        {
            return SensorType.GPS;
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

            String[] deviceIdbuff = this.deviceId.Split(',');
            port = new SerialPort(deviceIdbuff[0], Int32.Parse(deviceIdbuff[1]), Parity.None, Int32.Parse(deviceIdbuff[2]), StopBits.One);
            port.DataReceived += DataReceivedEvent;
        }

        public void connect()
        {
            try
            {
                if (port != null && !port.IsOpen)
                {
                    port.Open();
                    this.status = SensorStatus.BUSY;
                    //TODO: Check for sufficent NMEA Device
                }
            }
            catch (System.IO.IOException ioex)
            {
                new Notification("Error on GPS", "Unable to connect Port " + this.deviceId);
                this.status = SensorStatus.FAILURE;
            }
        }

        public void disconnect()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc));
        }

        private void ThreadProc(Object stateInfo)
        {
            // Attempt to close serial port
            if (this.port.IsOpen == true)
            {
                this.port.Close();
                this.status = SensorStatus.DISCONNECTED;
            }
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
            return this.value;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            name = reader.GetAttribute("name");
            maxValue = Int32.Parse(reader.GetAttribute("maxValue"));
            minValue = Int32.Parse(reader.GetAttribute("minValue"));
            deviceId = reader.GetAttribute("id");

            String[] deviceIdbuff = this.deviceId.Split(',');
            port = new SerialPort(deviceIdbuff[0], Int32.Parse(deviceIdbuff[1]), Parity.None, Int32.Parse(deviceIdbuff[2]), StopBits.One);
            port.DataReceived += DataReceivedEvent;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("id", deviceId);
            writer.WriteAttributeString("minValue", minValue.ToString());
            writer.WriteAttributeString("maxValue", maxValue.ToString());
            writer.WriteAttributeString("name", name);

        }

        private void DataReceivedEvent(object sender, SerialDataReceivedEventArgs e)
        {
            String inBuff;
            
            if (port.IsOpen)
            {
                inBuff = port.ReadExisting();
                if (inBuff != null)
                {
                    if (inBuff.StartsWith("$"))
                    {
                        inString = inBuff;
                    }
                    else
                    {
                        inString += inBuff;
                    }

                    if((inString.StartsWith("$GPRMC") || inString.StartsWith("$GPGGA")) && inString.EndsWith("\r\n")) {
                        try
                        {
                            NMEASentence sentence = NMEAParser.Parse(inString);
                            String text = "";
                            foreach (Object obj in sentence.parameters)
                            {
                                String outString;
                                if (obj == null)
                                {
                                    outString = "null";
                                }
                                else
                                {
                                    outString = obj.ToString();
                                }
                                text += outString + System.Environment.NewLine;

                            }

                            if (inString.StartsWith("$GPGGA"))
                            {
                                if (sentence.parameters[5].Equals("Fix not availible"))
                                {
                                    this.fix = false;
                                    this.setValue(-1f);
                                }
                                else
                                {
                                    this.fix = true;
                                }
                            }
                            else
                            {
                                if (fix)
                                {
                                    
                                    float f;
                                    float.TryParse(sentence.parameters[6].ToString(), out f);

                                    //Convert to m/s
                                    f = f * (463f / 900f);

                                    //Convert to km/h
                                    f = f * 3.6f;

                                    this.setValue(f);
                                    this.status = SensorStatus.CONNECTED;
                                }
                            }
                        }
                        catch (ArgumentException aex)
                        {
                            Console.WriteLine(inString, aex);
                        }
                    }
                }
            }
        }

        private int maxValue { get; set; }

        private int minValue { get; set; }

        public SensorUnit getUnit()
        {
            return SensorUnit.KMH;
        }
    }
}
