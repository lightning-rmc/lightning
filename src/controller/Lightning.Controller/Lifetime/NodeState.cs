namespace Lightning.Controller.Lifetime
{
	public enum NodeState : byte
	{
		/// <summary>
		/// Node is disconnected from controller.
		/// </summary>
		Offline = 1,
		/// <summary>
		/// Node shows debug information on output screen.
		/// </summary>
		Info = 2,
		/// <summary>
		/// Rendertree can be modified, output screen is blank.
		/// </summary>
		Edit = 3,
		/// <summary>
		/// Node is on air and shows content. Rendertree is fixed.
		/// </summary>
		Live = 4,

		/// <summary>
		/// Not in use Today.
		/// </summary>
		Error = byte.MaxValue
	}
}
