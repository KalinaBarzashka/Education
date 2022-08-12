from cs50 import get_string


def main():
    text = get_string("Text: ")
    # get letters count from text
    letters = count_letters(text)
    # get words count from text
    words = count_words(text)
    # get sentences count from text
    sentences = count_sentences(text)

    # average letters per words
    l = (letters / float(words)) * 100
    # average sentences per words
    s = (sentences / float(words)) * 100
    # use Coleman-Liau index
    index = round(0.0588 * l - 0.296 * s - 15.8)

    if index < 1:
        print("Before Grade 1")
    elif index > 16:
        print("Grade 16+")
    else:
        print(f"Grade {index}")


def count_letters(text):
    count = 0
    for i in range(len(text)):
        symbol = text[i]
        # check if symbol is a letter
        if symbol.isalpha():
            count += 1

    return count


def count_words(text):
    count = 0
    lenght = len(text)
    for i in range(lenght):
        symbol = text[i]
        prev_symbol = "" if i == 0 else text[i-1]
        # check if symbol is a space(32) and if the symbol before was not a space
        if symbol.isspace() and not prev_symbol.isspace():
            count += 1

    # check if text starts with space and if it does substract one
    if text[0].isspace():
        count -= 1
    # check if text ends with space and if it does substract one
    if text[lenght-1].isspace():
        count -= 1

    # return count + 1 for the last word in the sentance
    return count + 1


def count_sentences(text):
    count = 0
    for i in range(len(text)):
        symbol = text[i]
        # check if symbol is a !(33), ?(63) or .(46)
        exclam_mark = symbol.count('!')
        quest_mark = symbol.count('?')
        dot = symbol.count('.')

        if exclam_mark == 1 or quest_mark == 1 or dot == 1:
            count += 1
    return count


main()