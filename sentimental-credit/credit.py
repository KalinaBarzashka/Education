# American Express - 15-digit numbers - starts with 34 or 37
# MasterCard - 16-digit numbers - starts with 51, 52, 53, 54, or 55
# Visa - 13 and 16-digit numbers - 4
# checksum

import re
from cs50 import get_string


def main():
    card_number = get_string("Number: ")
    length = len(card_number)
    card_num_int = int(card_number)
    first_number = int(card_number[0])
    second_number = int(card_number[1])

    valid = checksum(card_num_int, length)

    if valid == True:
        if (length == 15 and first_number == 3 and (second_number == 4 or second_number == 7)):
            print("AMEX\n")
        elif ((length == 13 or length == 16) and first_number == 4):
            print("VISA\n")
        elif (length == 16 and first_number == 5 and (second_number == 1 or second_number == 2 or second_number == 3 or second_number == 4 or second_number == 5)):
            print("MASTERCARD\n")
        else:
            print("INVALID\n")
    else:
        print("INVALID\n")


def checksum(card_number, length):
    # sum of multiplied numbers
    sum_from_multiplication = 0
    # temp variable to store multiplied number, so if > 9 we can get the separate digits
    temp = 0
    # sum of other digits
    sum_not_multiplied = 0

    for i in range(length):
        print(i % 2)
        print(card_number % 10)
        continue
        # sum every even digit
        if i % 2 == 0:
            sum_not_multiplied = sum_not_multiplied + (card_number % 10)
        # multiply every other digit starting from the second to last
        else:
            temp = (card_number % 10) * 2

            if temp > 9:
                sum_from_multiplication = sum_from_multiplication + temp % 10
                temp = temp / 10
                sum_from_multiplication = sum_from_multiplication + temp % 10
            else:
                sum_from_multiplication = sum_from_multiplication + temp

        card_number = card_number / 10

    # get final sum
    final_sum = sum_from_multiplication + sum_not_multiplied

    # get last digit from final sum
    if final_sum % 10 == 0:
        return True

    return False


main()