using meTesting.Sauron;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meTesting.Sandbaad.Sdk;

public static class Helpers
{
    public static IServiceCollection AddSandbaadClient(this IServiceCollection services, SandbaadConfig config)
    {
        services.AddSingleton<SandbaadClient>();

        services.AddSauronHttpClient<SandbaadClient>((s, h) =>
        {
            h.BaseAddress = new Uri(config.BaseUrl);
        });

        return services;
    }

    public static WebApplication UseSandbaad(this WebApplication app, IServiceProvider service, string appKey, string url)
    {
        var sand = service.GetRequiredService<SandbaadClient>();
        sand.Introduce(appKey, url).Wait();
        return app;
    }
}
public class SandbaadConfig
{
    public string BaseUrl { get; set; }
    public string GetInstanceUrlBuilder(string dest) => $"{BaseUrl}/Discovery/GetInstance/{dest}";

    public string IntroduceUrlBuilder(string dest) => $"{BaseUrl}/Discovery/Introduce/{dest}";
}