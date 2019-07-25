using SensiEdge.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace SensiEdge.Device
{
    public delegate void OnDisconnect();
    public interface IDevice : IDisposable
    {
        //Info
        Task Connect();
        void Disconnect();
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
