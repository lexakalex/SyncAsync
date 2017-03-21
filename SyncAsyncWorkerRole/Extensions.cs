using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace SyncAsyncWorkerRole
{
    public static class Extensions
    {
        public static void EnableApplicationInsights(this HttpConfiguration configuration)
        {
            ITraceWriter writer;
            var currentWriter = configuration.Services.GetTraceWriter();
            if (currentWriter != null)
            {
                writer = new CompositTraceWriter(
                    new ITraceWriter[]
                        {
                            new ApplicationInsightsTraceWriter(),
                            currentWriter,
                        });
            }
            else
            {
                writer = new ApplicationInsightsTraceWriter();
            }

            configuration.Services.Replace(
                typeof(ITraceWriter),
                writer);
        }
    }
}
