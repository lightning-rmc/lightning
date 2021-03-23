using Lightning.Core.Lifetime;
using Lightning.Core.Rendering;
using Lightning.Core.Rendering.Time;
using Lightning.Core.Utils;
using Lightning.Node.Lifetime;
using System.Threading.Tasks;

namespace Lightning.Node.Rendering
{
	public class RenderController : ICreateOnStartup
	{
		private readonly IRenderHost _renderHost;
		private readonly IRenderTimer _renderTimer;

		public RenderController(IRenderHost renderHost, IRenderTimer renderTimer, INodeStateNotifier commandNotifier)
		{
			_renderHost = renderHost;
			_renderTimer = renderTimer;

			commandNotifier.StateChangeRequested += CommandNotifier_CommandRequested;
		}

		private void CommandNotifier_CommandRequested(object? sender, StateChangeRequestEventArgs<NodeState> e)
		{
			if (e.State == NodeState.Live)
			{
				e.AddTask("Start Timer",_renderTimer.StartTimerAsync());
				e.AddTask("Start Renderhost",_renderHost.StartAsync());

			}

			if (e.State == NodeState.Ready)
			{
				_renderTimer.StopTimer();
				_renderHost.Stop();
				//Note: No need for Adding Task.CompletedTask, it's only for logging propose.
				e.AddTask("Stop Timer",Task.CompletedTask);
				e.AddTask("Stop RenderHost",Task.CompletedTask);
			}
		}
	}
}
