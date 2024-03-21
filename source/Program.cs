using Microsoft.Extensions.Hosting;

namespace CampaignWebhook
{
    /// <summary>
    /// Starting program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}