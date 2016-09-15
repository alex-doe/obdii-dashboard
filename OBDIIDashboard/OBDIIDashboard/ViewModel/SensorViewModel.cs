using System;
using System.Windows.Input;
using ODBIIDashboard.Core.Model;

namespace OBDIIDashboard.ViewModel {
    internal class SensorViewModel : BaseViewModel {
        private string _title;
        private IOdb2Command<int?> _command;
        private bool _visible;
        private bool _running;
        private int? _result;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public IOdb2Command<int?> Command
        {
            get { return _command; }
            set
            {
                _command = value;
                RaisePropertyChanged();
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                RaisePropertyChanged();
            }
        }

        public bool Running
        {
            get { return _running; }
            set
            {
                _running = value;
                RaisePropertyChanged();
            }
        }

        public int? Result {
            get { return _result; }
            set {
                _result = value;
                RaisePropertyChanged();
            }
        }
    }
}