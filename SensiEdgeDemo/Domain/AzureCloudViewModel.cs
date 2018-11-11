using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SensiEdge.Data;
using SensiEdge.Device;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SensiEdgeDemo.Domain
{
    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            Properties.AzureSettings.Default.AccessKey = passwordBox.Password;
            _action();
        }
    }
    public class AzureCloudViewModel : INotifyPropertyChanged
    {
        private IDevice Device { get; set; }
        private ISource<Environmental> EnviromentalSource { get; set; }
        private ISource<LightSensor> LightSensorSource { get; set; }
        private ISource<AudioLevel> AudioLevelSource { get; set; }
        private Timer SendTimer { get; set; }
        private string Host { get; set; }
        private string DeviceId { get; set; }
        private string Password { get; set; }

        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double AudioLevel { get; set; }
        public double LightSensor { get; set; }

        public bool PressureChecked { get; set; }
        public bool HumidityChecked { get; set; }
        public bool TemperatureChecked { get; set; }
        public bool AudioLevelChecked { get; set; }
        public bool LightSensorChecked { get; set; }

        public void Activate()
        {
            if (EnviromentalSource.IsAvailable) EnviromentalSource.Enable();
            if (LightSensorSource.IsAvailable) LightSensorSource.Enable();
            if (AudioLevelSource.IsAvailable) AudioLevelSource.Enable();
        }
        public void Deactivate()
        {
            if (EnviromentalSource.IsAvailable) EnviromentalSource.Disable();
            if (LightSensorSource.IsAvailable) LightSensorSource.Disable();
            if (AudioLevelSource.IsAvailable) AudioLevelSource.Disable();
        }

        private string cloudButtonContent;
        public string CloudButtonContent
        {
            get { return cloudButtonContent; }
            set { this.MutateVerbose(ref cloudButtonContent, value, RaisePropertyChanged()); }
        }

        public bool IsStarted { get; set; }

        private Brush buttonBackground;
        public Brush ButtonBackground
        {
            get { return buttonBackground; }
            set { this.MutateVerbose(ref buttonBackground, value, RaisePropertyChanged()); }
        }

        private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set { this.MutateVerbose(ref errorText, value, RaisePropertyChanged()); }
        }    
                
        DeviceClient deviceClient;

        public AzureCloudViewModel(IDevice device)
        {
            Device = device;
            EnviromentalSource = device.EnvironmentalSource;
            LightSensorSource = device.LightSensorSource;
            AudioLevelSource = device.AudioLevelSource;
            EnviromentalSource.OnChange += (Environmental value) =>
            {
                Pressure = value.Pressure * 0.01;
                Humidity = value.Humidity * 0.1;
                Temperature = value.Temperature * 0.1;
            };
            AudioLevelSource.OnChange += (AudioLevel value) =>
            {
                AudioLevel = value.Level;
            };
            LightSensorSource.OnChange += (LightSensor value) =>
            {
                LightSensor = value.Value;
            };

            SendTimer = new Timer(1000);
            SendTimer.Elapsed += async (object sender, ElapsedEventArgs e) =>
            {
                try
                {
                    Message message = GetState(); 
                    await deviceClient.SendEventAsync(message).ConfigureAwait(false);
                }
                catch
                {
                    ErrorText = "Error: Can't send data to cloud";
                    Stop();
                }
            };

            _canExecute = true;
            CloudButtonContent = "Push";
        }

        private Message GetState()
        {
            dynamic state = new System.Dynamic.ExpandoObject();
            if (PressureChecked) state.pressure = Pressure;
            if (HumidityChecked) state.humidity = Humidity;
            if (TemperatureChecked) state.temperature = Temperature;
            if (AudioLevelChecked) state.audiolevel = AudioLevel;
            if (LightSensorChecked) state.lightsensor = LightSensor;
            var json = JsonConvert.SerializeObject(state);
            return new Message(Encoding.UTF8.GetBytes(json));
        }

        public void ActionClick()
        {
            if (CloudButtonContent == "Push") Start();
            else if (CloudButtonContent == "Stop") Stop();
        }
        private void Start()
        {
            try
            {
                deviceClient = DeviceClient.Create(Properties.AzureSettings.Default.Host,
                                new DeviceAuthenticationWithRegistrySymmetricKey(Properties.AzureSettings.Default.DeviceId,
                                Properties.AzureSettings.Default.AccessKey), TransportType.Mqtt);
            }
            catch { ErrorText = "Error: Can't create cloud client"; }
            SendTimer.Start();
            CloudButtonContent = "Stop";
        }
        private void Stop()
        {
            SendTimer.Stop();
            CloudButtonContent = "Push";
        }
        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => ClickAction(), _canExecute));
            }
        }
        private bool _canExecute;
        public void ClickAction()
        {
            ActionClick();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
