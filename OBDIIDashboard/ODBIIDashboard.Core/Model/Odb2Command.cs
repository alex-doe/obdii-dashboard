using System;
using ODBIIDashboard.Core.Factory;

namespace ODBIIDashboard.Core.Model {
    public class Odb2Command<T> : Odb2CommandBase, IOdb2Command<T> {
        public T Resultvalue { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Func<string, T> ResultExtractor { get; set; }

        public T InvokeCommand() {
            //first check com connection
            if (!ComFactory.Connected) {
                //keine Comconnection
                throw new InvalidOperationException("No Serial Connection available");
            }
            var obdMessage = Message;
            //if (OnGetEngineRpmInit != null)
            //    OnGetEngineRpmInit(new OBDIIEngineEventArgs(null, obdMessage));
            var data = GetValue(obdMessage);
            return ResultExtractor.Invoke(data);
        }
    }
}