using System;
using System.Collections.Generic;

namespace BlazorAppHttps.Data
{
    public interface IReportService
    {
        public string TimeFormat => "yyyy/MM/dd HH:mm:ss";

        public ProtocolModel Protocol { get; set; }

        public string OperatorName { get; set; }

        public DateTime ProtocolStart { get; }

        public DateTime ProtocolEnd { get; }

        public Dictionary<string, DateTime> Logs { get; }

        public IProtocolLogInputs Inputs { get; set; }

        public List<string> ErrorMessages { get; }

        public bool IsValid { get; }

        public void ValidateInputs();

        public void Clear();

        public void ProtocolBegin();

        public void UndoStep(string step);

        public void LogStep(string step);

        public void ProtocolCompleted();

        public void SendReport();
    }
}