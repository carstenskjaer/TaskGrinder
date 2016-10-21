using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrinder
{
	public enum RunState { NotStarted, Running, Done }

	public class TaskRunner : INotifyPropertyChanged
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

		public TaskRunner(string name, string fileName, string workingDir, string arguments)
		{
			Name = name;
			FileName = fileName;
			WorkingDir = workingDir;
			Arguments = arguments;
		}

		public string Name { get; private set; }
		public string FileName { get; private set; }
		public string WorkingDir { get; private set; }
		public string Arguments { get; private set; }

		public override string ToString() => Name;

		private RunState _runState = RunState.NotStarted;
		public RunState RunState
		{
			get { return _runState; }
			private set { if (value != _runState) { _runState = value; NotifyPropertyChanged(); } }
		}

		private StringBuilder _Output = new StringBuilder();
		public string Output
		{
			get { return _Output.ToString(); }
		}

		public int ReturnCode { get; private set; } = -1;

		public bool Succeeded { get { return RunState == RunState.Done && ReturnCode == 0; } }

		public Task<bool> Execute()
		{

			Win32ProcessUtils.RunSubProcess(FileName + " " + Arguments);


			var tcs = new TaskCompletionSource<bool>();

			var process = new System.Diagnostics.Process
			{
				StartInfo = {
					FileName = FileName,
					WorkingDirectory = WorkingDir,
					Arguments = Arguments,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					//CreateNoWindow = true,
				},
				EnableRaisingEvents = true,
			};

			process.Exited += (sender, args) =>
			{
				ReturnCode = process.ExitCode;
				RunState = RunState.Done;
				tcs.SetResult(Succeeded);
				process.Dispose();
			};
			
			process.OutputDataReceived += (sender, e) =>
			{
				_Output.AppendLine(e.Data);
				NotifyPropertyChanged("Output");
			};
			process.Start();

			process.BeginOutputReadLine();

			RunState = RunState.Running;

			return tcs.Task;
		}

	}
}
