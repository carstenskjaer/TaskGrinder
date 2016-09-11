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

	int sleepTimeMS = 1000;
	if (argc > 1)
	{
		sleepTimeMS = atoi(argv[1]);
	}

	int returnCode = 0;
	if (argc > 2)
	{
		returnCode = atoi(argv[2]);
	}

	printf("Sleeping %i\n", sleepTimeMS);
	Sleep(sleepTimeMS);

	printf("Returning %i\n", returnCode);

	return returnCode;
}

