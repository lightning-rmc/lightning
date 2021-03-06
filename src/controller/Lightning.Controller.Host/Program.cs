using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Portable.Xaml;
using GenericHost = Microsoft.Extensions.Hosting.Host;

namespace Lightning.Controller.Host
{
	public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
			GenericHost.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
					webBuilder.ConfigureKestrel(o =>
					{
						o.ListenAnyIP(5000);
						o.ListenAnyIP(5001, opt => opt.UseHttps());
					});
                    webBuilder.UseStartup<Startup>();
                });
    }
}
