using Lightning.Core.Definitions;
using Lightning.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.DTO
{
	public class NodeDTO
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string? State { get; set; } = string.Empty;
		public int FramesPerSecond { get; set; }
		public NodeResolutionDTO Resolution { get; set; }

		public static NodeDTO FromDefinition(NodeDefinition def, NodeState? state = null)
		{
			return new()
			{
				Id = def.Id,
				Name = def.Name,
				FramesPerSecond = def.FramesPerSecond,
				Resolution = NodeResolutionDTO.FromDefinition(def.Resolution),
				State = state?.ToString()
			};
		}
	}

	public struct NodeResolutionDTO
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public static NodeResolutionDTO FromDefinition(ResolutionDefinition def)
		{
			return new()
			{
				Width = def.Width,
				Height = def.Height
			};
		}
	}
}
