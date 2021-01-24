using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning.Controller.Projects
{
	public interface IProjectManager
	{

		void UpdateNodeInfo();


		string ExportProject();
		bool ImportProject(string import);
	}
}
