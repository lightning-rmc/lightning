using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public abstract class DefinitionBaseType : INotifyPropertyChanged, INotifyConfigurationChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		public event EventHandler<ConfigurationChangedEventArgs>? ConfigurationChanged;


		protected DefinitionBaseType()
		{
			Id = Guid.NewGuid().ToString();
		}

		protected abstract ConfigurationChangedTarget Type { get; }

		//TODO: maybe Change to init and Check if XAML can handle that
		public string Id { get; set; }

		protected void Set<TValue>(ref TValue storage, TValue value, [CallerMemberName] string? name = null)
		{
			if (!object.Equals(storage,value))
			{
				storage = value;
				RaiseNotifyPropertyChanged(name);
				RaiseConfigurationChanged(Id, name!, value);
			}
		}

		private void RaiseConfigurationChanged<TValue>(string id, string name, TValue value)
		{
			//TODO: Remove TryCatch
			try
			{
				ConfigurationChanged?.Invoke(this, new(new ConfigurationValueChangedContext<TValue>(id, name, Type, value)));
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Achtung Target Type wurde noch nicht angelegt. Unbedingt Fixen!!!" + e.ToString());
				Console.ResetColor();
			}
		}

		protected void RaiseNotifyPropertyChanged([CallerMemberName] string? name = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
