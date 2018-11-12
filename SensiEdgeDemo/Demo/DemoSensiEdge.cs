using SensiEdge.Data;
using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace SensiEdgeDemo.Demo
{
    public class DemoSensiEdge : IDevice
    {
        public ISource<Environmental> EnvironmentalSource => new DemoDataSource<Environmental>(new DemoEnvironmental());
        public ISource<AccGyroMag> AccGyroMagSource => new DemoDataSource<AccGyroMag>(new DemoAccGyroMag());
        public ISource<AudioLevel> AudioLevelSource => new DemoDataSource<AudioLevel>(new DemoAudioLevel());
        public ISource<LEDState> LEDStateSource => new DemoDataSource<LEDState>(new DemoLEDState());
        public ISource<LightSensor> LightSensorSource => new DemoDataSource<LightSensor>(new DemoLightSensor());
        public ISource<BatterySatus> BatteryStatusSource => new DemoDataSource<BatterySatus>(new DemoBatterySatus());
        public ISource<Orientation> OrientationSource => new DemoDataSource<Orientation>(new DemoOrientation());
        public ISource<Compass> CompassSource => new DemoDataSource<Compass>(new DemoCompass());
        public ISource<ActivityRecognition> ActivityRecognitionSource => new DemoDataSource<ActivityRecognition>(new DemoActivityRecognition());
        public ISource<CarryPosition> CarryPositionSource => new DemoDataSource<CarryPosition>(new DemoCarryPosition());
        public ISource<GestureRecognition> GestureRecognitionSource => new DemoDataSource<GestureRecognition>(new DemoGestureRecognition());

        public ISource<LEDStateConfig> LEDStateConfigSource => new DemoDataSource<LEDStateConfig>(new DemoLEDStateConfig());

        public ISource<Proximity> ProximitySource => new DemoDataSource<Proximity>(new DemoProximity());
        public ISource<UltraViolet> UltraVioletSource => new DemoDataSource<UltraViolet>(new DemoUltraViolet());
        public ISource<SmokeSensor> SmokeSensorSource => new DemoDataSource<SmokeSensor>(new DemoSmokeSensor());

        public BluetoothLEDevice Ble => throw new NotSupportedException();
        public IModel Model => throw new NotSupportedException();


        public event OnDisconnect OnDisconnect;
    }
}
