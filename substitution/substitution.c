#include <cs50.h>
#include <stdio.h>
#include <ctype.h>
#include <string.h>

bool only_alphab_chars(string key);
char rotate(char symbol, string key);

int main(int argc, string argv[])
{
    //check if there are only 2 arguments: program name and a key
    if (argc != 2)
    {
        printf("Usage: ./caesar key\n");
        return 1;
    }

    string key = argv[1];
    if (strlen(key) != 26 || !only_alphab_chars(key)) //TODO: containing each letter exactly once
    {
        printf("Key must contain 26 characters.\n");
        return 1;
    }

    //prompt for plain text
    string plain_text = get_string("plaintext:  ");
    //set initial cipher text which will be rotated
    string cipher_text = plain_text;
    //get length of plain text
    int length = strlen(plain_text);

    //iterate over every character
    for (int i = 0; i < length; i++)
    {
        char symbol = plain_text[i];
        cipher_text[i] = rotate(symbol, key);
    }

    printf("ciphertext: %s\n", cipher_text);
    return 0;
}

char rotate(char symbol, string key)
{
    if (!isalpha(symbol))
    {
        return symbol;
    }

    int ascii_starts_with = 0;
    bool isupper = false;
    if (isupper(symbol))
    {
        ascii_starts_with = 65;
        isupper = true;
    }
    else if (islower(symbol))
    {
        ascii_starts_with = 97;
    }

    //get ascii number of symbol
    int symbol_ascii = (int)symbol;
    //get letter number (number between 0 and 25 - 26 letters total)
    int letter_count = symbol_ascii - ascii_starts_with;
    //increment letter with key
    char new_letter = key[letter_count];

    if (isupper)
    {
        return toupper(new_letter);
    }

    return tolower(new_letter);

}

bool only_alphab_chars(string key)
{
    int key_length = strlen(key);
    for (int i = 0; i < key_length; i++)
    {
        //check if character is not a aplhabetic character return false;
        if (!isalpha(key[i]))
        {
            return false;
        }
    }

    //if every character is a alphabetic character - ok, return true
    return true;
}