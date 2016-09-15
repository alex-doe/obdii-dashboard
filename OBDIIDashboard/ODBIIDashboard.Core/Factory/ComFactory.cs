using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;

namespace ODBIIDashboard.Core.Factory {
    public static class ComFactory {
        //private readonly Logger _logger;
        private static List<string> _availablePorts;
        private static readonly int _baudrate = 9600;

        static ComFactory() {
        }

        public static List<string> AvailablePorts {
            get {
                SearchAvailableComPorts();
                return _availablePorts;
            }
        }

        public static SerialPort ConnectedPort { get; set; }
        public static bool Connected { get; set; }

        private static void SearchAvailableComPorts() {
            //_logger.WriteLine("Suche Com Ports");
            _availablePorts = SerialPort.GetPortNames().ToList();
        }
        //todo lock
        public static void ConnectComPort(string selcomport) {
            if (selcomport == null) {
                //_logger.WriteLine("Kein Com port gewählt --> exit");
                return;
            }
            //_logger.WriteLine($"Versuche verbindung auf {selcomport}");
            ConnectedPort = new SerialPort(selcomport, _baudrate);
            try {
                ConnectedPort.Open();
                var startconnecttime = new Stopwatch();
                startconnecttime.Start();
                while (!ConnectedPort.IsOpen) {
                    if (startconnecttime.Elapsed <= TimeSpan.FromSeconds(20)) continue;
                    //_logger.WriteLine("Timeout beim connect mit com");
                    return;
                }
                ConnectedPort.DiscardInBuffer();
                ConnectedPort.DiscardOutBuffer();
                startconnecttime.Stop();
                Connected = true;
                //_logger.WriteLine($"Verbindung erfolgreich in {startconnecttime.ElapsedMilliseconds}ms");
            }
            catch (UnauthorizedAccessException) {
                //_logger.WriteLine("Verbindung mit Port konnte nicht hergestellt werden --> schon belegt?");
            }
        }

        public static void CloseComportConnection() {
            if (ConnectedPort == null) {
                //_logger.WriteLine("Kein port vorhanden --> fehler");
                return;
            }
            if (!ConnectedPort.IsOpen) {
                //_logger.WriteLine("Keine Verbindung mit dem Port vorhanden --> kein close nötig");
                return;
            }
            ConnectedPort.Close();
            var statclose = DateTime.Now;
            while (ConnectedPort.IsOpen) {
                if ((DateTime.Now - statclose) > TimeSpan.FromSeconds(5)) {
                    //_logger.WriteLine("Der Port wurde nach 5 sekunden nicht disconnected --> error");
                    return;
                }
            }
            Connected = false;
            //_logger.WriteLine("Verbindung mit port geschlossen");
        }
    }
}