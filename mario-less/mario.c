#include <cs50.h>
#include <stdio.h>

int main(void)
{
    int height = 0;
    do
    {
        height = get_int("Enter positive height of the pyramid between 1 and 8: ");
    }
    while(height <= 0 || height > 8);

    printf("Height is: %i \n", height);
}