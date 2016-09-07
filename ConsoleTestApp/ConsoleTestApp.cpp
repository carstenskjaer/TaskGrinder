// ConsoleTestApp.cpp : Defines the entry point for the console application.
//

#include <stdio.h>

int main(int argc, char** argv)
{
	printf("ConsoleTestApp\n\nArguments:\n");

	for (int i = 0; i < argc; ++i)
	{
		printf("%i: %s\n", i, argv[i]);
	}

    return 0;
}

