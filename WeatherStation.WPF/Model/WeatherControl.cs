using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherStation.WPF.Model
{
    public class WeatherControl : INotifyPropertyChanged
    {
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private float humidity;

        public float Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                OnPropertyChanged("Humidity");
            }
        }

        private float temperatureC;

        public float TemperatureC
        {
            get { return temperatureC; }
            set
            {
                temperatureC = value;
                OnPropertyChanged("TemperatureC");
            }
        }

        private float temperatureF;

        public float TemperatureF
        {
            get { return temperatureF; }
            set
            {
                temperatureF = value;
                OnPropertyChanged("TemperatureF");
            }
        }

        private float temperatureK;

        public float TemperatureK
        {
            get { return temperatureK; }
            set
            {
                temperatureK = value;
                OnPropertyChanged("TemperatureK");
            }
        }

        private float dewPoint;

        public float DewPoint
        {
            get { return dewPoint; }
            set
            {
                dewPoint = value;
                OnPropertyChanged("DewPoint");
            }
        }

        private float dewPointFast;

        public float DewPointFast
        {
            get { return dewPointFast; }
            set
            {
                dewPointFast = value;
                OnPropertyChanged("DewPointFast");
            }
        }

        private string city;

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public WeatherControl()
        {
            if((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                this.Humidity = 35f;
                this.TemperatureC = 20.5f;
                this.TemperatureF = 70.3f;
                this.TemperatureK = 293f;
                this.DewPoint = 8.43f;
                this.DewPointFast = 9.1f;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
