using System;

namespace SensiEdge.Device
{
    public interface IModel
    {
        Guid SensorsServiceID { get; }
        Guid ConfigServiceID { get; }
        Guid EnvironmentalID { get; }
        Guid AccGyroMagID { get; }
        Guid AccelerometerEventsID { get; }
        Guid AudioLevelID { get; }
        Guid LEDStateID { get; }
        Guid LightSensorID { get; }
        Guid BatteryStatusID { get; }
        Guid OrientationID { get; }
        Guid CompassID { get; }
        Guid ActivityRecognitionID { get; }
        Guid CarryPositionID { get; }
        Guid GestureRecognitionID { get; }
        Guid BlueVoiceID { get; }
        Guid BlueVoiceSyncID { get; }
        Guid LEDStateConfigID { get; }
        Guid ProximityID { get; }
        Guid UltraVioletID { get; }
        Guid SmokeSensorID { get; }
    }
}
