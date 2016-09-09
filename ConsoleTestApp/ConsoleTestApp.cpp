// ConsoleTestApp.cpp : Defines the entry point for the console application.
//

#include <stdio.h>
#include <windows.h>

int main(int argc, char** argv)
{
	printf("ConsoleTestApp\n\nArguments:\n");

	for (int i = 0; i < argc; ++i)
	{
		printf("%i: %s\n", i, argv[i]);
	}

	Sleep(2000);

    return 0;
}

