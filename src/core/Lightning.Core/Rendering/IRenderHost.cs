using System;

namespace Lightning.Core.Rendering
{
    public interface IRenderHost
    {
        bool IsRunning { get; }

        void Start();
        void Stop();
    }
}
