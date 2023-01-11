using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace AzureIoT
{
	public class AzureDevice
	{
        ServiceClient serviceClient;
        string connectionString { set; get; }
        string targetDevice { set; get; }
		public bool IsReady { get; internal set; } = false;
        public AzureDevice(string ConnectionString, string TargetDevice)
		{
			this.connectionString = ConnectionString;
			this.targetDevice = TargetDevice;
			Setup();
		}

		public async Task<bool> SendMessageToDevice(string Message)
		{
			if (!IsReady) return false;
			if (string.IsNullOrEmpty(Message))
			{
				System.Console.WriteLine("Please fill message");
				return false;
			}
            var commandMessage = new Message(Encoding.ASCII.GetBytes(Message));
            await serviceClient.SendAsync(targetDevice, commandMessage);
			return true;
        }

		void Setup()
		{
			try
			{
                serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
				IsReady = true;
            }
            catch (Exception ex)
			{
				System.Console.WriteLine(ex);
				IsReady = false;
			}
        }
	}
}

