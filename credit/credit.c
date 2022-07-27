#include <cs50.h>
#include <stdio.h>
#include <math.h>

bool checksum(long number, int length_of_number);

int main(void)
{
    //American Express - 15-digit numbers - starts with 34 or 37
    //MasterCard - 16-digit numbers - starts with 51, 52, 53, 54, or 55
    //Visa - 13 and 16-digit numbers - 4
    //checksum

    long card_number = get_long("Number: ");
    //get length of number
    int length_of_number = floor(log10(number)) + 1;

    bool valid = checksum(card_number, length_of_number);

    if (valid)
    {
        if (length_of_number == 15 && )
        {

        }
    }
    else
    {
        printf("INVALID\n");
    }
}

bool checksum(long number, int length_of_number)
{
    //sum of multiplied numbers
    int sum_from_multiplication = 0;

    //sum of other digits
    int sum_not_multiplied = 0;

    int

    for (int i = 0; i < length_of_number; i++)
    {
        if (i % 2 == 0)
        {
            sum_not_multiplied = sum_not_multiplied + (number % 10);
        }
        else
        {
            sum_from_multiplication = sum_from_multiplication + (number % 10) * 2;
        }

        number = number / 10;
    }

    int final_sum = sum_from_multiplication + sum_not_multiplied;

    if (final_sum % 10 == 0)
    {
        return true;
    }

    return false;
}