using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class DefinitionBaseType : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;


		public DefinitionBaseType()
		{

		}


		protected void Set<T>(ref T storage, T value, [CallerMemberName]string? name = null)
		{
			if (!storage?.Equals(value) ?? false)
			{
				storage = value;
				RaiseNotifyPropertyChanged(name);
			}
		}

		protected void RaiseNotifyPropertyChanged([CallerMemberName] string? name = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
