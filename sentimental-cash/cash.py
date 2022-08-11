from cs50 import get_float

change = get_float("Change owed: ")
coins = 0

while change < 1:
    change = get_float("Change owed: ")

print(change)