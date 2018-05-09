using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Weather
    {
        public string City { get; set; }
        public string Weekday { get; set; }
        public string Sun_rise { get; set; }
        public string Sunset { get; set; }
        public int TemperatureNow { get; set; }
        public string WeatherType { get; set; }

    }
    public class WeatherTable
    {
        string[] PropertyArray { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
        public string T { get; set; }
        public WeatherTable(string property, string value)
        {
            PropertyArray = new string[2];
            PropertyArray[0] = property;
            Property = property;
            Value = value;
            //T = propertyarray[0];
        }
    }

    public class WeatherRow
    {
        public string property { get; set; }
        public string value { get; set; }
        public string weather { get; set; }

        public WeatherRow (string property,string value,string weather = "")
        {
            this.property = property;
            this.value = value;
            this.weather = weather;
        }
    }
}
