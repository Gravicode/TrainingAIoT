using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataReceiver
{
    internal class Program
    {
        private const string DEFAULT_CONSUMER_GROUP = "$Default";
        static async Task Main(string[] args)
        {
            HubProcessorService service = new HubProcessorService();
            service.MessageReceived += (a, msg) => {
                Console.WriteLine("data :" + msg.Message); 
                var obj = JsonSerializer.Deserialize<SensorData>(msg.Message);
                if (obj != null)
                {
                    //add to database
                }
            };
            await service.Start("myFirstDevice", "HostName=BmcIoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=1hyvQvLk7mWsQmBJ/MuTxWqiwr1nqtNnBs6gNqspta4=", DateTime.Now, DEFAULT_CONSUMER_GROUP);
            Console.ReadLine();
        }
    }
}
