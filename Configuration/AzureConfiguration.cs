using Microsoft.Extensions.Configuration;

namespace DreamInMars.Configuration
{
    public class AzureConfiguration
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string FileUrl { get; set; }
    }
}
