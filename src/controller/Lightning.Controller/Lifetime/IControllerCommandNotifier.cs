using Lightning.Core.Lifetime;

namespace Lightning.Controller.Lifetime
{
	public interface IControllerCommandNotifier :
		ICommandNotifier<ControllerCommandRequest, ControllerCommandResponse>
	{
	}
}
