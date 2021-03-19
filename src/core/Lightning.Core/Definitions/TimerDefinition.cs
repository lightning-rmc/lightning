using Lightning.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class TimerDefinition : DefinitionBaseType
	{

		public TimerDefinition()
		{
			_defaultFrameRate = 60;
			_sychronisationInterval = 10;
		}

		private int _defaultFrameRate;

		public int DefaultFrameRate
		{
			get => _defaultFrameRate;
			set => Set(ref _defaultFrameRate, value);
		}

		private int _sychronisationInterval;

		public int SychronisationInterval
		{
			get => _sychronisationInterval;
			set => Set(ref _sychronisationInterval, value);
		}


		protected override ConfigurationChangedTarget Type => throw new NotImplementedException();
	}
}
