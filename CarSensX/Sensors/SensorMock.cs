using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSensX.Sensors
{
    class SensorMock : Sensor
    {
        String name = "Fake";
        String id = "";
       private static Random rand;

        public SensorMock()
        {
            if(SensorMock.rand == null) {
                rand = new Random(999999);
            }
            this.id = "MOCK" + SensorMock.rand.Next();
        }

        public string getValue()
        {
            return getFloatValue().ToString();
            
        }

        public string getAverageValue()
        {
            Random rand = new Random();
            return rand.Next(1000).ToString();
        }

        public SensorType getType()
        {
            return SensorType.FAKE;
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
            this.id = identifier;
        }

        public void connect()
        {
            
        }

        public void disconnect()
        {
            
        }

        public SensorStatus getStatus()
        {
            return SensorStatus.CONNECTED;
        }

        public string getIdentifier()
        {
        
            return this.id;
        }

        public int getMaximumValue()
        {
            return 1000;
        }

        public int getMinimumValue()
        {
            return 0;
        }

        public void setMaximumValue(int max)
        {
            throw new NotImplementedException();
        }

        public void setMinimumValue(int min)
        {
            throw new NotImplementedException();
        }

        public float getFloatValue()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            return rand.Next(100);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            id = reader.GetAttribute("id");
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("id", id);
        }


        public SensorUnit getUnit()
        {
            return SensorUnit.FAHRENHEIT;
        }
    }
}
