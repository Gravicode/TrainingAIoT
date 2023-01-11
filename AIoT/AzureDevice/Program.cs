
using nanoFramework.Networking;
using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using nanoFramework.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Device.Wifi;
using SASBuilder;
using System.Text;
using nanoFramework.M2Mqtt;
using BluinoNet;
using BMC.Drivers.BasicGraphics;

namespace AzureDevice
{
    public class Program
    {
        const string Ssid = "Wifidadap14";//"WholeOffice";
        const string Password = "123qweasd";
        static ESP32StarterKit board;
        
        static uint colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
        static bool Connected = false;
        static SSD1306Imp screen;
       
        public static void Main()
        {
            //init board and peripheral
            board = new ESP32StarterKit();
            
            board.SetupLightSensor(ESP32Pins.IO36, 0, ESP32ADCs.ADC0);
            //RGB - PIN : 2,4,8
            board.SetupLed(ESP32Pins.IO02, ESP32Pins.IO04, ESP32Pins.IO05, ESP32Pins.IO23);
            board.SetupBMP180();
            board.SetupDisplay();
            board.SetupRelay(ESP32Pins.IO15);
            var colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
            screen = board.BoardDisplay;

            ConnectToWifi();

            var caCert = new X509Certificate(Resource1.GetBytes(Resource1.BinaryResources.AzureRoot));

            var iotHubName = "BmcIoTHub.azure-devices.net";
            var iotHubPort = 8883;

            // device/client information
            const string deviceId = "myFirstDevice";

            var username = string.Format("{0}/{1}", iotHubName, deviceId);

            // Data time is important for calculate expire time
            //SystemTime.SetTime(new DateTime(2023, 1, 10));
            
            var sas = new SharedAccessSignatureBuilder()
            {
                Key = "1hyvQvLk7mWsQmBJ/MuTxWqiwr1nqtNnBs6gNqspta4=",
                KeyName = "iothubowner",
                Target = "BmcIoTHub.azure-devices.net",
                TimeToLive = TimeSpan.FromDays(365) // at least 1 day.
            };
            
            // define topics
            var topicDeviceToServer =
            string.Format("devices/{0}/messages/events/", deviceId);

            var topicService2Device =
            string.Format("devices/{0}/messages/devicebound/#", deviceId);

            try
            {

                var client = new MqttClient(iotHubName, iotHubPort, true, caCert, null, MqttSslProtocols.TLSv1_2);

                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;


                //var sas = "SharedAccessSignature sr=BmcIoTHub.azure-devices.net&sig=uhce%2BkWkx5k299CKCqQbhe5DT9EyzrISTEczzSznA%2BE%3D&se=1704889619&skn=iothubowner";
                var returnCode = client.Connect(deviceId, username, sas.ToSignature());

                if (returnCode != nanoFramework.M2Mqtt.Messages.MqttReasonCode.Success)
                    throw new Exception("Could not connect!");

                

                client.Subscribe(new string[] { topicService2Device }, new nanoFramework.M2Mqtt.Messages.MqttQoSLevel[]
                    {  nanoFramework.M2Mqtt.Messages.MqttQoSLevel.ExactlyOnce });

                client.Subscribe(new string[] { topicDeviceToServer }, new nanoFramework.M2Mqtt.Messages.MqttQoSLevel[]
                    {  nanoFramework.M2Mqtt.Messages.MqttQoSLevel.ExactlyOnce });
                var data = new SensorData();
                var json = string.Empty;
                while(true)
                {
                    data.TimeStamp = DateTime.UtcNow;
                    data.Pressure = board.BoardBMP180.ReadPressure();
                    data.Temp = board.BoardBMP180.ReadTemperature();
                    data.Light = board.BoardLightSensor.GetIlluminance();
                    json = JsonConvert.SerializeObject(data);
                    client.Publish(topicDeviceToServer, Encoding.UTF8.GetBytes(json));
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }


            Thread.Sleep(Timeout.InfiniteTimeSpan);
        }

        private static void Client_MqttMsgPublishReceived(object sender, nanoFramework.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            var obj = (DeviceAction)JsonConvert.DeserializeObject(msg, typeof(DeviceAction));
            if (screen != null && obj!=null)
            {
                screen.Clear();
                screen.DrawString("Action:", colorB, 0, 1, 1, 1);
                screen.DrawString(obj.Action, colorB, 0, 6, 1, 1); 
                screen.DrawString("Param1:", colorB, 0, 12, 1, 1);
                screen.DrawString(obj.Param1, colorB, 0, 18, 1, 1);
                screen.DrawString("Param2:", colorB, 0, 24, 1, 1);
                screen.DrawString(obj.Param2, colorB, 0, 30, 1, 1);
                screen.Flush();

                switch (obj.Action)
                {
                    case "LIGHT":
                        if (obj.Param1 == "ON")
                        {
                            switch (obj.Param2)
                            {
                                case "R":
                                    board.BoardLed1.TurnOn();
                                    break;
                                case "G":
                                    board.BoardLed2.TurnOn();
                                    break;
                                case "B":
                                    board.BoardLed3.TurnOn();
                                    break;
                            }
                        }
                        else
                        {
                            switch (obj.Param2)
                            {
                                case "R":
                                    board.BoardLed1.TurnOff();
                                    break;
                                case "G":
                                    board.BoardLed2.TurnOff();
                                    break;
                                case "B":
                                    board.BoardLed3.TurnOff();
                                    break;
                            }
                        }
                        break;
                    case "RELAY":
                        if (obj.Param1 == "ON")
                        {
                            board.BoardRelay.TurnOn();
                        }
                        else
                        {
                            board.BoardRelay.TurnOff();
                        }
                        break;
                }
            }
            Debug.WriteLine("Received message: " + msg);

        }

        #region Wifi
        /// <summary>
        /// Event handler for when Wifi scan completes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            Debug.WriteLine("Wifi_AvailableNetworksChanged - get report");

            // Get Report of all scanned Wifi networks
            WifiNetworkReport report = sender.NetworkReport;

            // Enumerate though networks looking for our network
            foreach (WifiAvailableNetwork net in report.AvailableNetworks)
            {
                // Show all networks found
                Debug.WriteLine($"Net SSID :{net.Ssid},  BSSID : {net.Bsid},  rssi : {net.NetworkRssiInDecibelMilliwatts.ToString()},  signal : {net.SignalBars.ToString()}");

                // If its our Network then try to connect
                if (net.Ssid == Ssid)
                {
                    // Disconnect in case we are already connected
                    sender.Disconnect();

                    // Connect to network
                    WifiConnectionResult result = sender.Connect(net, WifiReconnectionKind.Automatic, Password);

                    // Display status
                    if (result.ConnectionStatus == WifiConnectionStatus.Success)
                    {
                        Debug.WriteLine("Connected to Wifi network");
                        Connected = true;
                    }
                    else
                    {
                        Debug.WriteLine($"Error {result.ConnectionStatus.ToString()} connecting o Wifi network");
                    }
                }
            }
        }
        static void ConnectToWifi()
        {
            try
            {
                // Get the first WiFI Adapter
                WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];

                // Set up the AvailableNetworksChanged event to pick up when scan has completed
                wifi.AvailableNetworksChanged += Wifi_AvailableNetworksChanged;

                // give it some time to perform the initial "connect"
                // trying to scan while the device is still in the connect procedure will throw an exception


                // Loop forever scanning every 30 seconds
                while (!Connected)
                {
                    try
                    {
                        Debug.WriteLine("starting Wi-Fi scan");
                        wifi.ScanAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failure starting a scan operation: {ex}");
                    }

                    Thread.Sleep(15_000);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("message:" + ex.Message);
                Debug.WriteLine("stack:" + ex.StackTrace);
            }
        }
        #endregion

    }
    #region Data Model
    public class SensorData
    {
        public double Temp { get; set; }
        public double Light { get; set; }
        public double Pressure { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    public class DeviceAction
    {
        public string Action { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
    }
    #endregion
}
