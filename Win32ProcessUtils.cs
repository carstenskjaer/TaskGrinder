using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrinder
{
	static class Win32ProcessUtils
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct STARTUPINFO
		{
			public Int32 cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public Int32 dwX;
			public Int32 dwY;
			public Int32 dwXSize;
			public Int32 dwYSize;
			public Int32 dwXCountChars;
			public Int32 dwYCountChars;
			public Int32 dwFillAttribute;
			public Int32 dwFlags;
			public Int16 wShowWindow;
			public Int16 cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct PROCESS_INFORMATION
		{
			public IntPtr hProcess;
			public IntPtr hThread;
			public int dwProcessId;
			public int dwThreadId;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct SECURITY_ATTRIBUTES
		{
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public int bInheritHandle;
		}

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern bool CreateProcess(
			string lpApplicationName,
			string lpCommandLine,
			ref SECURITY_ATTRIBUTES lpProcessAttributes,
			ref SECURITY_ATTRIBUTES lpThreadAttributes,
			bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			[In] ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);
		
		[DllImport("kernel32.dll")]
		static extern bool CreatePipe(out IntPtr hReadPipe, out IntPtr hWritePipe,
		   ref SECURITY_ATTRIBUTES lpPipeAttributes, uint nSize);

		[Flags]
		enum HANDLE_FLAGS : uint
		{
			None = 0,
			INHERIT = 1,
			PROTECT_FROM_CLOSE = 2
		}
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool SetHandleInformation(IntPtr hObject, HANDLE_FLAGS dwMask, HANDLE_FLAGS dwFlags);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
		const UInt32 INFINITE = 0xFFFFFFFF;
		const UInt32 WAIT_ABANDONED = 0x00000080;
		const UInt32 WAIT_OBJECT_0 = 0x00000000;
		const UInt32 WAIT_TIMEOUT = 0x00000102;

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);


		public static void RunSubProcess(string commandline)
		{
			var securityAttributes = new SECURITY_ATTRIBUTES();
			securityAttributes.bInheritHandle = 1;
			securityAttributes.nLength = Marshal.SizeOf<SECURITY_ATTRIBUTES>();

			IntPtr stdOutReadHandle, stdOutWriteHandle;
			if (!CreatePipe(out stdOutReadHandle, out stdOutWriteHandle, ref securityAttributes, 0))
			{
				throw new Exception("Failed to create stdout pipe");
			}
			if (!SetHandleInformation(stdOutReadHandle, HANDLE_FLAGS.INHERIT, HANDLE_FLAGS.None))
			{
				throw new Exception("Failed to set inherit flag");
			}

			IntPtr stdErrReadHandle, stdErrWriteHandle;
			if (!CreatePipe(out stdErrReadHandle, out stdErrWriteHandle, ref securityAttributes, 0))
			{
				throw new Exception("Failed to create pipe");
			}
			if (!SetHandleInformation(stdErrReadHandle, HANDLE_FLAGS.INHERIT, HANDLE_FLAGS.None))
			{
				throw new Exception("Failed to set inherit flag");
			}

			IntPtr stdInReadHandle, stdInWriteHandle;
			if (!CreatePipe(out stdInReadHandle, out stdInWriteHandle, ref securityAttributes, 0))
			{
				throw new Exception("Failed to create pipe");
			}
			if (!SetHandleInformation(stdInWriteHandle, HANDLE_FLAGS.INHERIT, HANDLE_FLAGS.None))
			{
				throw new Exception("Failed to set inherit flag");
			}

			var processInformation = new PROCESS_INFORMATION();
			var startupInfo = new STARTUPINFO();

			startupInfo.dwFlags = /*STARTF_USESTDHANDLES*/ 0x00000100;
			startupInfo.hStdOutput = stdOutWriteHandle;
			startupInfo.hStdError = stdErrWriteHandle;
			startupInfo.hStdInput = stdInReadHandle;
			startupInfo.dwFlags = 0x00000001; // STARTF_USESHOWWINDOW;
			startupInfo.wShowWindow = 0; // SW_HIDE;

			securityAttributes.bInheritHandle = 0;

			if (!CreateProcess(
				null,
				commandline,
				ref securityAttributes,
				ref securityAttributes,
				true,
				0, // Flags
				IntPtr.Zero,
				null,
				ref startupInfo,
				out processInformation
			))
			{
				throw new Exception("Failed to create process");
			}

			var processHandle = processInformation.hProcess;

			if (WaitForSingleObject(processHandle, INFINITE) != WAIT_OBJECT_0)
			{
				throw new Exception("Failed to wait for process");
			}

			uint exitCode;
			if (!GetExitCodeProcess(processHandle, out exitCode))
			{
				throw new Exception("Failed to close process");
			}

			if (!CloseHandle(processHandle))
			{
				throw new Exception("Failed to close process");
			}
			if (!CloseHandle(processInformation.hThread))
			{
				throw new Exception("Failed to close thread");
			}

		}

	}
}
