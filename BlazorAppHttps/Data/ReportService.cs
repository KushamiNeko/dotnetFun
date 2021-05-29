using System;
using System.Linq;
// using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlazorAppHttps.Data
{
    public class ReportService
    {

        public Operator Operator
        {
            get
            {
                return _operator;
            }
        }

        public Dictionary<string, DateTime> Logs
        {
            get
            {
                return _logs;
            }
        }

        private Operator _operator = null;

        private Dictionary<string, DateTime> _logs = new();

        public void LogOperator(Operator op) {
            _operator = op;
        }

        public void UndoStep(string step)
        {
            // _logs.Add(step, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            // Console.WriteLine(_logs[step]);

            _logs.Remove(step);
        }

        public void LogStep(string step)
        {
            if (!_logs.ContainsKey(step))
            {
                _logs.Add(step, DateTime.Now);
            }
            else
            {
                _logs[step] = DateTime.Now;
            }

        }

        public void CompleteProtocol() {
            _operator.ProtocolEnd = DateTime.Now;
        }
    }
}
