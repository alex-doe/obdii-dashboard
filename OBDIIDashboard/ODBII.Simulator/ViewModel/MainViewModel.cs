using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ODBIIDashboard.Core.Factory;

namespace ODBII.Simulator.ViewModel {
    internal class MainViewModel : BaseViewModel {
        public MainViewModel() {
            AvailableComPorts = ComFactory.AvailablePorts;
            LogGridItems = new ObservableCollection<LogEntry>();
            SimStarted = false;
            SimStatus = "stoped";
        }

        private const string COMHANDLER = "COMHANDLER";
        private const string DATARECIEVER = "DATARECIVER";

        private object _comStatus;
        private List<string> _availableComPorts;
        private string _selectedComPort;
        private bool _connected;
        private ObservableCollection<LogEntry> _logGridItems;
        private string _textToSend;
        private bool _simStarted;
        private string _simStatus;

        public ICommand ConnectComPortCommmand {
            get { return new DelegateCommand(ConnectComPort); }
        }

        public object ComStatus {
            get { return _comStatus; }
            set {
                _comStatus = value;
                RaisePropertyChanged();
            }
        }

        public object DisconnectComPortCommmand {
            get { return new DelegateCommand(DisconnectComPort); }
        }

        public List<string> AvailableComPorts {
            get { return _availableComPorts; }
            set {
                _availableComPorts = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedComPort {
            get { return _selectedComPort; }
            set {
                _selectedComPort = value;
                RaisePropertyChanged();
            }
        }

        public bool Connected {
            get { return _connected; }
            set {
                _connected = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<LogEntry> LogGridItems {
            get { return _logGridItems; }
            set {
                _logGridItems = value;
                RaisePropertyChanged();
            }
        }

        public object StartComSimulator {
            get { return new DelegateCommand(StartSimulator); }
        }

        public bool SimStarted {
            get { return _simStarted; }
            set {
                _simStarted = value;
                RaisePropertyChanged();
            }
        }

        public string TextToSend {
            get { return _textToSend; }
            set {
                _textToSend = value;
                RaisePropertyChanged();
            }
        }

        public object SendToPortPressed {
            get { return new DelegateCommand(SendToPort); }
        }

        public object StopSimulatorCommand {
            get { return new DelegateCommand(StopSimulator); }
        }

        public string SimStatus {
            get { return _simStatus; }
            set {
                _simStatus = value;
                RaisePropertyChanged();
            }
        }

        private void StopSimulator(object obj) {
            ComFactory.ConnectedPort.DataReceived -= ConnectedPortOnDataReceived;
            SimStarted = false;
            SimStatus = "running";
        }

        private void SendToPort(object obj) {
            ComFactory.ConnectedPort.Write(TextToSend);
            TextToSend = string.Empty;
        }

        private void StartSimulator(object obj) {
            if (ComFactory.ConnectedPort == null) {
                MessageBox.Show("Kein COM-Port verbunden!");
                return;
            }
            ComFactory.ConnectedPort.DataReceived += ConnectedPortOnDataReceived;
            SimStarted = true;
            SimStatus = "stoped";
        }

        private void ConnectedPortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs) {
            WriteLogEntry("data angekommen", DATARECIEVER);
        }

        //void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e) {
        //    int dataLength = _serialPort.BytesToRead;
        //    byte[] data = new byte[dataLength];
        //    int nbrDataRead = _serialPort.Read(data, 0, dataLength);
        //    if (nbrDataRead == 0)
        //        return;

        //    // Send data to whom ever interested
        //    if (NewSerialDataRecieved != null)
        //        NewSerialDataRecieved(this, new SerialDataEventArgs(data));

        //}

        private void DisconnectComPort(object obj) {
            WriteLogEntry(string.Format("Try disconnect Port {0}", SelectedComPort), COMHANDLER);
            ComFactory.CloseComportConnection();
            //set connected Port
            Connected = ComFactory.Connected;
            WriteLogEntry(string.Format("Port {0} disconnected", SelectedComPort), COMHANDLER);
            ComStatus = "disconnected";
        }

        private void ConnectComPort(object o) {
            WriteLogEntry(string.Format("Try connect to {0}", SelectedComPort), COMHANDLER);
            ComFactory.ConnectComPort(SelectedComPort);
            Connected = ComFactory.Connected;
            WriteLogEntry(string.Format("Port {0} connected", SelectedComPort), COMHANDLER);
            ComStatus = "connected";
        }

        private void WriteLogEntry(string text, string source) {
            Application.Current.Dispatcher.Invoke(() => {
                var entrys = LogGridItems;
                entrys.Add(new LogEntry(text, source));
                entrys = new ObservableCollection<LogEntry>(entrys.OrderByDescending(e => e.Timestamp.Ticks));
                LogGridItems = entrys;
            });
        }
    }

    internal class LogEntry {
        public LogEntry(string entry, string source) {
            Entry = entry;
            Source = source;
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; set; }
        public string Entry { get; set; }
        public string Source { get; set; }
    }
}