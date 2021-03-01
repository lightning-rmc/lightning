using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Configuration
{
	public abstract record ConfigurationChangedContext(string Id, ConfigurationChangedTarget Target);

	public record ConfigurationValueChangedContext<TValue>(string Id, string Name, ConfigurationChangedTarget Target, TValue Value)
		: ConfigurationChangedContext(Id, Target);

	public record ConfiguartionStructurChangedContext();
}
