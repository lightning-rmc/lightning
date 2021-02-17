using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lightning.Core.SourceGenerators
{
	[Generator]
	public class NotifyPropertyChangedGenerator : ISourceGenerator
	{
		private const string notifyAttribute = @"using System;

namespace Lightning.Core.SourceGenerators
{

	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	sealed class AutoNotifyAttribute : Attribute
	{
		public AutoNotifyAttribute()
		{
			PropertyName = string.Empty;
		}
		public string PropertyName { get; set; }
	}

}
";


		public void Execute(GeneratorExecutionContext context)
		{
			context.AddSource("AutoNotifyAttribute", SourceText.From(notifyAttribute, Encoding.UTF8));
		}

		public void Initialize(GeneratorInitializationContext context)
		{
		}
	}
}
