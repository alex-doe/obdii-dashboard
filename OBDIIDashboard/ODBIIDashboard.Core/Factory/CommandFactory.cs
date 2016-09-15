using System;
using System.Collections.Generic;
using ODBIIDashboard.Core.Model;

namespace ODBIIDashboard.Core.Factory {
    public static class CommandFactory {
        public static IOdb2Command<int?> GetEngineRpmCommand() {
            var command = new Odb2Command<int?> {
                Title = "Engine RPM",
                Message = "010C",
                ResultExtractor = delegate(string data) {
                    var data1 = Convert.ToInt32(data.Split(' ')[2].Replace("\r>", string.Empty), 16);
                    var data2 = Convert.ToInt32(data.Split(' ')[3].Replace("\r>", string.Empty), 16);
                    int? retVal;
                    if (data.Contains("NO DATA")) {
                        retVal = null;
                    }
                    else {
                        retVal = ((data1*256) + data2)/4;
                    }
                    return retVal;
                }
            };
            return command;
        }

        public static IOdb2Command<int?> GetSpeedKmhCommand() {
            var command = new Odb2Command<int?> {
                Title = "Speed km/h",
                Message = "010D",
                ResultExtractor = delegate(string data) {
                    if (data.Contains("NO DATA")) return null;
                    return (int?) Convert.ToInt32(data.Split(' ')[2].Replace("\r>", string.Empty), 16);
                }
            };
            return command;
        }

        public static IOdb2Command<int?> GetThrottlePositionCommand() {
            var command = new Odb2Command<int?>
            {
                Title = "Throttle Position",
                Message = "0111",
                ResultExtractor = delegate (string data) {
                    if (data.Contains("NO DATA")) return null;
                    else return (int?) Convert.ToInt32(data.Split(' ')[2].Replace("\r>", string.Empty), 16)*100/255;
                }
            };
            return command;
        }

        //todo int? to IResultvalue mit eigenen Typen
        public static List<IOdb2Command<int?>> GetAllOdb2Commands() {
            return new List<IOdb2Command<int?>> {
                GetEngineRpmCommand(),
                GetSpeedKmhCommand(),
                GetThrottlePositionCommand()
            };
        }
    }
}