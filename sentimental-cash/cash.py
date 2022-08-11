from cs50 import get_float



def main():
    coins = 0
    change = get_float("Change owed: ")

    # if change is negative value, re-promt user for change until its positive float value
    while change < 0:
        change = get_float("Change owed: ")

    coins += calculate_quarters(change)
    coins += calculate_dimes(change)
    coins += calculate_nickels(change)
    coins += calculate_pennies(change)
    print(coins)

def calculate_quarters(change):
    return change / 0.25

def calculate_dimes(change):
    return change / 0.10

def calculate_nickels(change):
    return change / 0.05

def calculate_pennies(change):
    return change / 0.01

main()