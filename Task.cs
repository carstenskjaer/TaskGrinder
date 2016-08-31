﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrinder
{
	public class Task
	{
		public Task(string name)
		{
			Name = name;
		}

		public TaskRunner GetTaskRunner()
		{
			return new TaskRunner(Name,
				"",
				"",
				"");
		}

		public string Name { get; set; } = "";

		public string CommandLine { set; get; }

		public override string ToString()
		{
			return Name;
		}
	}

}
