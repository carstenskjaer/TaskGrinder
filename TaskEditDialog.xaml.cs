﻿using System;
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
			Task = task;
			InitializeComponent();
		}

		public Task Task = null;

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}