#include <cs50.h>
#include <stdio.h>

int main(int argc, string argv[])
{
    //check if there are only 2 arguments: program name and a key
    if (argc != 2)
    {
        printf("Usage: ./caesar key\n");
        return 1;
    }
    else if (strlen(argv[1]) != 26)
    {
        printf("Key must contain 26 characters.\n");
        return 1;
    }
}