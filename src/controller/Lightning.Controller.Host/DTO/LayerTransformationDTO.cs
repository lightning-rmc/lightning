using Lightning.Core.Definitions;

namespace Lightning.Controller.Host.DTO
{
	public class LayerTransformationDTO
	{
		public int ScaleX { get; set; }
		public int ScaleY { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int Rotation { get; set; }

		public static LayerTransformationDTO FromDefinition(TransformProcessorDefinition def)
		{
			return new()
			{
				ScaleX = def.ScaleX,
				ScaleY = def.ScaleY,
				PosX = def.X,
				PosY = def.Y,
				Rotation = def.Rotation
			};
		}
	}
}
