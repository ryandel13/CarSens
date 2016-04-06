using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace CarSensX.Sensors
{
    /*
     * Interface  for connecting Sensors
     */
    public interface Sensor : IXmlSerializable
    {
        /// <summary>
        /// Getter for the current Value
        /// </summary>
        /// <returns></returns>
        String getValue();
        /// <summary>
        /// Getter for the average value
        /// </summary>
        /// <returns></returns>
        String getAverageValue();
        /// <summary>
        /// Getter for the average value
        /// </summary>
        /// <returns></returns>
        SensorType getType();
        /// <summary>
        /// Getter for the SensorType
        /// </summary>
        /// <returns></returns>
        String getName();
        /// <summary>
        /// Setter for the Sensor name
        /// </summary>
        /// <param name="name"></param>
        void setName(String name);
        /// <summary>
        /// Setter for the identifier
        /// </summary>
        /// <param name="identifier"></param>
        void setDeviceIdentifier(String identifier);
        /// <summary>
        /// Connects this Sensor
        /// </summary>
        void connect();
        /// <summary>
        /// Disconnects the sensor
        /// </summary>
        void disconnect();
        /// <summary>
        /// Getter for sensor Status
        /// </summary>
        /// <returns></returns>
        SensorStatus getStatus();
        /// <summary>
        /// Getter for the sensors identifier
        /// </summary>
        /// <returns></returns>
        string getIdentifier();
        /// <summary>
        /// Getter for the maximum value
        /// </summary>
        /// <returns></returns>
        int getMaximumValue();
        /// <summary>
        /// Getter for the minimum Value
        /// </summary>
        /// <returns></returns>
        int getMinimumValue();
        /// <summary>
        /// Setter for the maximum value
        /// </summary>
        /// <param name="max"></param>
        void setMaximumValue(int max);
        /// <summary>
        /// Setter for the minimum value
        /// </summary>
        /// <param name="min"></param>
        void setMinimumValue(int min);
        /// <summary>
        /// Getter for the current value as float.
        /// </summary>
        /// <returns></returns>
        float getFloatValue();
        /// <summary>
        /// Getter for the Unit.
        /// </summary>
        /// <returns></returns>
        SensorUnit getUnit();
    }
}
