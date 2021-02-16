using System;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
    public interface IRenderHost
    {
        bool IsRunning { get; }

        Task StartAsync();
        void Stop();
    }
}
