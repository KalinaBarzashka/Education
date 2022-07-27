#include <cs50.h>
#include <stdio.h>
#include <string.h>
#include <ctype.h>

int count_letters(string text);
int count_words(string text);

int main(void)
{
    string text = get_string("Text: ");
    int letters = count_letters(text);
    int words = count_words(text);
    int sentences = 0;

    printf("%i letters\n", letters);
    printf("%i words\n", words);
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
    
}