using Lightning.Core.Definitions;

namespace Lightning.Controller.Host.DTO
{
	public class LayerDTO
	{
		public string Id { get; set; } = string.Empty;
		public LayerSourceDTO? Source { get; set; }
		public LayerTransformationDTO? Transform { get; set; }

		public static LayerDTO FromDefinition(LayerDefinition ld) =>
			new LayerDTO
			{
				Id = ld.Id,
				Source = ld.Input switch
				{
					FileInputLayerDefinition def => LayerSourceDTO.FromDefinition(def),
					object o => throw new UnsupportedDefinitionException(o.GetType())
				},
				Transform = LayerTransformationDTO.FromDefinition(ld.Transformation)
			};
	}
}
