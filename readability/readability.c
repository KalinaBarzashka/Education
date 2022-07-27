#include <cs50.h>
#include <stdio.h>
#include <string.h>
#include <ctype.h>
#include <math.h>

int count_letters(string text);
int count_words(string text);
int count_sentences(string text);

int main(void)
{
    string text = get_string("Text: ");
    int letters = count_letters(text);
    int words = count_words(text);
    int sentences = count_sentences(text);

    printf("letters %i\n", letters);
    printf("words %i\n", words);
    printf("sentences %i\n", sentences);

    // average letters per words
    double l = letters / words * 100;
    // average sentences per words
    double s = sentences / words * 100;

    int index = round(0.0588 * l - 0.296 * s - 15.8);

    if (index < 1)
    {
        printf("Before Grade 1\n");
    }
    else if (index > 16)
    {
        printf("Grade 16+\n");
    }
    else
    {
        printf("Grade %i\n", index);
    }
}

int count_letters(string text)
{
    int count = 0;
    for (int i = 0, n = strlen(text); i < n; i++)
    {
        char symbol = text[i];
        if (isalpha(symbol))
        {
            count++;
        }
    }

    return count;
}

int count_words(string text)
{
    int count = 0;
    int n = strlen(text);
    for (int i = 0; i < n; i++)
    {
        char symbol = text[i];
        if ((int)symbol == 32 && !((int)text[i - 1] == 32))
        {
            count++;
        }
    }

    if (text[0] == ' ')
    {
        count--;
    }

    if (text[n - 1] == ' ')
    {
        count--;
    }

    //return count + 1 for the last word in the sentance
    return count + 1;
}

int count_sentences(string text)
{
    int count = 0;
    for (int i = 0, n = strlen(text); i < n; i++)
    {
        char symbol = text[i];
        if (symbol == 33 || symbol == 63 || symbol == 46)
        {
            count++;
        }
    }

    return count;
}