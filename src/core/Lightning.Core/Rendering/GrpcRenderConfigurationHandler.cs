﻿using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{
    public class GrpcRenderConfigurationHandler : IConfigurationHandler<RenderConfiguration>, IDisposable
        //NOTE: Maybe change to IAsyncDisposable?
    {
        private readonly RenderConfiguration _configuration;

        public GrpcRenderConfigurationHandler()
        {
            _configuration = new RenderConfiguration
            {
                FramesPerSecond = 120
            };
        }

        public Task ConnectAsync(CancellationToken token = default)
        {
            //Todo: place to connect to Connect to the Grpc Service 
            return Task.CompletedTask;
        }
       
        public RenderConfiguration GetConfiguration(string? name = null)
        {
            return _configuration;
        }

        public void Dispose()
        {
            //TODO: Close gRPC connection
        }
    }
}
