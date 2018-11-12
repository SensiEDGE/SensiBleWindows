using SensiEdge.Data;
using Windows.Devices.Bluetooth;

namespace SensiEdge.Device
{
    public delegate void OnDisconnect();
    public interface IDevice
    {
        //Info
        BluetoothLEDevice Ble { get; }
        IModel Model { get; }
        event OnDisconnect OnDisconnect;
        //Sources
        ISource<Environmental> EnvironmentalSource { get; }
        ISource<AccGyroMag> AccGyroMagSource { get; }
        ISource<AudioLevel> AudioLevelSource { get; }
        ISource<LEDState> LEDStateSource { get; }
        ISource<LightSensor> LightSensorSource { get; }
        ISource<BatterySatus> BatteryStatusSource { get; }
        ISource<Orientation> OrientationSource { get; }
        ISource<Compass> CompassSource { get; }
        ISource<ActivityRecognition> ActivityRecognitionSource { get; }
        ISource<CarryPosition> CarryPositionSource { get; }
        ISource<GestureRecognition> GestureRecognitionSource { get; }
        ISource<LEDStateConfig> LEDStateConfigSource { get; }
        ISource<Proximity> ProximitySource { get; }
        ISource<UltraViolet> UltraVioletSource { get; }
        ISource<SmokeSensor> SmokeSensorSource { get; }
    }
}
