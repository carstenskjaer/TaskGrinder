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

		private string _name = "New task";
		[DataMember]
		public string Name
		{
			get { return _name; }
			set { if (value != _name) { _name = value; NotifyPropertyChanged(); } }
		}
		
		private string _fileName = "";
		[DataMember]
		public string FileName
		{
			get { return _fileName; }
			set { if (value != _fileName) { _fileName = value; NotifyPropertyChanged(); } }
		}
		
		private string _workingDir = "";
		[DataMember]
		public string WorkingDir
		{
			get { return _workingDir; }
			set { if (value != _workingDir) { _workingDir = value; NotifyPropertyChanged(); } }
		}

		private string _arguments = "";
		[DataMember]
		public string Arguments
		{
			get { return _arguments; }
			set { if (value != _arguments) { _arguments = value; NotifyPropertyChanged(); } }
		}
		public override string ToString() => Name;
	}
}
