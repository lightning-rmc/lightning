using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.DTO
{
	public record NodeDTO(string Id, string Name, NodeState State, int framesPerSecond, ResolutionDTO resolution);
	public record ResolutionDTO(int width, int height);
}
