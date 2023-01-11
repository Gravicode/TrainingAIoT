using AzureIoT;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Traffic_Signal
{
    public partial class Form1 : Form
    {
        AzureDevice device;
        public Form1()
        {
            InitializeComponent();

            device = new AzureDevice("HostName=BmcIoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=1hyvQvLk7mWsQmBJ/MuTxWqiwr1nqtNnBs6gNqspta4=", "myFirstDevice");

            RedStopLight.Visible = true;
            YellowStopLight.Visible = false;
            GreenStopLight.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (RedStopLight.Visible==true)
            {
                RedStopLight.Visible = false;
                YellowStopLight.Visible = false;
                GreenStopLight.Visible = true;
                
            }
            else if(GreenStopLight.Visible==true)
            {
                RedStopLight.Visible = false;
                YellowStopLight.Visible = true;
                GreenStopLight.Visible = false;

            }
            else if(YellowStopLight.Visible==true)
            {
                RedStopLight.Visible = true;
                YellowStopLight.Visible = false;
                GreenStopLight.Visible = false;

            }
            //green
            var json = JsonConvert.SerializeObject(new DeviceAction() { Action = "LIGHT", Param1 = GreenStopLight.Visible ? "ON" : "OFF", Param2 = "G" });
            device.SendMessageToDevice(json);
            //
            json = JsonConvert.SerializeObject(new DeviceAction() { Action = "LIGHT", Param1 = RedStopLight.Visible ? "ON" : "OFF", Param2 = "R" });
            device.SendMessageToDevice(json);

            json = JsonConvert.SerializeObject(new DeviceAction() { Action = "LIGHT", Param1 = YellowStopLight.Visible ? "ON" : "OFF", Param2 = "Y" });
            device.SendMessageToDevice(json);

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

        }
    }
}
