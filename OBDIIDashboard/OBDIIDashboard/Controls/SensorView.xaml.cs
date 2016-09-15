using System;
using System.Threading;
using System.Windows;
using OBDIIDashboard.ViewModel;
using ODBIIDashboard.Core.Factory;

namespace OBDIIDashboard.Controls {
    /// <summary>
    /// Interaction logic for SensorView.xaml
    /// </summary>
    public partial class SensorView {
        private Thread _sensorThread;
        private SensorViewModel _viewmodel;

        public SensorView() {
            InitializeComponent();

        }

        private void _removeSensor_OnClick(object sender, RoutedEventArgs e) {
            //var viewmodel = GetViewModel();
            //viewmodel.Visible = false;
            //Aktuell auskommentiert da das Hinzufügen fehlt
        }

        private SensorViewModel GetViewModel() {
            var viewmodel = DataContext as SensorViewModel;
            if (viewmodel == null) {
                throw  new NullReferenceException("viewmodel nicht erkannt");
            }
            return viewmodel;
        }

        private void _sensorStart_OnClick(object sender, RoutedEventArgs e) {
            _viewmodel = GetViewModel();
            //todo threadpool + close
            _sensorThread = new Thread(DoWork);
            _sensorThread.Start();
            //spawn thread
            _viewmodel.Running = true;
        }

        private void DoWork() {
            while (_sensorThread.IsAlive) {
                var result = _viewmodel.Command.InvokeCommand();
                _viewmodel.Result = result;
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
                //Invoke(ComFactory.ConnectedPort.PortName);
            }

        }

        private void _sensorStop_OnClick(object sender, RoutedEventArgs e) {
            var viewmodel = GetViewModel();
            _sensorThread.Abort();
            viewmodel.Running = false;
        }
    }
}
