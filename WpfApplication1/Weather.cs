﻿using System;
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
        public string Property { get; set; }
        public string Value { get; set; }
        public WeatherTable(string property,string value)
        {
            Property = property;
            Value = value;
        }
    }
}
