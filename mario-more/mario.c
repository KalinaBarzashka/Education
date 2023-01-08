#include <cs50.h>
#include <stdio.h>

int main(void)
{
    int height = 1;

    //ask for height in do while for height between 1 and 8 inclusive
    do
    {
        height = get_int("Height: ");
    }
    while (height <= 0 || height > 8);

    for (int i = 0; i < height; i++)
    {
        int spaces = height - i - 1;
        int dashes = i + 1;

        //cycle for spaces
        for (int j = 0; j < spaces; j++)
        {
            printf(" ");
        }
        //cycle for dashes
        for (int j = 0; j < dashes; j++)
        {
            printf("#");
        }

        printf("  ");

        //cycle for second dashes
        for (int j = 0; j < dashes; j++)
        {
            printf("#");
        }

        //print new line
        printf("\n");
    }
}