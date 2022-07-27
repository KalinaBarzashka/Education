#include <cs50.h>
#include <stdio.h>

int main(void)
{
    int height = 0;
    do
    {
        height = get_int("Height: ");
    }
    while(height <= 0 || height > 8);

    for(int i = 1; i <= height; i++)
    {
        for(int j = height - i; j <= 0; j++)
        {
            printf("%0*d", j, ' ');
        }
        printf("%0*d\n", i, '#');
    }
}