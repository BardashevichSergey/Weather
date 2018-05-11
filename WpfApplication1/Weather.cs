using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
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
