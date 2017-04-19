using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs.TestJob
{
    public sealed class Job : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Job));

        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("独立项目的TestJob测试");
        }
    }
}
