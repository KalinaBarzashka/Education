#include <cs50.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

bool check_key(string key, int key_length);

int main(int argc, string argv[])
{
    if (argc != 2)
    {
        printf("Usage: ./caesar key");
        return 1;
    }

    int key_length = strlen(argv[1]);
    string key = argv[1];
    if(!check_key)
    {
        printf("Usage: ./caesar key");
        return 1;
    }

    //convert string key to int key
    int key = atoi(key);

    string plain_text = get_string("plaintext:  ");
    string cipher_text = plain_text;
    int length = strlen(plain_text);

    for (int i = 0; i < length; i++)
    {
        if (isalpha(plain_text[i]))
        {
            cipher_text[i] = '';
        }
    }

    printf("ciphertext: %s\n", cipher_text);
    return 0;
}

bool check_key(string key, int key_length)
{
    for (int i = 0; i < key_length; i++)
    {
        if (!isdigit(key[i]))
        {
            printf("Usage: ./caesar key");
            return false;
        }
    }

    return true;
}