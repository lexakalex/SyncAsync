using Owin;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using Microsoft.Owin.Host.HttpListener;

namespace SyncAsyncWorkerRole
{
    class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            TelemetryConfiguration.Active.InstrumentationKey =
                CloudConfigurationManager.GetSetting("APPINSIGHTS_INSTRUMENTATIONKEY");

            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            var owinListenerName = "Microsoft.Owin.Host.HttpListener.OwinHttpListener";
            var owinListener = (OwinHttpListener)appBuilder.Properties[owinListenerName];
            owinListener.SetRequestQueueLimit(260000);

            appBuilder.UseWebApi(config);

            // report unhandled exceptions from WebAPI 2 controllers to AI
            config.Services.Add(typeof(IExceptionLogger), new AiWebApiExceptionLogger());
            config.EnableApplicationInsights();
        }
    }
}