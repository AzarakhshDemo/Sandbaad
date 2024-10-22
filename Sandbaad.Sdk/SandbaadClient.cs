using meTesting.Sandbaad.Sdk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.ExceptionServices;

namespace meTesting.Sandbaad.Sdk;

public class SandbaadClient(
    ILogger<SandbaadClient> logger,
    IOptions<SandbaadConfig> config,
    HttpClient httpClient)

{
    public async Task Introduce(string appKey, string url)
    {
        logger.LogInformation("Initiate Registering service to Sandbaad..");

        var res = await httpClient.PostAsJsonAsync(new Uri(config.Value.IntroduceUrlBuilder(appKey)),
            new ServiceMetadata
            {
                Url = url,
                Host = Dns.GetHostName()
            });

        if (!res.IsSuccessStatusCode)
            logger.LogError($"Registering encountered an error: {await res.Content.ReadAsStringAsync()}");
    }
    public async Task<ServiceMetadata> GetUrl(string appKey)
    {
        ServiceMetadata res = default;
        try
        {
            logger.LogInformation("Getting instance of {App}", appKey);

            res = await httpClient.GetFromJsonAsync<ServiceMetadata>(new Uri(config.Value.GetInstanceUrlBuilder(appKey)));

            if (res is null)
                throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Can not get instance for {App}", appKey);
            throw;
        }
        return res;

    }
}
