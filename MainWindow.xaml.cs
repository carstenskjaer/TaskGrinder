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
	}
}
