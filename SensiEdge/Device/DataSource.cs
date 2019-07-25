using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using GattValue = Windows.Devices.Bluetooth.GenericAttributeProfile.GattClientCharacteristicConfigurationDescriptorValue;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth;

namespace SensiEdge.Device
{
    public class DataSource<T> : ISubscribe, ISource<T> where T : IParse, new()
    {
        public GattCharacteristic Characteristic { get; private set; }
        public bool IsAvailable => Characteristic != null;

        public DataSource(GattCharacteristic characteristic)
        {
            Characteristic = characteristic;
        }

        public event OnChange<T> OnChange = null;
        public event OnSubscribe BeforeSubscribe;
        public event OnSubscribe AfterUnsubscribe;

        public async Task<T> GetValue()
        {
            try
            {
                var result = await Characteristic.ReadValueAsync(BluetoothCacheMode.Uncached);
                var dataReader = DataReader.FromBuffer(result.Value);
                var output = new byte[result.Value.Length];
                dataReader.ReadBytes(output);
                T data = new T();
                data.Parse(output);
                return data;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        public async void SetValue(T value)
        {
            try
            {
                var result = await Characteristic.WriteValueAsync(value.ToSetValue().AsBuffer());
            }
            catch { }
        }
        public async void Enable()
        {
            try
            {
                BeforeSubscribe?.Invoke();
                var result = await Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattValue.Notify);
                Characteristic.ValueChanged += ValueChanged;
            }
            catch (Exception ex)
            { }
        }

        public async void Disable()
        {
            try
            {
                BeforeSubscribe?.Invoke();
                var result = await Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattValue.None);
                Characteristic.ValueChanged -= ValueChanged;
            }
            catch { }
        }

        public void ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var dataReader = DataReader.FromBuffer(args.CharacteristicValue);
            var output = new byte[args.CharacteristicValue.Length];
            dataReader.ReadBytes(output);
            T data = new T();
            data.Parse(output);
            OnChange?.Invoke(data);
        }

        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            try
            {
                Characteristic?.Service?.Dispose();
            }
            catch (ObjectDisposedException) { }
        }
    }
}
