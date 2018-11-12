# SensiBLEWindows

SensiEdge libruary is intended for communication between SensiEDGE Development Board and Windows 10 applications

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

* Windows 10 version 1709 December 12, 2017 (KB4056342) and upper
* Active BLE 4.0+ module onboard

### Installing

Libruary is available on nuget

```
Install-Package SensiBle -Version 1.0.0
```

### Basical Usage in Custom Application

Libruary may used in user's application 
[Part 1](https://drive.google.com/file/d/1zdoHZE6-nip1_sDNdB8GMCDlSQi7un14/view?usp=sharing)
[Part 2](https://drive.google.com/file/d/1M2o3B2ZJKC1Rn4feiKv6KKx54ZWg9ISc/view?usp=sharing)

#### Main cases needed to be done

Find SensiBLE Device with BleWatcher

```
void Find()
{
  BleWatcher.Changed += BleWatcher_Changed;
  BleWatcher.StartWatch();
}

void BleWatcher_Changed()
{
  var Devices = BleWatcher.Devices;
  //Process needed device
}
```

Create and connect to BLE Deveice and set datasource enabled
For example, Enviromental

```
async void Connect(DeviceInformation info)
{
  var device = await DeviceFactory.Get(info.Id);
  var source = device.EnvironmentalSource;
  source.OnChange += Source_OnChange;
  source.Enable();   
}
void Source_OnChange(Environmental value)
{
  var Pressure = value.Pressure;
  //Process other variables
}
```

After using sources don't forget deacivate them

```
source.Disable();   
```
```
BleWatcher.StopWatch();
```
## Deployment

[This video](https://drive.google.com/file/d/10YzZxabAXPjlkSdLYAwwHWe2F1CnvQCT/view?usp=sharing) shows how the libruary could be used in your's applications

## Cloud

Data from sensors could be sent to cloud, for example [Azure](https://drive.google.com/file/d/1mUEKh2GYpDrWK4uy7zb7cXQmK3alB26f/view?usp=sharing)

## Built With

* [Visual Studio](https://visualstudio.microsoft.com) - IDE used
* [.NET Framework](https://www.microsoft.com/net/) - Main framework
* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - Used for code implementation

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/SensiEDGE/SensiBleWindows/tags).

## Builds

* [Version 1.1.0](https://drive.google.com/file/d/1w0iwY89-oqBvyxjW6swtAOu4lVATaLov/view?usp=sharing)

## Authors

* **Andrey Nosov** - [Andrey Nosov](https://github.com/andreynosov2)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details