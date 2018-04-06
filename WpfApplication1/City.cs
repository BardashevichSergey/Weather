using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class City
    {
        public string Name { get; }
        public int RegionNum { get; }
        public City(string name, int regionnum)
        {
            this.Name = name;
            this.RegionNum = regionnum;
        }
    }
}
