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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using HtmlAgilityPack;
using System.Threading;
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
        List<string> weatherTable;
        string[] TabControlItems = { "Сегодня", "Неделя", "2 недели" };
        bool isInit = false;
        enum selection
        {
            now = 0,
            today = 1,
            week = 2
        }
        
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            weatherTable = new List<string>();
            weatherTable.Add("asdsa");
            weatherTable.Add("asdsa1");
            weatherTable.Add("asdsa2");
            dataGrid.ItemsSource = weatherTable;
            DataGridTextColumn col = new DataGridTextColumn();
            col.Header = "asda";
            dataGrid.Columns.Add(col);

            List<City> CityListTmp = new List<City>();
            CityList = new List<City>();
            CityListTmp.Add(new City("Брянск"));
            CityListTmp.Add(new City("Якутск"));
            CityListTmp.Add(new City("Москва"));
            CityListTmp.Add(new City("Орел"));
            var items = from i in CityListTmp orderby i.Name select i;
            foreach (City item in items)
                CityList.Add(item);
            CityListTmp.Clear();
            listBox.ItemsSource = CityList;
            foreach (var i in TabControlItems)
            {
                TabItem tab = new TabItem();
                tab.Header = i;
                tab.Content = new System.Windows.Controls.DataGrid();
                tabControl.Items.Add(tab);
            }

            //getCitylistFromHTML();
            isInit = true;
        }
        private List<City> getCitylistFromHTML()
        {
            List<City> result = new List<City>();
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            string page = GetHtmlPageText("https://www.meteoservice.ru/content/export.html");
            if (page != null)
            {
                html.LoadHtml(page);
                Weather weather = new Weather();
                List<WeatherTable> weatherlist = new List<WeatherTable>();

                //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("option");

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(page);

                var options = doc.DocumentNode.Descendants("option").Skip(1)
                                .Select(n => new
                                {
                                    Value = n.Attributes["value"].Value,
                                    Text = n.InnerText
                                })
                                .ToList();


                //HtmlDocument doc = new HtmlDocument();
                //HtmlAgilityPack.HtmlNode.ElementsFlags.Remove("option");

                doc.LoadHtml("https://www.meteoservice.ru/content/export.html");
                //HtmlNode.ElementsFlags["option"] = HtmlElementFlag.Closed;

                HtmlNodeCollection select_node_collection2 = doc.DocumentNode.SelectNodes("//option");
                HtmlNodeCollection select_node_collection1 = doc.DocumentNode.SelectNodes("//select[@name='country_list']//option");

                HtmlNodeCollection select_node_collection = doc.DocumentNode.SelectNodes("//select[@name='city_list']//option");

                doc.LoadHtml("https://www.meteoservice.ru/content/export.html");
                var city_list1 = html.DocumentNode.SelectSingleNode("//option");

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//*/select[@id='city_list']//option"))
                {
                    Console.WriteLine("Value=" + node.Attributes["value"].Value);
                    Console.WriteLine("InnerText=" + node.InnerText);
                    Console.WriteLine();
                }

                var city_list = html.DocumentNode.SelectSingleNode("//*[contains(@name,'city_list')/option]");
                /*weatherlist.Add(new WeatherTable("Температура воздуха", Now.InnerText));
                for (int i = 2; i < 11; i++)
                {
                    var NowWeather = html.DocumentNode.SelectSingleNode("//table/tr[" + i + "]");
                    weatherlist.Add(new WeatherTable(NowWeather.ChildNodes[1].InnerText, NowWeather.ChildNodes[3].InnerText));
                }
                return weatherlist;*/
                
            }
            return result;
        }
        private static string GetHtmlPageText(string url)
        {
            string text = "";
            WebRequest request = WebRequest.Create(url);
            try
            {
                request.Timeout = 10000;
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8))
                        text = sr.ReadToEnd();
                }
            }
            catch (WebException exception)
            {
                /*ErrorWindow errorWindow = new ErrorWindow("Ошибка получения данных", exception.Message);
                errorWindow.Left = this.Left + (this.Width - errorWindow.Width) / 2;
                errorWindow.Top = this.Top + (this.Height - errorWindow.Height) / 2;
                errorWindow.Show();*/
                //using (var response = (HttpWebResponse)exception.Response)
                Task task1 = new Task(() => System.Windows.MessageBox.Show(exception.Message, "Ошибка получения данных"));
                task1.Start();

                return null;
            }
            return text;
        }
        private void SaveCityList(List<City> List)
        {
            
        }

        private static List<WeatherTable> ParsePage(string CityName, selection select)
        {
            
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            string page = GetHtmlPageText("https://www.meteoservice.ru/weather/"+select+"/" + CityName + ".html");
            if (page != null)
            {
                html.LoadHtml(page);
                Weather weather = new Weather();
                List<WeatherTable> weatherlist = new List<WeatherTable>();

                switch (select)
                {
                    case selection.now:
                        {
                            //var Now = html.DocumentNode.SelectSingleNode("//*[contains(@class,'temperature')]");
                            HtmlNodeCollection Now = html.DocumentNode.SelectNodes("//*[contains(@class,'callout')]/div/div");
                            
                            //weatherlist.Add(new WeatherTable("Температура воздуха", Now.InnerText));
                            for (int i = 0; i < Now.Count; i++)
                            {
                                //var NowWeather = html.DocumentNode.SelectSingleNode("//table/tr[" + i + "]");
                                //HtmlNodeCollection now2 = Now[i].SelectNodes("//div");

                                string str = (Now[i].InnerText.Replace("\t", "")).Replace("\n", "");
                                weatherlist.Add(new WeatherTable(str, str));
                                
                            }
                            return weatherlist;
                            break;
                        }
                    case selection.today:
                        {
                            return weatherlist;

                            break;
                        }
                    case selection.week:
                        {
                            HtmlAgilityPack.HtmlNodeCollection Now = (html.DocumentNode.SelectSingleNode("//*[contains(@class,'row')]")).SelectNodes("//div/h3");
                            var Week = (html.DocumentNode.SelectSingleNode("//*[contains(@class,'row')]")).SelectNodes("//*[contains(@class,'data three_hour')]");
                            for (int j = 0; j < Week.Count; j++)
                            {
                                weatherlist.Add(new WeatherTable(Now[j].InnerText, ""));
                                var DayTime = Week[j].SelectSingleNode("//table[" + (j + 1) + "]/tbody/tr[1]");
                                var DayTemp = Week[j].SelectSingleNode("//table[" + (j + 1) + "]/tbody/tr[3]");
                                if (DayTime != null & DayTemp != null)
                                    for (int k = 0; k < DayTime.ChildNodes.Count; k++)
                                    {
                                        var Time = DayTime.SelectSingleNode("//table[" + (j + 1) + "]/tbody/tr[1]/td[" + (k + 1) + "]");
                                        var Temp = DayTemp.SelectSingleNode("//table[" + (j + 1) + "]/tbody/tr[3]/td[" + (k + 1) + "]/div");
                                        if (Time != null & Temp != null)
                                            weatherlist.Add(new WeatherTable(Time.InnerText.Substring(0, 5), Temp.InnerText.Substring(0, Temp.InnerText.Length - 5), new string[] { "ds","sdew"}));
                                    }
                            }
                            return weatherlist;
                            break;
                        }
                    default:
                        return weatherlist;
                        break;
                }
            }
            else
                return null;
        }

        private void getWeather(int index=0)
        {
            if (index < 0)
                index = 0;
            List<WeatherTable> weather = new List<WeatherTable>();
            selection select = selection.now;
            if(CityList!=null)
                if (CityList.Count > 0)
                {
                    City c = CityList.ElementAt(index);
                    switch( tabControl.SelectedIndex)
                    {
                        case 0: select = selection.now;
                            break;
                        case 1: select = selection.week;
                            break;
                    }
                    weather = ParsePage(c.NameTranslit, select);
                    /*foreach(var i in weather)
                    {
                        weatherTable.Add(i.Property);
                    }*/
                    System.Windows.Controls.DataGrid dg = ((tabControl.Items[tabControl.SelectedIndex] as TabItem).Content as System.Windows.Controls.DataGrid);
                    if (weather != null)
                    {
                        dataGrid_Now.ItemsSource = weather;
                        //dg.ItemsSource = weather;
                        CurentCity.Content = c.Name;
                    }
                    else
                    {
                        //dg.ItemsSource = null;
                        CurentCity.Content = c.Name;
                    }
                }
        }
       
        private void button_Add_City_Click(object sender, RoutedEventArgs e)
        {
            AddCity addCity = new AddCity();
            addCity.Left = this.Left + (this.Width - addCity.Width) / 2;
            addCity.Top = this.Top + (this.Height - addCity.Height) / 2;
            if (addCity.ShowDialog() == true)
                if (addCity.CityName != "")
                {
                    CityList.Add(new City(addCity.CityName));
                    listBox.Items.Refresh();
                }
        }

        private void Delete_City(int index )
        {
            CityList.RemoveAt(index);
            listBox.Items.Refresh();
        }
        private void button_Save_Citys_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = System.Windows.Forms.Application.ExecutablePath;
            saveDialog.Filter = "JSON-фалы(*.json)|*.json";
            saveDialog.FileName = "Список городов";
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<City>));
                using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.OpenOrCreate))
                    jsonFormatter.WriteObject(fs, CityList);
            }
        }
        private void button_Load_Citys_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.InitialDirectory = System.Windows.Forms.Application.ExecutablePath;
                openDialog.CheckFileExists = true;
                openDialog.Filter = "JSON-фалы(*.json)|*.json";
                openDialog.ShowDialog();
                string fileName = openDialog.FileName;

                if (fileName != null)
                    using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<City>));

                        List<City> newpeople = (List<City>)jsonFormatter.ReadObject(fs);
                        CityList.Clear();
                        foreach (var i in newpeople)
                            CityList.Add(i);
                        listBox.Items.Refresh();
                    }
            }
            catch(Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow("Ошибка загрузки городов", ex.Message);
                errorWindow.Show();
            }    
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getWeather(listBox.SelectedIndex);
        }

        private void listbox_delete_item(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedIndex>=0)
                Delete_City(listBox.SelectedIndex);
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInit)
                return;
            getWeather(listBox.SelectedIndex);
 
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
