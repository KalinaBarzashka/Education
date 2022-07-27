#include <cs50.h>
#include <stdio.h>

int main(void)
{
    int height = 1;
    do
    {
        height = get_int("Height: ");
    }
    while(height <= 0 || height > 8);

    for(int i = 1; i <= height; i++)
    {
        printf("#");
    }
}