using System;
using SensiEdge.Data;
using Windows.Devices.Bluetooth;
using Status = Windows.Devices.Bluetooth.BluetoothConnectionStatus;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Linq;
using System.Threading.Tasks;

namespace SensiEdge.Device
{
    public class SensiDevice : IDevice
    {
        private ulong BluetoothAddress { get; set; }
        private BluetoothLEDevice Ble { get; set; }
        #region sources
        public ISource<Environmental> EnvironmentalSource { get; private set; } = new DataSource<Environmental>(null);
        public ISource<AccGyroMag> AccGyroMagSource { get; private set; } = new DataSource<AccGyroMag>(null);
        public ISource<AudioLevel> AudioLevelSource { get; private set; } = new DataSource<AudioLevel>(null);
        public ISource<LEDState> LEDStateSource { get; private set; } = new DataSource<LEDState>(null);
        public ISource<LightSensor> LightSensorSource { get; private set; } = new DataSource<LightSensor>(null);
        public ISource<BatterySatus> BatteryStatusSource { get; private set; } = new DataSource<BatterySatus>(null);
        public ISource<Orientation> OrientationSource { get; private set; } = new DataSource<Orientation>(null);
        public ISource<Compass> CompassSource { get; private set; } = new DataSource<Compass>(null);
        public ISource<ActivityRecognition> ActivityRecognitionSource { get; private set; } = new DataSource<ActivityRecognition>(null);
        public ISource<CarryPosition> CarryPositionSource { get; private set; } = new DataSource<CarryPosition>(null);
        public ISource<GestureRecognition> GestureRecognitionSource { get; private set; } = new DataSource<GestureRecognition>(null);
        public ISource<LEDStateConfig> LEDStateConfigSource { get; private set; } = new DataSource<LEDStateConfig>(null);
        public ISource<Proximity> ProximitySource { get; private set; } = new DataSource<Proximity>(null);
        public ISource<UltraViolet> UltraVioletSource { get; private set; } = new DataSource<UltraViolet>(null);
        public ISource<SmokeSensor> SmokeSensorSource { get; private set; } = new DataSource<SmokeSensor>(null);
        #endregion


        public event OnDisconnect OnDisconnect;

        public SensiDevice(ulong bluetoothAddress)
        {
            BluetoothAddress = bluetoothAddress;
        }

        public async Task Connect()
        {
            Ble = await BluetoothLEDevice.FromBluetoothAddressAsync(BluetoothAddress);

            if (Ble != null)
            {
                Ble.ConnectionStatusChanged += StatusChanged;
                var servicesResult = await Ble.GetGattServicesAsync(BluetoothCacheMode.Uncached);

                var services = servicesResult.Services.ToList();
                foreach (var service in services)
                {
                    var charsResult = await service.GetCharacteristicsAsync(BluetoothCacheMode.Uncached);

                    var characteristics = charsResult.Characteristics.ToList();
                    foreach (var characteristic in characteristics)
                    {
                        if (Dictionaries.Sensors.ContainsValue(characteristic.Uuid))
                        {
                            var kvp = Dictionaries.Sensors.First(x => x.Value == characteristic.Uuid);
                            SetSource(kvp.Key, characteristic);
                        }
                    }
                }
            }
        }

        private void SetSource(SensorType type, GattCharacteristic characteristic)
        {
            switch (type)
            {
                case SensorType.AccGyroMagID:
                    AccGyroMagSource = new DataSource<AccGyroMag>(characteristic);
                    break;
                //case SensorType.AccelerometerEventsID: return new DataSource<>(characteristic);
                case SensorType.AudioLevelID:
                    AudioLevelSource = new DataSource<AudioLevel>(characteristic);
                    break;
                case SensorType.LEDStateID:
                    LEDStateSource = new DataSource<LEDState>(characteristic);
                    break;
                case SensorType.EnvironmentalID:
                    EnvironmentalSource = new DataSource<Environmental>(characteristic);
                    break;
                case SensorType.LightSensorID:
                    LightSensorSource = new DataSource<LightSensor>(characteristic);
                    break;
                case SensorType.BatteryStatusID:
                    BatteryStatusSource = new DataSource<BatterySatus>(characteristic);
                    break;
                case SensorType.OrientationID:
                    OrientationSource = new DataSource<Orientation>(characteristic);
                    break;
                case SensorType.CompassID:
                    CompassSource = new DataSource<Compass>(characteristic);
                    break;
                case SensorType.ActivityRecognitionID:
                    ActivityRecognitionSource = new DataSource<ActivityRecognition>(characteristic);
                    break;
                case SensorType.CarryPositionID:
                    CarryPositionSource = new DataSource<CarryPosition>(characteristic);
                    break;
                case SensorType.GestureRecognitionID:
                    GestureRecognitionSource = new DataSource<GestureRecognition>(characteristic);
                    break;
                //case SensorType.BlueVoiceID: return new DataSource<>(characteristic);
                //case SensorType.BlueVoiceSyncID: return new DataSource<>(characteristic);
                case SensorType.LEDStateConfigID:
                    LEDStateConfigSource = new DataSource<LEDStateConfig>(characteristic);
                    break;
                case SensorType.ProximityID:
                    ProximitySource = new DataSource<Proximity>(characteristic);
                    break;
                case SensorType.UltraVioletID:
                    UltraVioletSource = new DataSource<UltraViolet>(characteristic);
                    break;
                case SensorType.SmokeSensorID:
                    SmokeSensorSource = new DataSource<SmokeSensor>(characteristic);
                    break;
            }
        }

        public void Disconnect()
        {
            if (Ble != null)
            {
                //unsubscribe
                Ble.ConnectionStatusChanged -= StatusChanged;
                //sources(services)
                AudioLevelSource.Dispose();
                AccGyroMagSource.Dispose();
                ActivityRecognitionSource.Dispose();
                BatteryStatusSource.Dispose();
                CarryPositionSource.Dispose();
                CompassSource.Dispose();
                EnvironmentalSource.Dispose();
                GestureRecognitionSource.Dispose();
                LEDStateConfigSource.Dispose();
                LightSensorSource.Dispose();
                OrientationSource.Dispose();
                ProximitySource.Dispose();
                SmokeSensorSource.Dispose();
                LEDStateConfigSource.Dispose();
                UltraVioletSource.Dispose();
                //device
                Ble.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Ble = null;
            }
        }


        private void StatusChanged(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus == Status.Disconnected)
                OnDisconnect?.Invoke();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
