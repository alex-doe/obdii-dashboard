using System.Collections.Generic;
using ODBIIDashboard.Core.Factory;
using ODBIIDashboard.Core.Model;

namespace OBDIIDashboard.Factorys {
    public static class SensorFactory {
        public static List<IOdb2Command<int?>> GetAllSensors() {
            return CommandFactory.GetAllOdb2Commands();
        }
    }
}