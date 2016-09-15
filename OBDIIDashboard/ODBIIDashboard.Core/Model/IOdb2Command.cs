using System;

namespace ODBIIDashboard.Core.Model {
    public interface IOdb2Command<T> {
        T Resultvalue { get; set; }
        string Title { get; set; }
        string Message { get; set; }
        Func<string, T> ResultExtractor { get; set; }
        T InvokeCommand();
    }
}