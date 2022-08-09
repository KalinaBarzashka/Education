// Implements a dictionary's functionality

#include <ctype.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <strings.h>

#include "dictionary.h"

// Represents a node in a hash table
typedef struct node
{
    char word[LENGTH + 1];
    struct node *next;
}
node;

// TODO: Choose number of buckets in hash table
const unsigned int N = 26;
int words_count = 0;

// Hash table
node *table[N];

//traverse function
bool traverse_find_word(node *n, const char *word);

// Returns true if word is in dictionary, else false
bool check(const char *word)
{
    //make word lowercase
    char *lower_word = malloc(sizeof(word));
    for (int i = 0, n = strlen(word); i < n; i++)
    {
        lower_word[i] = tolower(word[i]);
    }

    // has word, find index in hash table
    int index = hash(lower_word);
    free(lower_word);

    node *n = table[index];
    //if there are no nodes
    if (n == NULL)
    {
        return false;
    }

    return traverse_find_word(n, word);
}

// Hashes word to a number
unsigned int hash(const char *word)
{
    // TODO: Improve this hash function
    return toupper(word[0]) - 'A';
}

// Loads dictionary into memory, returning true if successful, else false
bool load(const char *dictionary)
{
    // TODO
    FILE *dict = fopen(dictionary, "r");
    if (dict == NULL)
    {
        return false;
    }

    char *word = NULL;
    while (fscanf(dict, "%s", word) != EOF)
    {
        node *n = malloc(sizeof(node));
        if (n == NULL)
        {
            return false;
        }

        strcpy(n->word, word);
        int index = hash(word);

        if (table[index] == NULL)
        {
            table[index] = n;
        }
        else
        {
            n->next = table[index];//first element in the linked list
            table[index] = n;
        }

        free(n);
        words_count++;
    }

    fclose(dict);
    return true;
}

// Returns number of words in dictionary if loaded, else 0 if not yet loaded
unsigned int size(void)
{
    // return words count, counted in load functionality
    return words_count;
}

// Unloads dictionary from memory, returning true if successful, else false
bool unload(void)
{

    return false;
}

bool traverse_find_word(node *n, const char *word)
{
    if (strcasecmp(word, n->word) == 0)
    {
        return true;
    }

    if (n->next != NULL)
    {
        n = n->next;
        return traverse_find_word(n, word);
    }

    return false;
}