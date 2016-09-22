using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskGrinder
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = Controller.Instance;
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			Controller.Instance.AddTask();
		}

		private void taskListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var selectedTask = taskListBox.SelectedItem as Task;
			if (selectedTask != null)
			{
				Controller.Instance.AddTaskToWorkList(selectedTask);
			}
		}

		private void DeleteTask_Click(object sender, RoutedEventArgs e)
		{
			var selectedTask = taskListBox.SelectedItem as Task;
			if (selectedTask != null)
			{
				Controller.Instance.DeleteTask(selectedTask);
			}
		}
		private void EditTask_Click(object sender, RoutedEventArgs e)
		{
			var selectedTask = taskListBox.SelectedItem as Task;
			if (selectedTask != null)
			{
				Controller.Instance.EditTask(selectedTask);
			}
		}

		private void PausedButton_Click(object sender, RoutedEventArgs e)
		{
			Controller.Instance.Paused = !Controller.Instance.Paused;
		}

		private Task draggedTask;

		private void taskListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (draggedTask != null) return;

			UIElement element = taskListBox.InputHitTest(e.GetPosition(taskListBox)) as UIElement;

			while (element != null)
			{
				if (element is ListBoxItem)
				{
					draggedTask = (Task)((ListBoxItem)element).Content;
					break;
				}
				element = VisualTreeHelper.GetParent(element) as UIElement;
			}
		}

		private void Window_MouseMove(object sender, MouseEventArgs e)
		{
			if (draggedTask == null)
				return;

			if (e.LeftButton == MouseButtonState.Released)
			{
				draggedTask = null;
				return;
			}
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DataObject obj = new DataObject("TaskDrag", draggedTask);
				DragDrop.DoDragDrop(sender as Window, obj, DragDropEffects.All);
			}
		}

		private void WorkListBox_DragEnter(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None;
			if (draggedTask != null && e.Data.GetDataPresent("TaskDrag", true))
			{
				var mousePos = e.GetPosition(WorkListBox);
				var droppedOn = Util.GetObjectAtPoint<ListBoxItem>(WorkListBox, mousePos) as TaskRunner;
				if (droppedOn == null || droppedOn.RunState == RunState.NotStarted)
				{
					e.Effects = DragDropEffects.Move;
				}
			}
			e.Handled = true;
		}

		private void WorkListBox_Drop(object sender, DragEventArgs e)
		{
			var newTaskRunner = draggedTask.GetTaskRunner();

			var mousePos = e.GetPosition(WorkListBox);
			var droppedOn = (TaskRunner)Util.GetObjectAtPoint<ListBoxItem>(WorkListBox, mousePos);
			if (droppedOn != null)
			{
				var index = Controller.Instance.WorkList.IndexOf(droppedOn);
				Controller.Instance.WorkList.Insert(index, newTaskRunner);
			}
			else
			{
				Controller.Instance.WorkList.Add(newTaskRunner);
			}
		}

		private void WorkListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var selectedTaskRunner = WorkListBox.SelectedItem as TaskRunner;
			if (selectedTaskRunner != null && selectedTaskRunner.RunState == RunState.NotStarted)
			{
				Controller.Instance.CancelTask(selectedTaskRunner);
			}
		}
	}
}
