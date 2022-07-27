#include <cs50.h>
#include <stdio.h>
#include <string.h>

int main(int argc, string argv[])
{
    if (argc != 2)
    {
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