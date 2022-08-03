#include "helpers.h"
#include <math.h>

int get_less(int value);

// Convert image to grayscale
void grayscale(int height, int width, RGBTRIPLE image[height][width])
{
    //each row
    for (int h = 0; h < height; h++)
    {
        //each column / pixel
        for (int w = 0; w < width; w++)
        {
            RGBTRIPLE pixel = image[h][w];
            //rgbtBlue/rgbtGreen/rgbtRed
            int sum = round((pixel.rgbtBlue + pixel.rgbtGreen + pixel.rgbtRed) / 3.0);
            image[h][w].rgbtBlue = sum;
            image[h][w].rgbtGreen = sum;
            image[h][w].rgbtRed = sum;
        }
    }

    return;
}

// Convert image to sepia
void sepia(int height, int width, RGBTRIPLE image[height][width])
{
    //each row
    for (int h = 0; h < height; h++)
    {
        //each column / pixel
        for (int w = 0; w < width; w++)
        {
            RGBTRIPLE pixel = image[h][w];
            BYTE original_red = pixel.rgbtRed;
            BYTE original_green = pixel.rgbtGreen;
            BYTE original_blue = pixel.rgbtBlue;

            int sepia_red = round(.393 * original_red + .769 * original_green + .189 * original_blue);
            int sepia_green = round(.349 * original_red + .686 * original_green + .168 * original_blue);
            int sepia_blue = round(.272 * original_red + .534 * original_green + .131 * original_blue);

            sepia_red = get_less(sepia_red);
            sepia_green = get_less(sepia_green);
            sepia_blue = get_less(sepia_blue);

            image[h][w].rgbtBlue = sepia_blue;
            image[h][w].rgbtGreen = sepia_green;
            image[h][w].rgbtRed = sepia_red;
        }
    }

    return;
}

int get_less(int value)
{
    if (value > 255)
    {
        return 255;
    }

    return value;
}

// Reflect image horizontally
void reflect(int height, int width, RGBTRIPLE image[height][width])
{
    //each row
    for (int h = 0; h < height; h++)
    {
        //each column / pixel
        for (int w = 0, i = width / 2; w < i; w++)
        {
            RGBTRIPLE tmpPixel = image[h][w];
            image[h][w] = image[h][width - w - 1];
            image[h][width - w - 1] = tmpPixel;
        }
    }

    return;
}

// Blur image
void blur(int height, int width, RGBTRIPLE image[height][width])
{
    //make a cory of the image
    RGBTRIPLE img_copy[height][width];
    for (int h = 0; h < height; h++)
    {
        for (int w = 0; w < width; w++)
        {
            img_copy[h][w] = image[h][w];
        }
    }

    //each row
    for (int h = 0; h < height; h++)
    {
        //each column / pixel
        for (int w = 0; w < width; w++)
        {
            //rgbtBlue/rgbtGreen/rgbtRed
            double counter = 0.0;
            //new avg colors
            int avgRed = 0;
            int avgGreen = 0;
            int avgBlue = 0;

            RGBTRIPLE crtPixel = img_copy[h][w];

            //top and bottom row pixels
            RGBTRIPLE topLeft = NULL;
            RGBTRIPLE topMiddle = NULL;
            RGBTRIPLE topRight = NULL;

            RGBTRIPLE bottomLeft = NULL;
            RGBTRIPLE bottomMiddle = NULL;
            RGBTRIPLE bottomRight = NULL;

            if (h - 1 >= 0) //check if we have top row
            {
                topMiddle = img_copy[h - 1][w];
                if (w - 1 >= 0) //check if we have top left pixel
                {
                    topLeft = img_copy[h - 1][w - 1];
                }
                else if (w + 1 < width) //check if we have top right pixel
                {
                    topRight = img_copy[h - 1][w + 1];
                }
            }
            else if (h + 1 < height) //check if we have bottom row
            {
                bottomMiddle = img_copy[h + 1][w];
                if (w - 1 >= 0) //check if we have bottom left pixel
                {
                    bottomLeft = img_copy[h + 1][w - 1];
                }
                else if (w + 1 < width) //check if we have top right pixel
                {
                    bottomRight = img_copy[h + 1][w + 1];
                }
            }

            //right and left current row pixels
            RGBTRIPLE crtRight = NULL;
            RGBTRIPLE crtLeft = NULL;
            if (w + 1 < width)
            {
                crtRight = img_copy[h][w + 1];
            }
            else if (w - 1 >= 0)
            {
                crtLeft = img_copy[h][w - 1];
            }



            if (topLeft != NULL) //top left
            {
                avgRed += topLeft.rgbtRed;
                avgGreen += topLeft.rgbtGreen;
                avgBlue += topLeft.rgbtBlue;
                counter++;
            }

            if (topMiddle != NULL) //top middle
            {
                avgRed += topMiddle.rgbtRed;
                avgGreen += topMiddle.rgbtGreen;
                avgBlue += topMiddle.rgbtBlue;
                counter++;
            }

            if (topRight != NULL) //top right
            {
                avgRed += topRight.rgbtRed;
                avgGreen += topRight.rgbtGreen;
                avgBlue += topRight.rgbtBlue;
                counter++;
            }

            if (bottomLeft != NULL) //bottom left
            {
                avgRed += bottomLeft.rgbtRed;
                avgGreen += bottomLeft.rgbtGreen;
                avgBlue += bottomLeft.rgbtBlue;
                counter++;
            }

            if (bottomMiddle != NULL) //bottom middle
            {
                avgRed += bottomMiddle.rgbtRed;
                avgGreen += bottomMiddle.rgbtGreen;
                avgBlue += bottomMiddle.rgbtBlue;
                counter++;
            }

            if (bottomRight != NULL) //bottom right
            {
                avgRed += bottomRight.rgbtRed;
                avgGreen += bottomRight.rgbtGreen;
                avgBlue += bottomRight.rgbtBlue;
                counter++;
            }

            if (crtRight != NULL) //current right
            {
                avgRed += crtRight.rgbtRed;
                avgGreen += crtRight.rgbtGreen;
                avgBlue += crtRight.rgbtBlue;
                counter++;
            }

            if (crtLeft != NULL) //current left
            {
                avgRed += crtLeft.rgbtRed;
                avgGreen += crtLeft.rgbtGreen;
                avgBlue += crtLeft.rgbtBlue;
                counter++;
            }

            if (crtPixel != NULL) //current pixel
            {
                avgRed += crtPixel.rgbtRed;
                avgGreen += crtPixel.rgbtGreen;
                avgBlue += crtPixel.rgbtBlue;
                counter++;
            }

            avgRed = round(avgRed / counter);
            avgGreen = round(avgGreen / counter);
            avgBlue = round(avgBlue / counter);

            image[h][w].rgbtRed = avgRed;
            image[h][w].rgbtGreen = avgGreen;
            image[h][w].rgbtBlue = avgBlue;
        }
    }



    return;
}
