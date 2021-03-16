using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Core.Rendering
{

	public interface IWindowHost : IDisposable
	{
		void ShowWindow();
		void HideWindow();
	}

	public interface IWindowHost<in TFrame> : IWindowHost
	{
		void WriteFrame(TFrame mat);
	}
}
