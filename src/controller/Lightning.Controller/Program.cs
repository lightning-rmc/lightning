using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Lightning.Controller
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            host.Build().Run();
        }
    }
}
