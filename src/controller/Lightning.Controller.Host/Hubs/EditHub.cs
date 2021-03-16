using Lightning.Controller.Projects;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Lightning.Controller.Host.Hubs
{
	public class EditHub : Hub
	{
		private readonly IProjectManager _projectManager;

		public EditHub(IProjectManager projectManager)
		{
			_projectManager = projectManager;
		}

		public override Task OnConnectedAsync()
		{
			return base.OnConnectedAsync();
		}
	}
}
