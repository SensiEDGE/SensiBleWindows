using SensiEdge.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensiEdge.Device
{
    public static class Dictionaries
    {
        public static Dictionary<SensorType, Guid> Sensors { get; } = new Dictionary<SensorType, Guid>
            {
                {  SensorType.EnvironmentalID, new Guid("001c0000-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.AccGyroMagID, new Guid("00e00000-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.AccelerometerEventsID, new Guid("00000400-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.AudioLevelID, new Guid("04000000-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.LEDStateID, new Guid("20000000-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.LightSensorID, new Guid("01000000-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.BatteryStatusID, new Guid("00020000-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.OrientationID, new Guid("00000100-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.CompassID, new Guid("00000040-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.ActivityRecognitionID, new Guid("00000010-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.CarryPositionID, new Guid("00000008-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.GestureRecognitionID, new Guid("00000002-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.BlueVoiceID, Guid.Empty},
                {  SensorType.BlueVoiceSyncID, Guid.Empty},
                {  SensorType.LEDStateConfigID, new Guid("00000002-000f-11e1-ac36-0002a5d5c51b")},
                {  SensorType.ProximityID, new Guid("02000000-0001-11e1-ac36-0002a5d5c51b")},
                {  SensorType.UltraVioletID, new Guid("11111111-0002-11e1-ac36-0002a5d5c51b")},
                {  SensorType.SmokeSensorID, new Guid("8ef07f96-b69c-4acf-a27d-873fc0b611b0")}
            };
    }
}