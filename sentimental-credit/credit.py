# American Express - 15-digit numbers - starts with 34 or 37
# MasterCard - 16-digit numbers - starts with 51, 52, 53, 54, or 55
# Visa - 13 and 16-digit numbers - 4
# checksum

import re
from cs50 import get_string

card_number = get_string("Number: ")
length = len(card_number)
card_num_int = int(card_number)
first_number = int(card_number[0])
second_number = int(card_number[1])


print(card_num_int)