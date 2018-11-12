using System;
using System.Threading.Tasks;
using SensiEdge.Data;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace SensiEdge.Device
{
    public class SensiDevice : IDevice, IDisposable
    {
        private BluetoothLEDevice ble;
        private GattDeviceService service;
        private IModel model;
        private ISubscribe current;
        private DataSource<Environmental> environmentalSource;
        private DataSource<AccGyroMag> accGyroMagSource;
        private DataSource<AudioLevel> audioLevelSource;
        private DataSource<LEDState> ledStateSource;
        private DataSource<LightSensor> lightSensorSource;
        private DataSource<BatterySatus> batteryStatusSource;
        private DataSource<Orientation> orientationSource;
        private DataSource<Compass> compassSource;
        private DataSource<ActivityRecognition> activityRecognitionSource;
        private DataSource<CarryPosition> carryPositionSource;
        private DataSource<GestureRecognition> gestureRecognitionSource;
        private DataSource<LEDStateConfig> ledStateConfigSource;
        private DataSource<Proximity> proximitySource;
        private DataSource<UltraViolet> ultraVioletSource;
        private DataSource<SmokeSensor> smokeSensorSource;

        public event OnDisconnect OnDisconnect;

        public SensiDevice(BluetoothLEDevice ble, IModel model)
        {
            this.ble = ble;
            this.model = model;
            this.ble.ConnectionStatusChanged += Ble_ConnectionStatusChanged;
        }

        private void Ble_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus == BluetoothConnectionStatus.Disconnected)
                OnDisconnect?.Invoke();
        }

        public async Task InitAsync()
        {
            await InitSensorsAsync();
            await InitConfigAsync();
        }

        public static async Task<SensiDevice> Get(BluetoothLEDevice ble, IModel model)
        {
            var device = new SensiDevice(ble, model);
            await device.InitAsync();
            return device;
        } 

        public async Task InitSensorsAsync()
        {
            var services = await ble.GetGattServicesForUuidAsync(model.SensorsServiceID);
            //TODO: check status
            service = services.Services[0];
            //Enviromental
            environmentalSource = new DataSource<Environmental>(service, model.EnvironmentalID);
            //AccGyroMag
            accGyroMagSource = new DataSource<AccGyroMag>(service, model.AccGyroMagID);
            //AudioLevel
            audioLevelSource = new DataSource<AudioLevel>(service, model.AudioLevelID);
            //LedState
            ledStateSource = new DataSource<LEDState>(service, model.LEDStateID);
            //LightSensor
            lightSensorSource = new DataSource<LightSensor>(service, model.LightSensorID);
            //BatteryStatus
            batteryStatusSource = new DataSource<BatterySatus>(service, model.BatteryStatusID);
            //Orientation
            orientationSource = new DataSource<Orientation>(service, model.OrientationID);
            //Compass
            compassSource = new DataSource<Compass>(service, model.CompassID);
            //ActivityRecognition
            activityRecognitionSource = new DataSource<ActivityRecognition>(service, model.ActivityRecognitionID);
            //CarryPosition
            carryPositionSource = new DataSource<CarryPosition>(service, model.CarryPositionID);
            //GestureRecognition
            gestureRecognitionSource = new DataSource<GestureRecognition>(service, model.GestureRecognitionID);
            //Proximity
            proximitySource = new DataSource<Proximity>(service, model.ProximityID);
            //UltraViolet
            ultraVioletSource = new DataSource<UltraViolet>(service, model.UltraVioletID);
            //SmokeSensor
            smokeSensorSource = new DataSource<SmokeSensor>(service, model.SmokeSensorID);
        }

        public async Task InitConfigAsync()
        {
            var services = await ble.GetGattServicesForUuidAsync(model.ConfigServiceID);
            //TODO: check status
            service = services.Services[0];
            //LedStateConfig
            ledStateConfigSource = new DataSource<LEDStateConfig>(service, model.LEDStateConfigID);
            ledStateConfigSource.BeforeSubscribe += () => current?.Unsubscribe();
            ledStateConfigSource.AfterUnsubscribe += () => current = ledStateConfigSource;
        }

        public void Dispose() => ble?.Dispose();
        

        public ISource<Environmental> EnvironmentalSource => environmentalSource;

        public ISource<AccGyroMag> AccGyroMagSource => accGyroMagSource;

        public ISource<AudioLevel> AudioLevelSource => audioLevelSource;

        public ISource<LEDState> LEDStateSource => ledStateSource;

        public ISource<LightSensor> LightSensorSource => lightSensorSource;

        public ISource<BatterySatus> BatteryStatusSource => batteryStatusSource;

        public ISource<Orientation> OrientationSource => orientationSource;

        public ISource<Compass> CompassSource => compassSource;

        public ISource<ActivityRecognition> ActivityRecognitionSource => activityRecognitionSource;

        public ISource<CarryPosition> CarryPositionSource => carryPositionSource;

        public ISource<GestureRecognition> GestureRecognitionSource => gestureRecognitionSource;

        public ISource<LEDStateConfig> LEDStateConfigSource => ledStateConfigSource;

        public ISource<Proximity> ProximitySource => proximitySource;

        public ISource<UltraViolet> UltraVioletSource => ultraVioletSource;

        public ISource<SmokeSensor> SmokeSensorSource => smokeSensorSource;

        public BluetoothLEDevice Ble => ble;

        public IModel Model => model;
    }
}
