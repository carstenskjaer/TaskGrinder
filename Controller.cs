using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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

		public ObservableCollection<Task> Tasks { get; } = new ObservableCollection<Task>();
		public ObservableCollection<TaskRunner> WorkList { get; } = new ObservableCollection<TaskRunner>();

		private string TaskFileName = "tasks.xml";

		public void LoadTasks()
		{
			var ds = new DataContractSerializer(typeof(Task));

			using (var stream = File.OpenRead(TaskFileName))
			{
				while(true)
				{
					Task t = (Task)ds.ReadObject(stream);
					if (t == null)
						break;

					Tasks.Add(t);
				}
			}
		}

		public void SaveTasks()
		{
			var ds = new DataContractSerializer(typeof(Task));

			using (var stream = File.Create(TaskFileName))
			{
				foreach(var t in Tasks)
				{
					ds.WriteObject(stream, t);
				}
			}
		}

		public void AddTask()
		{
			SaveTasks();

			var t = new Task();
			Tasks.Add(t);
			var taskEditDialog = new TaskEditDialog(t);
			var result = taskEditDialog.ShowDialog();

			if (result ?? true)
			{
				SaveTasks();
			}
			else
			{
				LoadTasks();
			}
		}
	}
}
