using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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
using System.Windows.Threading;
using WeatherStation.WPF.Model;
using WeatherStation.WPF.ViewModel;

namespace WeatherStation.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WeatherControl currentWeather;
        private VMWeatherControl vmWeather;

        DispatcherTimer temporizador = new DispatcherTimer();
        DispatcherTimer temporizadorDePrueba = new DispatcherTimer();
        SerialPort myPort = new SerialPort();

        public MainWindow()
        {
            InitializeComponent();

            currentWeather = new WeatherControl();
            currentDataContainer.DataContext = currentWeather;

            vmWeather = new VMWeatherControl();

            RegisterCommand();
        }

        private void RegisterCommand()
        {
            try
            {
                myPort.BaudRate = 9600;
                myPort.PortName = "COM3";
                myPort.Open();

                //! Comentar para valores reales leidos desde Arduino
                /*temporizador.Tick += temporizador_Tick;
                temporizador.Interval = TimeSpan.FromSeconds(1);
                temporizador.Start();*/

                //! Descomentar para pruebas con valores predeterminados
                temporizadorDePrueba.Tick += Temporizador_Tick;
                temporizadorDePrueba.Interval = TimeSpan.FromSeconds(1);
                temporizadorDePrueba.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Temporizador_Tick(object sender, EventArgs e)
        {
            currentWeather.DewPoint = 3;
            currentWeather.DewPointFast = 2.5f;
            currentWeather.Humidity = 45;
            currentWeather.TemperatureC = 10;
            currentWeather.TemperatureF = 50;
            currentWeather.TemperatureK = 283.15f;
            currentWeather.City = "Ciudad de México";

            try
            {
                await vmWeather.InsertData(currentWeather);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async void temporizador_Tick(object sender, System.EventArgs e)
        {
            //! Lee desde el puerto serial
            string readData = myPort.ReadLine().Trim();

            int indexHumidity = readData.IndexOf('h');
            int indexCelsius = readData.IndexOf('c');
            int indexFahrenheit = readData.IndexOf('f');
            int indexKelvin = readData.IndexOf('k');
            int indexDewPoint = readData.IndexOf('d');
            int indexDewPointFast = readData.IndexOf('w');

            try
            {
                if (indexHumidity >= 0 && indexCelsius >= 0)
                {
                    var substring = readData.Substring(indexHumidity + 1, 15).Trim();
                    float humidity = float.Parse(substring);
                    currentWeather.Humidity = humidity;
                }
                if(indexCelsius >= 0 && indexFahrenheit >= 0)
                {
                    var substring = readData.Substring(indexCelsius + 1, 15).Trim();
                    float celsius = float.Parse(substring);
                    currentWeather.TemperatureC = celsius;
                }
                if(indexFahrenheit >= 0 && indexKelvin >= 0)
                {
                    var substring = readData.Substring(indexFahrenheit + 1, 15).Trim();
                    float fahrenheit = float.Parse(substring);
                    currentWeather.TemperatureF = fahrenheit;
                }
                if (indexKelvin >= 0 && indexDewPoint >= 0)
                {
                    var substring = readData.Substring(indexKelvin + 1, 15).Trim();
                    float kelvin = float.Parse(substring);
                    currentWeather.TemperatureK = kelvin;
                }
                if(indexDewPoint >= 0 && indexDewPointFast >= 0)
                {
                    var substring = readData.Substring(indexDewPoint + 1, 14).Trim();
                    float dewPoint = float.Parse(substring);
                    currentWeather.DewPoint = dewPoint;

                    substring = readData.Substring(indexDewPointFast + 1).Trim();
                    float dewPointFast = float.Parse(substring);
                    currentWeather.DewPointFast = dewPointFast;
                }

                currentWeather.City = "Tulancingo";
                await vmWeather.InsertData(currentWeather);
            }
            catch(Exception ex)
            {
                lblState.Text = ex.Message;
            }
        }
    }
}
