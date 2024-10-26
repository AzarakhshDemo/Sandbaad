using meTesting.Sandbaad.Sdk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace meTesting.Sandbaad.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscoveryController : ControllerBase
{
    public static ConcurrentBag<Service> services = new();
    private readonly ILogger<DiscoveryController> _logger;

    public DiscoveryController(ILogger<DiscoveryController> logger)
    {
        _logger = logger;
    }

    [HttpPost("[action]/{key}")]
    public async Task<IActionResult> Introduce(string Key, ServiceMetadata metadata)
    {
        var srv = services.FirstOrDefault(a => a.Key == Key);
        if (srv is null)
        {
            srv = new Service { Key = Key };
            services.Add(srv);
        }
        var meta = srv.RegisteredInstances?.FirstOrDefault(a => a.Url == metadata.Url);
        if (meta is null)
            srv.RegisteredInstances.Add(metadata);
        return Ok();
    }
    [HttpGet("[action]")]
    public async Task<ActionResult<Service>> GetAll(string Key)
    {
        var srv = services.FirstOrDefault(a => a.Key == Key);

        if (srv is null)
            return NotFound();

        return Ok(srv);
    }
    [HttpGet("[action]")]
    public async Task<ActionResult<Service>> SnapShot()
    {
        return Ok(services);
    }
    [HttpGet("[action]/{key}")]
    public async Task<ActionResult<ServiceMetadata>> GetInstance(string Key)
    {
        var srv = services.FirstOrDefault(a => a.Key == Key);

        if (srv is null)
            return NotFound();

        var instance = srv.RegisteredInstances[Random.Shared.Next(0, srv.RegisteredInstances.Count)];

        return Ok(instance);
    }
}


