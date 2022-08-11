from cs50 import get_int

height = 0
while height < 1 or height > 8:
    height = get_int("Height: ")

for i in range(4):
    for j in range(4):
        print("#", end="")
    print()