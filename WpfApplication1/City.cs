using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WpfApplication1
{
    [DataContract]
    class City
    {
        string[] rus = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й",
          "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц",
          "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
        string[] eng = { "A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "Y",
          "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS",
          "CH", "SH", "SHCH", null, "Y", null, "E", "YU", "YA" };
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string NameTranslit { get; set; }
        [DataMember]
        public int RegionNum { get; set; }
        public City(string name, int regionnum=0)
        {
            this.Name = name;
            this.RegionNum = regionnum;
            name = name.ToUpper();
            for (int j = 0; j < name.Length; j++)
                for (int i = 0; i < rus.Length; i++)
                    if (name.Substring(j, 1) == rus[i])
                        NameTranslit +=eng[i];
            NameTranslit = NameTranslit.ToLower();
        }
    }
}
