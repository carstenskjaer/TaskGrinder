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
		}

		public ObservableCollection<Task> Tasks { get; } = new ObservableCollection<Task>();
		public ObservableCollection<TaskRunner> WorkList { get; } = new ObservableCollection<TaskRunner>();

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Tasks.Add(new Task("new"));
		}

		private void taskListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (taskListBox.SelectedItem != null)
			{
				WorkList.Add(((Task)taskListBox.SelectedItem).GetTaskRunner());
			}
		}
	}
}
