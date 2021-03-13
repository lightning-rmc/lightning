using Lightning.Core.Lifetime;

namespace Lightning.Controller.Lifetime
{
	public interface IControllerCommandReceiver :
		ICommandReceiver<ControllerCommandRequest, ControllerCommandResponse>
	{
	}
}
