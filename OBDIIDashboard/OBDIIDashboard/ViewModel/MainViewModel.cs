using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OBDIIDashboard.Controls;
using OBDIIDashboard.Factorys;
using ODBIIDashboard.Core.Model;

namespace OBDIIDashboard.ViewModel {
    internal class MainViewModel : BaseViewModel {
        public MainViewModel() {
            DisplayedSensors = new List<SensorView>();
            _sensors = SensorFactory.GetAllSensors();
            ComConnectionStatus = false;
            InitInternal();
        }

        private void InitInternal() {
            _sensors.ToList().ForEach(AddNewSensor);
        }

        #region Props

        private List<SensorView> _displayedSensors;
        private readonly List<IOdb2Command<int?>> _sensors;
        private string _selectedSensorToAdd;

        public List<SensorView> DisplayedSensors
        {
            get { return _displayedSensors; }
            set
            {
                _displayedSensors = value;
                RaisePropertyChanged();
            }
        }

        public string ProgramTitle
        {
            get { return string.Format("ODBII Dashboard v{0}", Assembly.GetExecutingAssembly().GetName().Version); }
        }

        public List<SensorViewModel> AllInvisibleSensors
        {
            get
            {
                var sensors = DisplayedSensors
                    .Where(e => ((SensorViewModel)e.DataContext).Visible == false)
                    .Select(b => ((SensorViewModel)b.DataContext)).ToList();
                return sensors;
            }
        }

        public string SelectedSensorToAdd
        {
            get { return _selectedSensorToAdd; }
            set
            {
                _selectedSensorToAdd = value;
                RaisePropertyChanged();
            }
        }

        private bool _comConnectionStatus;
        private List<string> _availableComPorts;
        private string _selectedComPort;

        public bool ComConnectionStatus {
            get { return _comConnectionStatus; }

            set {
                _comConnectionStatus = value;
                RaisePropertyChanged();
            }
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

        #endregion

        #region Methods

        internal void AddNewSensor(IOdb2Command<int?> command) {
            var model = new SensorViewModel() {
                Title = command.Title,
                Command = command,
                Visible = true,
                Running = false
            };
            var sensor = new SensorView {
                DataContext = model
            };
            DisplayedSensors.Add(sensor);
            //AllInvisibleSensors.Remove(sensor);
        }

        #endregion

        public void DisplaySensor() {
            //var sensor = _viewmodel.AllInvisibleSensors.Single(a => a.Title == _viewmodel.SelectedSensorToAdd);
            //_viewmodel.DisplayedSensors.Add(_viewmodel.GetSensor(sensor));
        }
    }
}