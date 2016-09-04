using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrinder
{
	public class Task : INotifyPropertyChanged
	{
		// Support property binding from WPF gui
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public Task(string name)
		{
			Name = name;
		}

		public TaskRunner GetTaskRunner()
		{
			return new TaskRunner(Name,
				"",
				"",
				"");
		}

		public string Name { get; private set; }
		public string FileName { get; private set; }
		public string WorkingDir { get; private set; }
		public string Arguments { get; private set; }

		public override string ToString() => Name;
	}
}
