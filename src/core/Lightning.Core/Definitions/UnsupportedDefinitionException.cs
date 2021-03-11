using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Definitions
{
	public class UnsupportedDefinitionException : Exception
	{
		public UnsupportedDefinitionException(Type definitionType) : base($"Unsupported Definition: {definitionType.Name}")
		{

		}
	}
}
