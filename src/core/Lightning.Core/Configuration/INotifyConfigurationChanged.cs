using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Configuration
{
	public interface INotifyConfigurationChanged
	{
		public event EventHandler<ConfigurationChangedEventArgs>? ConfigurationChanged;
	}

	public class ConfigurationChangedEventArgs : EventArgs
	{
		public ConfigurationChangedEventArgs(ConfigurationChangedContext changedObject)
		{
			Context = changedObject;
		}

		public ConfigurationChangedContext Context { init; get; }
	}
}

