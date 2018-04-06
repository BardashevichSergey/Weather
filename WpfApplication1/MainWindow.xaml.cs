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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

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
            CityList.Add(new City("Брянск"));
            CityList.Add(new City("Москва"));
            for (int i = 0; i < CityList.Count; i++)
                comboBox.Items.Add(CityList[i].Name);
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
        private void SaveCityList(List<City> List)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<City>));

            using (FileStream fs = new FileStream("people.json", FileMode.OpenOrCreate))
            {
               jsonFormatter.WriteObject(fs, List);
            }
        }

        public static List<WeatherTable> ParsePage(string CityName)
        {
            
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(GetHtmlPageText("https://www.meteoservice.ru/weather/now/"+ CityName + ".html"));

            Weather weather = new Weather();

            List<WeatherTable> weatherlist = new List<WeatherTable>();
            var Now = html.DocumentNode.SelectSingleNode("//*[contains(@class,'temperature')]");
            weatherlist.Add(new WeatherTable("Температура воздуха", Now.InnerText));
            for (int i = 2; i < 11; i++)
            {
                var NowWeather = html.DocumentNode.SelectSingleNode("//table/tr["+i+"]");
                weatherlist.Add(new WeatherTable(NowWeather.ChildNodes[1].InnerText, NowWeather.ChildNodes[3].InnerText));
            }    
            return weatherlist;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<WeatherTable> weather = new List<WeatherTable>();
            City c = CityList.ElementAt(comboBox.SelectedIndex);
            weather = ParsePage(c.NameTranslit);
            dataGrid.ItemsSource = weather;
            dataGrid.Columns[0].Header = "Свойство";
            dataGrid.Columns[1].Header = "Значение";
        }

        private void button_Add_City_Click(object sender, RoutedEventArgs e)
        {
            AddCity addCity = new AddCity();
            if (addCity.ShowDialog() == true)
            {
                if (addCity.CityName != "")
                {
                    CityList.Add(new City(addCity.CityName));
                    comboBox.Items.Add(CityList.Last().Name);
                }
            }
        }

        private void button_Save_Citys_Click(object sender, RoutedEventArgs e)
        {
            SaveCityList(CityList);
        }
        private void button_Load_Citys_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream("people.json", FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<City>));

                List<City> newpeople = (List<City>)jsonFormatter.ReadObject(fs);
                CityList.Clear();
                comboBox.Items.Clear();
                foreach (var i in newpeople)
                {
                    CityList.Add(i);
                    comboBox.Items.Add(CityList.Last().Name);
                }
            }
            SaveCityList(CityList);
        }
    }
}
