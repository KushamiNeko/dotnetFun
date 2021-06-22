using System;
using System.Collections.Generic;

namespace BlazorAppHttps.Data
{
    public class ReportServiceLocal : IReportService
    {
        public ProtocolModel Protocol { get; set; } = null;

        public string OperatorName { get; set; } = null;

        public DateTime ProtocolStart { get; private set; }

        public DateTime ProtocolEnd { get; private set; }

        public Dictionary<string, DateTime> Logs { get; private set; }

        public IProtocolLogInputs Inputs { get; set; }

        public List<string> ErrorMessages { get; private set; } = null;

        public bool IsValid { get; private set; } = false;

        public void ValidateInputs()
        {
            if (ErrorMessages == null)
            {
                ErrorMessages = new List<string>();
            }
            else
            {
                ErrorMessages.Clear();
            }

            IsValid = true;

            if (Protocol == null)
            {
                IsValid = false;
                return;
            }

            if (ProtocolStart == default)
            {
                IsValid = false;
                return;
            }

            if (string.IsNullOrWhiteSpace(OperatorName))
            {
                ErrorMessages.Add("担当者を入力してください");
                IsValid = false;
            }

            if (Inputs != null)
            {
                Inputs.Validate();

                if (!Inputs.IsValid)
                {
                    ErrorMessages.AddRange(Inputs.ErrorMessages);
                    IsValid = false;
                }
            }
        }

        public void Clear()
        {
            Protocol = null;
            OperatorName = null;
            ProtocolStart = default;
            ProtocolEnd = default;
            Logs = null;
            Inputs = null;
            ErrorMessages = null;
            IsValid = false;
        }

        public void ProtocolBegin()
        {
            ProtocolStart = DateTimeOffset.UtcNow.AddHours(9.0).DateTime;
            Logs = new Dictionary<string, DateTime>();
        }

        public void UndoStep(string step)
        {
            Logs.Remove(step);
        }

        public void LogStep(string step)
        {
            if (!Logs.ContainsKey(step))
            {
                Logs.Add(step, DateTimeOffset.UtcNow.AddHours(9.0).DateTime);
            }
            else
            {
                Logs[step] = DateTimeOffset.UtcNow.AddHours(9.0).DateTime;
            }
        }

        public void ProtocolCompleted()
        {
            ProtocolEnd = DateTimeOffset.UtcNow.AddHours(9.0).DateTime;
        }

        public void SendReport()
        {
        }
    }
}