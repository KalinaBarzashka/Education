from cs50 import get_float
from math import floor


def main():
    coins = 0
    change = get_float("Change owed: ")

    # if change is negative value, re-promt user for change until its positive float value
    while change < 0:
        change = get_float("Change owed: ")

    # calculate quarters
    quarters = calculate_quarters(change)
    change = round(change - quarters * 0.25, 2)
    coins += quarters

    # calculate dimes
    dimes = calculate_dimes(change)
    change = round(change - dimes * 0.10, 2)
    coins += dimes

    # calculate nickels
    nickels = calculate_nickels(change)
    change = round(change - nickels * 0.05, 2)
    coins += nickels

    # calculate pennies
    coins += calculate_pennies(change)
    print(coins)


def calculate_quarters(change):
    return floor(change / 0.25)


def calculate_dimes(change):
    return floor(change / 0.10)


def calculate_nickels(change):
    return floor(change / 0.05)


def calculate_pennies(change):
    return floor(change / 0.01)


main()
