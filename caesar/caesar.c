#include <cs50.h>
#include <stdio.h>
#include <string.h>

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

    string plain_text = get_string("plaintext:  ");
    string cipher_text = plain_text;
    int length = strlen(plain_text);

    for (int i = 0; i < length; i++)
    {

    }

    printf("ciphertext: %s\n", cipher_text);
}

bool check_key(string key, int key_length)
{
    for (int i = 0; i < key_length; i++)
    {
        if (isalpha(key[i]))
        {
            printf("Usage: ./caesar key");
            return false;
        }
    }

    return true;
}