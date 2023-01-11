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
        public Form1()
        {
            InitializeComponent();

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
