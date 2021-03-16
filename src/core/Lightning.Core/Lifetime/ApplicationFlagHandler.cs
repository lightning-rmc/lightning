using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Core.Lifetime
{
	//public class ApplicationFlagHandler<TFlag> : IApplicationFlagHandler<TFlag>
	//{
	//	public event EventHandler<ApplicationFlagChangeEventArgs<TFlag>>? ApplicationFlagChanging;
	//	public event EventHandler<ApplicationFlagChangedEventArgs<TFlag>>? ApplicationFlagChanged;

	//	private readonly Channel<ApplicationFlag<TFlag>> _applicationFlagchangedChannel;
	//	private readonly ILogger? _logger;

	//	public ApplicationFlagHandler(ILogger? logger = null)
	//	{
	//		_applicationFlagchangedChannel = Channel.CreateUnbounded<ApplicationFlag<TFlag>>();
	//		_logger = logger;
	//	}

	//	public async Task SetApplicationFlagAsync(ApplicationFlag<TFlag> applicationFlag, string? nodeId, CancellationToken token = default)
	//	{
	//		_logger?.LogInformation("Invocation was request to set application flag: '{flagName}' to value: '{flagValue}'.", applicationFlag.Flagname, applicationFlag.Flagvalue);
	//		var eventArgs = new ApplicationFlagChangeEventArgs<TFlag>(applicationFlag, nodeId, token);
	//		ApplicationFlagChanging?.Invoke(this, eventArgs);

	//		if (eventArgs.Tasks.Count > 0)
	//		{
	//			_logger?.LogDebug("{count} services registered tasks for application flag change: '{flagName}'.", eventArgs.Tasks.Count, applicationFlag.Flagname);
	//			await Task.WhenAll(eventArgs.Tasks);
	//		}
	//		else
	//		{
	//			_logger?.LogDebug("No Task was queued for application flag change: '{flag}', no action.", applicationFlag);
	//		}

	//		_logger?.LogInformation("Application flag '{flagName}' changed to value '{flagValue}'.", applicationFlag.Flagname, applicationFlag.Flagvalue);
	//		_ = Task.Run(() => ApplicationFlagChanged?.Invoke(this, new ApplicationFlagChangedEventArgs<TFlag>(applicationFlag, nodeId)), CancellationToken.None);
	//		await _applicationFlagchangedChannel.Writer.WriteAsync(applicationFlag, token);
	//	}

	//}
}
