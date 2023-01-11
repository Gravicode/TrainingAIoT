using BluinoNet;
using BluinoNet.Modules;
using BMC.Drivers.BasicGraphics;
//using Iot.Device.Bmxx80;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace DemoESP32StarterKit
{
    public class Program
    {
        static ESP32StarterKit board;
        public static void Main()
        {
            board = new ESP32StarterKit();

            #region Servo
            //demo servo - uncomment to use. pin d5
            /*
            board.SetupServo(ESP32Pins.IO05);
            var Deg = 0;
            while (true)
            {
                if (Deg > 180) Deg = 0;
                board.BoardServo.Set(Deg);
                Thread.Sleep(500);
                Deg += 10;
            }
            */
            #endregion 
            
            #region LED
            //demo LED - uncomment to use. pin 23, pin 2, pin 4, pin 5
            /*
            board.SetupLed(ESP32Pins.IO23, ESP32Pins.IO02, ESP32Pins.IO04, ESP32Pins.IO05);
            while (true)
            {
                board.BoardLed1.TurnOn();
                Thread.Sleep(200);
                board.BoardLed1.TurnOff();
                Thread.Sleep(200);
                board.BoardLed2.TurnOn();
                Thread.Sleep(200);
                board.BoardLed2.TurnOff();
                Thread.Sleep(200);
                board.BoardLed3.TurnOn();
                Thread.Sleep(200);
                board.BoardLed3.TurnOff();
                Thread.Sleep(200);
                board.BoardLed4.TurnOn();
                Thread.Sleep(200);
                board.BoardLed4.TurnOff();
                Thread.Sleep(200);
            }*/
            #endregion

            #region Button and RGB LED
            //Demo button and RGB LED - button : pin12, pin14. Rgb: pin 2, pin 4, pin 5 - uncomment to use
            /*
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);
            board.SetupLedRgb(ESP32Pins.IO02, ESP32Pins.IO04, ESP32Pins.IO05);
            Random rnd = new Random();
            board.BoardButton1.ButtonReleased += (a, b) => {

                board.BoardLedRgb.SetColorRGB(rnd.Next(1,255), rnd.Next(1, 255), rnd.Next(1, 255));
            };
            */
            #endregion

            #region Display
            //Demo OLED Display -- uncomment to use
            DemoDisplay();
            #endregion

            #region BMP 180
            //Demo BMP 180
            /*
            board.SetupBMP180();
            board.SetupDisplay();
            var colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
            var screen = board.BoardDisplay;

            while (true)
            {

                screen.Clear();
                screen.DrawString("Temperature", colorB, 0, 1, 1, 1);
                screen.DrawString($"{board.BoardBMP180.ReadTemperature().ToString("n2")} C", colorB, 0, 6, 2, 2);
                screen.DrawString("Pressure", colorB, 0, 30, 1, 1);
                screen.DrawString($"{board.BoardBMP180.ReadPressure().ToString("n2")} atm", colorB, 0, 20, 2, 2);
                screen.Flush();
                Thread.Sleep(2000);

                screen.Clear();
                screen.DrawString("Altitude", colorB, 0, 1, 1, 1);
                screen.DrawString($"{board.BoardBMP180.ReadAltitude().ToString("n2")} m", colorB, 0, 6, 2, 2);
                screen.DrawString("Sea Level Pres.", colorB, 0, 30, 1, 1);
                screen.DrawString($"{board.BoardBMP180.ReadSeaLevelPressure().ToString("n2")} atm", colorB, 0, 20, 2, 2);
                screen.Flush();
                Thread.Sleep(2000);
            }
            */
            #endregion

            #region Button, Buzzer and Relay
            /*
            board.SetupBuzzer(ESP32Pins.IO23);
            board.SetupButton(ESP32Pins.IO12, ESP32Pins.IO14);
            board.SetupRelay(ESP32Pins.IO22);

            board.BoardButton1.ButtonReleased += (a, b) => {
                PlaySound();
            };
            board.BoardButton2.ButtonReleased += (a, b) =>
            {
                board.BoardRelay.Toggle();
            };
            */
            #endregion

            #region LDR and PIR
            /*
            board.SetupDisplay();
            board.SetupLightSensor(ESP32Pins.IO36, 0, ESP32ADCs.ADC0); // see https://docs.nanoframework.net/content/esp32/esp32_pin_out.html
            board.SetupPIR(ESP32Pins.IO22);
            var colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
            var screen = board.BoardDisplay;
            board.BoardPIR.MotionCaptured += (x,e) => {
                Debug.WriteLine("motion detected");
            };
            while (true)
            {

                screen.Clear();
                screen.DrawString("Light", colorB, 0, 1, 1, 1);
                screen.DrawString($"{board.BoardLightSensor.GetIlluminance().ToString("n2")} Lux", colorB, 0, 6, 2, 2);
                screen.DrawString("PIR", colorB, 0, 30, 1, 1);
                screen.DrawString($"MOVEMENT: {board.BoardPIR.IsCaptureMovement}", colorB, 0, 20, 2, 2);
                screen.Flush();
                Thread.Sleep(500);

            }
            */
            #endregion

            #region MPU6050
            /*
            board.SetupMpu6050();
            board.SetupDisplay();
            var colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
            var screen = board.BoardDisplay;
            board.BoardMpu6050.StartUpdating();
            board.BoardMpu6050.SensorInterruptEvent += (a, e) => {
                screen.Clear();
                for (int i = 0; i < e.Values.Length; i++)
                {
                    screen.DrawString("ACCEL", colorB, 0, 1, 1, 1);
                    screen.DrawString($"{e.Values[i].AccelerationX.ToString("n3")},{e.Values[i].AccelerationY.ToString("n3")},{e.Values[i].AccelerationZ.ToString("n3")} ", colorB, 0, 10, 1, 1);

                    screen.DrawString("GYRO", colorB, 0, 25, 1, 1);
                    screen.DrawString($"{e.Values[i].GyroX.ToString("n3")},{e.Values[i].GyroY.ToString("n3")},{e.Values[i].GyroZ.ToString("n3")} ", colorB, 0, 35, 1, 1);
                    break;
                }
                screen.Flush();
            };
            */
            
            #endregion

            #region DS18B20
            //Demo DS18B20
            /*
            board.SetupDS18B20(ESP32Pins.IO04);
            board.SetupDisplay();
            var colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
            var screen = board.BoardDisplay;

            while (true)
            {
                board.BoardDS18B20.Read();
                screen.Clear();
                screen.DrawString("Temperature", colorB, 0, 1, 1, 1);
                screen.DrawString($"{board.BoardDS18B20.Temperature.ToString("n2")} C", colorB, 0, 6, 2, 2);
                screen.Flush();
                Thread.Sleep(2000);
            }
            */
            #endregion

            #region Potensiometer
            /*
            board.SetupDisplay();
            board.SetupPotentiometer(ESP32Pins.IO36, 0, ESP32ADCs.ADC0); // see https://docs.nanoframework.net/content/esp32/esp32_pin_out.html
            var colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
            var screen = board.BoardDisplay;
            while (true)
            {

                screen.Clear();
                screen.DrawString("Potensiometer value", colorB, 0, 1, 1, 1);
                screen.DrawString($"{board.BoardPotentiometer.ReadProportion().ToString("n2")}", colorB, 0, 6, 2, 2);
                screen.Flush();
                Thread.Sleep(500);

            }
            */
            #endregion

            #region MicroSD
            /*
            MicroSd.SetupMicroSd(ESP32Pins.IO13);
            MicroSd.MountDrive();
            MicroSd.CreateFile("/text2.txt", "123qwe");
            MicroSd.CreateDirectory("/data/file/");
            MicroSd.RenameFile("/text2.txt", "/text1.txt");
            var contents = MicroSd.ListDirectory();
            */
            #endregion

            #region HCSR04
            //Demo HCSR04
            /*
            board.SetupHCSR04(ESP32Pins.IO13,ESP32Pins.IO12);
            board.SetupDisplay();
            var colorB = BasicGraphics.ColorFromRgb(255, 255, 255);
            var screen = board.BoardDisplay;

            while (true)
            {
                screen.Clear();
                screen.DrawString("Distance", colorB, 0, 1, 1, 1);
                screen.DrawString($"{board.BoardHCSR04.CurrentDistance.ToString("n2")}", colorB, 0, 6, 2, 2);
                screen.Flush();
                Thread.Sleep(2000);
            }
            */
            #endregion

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }


        static void DemoDisplay()
        {
            board.SetupDisplay();
            var basicGfx = board.BoardDisplay;
            var colorBlue = BasicGraphics.ColorFromRgb(0, 0, 255);
            var colorGreen = BasicGraphics.ColorFromRgb(0, 255, 0);
            var colorRed = BasicGraphics.ColorFromRgb(255, 0, 0);
            //var colorWhite = BasicGraphics.ColorFromRgb(255, 255, 255);

            basicGfx.Clear();
            basicGfx.DrawString("BluinoNet", colorGreen, 1, 1, 1, 1);
            basicGfx.DrawString("Kick Ass", colorBlue, 1, 20, 1, 1);
            basicGfx.DrawString("--By BMC--", colorRed, 1, 40, 1, 1);

            Random color = new Random();
            for (var i = 10; i < 100; i++)
                basicGfx.DrawCircle((uint)color.Next(), i, 60, 2);

            basicGfx.Flush();

            Thread.Sleep(3000);
            //bounching balls demo
            var balls = new BouncingBalls(basicGfx);
            Thread.Sleep(Timeout.Infinite);


        }

        //play sound
        #region buzzer
        static ArrayList music = new ArrayList();
        static void PlaySound()
        {

            Tunes.MusicNote note = new Tunes.MusicNote(Tunes.Tone.C4, 400);

            music.Add(note);

            //up
            PlayNote(Tunes.Tone.C4);
            PlayNote(Tunes.Tone.D4);
            PlayNote(Tunes.Tone.E4);
            PlayNote(Tunes.Tone.F4);
            PlayNote(Tunes.Tone.G4);
            PlayNote(Tunes.Tone.A4);
            PlayNote(Tunes.Tone.B4);
            PlayNote(Tunes.Tone.C5);

            // back down
            PlayNote(Tunes.Tone.B4);
            PlayNote(Tunes.Tone.A4);
            PlayNote(Tunes.Tone.G4);
            PlayNote(Tunes.Tone.F4);
            PlayNote(Tunes.Tone.E4);
            PlayNote(Tunes.Tone.D4);
            PlayNote(Tunes.Tone.C4);

            // arpeggio
            PlayNote(Tunes.Tone.E4);
            PlayNote(Tunes.Tone.G4);
            PlayNote(Tunes.Tone.C5);
            PlayNote(Tunes.Tone.G4);
            PlayNote(Tunes.Tone.E4);
            PlayNote(Tunes.Tone.C4);

            //tunes.Play();

            //Thread.Sleep(100);

            PlayNote(Tunes.Tone.E4);
            PlayNote(Tunes.Tone.G4);
            PlayNote(Tunes.Tone.C5);
            PlayNote(Tunes.Tone.G4);
            PlayNote(Tunes.Tone.E4);
            PlayNote(Tunes.Tone.C4);
            var notes = (Tunes.MusicNote[])music.ToArray(typeof(Tunes.MusicNote));
            board.BoardBuzzer.Play(notes);

        }
        static void PlayNote(Tunes.Tone tone)
        {
            Tunes.MusicNote note = new Tunes.MusicNote(tone, 200);

            music.Add(note);
        }
        #endregion

    }



    #region display helper
    public class BouncingBalls
    {
        struct Rectangle
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
            public Rectangle(int x, int y, int width, int height)
            {
                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Height = height;
            }
        }

        struct Point { public int X; public int Y; };
        private Rectangle[] BallLocation;
        private Point[] BallVelocity;
        private BasicGraphics Screen { get; set; }

        public BouncingBalls(SSD1306Imp fullScreen)
        {
            Screen = fullScreen;
            SetupBalls();
            looping = new Thread(new ThreadStart(Loop));
            looping.Start();

        }
        

        Thread looping;
        void Loop()
        {
            while (true)
            {

                MoveBalls();
                DrawBalls();
                if (Screen is SSD1306Imp)
                    ((SSD1306Imp)Screen).Flush();
                Thread.Sleep(1);
            }
        }

        private void SetupBalls()
        {
            Random rand = new Random();
            const int num_balls = 12;
            int vx = 0;
            int vy = 0;

            BallLocation = new Rectangle[num_balls];
            BallVelocity = new Point[num_balls];

            for (int iBall = 0; iBall < num_balls; iBall++)
            {
                int width = rand.Next(4, 10);
                BallLocation[iBall] = new Rectangle
                {
                    X = 1 + rand.Next(Screen.Width - 11),
                    Y = 1 + rand.Next(Screen.Height - 11),
                    Width = width,
                    Height = width
                };
                // Setup 1/2 the balls with different speeds

                if (iBall % 2 == 0)
                {
                    vx = rand.Next(1, 5);
                    vy = rand.Next(1, 5);
                }
                else
                {
                    vx = rand.Next(6, 10);
                    vy = rand.Next(6, 10);
                }

                // Setup random directions
                if (rand.Next(0, 2) == 0) vx = -vx;
                if (rand.Next(0, 2) == 0) vy = -vy;
                BallVelocity[iBall] = new Point { X = vx, Y = vy };
            }
        }

        private void MoveBalls()
        {
            for (int ball_num = 0;
                ball_num < BallLocation.Length;
                ball_num++)
            {
                // Move the ball.
                int new_x = BallLocation[ball_num].X +
                    BallVelocity[ball_num].X;
                int new_y = BallLocation[ball_num].Y +
                    BallVelocity[ball_num].Y;
                if (new_x <= 1)
                {
                    BallVelocity[ball_num].X = -BallVelocity[ball_num].X;
                }
                else if (new_x + BallLocation[ball_num].Width >= Screen.Width - 1)
                {
                    BallVelocity[ball_num].X = -BallVelocity[ball_num].X;
                }
                if (new_y <= 1)
                {
                    BallVelocity[ball_num].Y = -BallVelocity[ball_num].Y;
                }
                else if (new_y + BallLocation[ball_num].Height >= Screen.Height - 1)
                {
                    BallVelocity[ball_num].Y = -BallVelocity[ball_num].Y;
                }

                BallLocation[ball_num].X = new_x;
                BallLocation[ball_num].Y = new_y;
                
            }
        }

        private void DrawBalls()
        {
            Screen.Clear();

            for (int i = 0; i < BallLocation.Length; i++)
            {
                //teal
                Screen.DrawCircle((uint)8421376, BallLocation[i].X, BallLocation[i].Y, BallLocation[i].Height / 2);
            }
            //Screen.Flush();
        }
    }
    public static class Extensions
    {
        public static int Next(this Random rand, int min, int max)
        {
            if (max - min == 0)
                return min;
            return min + rand.Next(max - min);
        }
    }

    #endregion
}
