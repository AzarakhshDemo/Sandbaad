namespace meTesting.Sandbaad.Sdk.Models;

public class Service
{
    public string Key { get; set; }
    public List<ServiceMetadata> RegisteredInstances { get; set; } = new List<ServiceMetadata>();
}

public class ServiceMetadata
{
    private DateTime registerTime = DateTime.Now;

    public string Url { get; set; }
    public string Host { get; set; }
    public DateTime RegisterTime { get => registerTime; }

}