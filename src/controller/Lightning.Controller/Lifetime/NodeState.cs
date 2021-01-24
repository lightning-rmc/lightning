namespace Lightning.Controller.Lifetime
{
	public enum NodeState : byte
	{
		Offline = 1,
		Info = 2,
		Edit = 3,
		Live = 4,

		Error = byte.MaxValue
	}
}
