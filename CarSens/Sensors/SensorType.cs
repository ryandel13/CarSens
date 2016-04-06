using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSens.Sensors
{
    public static class AttributesHelperExtension
    {
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }

    public enum SensorType
    {
        THERMOMETER, VOLTMETER, GPS, FAKE
        
    }
    public enum SensorStatus
    {
        AVAILABLE, DISCONNECTED, CONNECTED, FAILURE, MAXIMUMEXCEEDED, MINIMUMEXCEEDED,
        BUSY
    }
    public enum SensorUnit
    {
        [Description("km/h")]   KMH, 
        [Description("mp/h")]   MPH, 
        [Description("°C")]     DEGREE, 
        [Description("°F")]     FAHRENHEIT,
        [Description("V")]      VOLT
    }
}
