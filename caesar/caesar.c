#include <cs50.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

bool only_digits (string key, int key_length);
char rotate(char symbol, int key);

int main(int argc, string argv[])
{
    //check if there are only 2 arguments: program name and a key
    if (argc != 2)
    {
        printf("Usage: ./caesar key\n");
        return 1;
    }

    int key_length = strlen(argv[1]);
    string key_as_str = argv[1];
    //ckeck if key got only digits!
    if(!only_digits(key_as_str, key_length))
    {
        printf("Usage: ./caesar key\n");
        return 1;
    }

    //convert string key to int key
    int key = atoi(key);

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

bool only_digits(string key, int key_length)
{
    for (int i = 0; i < key_length; i++)
    {
        //check if character is not a digit return false;
        if (!isdigit(key[i]))
        {
            return false;
        }
    }

    //if every character is a digit - ok, return true
    return true;
}

char rotate(char symbol, int key)
{
    if (isalpha(symbol))
    {

    }

    return symbol;
}