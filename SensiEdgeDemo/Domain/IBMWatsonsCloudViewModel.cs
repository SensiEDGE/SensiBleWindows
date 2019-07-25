using IBMWIoTP;
using Newtonsoft.Json;
using SensiEdge.Data;
using SensiEdge.Device;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;

namespace SensiEdgeDemo.Domain
{
    public class CommandHandlerIBMWatsons : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandlerIBMWatsons(Action action, bool canExecute)
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
            Properties.IBMWatsonsSettings.Default.AuthToken = passwordBox.Password;
            _action();
        }
    }
    public class IBMWatsonsCloudViewModel : INotifyPropertyChanged
    {
        private IDevice Device { get; set; }
        private ISource<Environmental> EnviromentalSource { get; set; }
        private ISource<LightSensor> LightSensorSource { get; set; }
        private ISource<AudioLevel> AudioLevelSource { get; set; }
        private Timer SendTimer { get; set; }

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

        private string testButtonContent;
        public string TestButtonContent
        {
            get { return testButtonContent; }
            set { this.MutateVerbose(ref testButtonContent, value, RaisePropertyChanged()); }
        }

        private bool cloudButtonEnable;
        public bool CloundButtonEnable
        {
            get { return cloudButtonEnable; }
            set { this.MutateVerbose(ref cloudButtonEnable, value, RaisePropertyChanged()); }
        }

        private bool testButtonEnable;
        public bool TestButtonEnable
        {
            get { return testButtonEnable; }
            set { this.MutateVerbose(ref testButtonEnable, value, RaisePropertyChanged()); }
        }

        private string cloudButtonContent;
        public string CloudButtonContent
        {
            get { return cloudButtonContent; }
            set { this.MutateVerbose(ref cloudButtonContent, value, RaisePropertyChanged()); }
        }

        public bool IsStarted { get; set; }

        private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set { this.MutateVerbose(ref errorText, value, RaisePropertyChanged()); }
        }

        DeviceClient deviceClient;

        public IBMWatsonsCloudViewModel(IDevice device)
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
                    string message = GetState();
                    await Task.Run(() => deviceClient.publishEvent("test", "json", message));
                }
                catch
                {
                    ErrorText = "Error: Can't send data to cloud";
                    Stop();
                }
            };

            _canExecute = true;
            _canTestExecute = true;
            CloudButtonContent = "Push";
            TestButtonContent = "Test connection";
            CloundButtonEnable = true;
            TestButtonEnable = true;
        }

        private string GetState()
        {
            dynamic state = new System.Dynamic.ExpandoObject();
            if (PressureChecked) state.pressure = Pressure;
            if (HumidityChecked) state.humidity = Humidity;
            if (TemperatureChecked) state.temperature = Temperature;
            if (AudioLevelChecked) state.audiolevel = AudioLevel;
            if (LightSensorChecked) state.lightsensor = LightSensor;
            var json = JsonConvert.SerializeObject(state);
            return json;
        }

        public void ActionClick()
        {
            if (CloudButtonContent == "Push")
            {
                Start(CloudType.New);
                TestButtonEnable = false;
                CloudButtonContent = "Stop";
            }

            else if (CloudButtonContent == "Stop")
            {
                Stop();
                TestButtonEnable = true;
                CloudButtonContent = "Push";
            }
        }

        private void Start(CloudType cloudType)
        {
            try
            {
                if (cloudType == CloudType.Test)
                {
                    string path = Path.GetRandomFileName();
                    path = path.Replace(".", ""); // Remove period.                
                    deviceClient = new DeviceClient("testdev", path);
                    System.Diagnostics.Process.Start("https://quickstart.internetofthings.ibmcloud.com/#/device/" + path);
                }
                else
                {
                    deviceClient = new DeviceClient(Properties.IBMWatsonsSettings.Default.OrgId,
                        Properties.IBMWatsonsSettings.Default.DeviceType,
                        Properties.IBMWatsonsSettings.Default.DeviceId, "token",
                        Properties.IBMWatsonsSettings.Default.AuthToken);
                }


                ErrorText = "";
                deviceClient.connect();
                deviceClient.subscribeCommand("testcmd", "json", 0);
                SendTimer.Start();
            }
            catch (Exception ex)
            {
                if (CloudButtonContent == "Stop")
                {
                    Stop();
                    TestButtonEnable = true;
                    CloudButtonContent = "Push";
                }
                if (TestButtonContent == "Stop testing")
                {
                    Stop();
                    CloundButtonEnable = true;
                    TestButtonContent = "Test connection";
                }
                ErrorText = "Error: Can't create cloud client";
            }
        }
        private void Stop()
        {
            SendTimer.Stop();
        }

        public void TestClickAction()
        {
            if (TestButtonContent == "Test connection")
            {
                Start(CloudType.Test);
                CloundButtonEnable = false;
                TestButtonContent = "Stop testing";
            }

            else if (TestButtonContent == "Stop testing")
            {
                Stop();
                CloundButtonEnable = true;
                TestButtonContent = "Test connection";
            }
        }

        private ICommand _testClick;
        public ICommand TestClick
        {
            get
            {
                return _testClick ?? (_testClick = new CommandHandlerIBMWatsons(() => TestClickAction(), _canTestExecute));
            }
        }
        private ICommand _clickCommand;
        public ICommand ClickCommandIBMW
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandlerIBMWatsons(() => ClickAction(), _canExecute));
            }
        }
        private bool _canTestExecute;
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

    enum CloudType
    {
        Test,
        New
    }
}
