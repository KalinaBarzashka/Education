#include <cs50.h>
#include <stdio.h>
#include <math.h>

bool checksum(long number);

int main(void)
{
    //American Express - 15-digit numbers - starts with 34 or 37
    //MasterCard - 16-digit numbers - starts with 51, 52, 53, 54, or 55
    //Visa - 13 and 16-digit numbers - 4
    //checksum

    long card_number = get_long("Number: ");
    bool valid = checksum(number);

    if (!valid)
    {
        printf("INVALID\n");
    }
    else
    {
        printf("VALID");
    }
}

bool checksum(long number)
{
    int length_of_number = floor(log10(abs(number))) + 1;
    for (int i = 0; i < number.length)
}