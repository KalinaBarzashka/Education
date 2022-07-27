#include <cs50.h>
#include <stdio.h>
#include <math.h>

bool checksum(long number, int length_of_number);
int get_first_number(long card_number, int length);
int get_second_number(long card_number, int length);

int main(void)
{
    //American Express - 15-digit numbers - starts with 34 or 37
    //MasterCard - 16-digit numbers - starts with 51, 52, 53, 54, or 55
    //Visa - 13 and 16-digit numbers - 4
    //checksum

    long card_number = get_long("Number: ");
    //get length of number
    int length_of_number = floor(log10(card_number)) + 1;

    //get first and second numbers
    int first_number = get_first_number(card_number, length_of_number);
    int second_number = get_second_number(card_number, length_of_number);

    bool valid = checksum(card_number, length_of_number);

    if (valid)
    {
        if (length_of_number == 15 && first_number == 3 && (second_number == 4 || second_number == 7))
        {
            printf("AMEX\n");
        }
        else if ((length_of_number == 13 || length_of_number == 16) && first_number == 4)
        {
            printf("VISA\n");
        }
        else if (length_of_number == 16 && first_number == 5 && (second_number == 1 || second_number == 2 || second_number == 3 || second_number == 4 || second_number == 5))
        {
            printf("MASTERCARD\n");
        }
        else
        {
            printf("INVALID\n");
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
    int temp = 0;

    //sum of other digits
    int sum_not_multiplied = 0;

    for (int i = 0; i < length_of_number; i++)
    {
        if (i % 2 == 0)
        {
            sum_not_multiplied = sum_not_multiplied + (number % 10);
        }
        else
        {
            temp = (number % 10) * 2;

            if(temp > 9)
            {
                sum_from_multiplication = sum_from_multiplication + temp % 10;
                temp = temp / 10;
                sum_from_multiplication = sum_from_multiplication + temp % 10;
            }
            else
            {
                sum_from_multiplication = sum_from_multiplication + temp;
            }
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

int get_first_number(long card_number, int length)
{
    long division = 1;
    for (int i = 0; i < length - 1; i++)
    {
        division = division * 10;
    }

    return card_number / division;
}

int get_second_number(long card_number, int length)
{
    long division = 1;
    for (int i = 0; i < length - 2; i++)
    {
        division = division * 10;
    }

    return (card_number / division) % 10;
}