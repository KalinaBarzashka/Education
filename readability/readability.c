#include <cs50.h>
#include <stdio.h>
#include <string.h>
#include <ctype.h>

int count_letters(string text);

int main(void)
{
    string text = get_string("Text: ");
    int letters = count_letters(text);
    int words = 0;
    int sentences = 0;

    printf("%i\n", letters);
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