using System;

namespace SensiEdge.Device
{
    public class ModelA : IModel
    {
        public Guid SensorsServiceID => new Guid("00000000-0001-11e1-9ab4-0002a5d5c51b");
        public Guid ConfigServiceID => new Guid("00000000-000f-11e1-9ab4-0002a5d5c51b");

        public Guid EnvironmentalID => new Guid("001c0000-0001-11e1-ac36-0002a5d5c51b");
        public Guid AccGyroMagID => new Guid("00e00000-0001-11e1-ac36-0002a5d5c51b");
        public Guid AccelerometerEventsID => new Guid("00000400-0001-11e1-ac36-0002a5d5c51b");
        public Guid AudioLevelID => new Guid("04000000-0001-11e1-ac36-0002a5d5c51b");
        public Guid LEDStateID => new Guid("20000000-0001-11e1-ac36-0002a5d5c51b");
        public Guid LightSensorID => new Guid("01000000-0001-11e1-ac36-0002a5d5c51b");
        public Guid BatteryStatusID => new Guid("00020000-0001-11e1-ac36-0002a5d5c51b");
        public Guid OrientationID => new Guid("00000100-0001-11e1-ac36-0002a5d5c51b");
        public Guid CompassID => new Guid("00000040-0001-11e1-ac36-0002a5d5c51b");
        public Guid ActivityRecognitionID => new Guid("00000010-0001-11e1-ac36-0002a5d5c51b");
        public Guid CarryPositionID => new Guid("00000008-0001-11e1-ac36-0002a5d5c51b");
        public Guid GestureRecognitionID => new Guid("00000002-0001-11e1-ac36-0002a5d5c51b");
        public Guid BlueVoiceID => Guid.Empty;
        public Guid BlueVoiceSyncID => Guid.Empty;
        public Guid LEDStateConfigID => new Guid("00000002-000f-11e1-ac36-0002a5d5c51b");
        public Guid ProximityID => Guid.Empty;
        public Guid UltraVioletID => Guid.Empty;
        public Guid SmokeSensorID => Guid.Empty;
    }
}
