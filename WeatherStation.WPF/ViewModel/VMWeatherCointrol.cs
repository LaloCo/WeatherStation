using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherStation.WPF.Model;
using Microsoft.ServiceBus.Messaging;

namespace WeatherStation.WPF.ViewModel
{
    public class VMWeatherControl : ObservableCollection<WeatherControl>
    {
        static string eventHubName = "weathereventhub";
        static string connectionString = "Endpoint=sb://weathereh-ns.servicebus.windows.net/;SharedAccessKeyName=Manage;SharedAccessKey=n0UWMze48Hi3q9Xo5KCE4kD3DJ12Hw5EBbKM1bR1b0k=";

        public VMWeatherControl()
        {
            //if((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            //{
            //    for(int i = 1; i <= 10; i++)
            //    {
            //        Add(new WeatherControl()
            //            {
            //                Humidity = i,
            //                Latitude = 19.3872f,
            //                Longitude = -99.1665f,
            //                RecordedDate = DateTime.Now,
            //                SensorName = "Am Inn's",
            //                Temperature = i
            //            });
            //    }
            //}
        }

        public async Task InsertData(WeatherControl weather)
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);

            try
            {
                var message = string.Format("{{ 'City': '{7}', 'Humidity': {0}, 'TemperatureC': {1}, 'TemperatureF': {2}, 'TemperatureK': {3}, 'Dew Point': {4}, 'DewPointFast': {5}, 'Time': '{6:yyyy-MM-ddTHH:mm:ss.sssZ}' }}",
                    weather.Humidity, weather.TemperatureC, weather.TemperatureF, weather.TemperatureK, weather.DewPoint, weather.DewPointFast, DateTimeOffset.UtcNow, weather.City);

                Console.WriteLine("{0} > Sending message: \n{1}", DateTime.Now, message);
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} > Exception: {1}", DateTime.Now, ex.Message);
                Console.ResetColor();
            }
        }
    }
}
