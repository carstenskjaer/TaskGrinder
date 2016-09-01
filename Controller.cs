using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrinder
{
	public enum RunState
	{
		Done, Working, Paused, Halted
	}

	public class Controller
	{
		private Controller()
		{
		}

		public static Controller Instance { get; } = new Controller();

		public RunState RunState { get; }

		public void ToggleRunState()
		{

		}
	}
}
