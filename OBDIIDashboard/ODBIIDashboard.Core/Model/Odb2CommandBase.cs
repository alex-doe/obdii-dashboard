using System.Text;
using System.Threading;
using ODBIIDashboard.Core.Factory;

namespace ODBIIDashboard.Core.Model {
    public class Odb2CommandBase {
        protected string GetValue(string pid) {
            lock (ComFactory.ConnectedPort) {
                //todo write in factory und da lock
                ComFactory.ConnectedPort.Write(pid + "\r");
                Thread.Sleep(100);
                const int BUFF_SIZE = 1024;
                var cont = true;
                // ReSharper disable once TooWideLocalVariableScope
                // ReSharper disable once RedundantAssignment
                var count = 0;
                var bff = new byte[BUFF_SIZE];
                var retVal = string.Empty;
                while (cont) {
                    count = ComFactory.ConnectedPort.Read(bff, 0, BUFF_SIZE);
                    retVal += Encoding.Default.GetString(bff, 0, count);
                    if (retVal.Contains(">")) {
                        cont = false;
                    }
                }
                return retVal.Replace("\n", "");
            }
        }
    }
}