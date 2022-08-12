from cs50 import get_string
import re

def main():
    text = get_string("Text: ")
    # get letters count from text
    letters = count_letters(text);
    # get words count from text
    words = count_words(text);
    # get sentences count from text
    sentences = count_sentences(text);


def count_letters(text):
    count = 0
    for i in range(len(text)):
        symbol = text[i]
        # check if symbol is a letter
        if isalpha(symbol):
            count += 1

    return count


def count_words(text):
    count = 0
    for i in range(len(text)):
        symbol = text[i]
        prev_symbol = i == 0 ? "" : text[i-1]
        # check if symbol is a space(32) and if the symbol before was not a space
        if isspace(symbol) and not isspace(prev_symbol):
            count += 1

    # check if text starts with space and if it does substract one
    if isspace(text[0]):
        count -= 1
    # check if text ends with space and if it does substract one
    if isspace(text[n - 1]):
        count -= 1

    # return count + 1 for the last word in the sentance
    return count


def count_sentences(text):
    count = 0
    for i in range(len(text)):
        symbol = text[i]
        # check if symbol is a !(33), ?(63) or .(46)
    return count


main()