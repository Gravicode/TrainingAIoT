using BluinoNet.Modules;
using BMC.Drivers.BasicGraphics;
using System;
//
using System.Device.I2c;
using System.Text;

namespace BluinoNet
{
    public class SSD1306Imp : BasicGraphics, IDisposable
    {

        //SSD1306 screen;
        SSD1306 screen;
        public SSD1306Imp(int I2cBusId=1) : base(128, 64, ColorFormat.OneBpp)
        {
            var con = SSD1306.GetConnectionSettings(I2cBusId);
            I2cDevice dev = new I2cDevice(con);
            screen = new SSD1306(dev); 
        }
       
        public void Flush()
        {
            screen.DrawBufferNative(this.Buffer);
            //do nothing
        }

        public void Dispose()
        {
            screen.Dispose();
            //do nothing
        }


        public Image CreateImage(
          int width,
          int height,
          double[] data,
          int hScale,
          int vScale,
          Image.Transform transform)
        {
          
            return new Image(data, width, height, hScale, vScale, transform);
        }

        public Image CreateImage(
          int width,
          int height,
          string data,
          int hScale,
          int vScale,
          Image.Transform transform)
        {
           
            return new Image(data, width, height, hScale, vScale, transform);
        }

        public void DrawImage(Image img, int x, int y)
        {
            int num = 0;
            for (int index1 = 0; index1 < img.Height; ++index1)
            {
                for (int index2 = 0; index2 < img.Width; ++index2)
                    this.SetPixel(x + index2, y + index1, (uint)img.Data[num++]);
            }
        }
    }
}
