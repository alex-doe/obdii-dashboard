using System;
using System.Windows;
using OBDIIDashboard.ViewModel;
using ODBIIDashboard.Core;
using ODBIIDashboard.Core.Factory;

namespace OBDIIDashboard {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly MainViewModel _viewmodel;

        public MainWindow() {
            InitializeComponent();
            _viewmodel = DataContext as MainViewModel;
            if (_viewmodel == null) {
                throw new NullReferenceException("Kein Viewmodel initialisiert --> fatal");
            }
        }

        private void _addNewSensor_OnClick(object sender, RoutedEventArgs e) {
            _viewmodel.DisplaySensor();
        }

        private void _searchCOMPort_OnClick(object sender, RoutedEventArgs e) {
            _viewmodel.AvailableComPorts = ComFactory.AvailablePorts;
        }

        private void _connectCOMPort_OnClick(object sender, RoutedEventArgs e) {
            var a = _viewmodel.SelectedComPort;
            ComFactory.ConnectComPort(a);
            _viewmodel.ComConnectionStatus = true;
        }

        private void _disconnectCOMPort_OnClick(object sender, RoutedEventArgs e) {
            ComFactory.CloseComportConnection();
            _viewmodel.ComConnectionStatus = false;
        }
    }
}