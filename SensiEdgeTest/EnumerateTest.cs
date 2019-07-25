using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensiEdge;
using SensiEdge.Device;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Navigation;
namespace SensiEdgeTest
{
    public class DevServices
    {
        public string Name { get; set; }
        public List<Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService> gattDeviceServices;
        public DevServices()
        {
            gattDeviceServices = new List<Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService>();
        }
    }
    [TestClass]
    public class EnumerateTest
    {
        private DeviceWatcher _deviceWatcher;
        private BluetoothLEAdvertisementWatcher watcher;
        [TestMethod]
        public void TestEnumerate()
        {
            BleWatcher.Changed += () =>
            {
                foreach (var device in BleWatcher.Devices)
                {
                    //Debug.WriteLine(device.Value.Advertisement.LocalName);
                }
            };
            BleWatcher.StartWatch();
            Thread.Sleep(10000);
            BleWatcher.StopWatch();
        }

        private void Factory_OnDeviceAdded(DeviceInformation deviceInformation)
        {
            Debug.WriteLine(deviceInformation.Name);
            //var connection = new BLEConnection(deviceInformation.Id);
            var Device = BluetoothLEDevice.FromIdAsync(deviceInformation.Id).GetAwaiter().GetResult();
            var items = Device.GetGattServicesAsync().GetResults().Services;
            //var item = items[0].OpenAsync(Windows.Devices.Bluetooth.GenericAttributeProfile.GattSharingMode);
        }

        [TestMethod]
        public void TestFindAll()
        {
            var devices =  DeviceInformation.FindAllAsync(
               $"{BluetoothLEDevice.GetDeviceSelectorFromPairingState(true)} OR " +
               $"{BluetoothLEDevice.GetDeviceSelectorFromPairingState(false)}").AsTask().GetAwaiter().GetResult();

            foreach (var device in devices)
                Debug.WriteLine(device.Name);
        }
        string[] additionalProperties =
            {
                "System.Devices.Aep.CanPair",
                "System.Devices.Aep.IsConnected",
                "System.Devices.Aep.IsPresent",
                "System.Devices.Aep.IsPaired"
            };
        
        [TestMethod]
        public void TestAdvertisement()
        {
            List<DevServices> devServices = new List<DevServices>();
            // _deviceWatcher = DeviceInformation.CreateWatcher(
            //             BluetoothLEDevice.GetDeviceSelectorFromPairingState(true),
            //             additionalProperties,
            //             DeviceInformationKind.AssociationEndpoint);
            // _deviceWatcher.Added += Added;
            // _deviceWatcher.Updated += Updated;
            // _deviceWatcher.Removed += Removed;
            // _deviceWatcher.EnumerationCompleted += EnumerationCompleted;
            //// _deviceWatcher.Stopped += Stopped;
            // _deviceWatcher.Start();
            //color.ToString();
            //DevicePicker picker = new DevicePicker();
            //picker.Filter.SupportedDeviceSelectors.Add(
            //BluetoothLEDevice.GetDeviceSelectorFromPairingState(false));
            //picker.Filter.SupportedDeviceSelectors.Add(
            //BluetoothLEDevice.GetDeviceSelectorFromPairingState(true));
            //var a = picker;
            //Publish();
            BluetoothLEAdvertisementWatcher BleWatcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };
            

            BleWatcher.Received += async (w, btAdv) => {
                var z=btAdv.Advertisement.ServiceUuids.ToList();
               
                var device = await BluetoothLEDevice.FromBluetoothAddressAsync(btAdv.BluetoothAddress);
                DeviceInformation deviceInformation = device.DeviceInformation;
                
                Dictionary<string, Guid> keyValuePairs = new Dictionary<string, Guid>();
                ConcurrentDictionary<string,Guid> keyValues = new ConcurrentDictionary<string,Guid>();
                if (device != null)
                {
                    var services = await device.GetGattServicesForUuidAsync(new Guid("001c0000-0001-11e1-ac36-0002a5d5c51b"));
                    //GattDeviceService service;
                    //service = services.Services[0];
                    // SERVICES!!
                    var gatt = await device.GetGattServicesAsync();
                    // Debug.WriteLine($"{device.Name} Services: {gatt.Services.Count}, {gatt.Status}, {gatt.ProtocolError}");

                    // CHARACTERISTICS!!
                    var characs = gatt.Services.ToList();
                    foreach (var i in characs)
                    {
                        var asm = await i.GetCharacteristicsAsync();

                        var rrr = asm.Characteristics.ToList();
                        foreach (var j in rrr)
                        {
                            if (Dictionaries.Sensors.Values.Contains(j.Uuid))
                            {
                                //keyValuePairs.Add(Dictionaries.Sensors.FirstOrDefault(m => m.Value == j.Uuid).Key, j.Uuid);
                                //keyValues.TryAdd(Dictionaries.Sensors.FirstOrDefault(m => m.Value == j.Uuid).Key, j.Uuid);
                            }
                        }

                    }
                   
                    //var asm = await characs[2].GetCharacteristicsAsync();
                    //string ss = "";
                    //var rrr = asm.Characteristics;
                    //var awftre = asm.Characteristics.ToList();
                    //for(int i=0;i<rrr.Count; i++)
                    //{
                    //    if (AllSensors.Sensors.Values.Contains(rrr[i].Uuid))
                    //    {
                    //        keyValuePairs.Add(AllSensors.Sensors.FirstOrDefault(m => m.Value == rrr[i].Uuid).Key, rrr[i].Uuid);
                    //        keyValues.TryAdd(AllSensors.Sensors.FirstOrDefault(m => m.Value == rrr[i].Uuid).Key, rrr[i].Uuid);
                    //        ss += rrr[i].Uuid + "; ";
                    //    }
                    //}
                    var ajkfj = keyValuePairs;
                    var ewfawe = keyValues;
                    
                    //devServices.Add(new DevServices { Name = device.Name, gattDeviceServices = characs });

                }
                //var charac = characs.Single(c => c.Uuid == SAMPLECHARACUUID);
                //await charac.WriteValueAsync(SOMEDATA);
            };
            BleWatcher.Start();
            ////thread.Start();
            Thread.Sleep(10000);
            var a = devServices;
            var n = 0;
        }
        string temp = "";
        private async void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var _args = args;
            var device = await BluetoothLEDevice.FromBluetoothAddressAsync(_args.BluetoothAddress);
            var data = args.Advertisement.ManufacturerData;
            var a = sender.AdvertisementFilter.Advertisement.LocalName;
            temp += args.BluetoothAddress+"; ";
        }
        private void Publish()
        {
            BluetoothLEAdvertisementPublisher publisher = new BluetoothLEAdvertisementPublisher();

            // Add custom data to the advertisement
            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;

            var writer = new DataWriter();
            writer.WriteString("Hello World");

            // Make sure that the buffer length can fit within an advertisement payload (~20 bytes). 
            // Otherwise you will get an exception.
            manufacturerData.Data = writer.DetachBuffer();

            // Add the manufacturer data to the advertisement publisher:
            publisher.Advertisement.ManufacturerData.Add(manufacturerData);

            publisher.Start();
        }

        private async void Added(DeviceWatcher sender, DeviceInformation deviceInformation)
        {
            var a = 5;
            
        }

        private async void Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInformationUpdate)
        {
           
        }

        private async void Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInformationUpdate)
        {
           
        }

        public void Start()
        {
            _deviceWatcher.Start();
        }

        public void Stop()
        {
            if (_deviceWatcher != null)
            {
                // Unregister the event handlers.
                _deviceWatcher.Added -= Added;
                _deviceWatcher.Updated -= Updated;
                _deviceWatcher.Removed -= Removed;
                _deviceWatcher.EnumerationCompleted -= EnumerationCompleted;
                

                // Stop the watcher.
                _deviceWatcher.Stop();
                _deviceWatcher = null;
            }
        }
        private void EnumerationCompleted(DeviceWatcher sender, object obj)
        {
            
        }

    }
}
