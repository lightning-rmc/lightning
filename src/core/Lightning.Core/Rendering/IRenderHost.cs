using System;

namespace Lightning.Core.Rendering
{
    public interface IRenderHost : IDisposable
    {
        bool IsRunning { get; }

        void Start();
        void Stop();
    }
}
