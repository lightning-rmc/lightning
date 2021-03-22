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
				_renderTimer.StartTimer();
				e.AddTask(Task.Run(async () =>
				{
					await _renderHost.StartAsync();
				}));

			}

			if (e.State == NodeState.Ready)
			{
				_renderTimer.StopTimer();
				_renderHost.Stop();
			}
		}
	}
}
