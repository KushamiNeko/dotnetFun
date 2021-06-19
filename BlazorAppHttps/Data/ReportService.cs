using System;
using System.Linq;
// using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlazorAppHttps.Data
{
    public class ReportService
    {
        public ProtocolModel Protocol { get; init; }

        public Operator Operator
        {
            get { return _operator; }
        }

        public Dictionary<string, DateTime> Logs
        {
            get { return _logs; }
        }

        private Operator _operator = null;

        private Dictionary<string, DateTime> _logs = new();

        public void LogOperator(Operator op)
        {
            _operator = op;
        }

        public void UndoStep(string step)
        {
            _logs.Remove(step);
        }

        public void LogStep(string step)
        {
            if (!_logs.ContainsKey(step))
            {
                _logs.Add(step, DateTimeOffset.UtcNow.AddHours(9.0).DateTime);
            }
            else
            {
                _logs[step] = DateTimeOffset.UtcNow.AddHours(9.0).DateTime;
            }
        }

        public void CompleteProtocol()
        {
            _operator.ProtocolEnd = DateTimeOffset.UtcNow.AddHours(9.0).DateTime;
        }
    }
}