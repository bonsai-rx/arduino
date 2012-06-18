﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Bonsai.IO;

namespace Bonsai.Arduino
{
    public class ArduinoDigitalOutput : Sink<bool>
    {
        IEnumerable<Action<bool>> digitalOutput;
        IEnumerator<Action<bool>> iterator;

        [TypeConverter(typeof(SerialPortNameConverter))]
        public string SerialPort { get; set; }

        public int Pin { get; set; }

        public override void Process(bool input)
        {
            iterator.Current(input);
        }

        public override IDisposable Load()
        {
            digitalOutput = ObservableArduino.DigitalOutput(SerialPort, Pin);
            iterator = digitalOutput.GetEnumerator();
            iterator.MoveNext();
            return base.Load();
        }

        protected override void Unload()
        {
            iterator.Dispose();
            digitalOutput = null;
            base.Unload();
        }
    }
}
