﻿using nanoFramework.Device.OneWire;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace BluinoNet.Modules
{/// <summary>
 /// DS18B20 Programmable Resolution 1-Wire Digital Thermometer
 /// </summary>
    public class DS18B20
    {
        public const byte FamilyCode = 0x28;

        // Commands
        public const byte ConvertT = 0x44;
        public const byte CopyScratchpad = 0x48;
        public const byte WriteScratchpad = 0x4E;
        public const byte ReadPowerSupply = 0xB4;
        public const byte RecallE2 = 0xB8;
        public const byte ReadScratchpad = 0xBE;
        public const byte SkipROM = 0xcc;
        public const byte MatchID = 0x55;
        // Helpers
        public static float GetTemperature(byte tempLo, byte tempHi)
        {
            return ((short)((tempHi << 8) | tempLo)) / 16F;
        }

        public DS18B20(int oneWirePin)
        {
           var oneWire = new OneWireHost();
            ////////////////////////////////////////////////////////////////////
            // Sample code to read serial number for a devices present in bus
            // 
            // With this approach, three steps are needed:
            // - Reset 1-Wire bus
            // - Transmit Read ROM command
            // - Read 8 bytes (serial number)
            ////////////////////////////////////////////////////////////////////
            
            byte[] state = new byte[13];
            // check for devices present with a bus reset
            if (oneWire.TouchReset())
            {
                Debug.WriteLine("Device present");

                // tx READ ROM
                var res = oneWire.WriteByte(0x33);

                // now read 8 byte for SN
                // and output serial number nicelly formated in hexa
                for (int i = 0; i < 8; i++)
                {
                    // read byte
                    state[i] = oneWire.TouchByte(0xFF);

                    // output byte
                    Debug.Write(state[i].ToString("X2"));
                }

                Debug.WriteLine("");
            }
            else
            {
                Debug.WriteLine("!! No devices found !!");
            }

            ////////////////////////////////////////////////////////////////////
            // Sample code to read serial number for a devices present in bus
            // 
            // Just call FindFirstDevice and it will fill OneWire SerialNumber 
            // buffer with the 1-Wire device serial number present in bus
            ////////////////////////////////////////////////////////////////////
            byte[] deviceId = null;
            if (oneWire.FindFirstDevice(true, false))
            {
                Debug.WriteLine("device found:");
                deviceId = oneWire.SerialNumber;
                // output serial number nicelly formated in hexa
                for (int i = 0; i < 8; i++)
                {
                    Debug.Write(oneWire.SerialNumber[i].ToString("X2"));
                }

                Debug.WriteLine("");
            }
            else
            {
                Debug.WriteLine("!! No devices found !!");
            }
            /*
            ////////////////////////////////////////////////////////////////////
            // Sample code to read serial number for all devices present in bus
            // 
            // Just call FindAllDevices and it will return an array list with 
            // all 1-Wire devices serial numbers present in bus
            ////////////////////////////////////////////////////////////////////

            ArrayList snList = oneWire.FindAllDevices();

            if (snList.Count > 0)
            {
                Debug.WriteLine(String.Format("{0} devices found", snList.Count));

                foreach (byte[] sn in snList)
                {
                    // output serial number nicelly formated in hexa
                    for (int i = 0; i < 8; i++)
                    {
                        Debug.Write(sn[i].ToString("X2"));
                    }

                    Debug.WriteLine("");
                }
            }
            else
            {
                Debug.WriteLine("!! No devices found !!");
            }*/
            OneWire = oneWire;
            _deviceId = deviceId;
        }

        /// <summary>
        /// The device ID.
        /// </summary>
        byte[] _deviceId;

        /// <summary>
        /// The one wire bus.
        /// </summary>
        public OneWireHost OneWire { get; private set; }

        /// <summary>
        /// The last read temperature.
        /// </summary>
        public float Temperature { get; private set; }

        /// <summary>
        /// Reads the latest temperature from the device.
        /// </summary>
        public void Read()
        {
            //if (_deviceId[0] == DS18B20.FamilyCode)
            {
                OneWire.TouchReset();
                OneWire.WriteByte(SkipROM); // Address all devices
                OneWire.WriteByte(DS18B20.ConvertT);
                Thread.Sleep(750);  // Wait Tconv (for default 12-bit resolution)

                // Write command and identifier at once
                //var matchRom = new byte[9];
                //Array.Copy(_deviceId, 0, matchRom, 1, 8);
                //matchRom[0] = MatchID;

                OneWire.TouchReset();
                //for (var index = 0; index < matchRom.Length; index++)
                //{
                //    OneWire.WriteByte(matchRom[index]);
                //}
                OneWire.WriteByte(SkipROM);
                OneWire.WriteByte(DS18B20.ReadScratchpad);

                // Read just the temperature (2 bytes)
                var tempLo = OneWire.ReadByte();
                var tempHi = OneWire.ReadByte();
                Temperature = GetTemperature(tempLo, tempHi);
            }
        }
    }
    /*
    public class DS18B20
    {
        object OneWireInstance = new object();
        #region Enums

        /// <summary>
        ///     Type of one wire buses allowed.
        /// </summary>
        public enum BusModeType
        {
            /// <summary>
            ///     Indicate that the OneWire bus has a single device attached.
            /// </summary>
            SingleDevice,

            /// <summary>
            ///     Indicate that the OneWire bus has multiple devices attached.
            /// </summary>
            MultimpleDevices
        }

        #endregion Enums

        #region Commands class

        /// <summary>
        ///     Constants representing the various commands that can be issued to
        ///     the DS18B20.
        /// </summary>
        protected class Commands
        {
            /// <summary>
            ///     Start the A to D conversion of the current temperature into
            ///     digital value.
            /// </summary>
            public const byte StartConversion = 0x44;

            /// <summary>
            ///     Read bytes from the scratch pad.
            /// </summary>
            /// <remarks>
            ///     This command can be used to read the temperature, alarms and
            ///     configuration bytes from the scratch pad.
            /// </remarks>
            public const byte ReadScratchPad = 0xbe;

            /// <summary>
            ///     Write data to the scratch pad.
            /// </summary>
            /// <remarks>
            ///     Write the temperature high, and low alarms along with the device
            ///     configuration byte to the scratch pad.
            ///
            ///     Note that all three bytes must be written with this command in order
            ///     to prevent the data from being corrupted.
            /// </remarks>
            public const byte WriteScratchPad = 0x4e;

            /// <summary>
            ///     Issue the following command(s) to all devices on the bus.
            /// </summary>
            /// <remarks>
            ///     This command is useful in two cases:
            ///         - Writing the same command to all devices (say StartConversion)
            ///         - When there is only one device on the bus, the device ID does
            ///           not have to be written.
            /// </remarks>
            public const byte SkipROM = 0xcc;

            /// <summary>
            ///     Retrieve the device ID.
            /// </summary>
            /// <remarks>
            ///     This is useful when there is only one device on the bus and allows
            ///     the device ID to be retrieved.
            /// </remarks>
            public const byte ReadID = 0x33;

            /// <summary>
            ///     Inform the devices on the bus that the following commands (up to a reset)
            ///     are to be processed by a specific device.
            /// </summary>
            /// <remarks>
            ///     The device ID (64-bits) should follow this command.
            /// </remarks>
            public const byte MatchID = 0x55;

            /// <summary>
            ///     Copy the alarm high, alarm low and configuration bytes from the
            ///     scratch pad into the EEPROM.
            /// </summary>
            /// <remarks>
            ///     This can be used to store the data in to the EEPROM to survive a
            ///     power on reset.
            /// </remarks>
            public const byte CopyScratchPadToEEPROM = 0x48;

            /// <summary>
            ///     Copy the alarm high, alarm low and configuration bytes from the
            ///     EEPROM into the scratch pad.
            /// </summary>
            /// <remarks>
            ///     Copy the stored data from the EEPROM into the scratch pad.
            /// </remarks>
            public const byte CopyEEPROMToScratchPad = 0xb8;
        }

        #endregion Command class

        #region ScratchPad class

        /// <summary>
        ///     Class holding the constants defining the layout of the DS18B20 Scratch Pad.
        /// </summary>
        protected class ScratchPad
        {
            /// <summary>
            ///     Lower 8 bits of the temperature following a conversion.
            /// </summary>
            /// <remarks>
            ///     At power on this will be set to 0x50.
            /// </remarks>
            public const byte TemperatureLow = 0;

            /// <summary>
            ///     Upper 8 bits of the temperature following a conversion.
            /// </summary>
            /// <remarks>
            ///     At power on this will be set to 0x05.
            /// </remarks>
            public const byte TemperatureHigh = 1;

            /// <summary>
            ///     Temperature high alarm (or user byte 1).
            /// </summary>
            public const byte HighAlarm = 2;

            /// <summary>
            ///     Temperature low alarm (or user byte 2).
            /// </summary>
            public const byte LowAlarm = 3;

            /// <summary>
            ///     Configuration byte.
            /// </summary>
            public const byte Configuration = 4;

            /// <summary>
            ///     Reserved byte (number 5) usually set to 0xff.
            /// </summary>
            public const byte ReservedByte5 = 5;

            /// <summary>
            ///     Reserved byte (number 6).
            /// </summary>
            public const byte ReservedByte6 = 6;

            /// <summary>
            ///     Reserved byte (number 7) usually set to 0x10.
            /// </summary>
            public const byte Reserved7 = 7;

            /// <summary>
            ///     CRC byte.
            /// </summary>
            /// <remarks>
            ///     Polynomial for the CRC is X^8 + X^5 + X^4 + 1
            /// </remarks>
            public const byte CRC = 8;
        }

        #endregion ScratchPad class

        #region Constants

        /// <summary>
        ///     Minimum value that should be used for the polling frequency.
        /// </summary>
        /// <remarks>>
        ///     Default assumes that the sensor is working in 12-bit mode (factory setting).
        /// </remarks>
        public const ushort MinimumPollingPeriod = 750;

        /// <summary>
        ///     Number of bytes in the scratch pad.
        /// </summary>
        public const byte ScratchPadSize = 9;

        /// <summary>
        ///     Number of bytes (including the CRC) in the device ID array.
        /// </summary>
        public const byte DeviceIDLength = 8;

        #endregion Constants

        #region Member variables

        /// <summary>
        ///     Update interval in milliseconds
        /// </summary>
        private ushort _updateInterval = 100;

        #endregion Member variables

        #region Properties

        /// <summary>
        ///     Instance of the DS18B20 temperature sensor
        /// </summary>
        protected OneWireController Sensor { get; private set; }

        /// <summary>
        ///     Bus mode type, default is single device on the bus.
        /// </summary>
        protected BusModeType BusMode = BusModeType.SingleDevice;

        /// <summary>
        ///     Temperature (in degrees centigrade).
        /// </summary>
        public float Temperature
        {
            get { return _temperature; }
            private set
            {
                _temperature = value;
                //
                //  Check to see if the change merits raising an event.
                //
                if ((_updateInterval > 0) && (System.Math.Abs(_lastNotifiedTemperature - value) >= TemperatureChangeNotificationThreshold))
                {
                    TemperatureChanged(this, new SensorFloatEventArgs(_lastNotifiedTemperature, value));
                    _lastNotifiedTemperature = value;
                }
            }
        }
        private float _temperature;
        private float _lastNotifiedTemperature = 0.0F;

        /// <summary>
        ///     Any changes in the temperature that are greater than the temperature
        ///     threshold will cause an event to be raised when the instance is
        ///     set to update automatically.
        /// </summary>
        public float TemperatureChangeNotificationThreshold { get; set; } = 0.001F;

        /// <summary>
        ///     Resolution in bit of the DS18B20.
        /// </summary>
        /// <remarks>
        ///     Returns (or sets) the number of bits used to hold a sensor reading.
        ///     Possible values are 9, 10, 11 or 12.
        /// </remarks>
        public int Resolution { get; set; }

        /// <summary>
        ///     Maximum conversion time (in milliseconds) for the DS18B20 based upon the resolution.
        /// </summary>
        public ushort MaximumConversionPeriod
        {
            get
            {
                ushort period = 750;      //  Default for 12-bit (default configuration).

                switch (Resolution)
                {
                    case 9:
                        period = 94;
                        break;
                    case 10:
                        period = 188;
                        break;
                    case 11:
                        period = 375;
                        break;
                }

                return (period);
            }
        }

        /// <summary>
        ///     Device ID of the DS18B20 temperature sensor.
        /// </summary>
        public UInt64 DeviceID { get; private set; }

        #endregion Properties

        #region Events and delegates

        /// <summary>
        ///     Event raised when the temperature change is greater than the 
        ///     TemperatureChangeNotificationThreshold value.
        /// </summary>
        public event SensorFloatEventHandler TemperatureChanged = delegate { };

        #endregion Events and delegates

        #region Constructor(s)

        /// <summary>
        ///     Default constructor is private to prevent it from being called.
        /// </summary>
        private DS18B20()
        {
        }

        /// <summary>
        ///     Create a new DS18B20 temperature sensor object with the specified configuration.
        /// </summary>
        /// <param name="oneWirePin">GPIO pin the DS18B20 is connected to.</param>
        /// <param name="deviceID">Address of the DS18B20 device.</param>
        /// <param name="updateInterval">Update period in milliseconds.  Note that this most be greater than the conversion period for the sensor.</param>
        /// <param name="temperatureChangeNotificationThreshold">Threshold for temperature changes that will generate an interrupt.</param>
        public DS18B20(int oneWirePin, UInt64 deviceID = 0, ushort updateInterval = MinimumPollingPeriod,
            float temperatureChangeNotificationThreshold = 0.001F)
        {
            // get controller for OneWire
            Sensor = new OneWireController();

            lock (OneWireInstance)
            {

                //Sensor = OneWireBus.Add(oneWirePin);
                if (!Sensor.TouchReset())
                {
                    throw new Exception("Cannot find DS18B20 sensor on the OneWire interface.");
                }
                var allDevices = Sensor.FindAllDevices();
                if (allDevices.Count == 1)
                {
                    BusMode = BusModeType.SingleDevice;
                }
                else
                {
                    if (deviceID == 0)
                    {
                        throw new ArgumentException("Device deviceID cannot be 0 on a OneWireBus with multiple devices.", nameof(deviceID));
                    }
                    BusMode = BusModeType.MultimpleDevices;
                }
                //
                //  Check for the ROM ID in the list of devices connected to the bus.
                //
                if (deviceID != 0)
                {
                    bool found = false;
                    UInt64 counter = 0;
                    foreach (byte[] sn in Sensor.FindAllDevices())
                    {
                        //var id = sn[i].ToString("X2"));
                        if (counter == deviceID)
                        {
                            found = true;
                            break;
                        }
                        counter++;
                    }

                    if (!found)
                    {
                        throw new Exception("Cannot locate the specified device ID on the OneWire bus.");
                    }
                }
            }

            DeviceID = deviceID;
            ReadConfiguration();
            if ((updateInterval != 0) && (MaximumConversionPeriod > updateInterval))
            {
                throw new ArgumentOutOfRangeException(nameof(updateInterval), "Temperature readings can take " + MaximumConversionPeriod + "ms at this resolution.");
            }

            TemperatureChangeNotificationThreshold = temperatureChangeNotificationThreshold;
            _updateInterval = updateInterval;

            if (updateInterval > 0)
            {
                StartUpdating();
            }
            else
            {
                Update();
            }
        }

        #endregion Constructor(s)

        #region Methods

        /// <summary>
        ///     Start the update process.
        /// </summary>
        private void StartUpdating()
        {
            Thread t = new Thread(() =>
            {
                while (true)
                {
                    Update();
                    Thread.Sleep(_updateInterval);
                }
            });
            t.Start();
        }

        /// <summary>
        ///     Send the device ID if necessary to ensure that the object is
        ///     talking to the right device.
        /// </summary>
        protected void SendDeviceID()
        {
            if (BusMode == BusModeType.SingleDevice)
            {
                //
                //  When there is only one device we can skip sending the device ID.
                //
                Sensor.WriteByte(Commands.SkipROM);
            }
            else
            {
                Sensor.WriteByte(Commands.MatchID);
                for (var index = 0; index < DeviceIDLength; index++)
                {
                    int places = 8 * index;
                    UInt64 value = DeviceID;
                    Sensor.WriteByte((byte)((value >> places) & 0xff));
                }
            }
        }

        /// <summary>
        ///     Update the Temperature property.
        /// </summary>
        public void Update()
        {
            UInt16 temperature = 0;
            lock (OneWireInstance)
            {
                Sensor.TouchReset();
                SendDeviceID();
                Sensor.WriteByte(Commands.StartConversion);
                while (Sensor.ReadByte() == 0) ;
                Sensor.TouchReset();
                SendDeviceID();
                Sensor.WriteByte(Commands.ReadScratchPad);
                //
                //  The conversions in the next two lines are required as the ReadByte
                //  method returns an int !
                //
                temperature = (byte)Sensor.ReadByte();
                temperature |= (UInt16)(Sensor.ReadByte() << 8);
            }
            Temperature = ((float)temperature) / 16;
        }

        /// <summary>
        ///     Read the device ID from the EEPROM on the DS18B20 and update the DeviceID property.
        /// </summary>
        /// <remarks>
        ///     This will only work if there is only one device on the one wire bus.
        /// </remarks>
        public void ReadDeviceID()
        {
            if (BusMode == BusModeType.MultimpleDevices)
            {
                throw new InvalidOperationException("Cannot read device IDs from the OneWire bus when more than one device is connected.");
            }

            UInt64 deviceID = 0;
            lock (OneWireInstance)
            {
                Sensor.TouchReset();
                Sensor.WriteByte(Commands.ReadID);
                for (var index = 0; index < DeviceIDLength; index++)
                {
                    int places = 8 * index;
                    deviceID |= ((UInt64)Sensor.ReadByte()) << places;
                }
            }
            DeviceID = deviceID;
        }

        /// <summary>
        ///     Read the scratch pad area from the DS18B20.
        /// </summary>
        /// <returns>Scratch pad contents as a byte array.</returns>
        protected byte[] ReadScratchPad()
        {
            byte[] scratchPad = new byte[ScratchPadSize];
            lock (OneWireInstance)
            {
                Sensor.TouchReset();
                SendDeviceID();
                Sensor.WriteByte(Commands.ReadScratchPad);
                for (var index = 0; index < ScratchPadSize; index++)
                {
                    scratchPad[index] = (byte)Sensor.ReadByte();
                }
            }
            //
            //  TODO: Could add CRC check here for completeness.
            //
            return (scratchPad);
        }

        /// <summary>
        ///     Read the configuration from the temperature sensor.
        /// </summary>
        /// <remarks>
        ///     This method will also update the Resolution property.
        ///
        ///     Format of the configuration register:
        ///         b7 b6 b5 b4 b3 b2 b1 b0
        ///         0  R1 R0 1  1  1  1  1
        ///
        ///     Where R0 and R1 are the resolution bits (0 = 9, 1 = 10, 2 = 11 and 3 = 12).
        /// </remarks>
        public void ReadConfiguration()
        {
            byte[] scratchPad = ReadScratchPad();
            Resolution = 9 + ((scratchPad[ScratchPad.Configuration] & 0x60) >> 5);
        }

        /// <summary>
        ///     Load the configuration from the device EEPROM.
        /// </summary>
        public void LoadConfiguration()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Save the current configuration to the device EEPROM.
        /// </summary>
        public void SaveConfiguration()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
    public class SensorFloatEventArgs : EventArgs
    {
        /// <summary>
        ///     Last value read from the sensor AND sent to the user in a notification.
        /// </summary>
        public float LastNotifiedValue { get; private set; }

        /// <summary>
        ///     Current sensor reading.
        /// </summary>
        public float CurrentValue { get; private set; }

        /// <summary>
        ///     Constructor for SensorFloatEventArgs objects.
        /// </summary>
        /// <param name="lastValue">Last sensor value sent through the eventing mechanism.</param>
        /// <param name="currentValue">Current sensor reading.</param>
        public SensorFloatEventArgs(float lastValue, float currentValue)
        {
            LastNotifiedValue = lastValue;
            CurrentValue = currentValue;
        }
    }

    /// <summary>
    ///     Delegate for the events that will return a SensorFloatEventArgs object.
    /// </summary>
    /// <param name="sender">Object sending the notification.</param>
    /// <param name="e">SensorFloatEventArgs object containing the data for the application.</param>
    public delegate void SensorFloatEventHandler(object sender, SensorFloatEventArgs e);
*/
}
