using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TaskGrinder
{
	/// <summary>
	/// Interaction logic for TaskEditWindow.xaml
	/// </summary>
	public partial class TaskEditDialog : Window
	{
		public TaskEditDialog(Task task)
		{
			DataContext = task;
			InitializeComponent();
			BindingGroup.BeginEdit();
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			BindingGroup.CommitEdit();
			DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			BindingGroup.CancelEdit();
			DialogResult = false;
		}

		private void ExeFileSelectButton_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new Microsoft.Win32.OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
				ExeTextBox.Text = openFileDialog.FileName;
		}

		private void WorkingDirFileSelectButton_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new Microsoft.Win32.OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
				WorkingDirTextBox.Text = openFileDialog.FileName;
		}
	}
}
