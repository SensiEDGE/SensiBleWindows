using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace SensiEdge.Device
{
    public class DataSource<T> : ISubscribe, ISource<T> where T : IParse, new ()
    {
        private GattDeviceService Service { get; set; }
        private GattCharacteristic Characteristic { get; set; }
        private Guid UUID { get; set; }
        public bool IsAvailable => !Guid.Empty.Equals(UUID);

        public DataSource(GattDeviceService service, Guid characteristicID)
        {
            Service = service;
            UUID = characteristicID;
        }

        public event OnChange<T> OnChange = null;
        public event OnSubscribe BeforeSubscribe;
        public event OnSubscribe AfterUnsubscribe;

        public async Task<T> GetValue()
        {
            try
            {
                var characteristics = await Service.GetCharacteristicsForUuidAsync(UUID);
                Characteristic = characteristics.Characteristics[0];
                var result = await Characteristic.ReadValueAsync();
                var dataReader = DataReader.FromBuffer(result.Value);
                var output = new byte[result.Value.Length];
                dataReader.ReadBytes(output);
                T data = new T();
                data.Parse(output);
                return data;
            }catch(Exception ex)
            {
                return default(T);
            }
        }
        public async void SetValue(T value)
        {
            try
            {
                var characteristics = await Service.GetCharacteristicsForUuidAsync(UUID);
                Characteristic = characteristics.Characteristics[0];
                var result = await Characteristic.WriteValueAsync(value.ToSetValue().AsBuffer());
            }
            catch { }
        }
        public async void Enable()
        {
            try
            {
                BeforeSubscribe?.Invoke();
                var characteristics = await Service.GetCharacteristicsForUuidAsync(UUID);
                Characteristic = characteristics.Characteristics[0];
                var result = await Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                Characteristic.ValueChanged += (GattCharacteristic sender, GattValueChangedEventArgs args) =>
                {
                    var dataReader = DataReader.FromBuffer(args.CharacteristicValue);
                    var output = new byte[args.CharacteristicValue.Length];
                    dataReader.ReadBytes(output);
                    T data = new T();
                    data.Parse(output);
                    OnChange?.Invoke(data);
                };
            }
            catch { }
        }

        public async void Disable()
        {
            try
            {
                BeforeSubscribe?.Invoke();
                var characteristics = await Service.GetCharacteristicsForUuidAsync(UUID);
                Characteristic = characteristics.Characteristics[0];
                var result = await Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.None);
                Characteristic.ValueChanged += (GattCharacteristic sender, GattValueChangedEventArgs args) =>
                {
                    var dataReader = DataReader.FromBuffer(args.CharacteristicValue);
                    var output = new byte[args.CharacteristicValue.Length];
                    dataReader.ReadBytes(output);
                    T data = new T();
                    data.Parse(output);
                    OnChange?.Invoke(data);
                };
            }
            catch { }
        }
        
        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }
    }
}
