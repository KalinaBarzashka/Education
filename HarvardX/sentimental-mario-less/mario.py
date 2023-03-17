from cs50 import get_int

# get height from user input between 1 and 8 inclusive
height = 0
while height < 1 or height > 8:
    height = get_int("Height: ")

for i in range(height):
    # print spaces
    for j in range(height-i-1):
        print(" ", end="")
    # print dashes
    for j in range(i+1):
        print("#", end="")
    print()