using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TaskGrinder
{

	public class Controller : INotifyPropertyChanged
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

		private Controller()
		{
			LoadTasks();

			System.Threading.Tasks.Task.Run(() => RunnerTask());
		}

		public static Controller Instance { get; } = new Controller();

		private bool _paused = false;
		public bool Paused
		{
			get { return _paused; }
			set { if (value != _paused) { _paused = value; NotifyPropertyChanged(); signal.Release(); } }
		}

		public ObservableCollection<Task> Tasks { get; } = new ObservableCollection<Task>();
		public ObservableCollection<TaskRunner> WorkList { get; } = new ObservableCollection<TaskRunner>();

		private string TaskFileName = "tasks.xml";

		public void LoadTasks()
		{
			var ds = new DataContractSerializer(typeof(ObservableCollection<Task>));

			try
			{
				using (var stream = File.OpenRead(TaskFileName))
				{
					var tasks = (ObservableCollection<Task>)ds.ReadObject(stream);
					foreach (var t in tasks)
						Tasks.Add(t);
				}
			}
			catch(IOException)
			{
				// Ignore
			}
		}

		public void SaveTasks()
		{
			var ds = new DataContractSerializer(typeof(ObservableCollection<Task>));

			using (var stream = File.Create(TaskFileName))
			{
				ds.WriteObject(stream, Tasks);
			}
		}

		public void AddTask()
		{
			var t = new Task();
			var taskEditDialog = new TaskEditDialog(t);
			var result = taskEditDialog.ShowDialog();

			if (result ?? false)
			{
				Tasks.Add(t);
				SaveTasks();
			}
		}

		public void AddTaskToWorkList(Task task)
		{
			try
			{
				WorkList.Add(task.GetTaskRunner());
				signal.Release();
			}
			catch(Exception e)
			{
                MessageBox.Show("Could not schedule task: " + e.Message, "Error", MessageBoxButton.OK);
			}
		}

		public void DeleteTask(Task task)
		{
			Tasks.Remove(task);
			SaveTasks();
		}

		public void EditTask(Task task)
		{
			var taskEditDialog = new TaskEditDialog(task);
			var result = taskEditDialog.ShowDialog();
			SaveTasks();
		}

		private SemaphoreSlim signal = new SemaphoreSlim(0, 1);

		private async void RunnerTask()
		{
			while(true)
			{
				var task = WorkList.FirstOrDefault(t => t.State == TaskRunner.RunState.NotStarted);

				if (Paused || task == null)
				{
					await signal.WaitAsync();
					continue;
				}

				await task.Execute();
				
			}
		}
	}
}
