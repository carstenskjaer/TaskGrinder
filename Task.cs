using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace TaskGrinder
{
	[DataContract]
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

		public Task()
		{
		}

		public TaskRunner GetTaskRunner()
		{
			var workDir = WorkingDir == "" ? Path.GetDirectoryName(FileName) : WorkingDir;
			return new TaskRunner(Name,
				FileName,
				workDir,
				Arguments);
		}

		[DataMember]
		public string Name { get; set; } = "";
		[DataMember]
		public string FileName { get; private set; } = "";
		[DataMember]
		public string WorkingDir { get; private set; } = "";
		[DataMember]
		public string Arguments { get; private set; } = "";

		public override string ToString() => Name;
	}
}
