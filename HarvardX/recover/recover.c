#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>

// Number of bytes in JPEG
const int BLOCK_SIZE = 512;
typedef uint8_t BYTE;

int main(int argc, char *argv[])
{
    // Check command-line arguments
    if (argc != 2)
    {
        printf("Usage: ./recover IMAGE\n");
        return 1;
    }

    //open file card for read
    FILE *card = fopen(argv[1], "r");
    //check if file is successfully opened
    if (card == NULL) //or !card
    {
        printf("Could not open file.\n");
        return 1;
    }

    int images_count = 0;
    BYTE buffer[BLOCK_SIZE];
    FILE *new_image;
    while (fread(buffer, 1, BLOCK_SIZE, card) == BLOCK_SIZE)
    {
        //check if a new jpeg file is reached - if now, write block; bitwise arithmetic
        if (buffer[0] == 0xff && buffer[1] == 0xd8 && buffer[2] == 0xff && (buffer[3] & 0xf0) == 0xe0)
        {
            if (images_count > 0)
            {
                fclose(new_image);
            }

            char image_name[8];
            sprintf(image_name, "%03i.jpg", images_count);
            new_image = fopen(image_name, "w");

            if (new_image == NULL)
            {
                printf("Could not open file.\n");
                return 1;
            }

            fwrite(buffer, BLOCK_SIZE, 1, new_image);

            images_count++;
            continue;
        }
        else if (images_count > 0)
        {
            fwrite(buffer, BLOCK_SIZE, 1, new_image);
        }
    }

    // Close files
    fclose(card);
    fclose(new_image);
    return 0;
}