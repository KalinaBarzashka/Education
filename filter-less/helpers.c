#include "helpers.h"
#include <math.h>

int get_less(int value);
void make_copy(int height, int width, RGBTRIPLE image[height][width], RGBTRIPLE img_copy[height][width]);

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
            RGBTRIPLE tmp_pixel = image[h][w];
            image[h][w] = image[h][width - w - 1];
            image[h][width - w - 1] = tmp_pixel;
        }
    }

    return;
}

// Blur image
void blur(int height, int width, RGBTRIPLE image[height][width])
{
    //make a cory of the image
    RGBTRIPLE img_copy[height][width];
    make_copy(height, width, image, img_copy);

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
            avgRed += crtPixel.rgbtRed;
            avgGreen += crtPixel.rgbtGreen;
            avgBlue += crtPixel.rgbtBlue;
            counter++;

            //top and bottom row pixels
            if (h - 1 >= 0) //check if we have top row
            {
                RGBTRIPLE topMiddle = img_copy[h - 1][w];
                avgRed += topMiddle.rgbtRed;
                avgGreen += topMiddle.rgbtGreen;
                avgBlue += topMiddle.rgbtBlue;
                counter++;

                if (w - 1 >= 0) //check if we have top left pixel
                {
                    RGBTRIPLE topLeft = img_copy[h - 1][w - 1];
                    avgRed += topLeft.rgbtRed;
                    avgGreen += topLeft.rgbtGreen;
                    avgBlue += topLeft.rgbtBlue;
                    counter++;
                }
                else if (w + 1 < width) //check if we have top right pixel
                {
                    RGBTRIPLE topRight = img_copy[h - 1][w + 1];
                    avgRed += topRight.rgbtRed;
                    avgGreen += topRight.rgbtGreen;
                    avgBlue += topRight.rgbtBlue;
                    counter++;
                }
            }
            else if (h + 1 < height) //check if we have bottom row
            {
                RGBTRIPLE bottomMiddle = img_copy[h + 1][w];
                avgRed += bottomMiddle.rgbtRed;
                avgGreen += bottomMiddle.rgbtGreen;
                avgBlue += bottomMiddle.rgbtBlue;
                counter++;

                if (w - 1 >= 0) //check if we have bottom left pixel
                {
                    RGBTRIPLE bottomLeft = img_copy[h + 1][w - 1];
                    avgRed += bottomLeft.rgbtRed;
                    avgGreen += bottomLeft.rgbtGreen;
                    avgBlue += bottomLeft.rgbtBlue;
                    counter++;
                }
                else if (w + 1 < width) //check if we have top right pixel
                {
                    RGBTRIPLE bottomRight = img_copy[h + 1][w + 1];
                    avgRed += bottomRight.rgbtRed;
                    avgGreen += bottomRight.rgbtGreen;
                    avgBlue += bottomRight.rgbtBlue;
                    counter++;
                }
            }

            //right and left current row pixels
            if (w + 1 < width)
            {
                RGBTRIPLE crtRight = img_copy[h][w + 1];
                avgRed += crtRight.rgbtRed;
                avgGreen += crtRight.rgbtGreen;
                avgBlue += crtRight.rgbtBlue;
                counter++;
            }
            else if (w - 1 >= 0)
            {
                RGBTRIPLE crtLeft = img_copy[h][w - 1];
                avgRed += crtLeft.rgbtRed;
                avgGreen += crtLeft.rgbtGreen;
                avgBlue += crtLeft.rgbtBlue;
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

void make_copy(int height, int width, RGBTRIPLE image[height][width], RGBTRIPLE img_copy[height][width])
{
    for (int h = 0; h < height; h++)
    {
        for (int w = 0; w < width; w++)
        {
            img_copy[h][w] = image[h][w];
        }
    }
}