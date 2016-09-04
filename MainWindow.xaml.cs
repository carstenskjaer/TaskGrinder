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
			Controller = Controller.Instance;
		}

		public Controller Controller { get; }


		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Controller.AddTask();
		}

		private void taskListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (taskListBox.SelectedItem != null)
			{
				//WorkList.Add(((Task)taskListBox.SelectedItem).GetTaskRunner());
			}
		}

		private void StatusButton_Click(object sender, RoutedEventArgs e)
		{
			Controller.ToggleRunState();
		}
	}
}
