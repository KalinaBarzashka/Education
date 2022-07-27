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

    for(int i = 0; i < height; i++)
    {
        for(int j = ; j++)
        {
            printf();
        }
        printf("#\n");
    }
}