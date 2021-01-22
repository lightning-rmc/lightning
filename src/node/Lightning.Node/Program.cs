using Microsoft.Extensions.Hosting;
using Lightning.Core.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Lightning.Node;

var host = Host.CreateDefaultBuilder()
               .ConfigureServices((c, s) =>
               {
                   s.AddRendering();
                   s.AddHostedService<NodeBootStrapper>();
               })
               .UseConsoleLifetime();
await host.RunConsoleAsync();


