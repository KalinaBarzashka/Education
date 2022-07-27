#include <cs50.h>
#include <stdio.h>

int main(int argc, string argv[])
{
    string plain_text = get_string("plaintext:  ");
    string cipher_text = plain_text;

    printf("ciphertext: %s", cipher_text);
}