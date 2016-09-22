using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

			DataObject obj = new DataObject("TaskDrag", draggedTask);
			DragDrop.DoDragDrop(sender as Window, obj, DragDropEffects.All);
		}

		private void WorkListBox_DragEnter(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None;
			if (draggedTask != null && e.Data.GetDataPresent("TaskDrag", true))
			{
				var mousePos = e.GetPosition(WorkListBox);
				var droppedOn = (TaskRunner)GetObjectAtPoint<ListBoxItem>(WorkListBox, mousePos);
				if (droppedOn != null)
				{
					if (droppedOn.RunState == RunState.NotStarted)
					{
						e.Effects = DragDropEffects.Move;
					}
				}
			}
		}

		private void WorkListBox_Drop(object sender, DragEventArgs e)
		{
			var newTaskRunner = draggedTask.GetTaskRunner();

			var mousePos = e.GetPosition(WorkListBox);
			var droppedOn = (TaskRunner)GetObjectAtPoint<ListBoxItem>(WorkListBox, mousePos);
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

		public object GetObjectAtPoint<ItemContainer>(ItemsControl control, Point p)
									 where ItemContainer : DependencyObject
		{
			// ItemContainer - can be ListViewItem, or TreeViewItem and so on(depends on control)
			ItemContainer obj = GetContainerAtPoint<ItemContainer>(control, p);
			if (obj == null)
				return null;

			return control.ItemContainerGenerator.ItemFromContainer(obj);
		}

		public ItemContainer GetContainerAtPoint<ItemContainer>(ItemsControl control, Point p)
								 where ItemContainer : DependencyObject
		{
			HitTestResult result = VisualTreeHelper.HitTest(control, p);
			DependencyObject obj = result.VisualHit;

			while (VisualTreeHelper.GetParent(obj) != null && !(obj is ItemContainer))
			{
				obj = VisualTreeHelper.GetParent(obj);
			}

			// Will return null if not found
			return obj as ItemContainer;
		}


	}
}
