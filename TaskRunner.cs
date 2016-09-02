using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrinder
{
	public class TaskRunner
	{
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

		public enum RunState { NotStarted, Running, Done }

		public RunState State { get; private set; } = RunState.NotStarted;

		public string Output { get; private set; }

		public int ReturnCode { get; private set; } = -1;

		public bool Succeeded { get { return State == RunState.Done && ReturnCode == 0; } }

		public Task<bool> Execute()
		{
			var tcs = new TaskCompletionSource<bool>();

			var process = new System.Diagnostics.Process
			{
				StartInfo = {
					FileName = FileName,
					WorkingDirectory = WorkingDir,
					Arguments = Arguments,
					UseShellExecute = false,
					RedirectStandardOutput = true,
				},
				EnableRaisingEvents = true,
			};

			process.Exited += (sender, args) =>
			{
				Output = process.StandardOutput.ReadToEnd();
				ReturnCode = process.ExitCode;
				tcs.SetResult(Succeeded);
				process.Dispose();
			};

			process.Start();

			return tcs.Task;
		}
	}
}
