#include "helpers.h"
#include <math.h>

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
            BYTE originalRed = pixel.rgbtRed;
            BYTE originalGreen = pixel.rgbtGreen;
            BYTE originalBlue = pixel.rgbtBlue;

            int sepiaRed = round(.393 * originalRed + .769 * originalGreen + .189 * originalBlue);
            int sepiaGreen = round(.349 * originalRed + .686 * originalGreen + .168 * originalBlue);
            int sepiaBlue = round(.272 * originalRed + .534 * originalGreen + .131 * originalBlue);

            if (sepiaRed > 255)
            {
                sepiaRed = 255;
            }
            else if (sepiaGreen > 255)
            {
                sepiaGreen = 255;
            }
            else if (sepiaBlue > 255)
            {
                sepiaBlue = 255;
            }

            image[h][w].rgbtBlue = sepiaBlue;
            image[h][w].rgbtGreen = sepiaGreen;
            image[h][w].rgbtRed = sepiaRed;
        }
    }

    return;
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
    //each row
    for (int h = 0; h < height; h++)
    {
        //each column / pixel
        for (int w = 0; w < width; w++)
        {
            RGBTRIPLE crtPixel = image[h][w];

            //top and bottom row pixels
            RGBTRIPLE topLeft = NULL;
            RGBTRIPLE topMiddle = NULL;
            RGBTRIPLE topRight = NULL;

            RGBTRIPLE bottomLeft = NULL;
            RGBTRIPLE bottomMiddle = NULL;
            RGBTRIPLE bottomRight = NULL;

            if (h - 1 >= 0) //check if we have top row
            {
                topMiddle = image[h - 1][w];
                if (w - 1 >= 0) //check if we have top left pixel
                {
                    topLeft = image[h - 1][w - 1];
                }
                else if (w + 1 < width) //check if we have top right pixel
                {
                    topRight = image[h - 1][w + 1];
                }
            }
            else if (h + 1 < heigth) //check if we have bottom row
            {
                bottomMiddle = image[h + 1][w];
                if (w - 1 >= 0) //check if we have bottom left pixel
                {
                    bottomLeft = image[h + 1][w - 1];
                }
                else if (w + 1 < width) //check if we have top right pixel
                {
                    bottomRight = image[h + 1][w + 1];
                }
            }

            //right and left current row pixels
            RGBTRIPLE crtRight = NULL;
            RGBTRIPLE crtLeft = NULL;
            if (w + 1 < width)
            {
                crtRight = image[h][w + 1];
            }
            else if (w - 1 >= 0)
            {
                crtLeft = image[h][w - 1];
            }

            
        }
    }

    return;
}
