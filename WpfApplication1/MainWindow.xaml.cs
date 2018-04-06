using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;


namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        List<City> CityList;

        public MainWindow()
        {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CityList = new List<City>();
            CityList.Add(new City("Брянск", 191));
            CityList.Add(new City("Ульяновск", 195));
            for (int i = 0; i < CityList.Count; i++)
                comboBox.Items.Add(CityList[i].Name);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*AddCity addCity = new AddCity();
            if(addCity.ShowDialog() == true )
            {
                if (addCity.CityName != "")
                {
                    CityList.Add(addCity.CityName);
                    comboBox.Items.Add(CityList.Last());
                }
            }*/

        }
        public static string GetHtmlPageText(string url)
        {
            string text = "";
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 10000;
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8))
                {
                    text = sr.ReadToEnd();
                }
            }
            return text;
        }
        public static List<Weather2> DownLoad(int region)
        {
            
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(GetHtmlPageText("https://export.yandex.ru/bar/reginfo.xml?region="+region.ToString()));

            Weather weather = new Weather();

            List<Weather2> weatherlist = new List<Weather2>();
            var Now = html.DocumentNode.SelectSingleNode("//day");
            var NowWeather = html.DocumentNode.SelectSingleNode("//day_part[1]");
            foreach (var i in Now.ChildNodes)
            {
                if (i.Name.Equals("title"))
                {
                    Weather2 weather2 = new Weather2();
                    weather.City = i.InnerText;
                    weather2.Property = "Город";
                    weather2.value = i.InnerText;
                    weatherlist.Add(weather2);
                }
                    
                if (i.Name.Equals("sun_rise"))
                {
                    Weather2 weather2 = new Weather2();
                    weather.Sun_rise = i.InnerText;
                    weather2.Property = "Рассвет";
                    weather2.value = i.InnerText;
                    weatherlist.Add(weather2);
                }
                if (i.Name.Equals("sunset"))
                {
                    Weather2 weather2 = new Weather2();
                    weather.Sunset = i.InnerText;
                    weather2.Property = "Закат";
                    weather2.value = i.InnerText;
                    weatherlist.Add(weather2);
                }
                if (i.Name.Equals("weekday"))
                    {
                    Weather2 weather2 = new Weather2();
                    weather.Weekday = i.InnerText;
                        weather2.Property = "День недели";
                        weather2.value = i.InnerText;
                    weatherlist.Add(weather2);
                }
            }
            foreach (var i in NowWeather.ChildNodes)
            {
                if (i.Name.Equals("weather_type"))
                {
                    Weather2 weather2 = new Weather2();
                    weather.WeatherType = i.InnerText;
                    weather2.Property = "Тип погоды";
                    weather2.value = i.InnerText;
                    weatherlist.Add(weather2);
                }
                if (i.Name.Equals("temperature"))
                {
                    Weather2 weather2 = new Weather2();
                    weather.TemperatureNow = Convert.ToInt32(i.InnerText);
                    weather2.Property = "Температура";
                    weather2.value = i.InnerText;
                    weatherlist.Add(weather2);
                }
            }
                return weatherlist;
        }

        private void Get_Weather_Click(object sender, RoutedEventArgs e)
        {
            List<Weather2> weather = new List<Weather2>();
            City c= CityList.ElementAt(comboBox.SelectedIndex);
            weather = DownLoad(c.RegionNum);
            dataGrid.ItemsSource = weather;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Weather2> weather = new List<Weather2>();
            City c = CityList.ElementAt(comboBox.SelectedIndex);
            weather = DownLoad(c.RegionNum);
            dataGrid.ItemsSource = weather;
            dataGrid.Columns[0].Header = "Свойство";
            dataGrid.Columns[1].Header = "Значение";
        }
    }
}
