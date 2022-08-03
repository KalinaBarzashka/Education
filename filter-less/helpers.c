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
    return;
}

// Reflect image horizontally
void reflect(int height, int width, RGBTRIPLE image[height][width])
{
    return;
}

// Blur image
void blur(int height, int width, RGBTRIPLE image[height][width])
{
    return;
}
