using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost.Logs;
using System.Web.Http.SelfHost.Models;

namespace System.Web.Http.SelfHost.Schedulers
{
    public class TaskLog : ILogger
    {
        public void ClearLog()
        {
            throw new NotImplementedException();
        }

        public void DeleteLog(Log log)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Log> GetAllLogs(DateTime? fromUtc = default(DateTime?), DateTime? toUtc = default(DateTime?), string message = "", LogLevel? logLevel = default(LogLevel?), int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public Log GetLogById(int logId)
        {
            throw new NotImplementedException();
        }

        public IList<Log> GetLogByIds(int[] logIds)
        {
            throw new NotImplementedException();
        }

        public Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "")
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel level)
        {
            throw new NotImplementedException();
        }
    }
}
