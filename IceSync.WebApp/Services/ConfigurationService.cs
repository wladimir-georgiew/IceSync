using IceSync.WebApp.Contracts;
using IceSync.WebApp.DTOs;
using Microsoft.Extensions.Configuration;

namespace IceSync.WebApp.Services;

public class ConfigurationService(IConfiguration Configuration) : IConfigurationService
{
    public Configuration GetConfiguration()
    {
        var monitoringTriggerTime = Configuration.GetSection("Services").GetSection("UniversalLoader")
            .GetValue<int>("MonitoringTriggerTimeMinutes");
        return new Configuration()
        {
            MonitoringTriggerTime = TimeSpan.FromMinutes(monitoringTriggerTime),
            Credentials = new Credentials()
            {
              CompanyId  = Configuration.GetSection("Services").GetSection("UniversalLoader").GetSection("Credentials")
                  .GetValue<string>("CompanyId"),
              UserId  = Configuration.GetSection("Services").GetSection("UniversalLoader").GetSection("Credentials")
                  .GetValue<string>("UserId"),
              Secret  = Configuration.GetSection("Services").GetSection("UniversalLoader").GetSection("Credentials")
                  .GetValue<string>("UserSecret"),
            },
        };
    }
}