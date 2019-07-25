using System;
using System.Threading;
using IBMWIoTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SensiEdgeTest
{
    [TestClass]    
    public class IBMW_Test
    {
        private static DeviceClient deviceClient;
        [TestMethod]
        public void Push()
        {
            string orgId = "a2dkar";
            string deviceType = "BlE";
            string deviceId = "ble2";
            string authToken = "ble2_121";

            deviceClient = new DeviceClient(orgId, deviceType, deviceId, "token", authToken);

            try
            {
                deviceClient.connect();
                deviceClient.subscribeCommand("testcmd", "json", 0);
                deviceClient.commandCallback += processCommand;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex:" + ex.Message);
            }
            for (int i = 0; i < 10; i++)
            {
                string data = "{\"temp\":" + (i * 5) + "}";
                Console.WriteLine(data);
                deviceClient.publishEvent("test", "json", data, 0);
                Thread.Sleep(1000);
            }
            deviceClient.disconnect();
        }
        public static void processCommand(string cmdName, string format, string data)
        {
            Console.WriteLine("Sample Device Client : Sample Command " + cmdName + " " + "format: " + format + "data: " + data);
        }
    }
}