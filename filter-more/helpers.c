#include "helpers.h"
#include <math.h>

void make_copy(int height, int width, RGBTRIPLE image[height][width], RGBTRIPLE img_copy[height][width]);
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

            //add_to_avg(h, w, img_copy, &avgRed, &avgGreen, &avgBlue, &counter);
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

                if (w + 1 < width) //check if we have top right pixel
                {
                    RGBTRIPLE topRight = img_copy[h - 1][w + 1];
                    avgRed += topRight.rgbtRed;
                    avgGreen += topRight.rgbtGreen;
                    avgBlue += topRight.rgbtBlue;
                    counter++;
                }
            }

            if (h + 1 < height) //check if we have bottom row
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

                if (w + 1 < width) //check if we have top right pixel
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

            if (w - 1 >= 0)
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

// Detect edges
void edges(int height, int width, RGBTRIPLE image[height][width])
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
            //new avg colors
            int avg_gx_red = 0;
            int avg_gx_green = 0;
            int avg_gx_blue = 0;

            int avg_gy_red = 0;
            int avg_gy_green = 0;
            int avg_gy_blue = 0;

            //top and bottom row pixels
            if (h - 1 >= 0) //check if we have top row
            {
                RGBTRIPLE topMiddle = img_copy[h - 1][w];
                avg_gy_red += topMiddle.rgbtRed * (-2);
                avg_gy_green += topMiddle.rgbtGreen * (-2);
                avg_gy_blue += topMiddle.rgbtBlue * (-2);

                if (w - 1 >= 0) //check if we have top left pixel
                {
                    RGBTRIPLE topLeft = img_copy[h - 1][w - 1];
                    avg_gx_red += topLeft.rgbtRed * (-1);
                    avg_gx_green += topLeft.rgbtGreen * (-1);
                    avg_gx_blue += topLeft.rgbtBlue * (-1);

                    avg_gy_red += topLeft.rgbtRed * (-1);
                    avg_gy_green += topLeft.rgbtGreen * (-1);
                    avg_gy_blue += topLeft.rgbtBlue * (-1);
                }

                if (w + 1 < width) //check if we have top right pixel
                {
                    RGBTRIPLE topRight = img_copy[h - 1][w + 1];
                    avg_gx_red += topRight.rgbtRed;
                    avg_gx_green += topRight.rgbtGreen;
                    avg_gx_blue += topRight.rgbtBlue;

                    avg_gy_red += topRight.rgbtRed * (-1);
                    avg_gy_green += topRight.rgbtGreen * (-1);
                    avg_gy_blue += topRight.rgbtBlue * (-1);
                }
            }

            if (h + 1 < height) //check if we have bottom row
            {
                RGBTRIPLE bottomMiddle = img_copy[h + 1][w];
                avg_gy_red += bottomMiddle.rgbtRed * 2;
                avg_gy_green += bottomMiddle.rgbtGreen * 2;
                avg_gy_blue += bottomMiddle.rgbtBlue * 2;

                if (w - 1 >= 0) //check if we have bottom left pixel
                {
                    RGBTRIPLE bottomLeft = img_copy[h + 1][w - 1];
                    avg_gx_red += bottomLeft.rgbtRed * (-1);
                    avg_gx_green += bottomLeft.rgbtGreen * (-1);
                    avg_gx_blue += bottomLeft.rgbtBlue * (-1);

                    avg_gy_red += bottomLeft.rgbtRed;
                    avg_gy_green += bottomLeft.rgbtGreen;
                    avg_gy_blue += bottomLeft.rgbtBlue;
                }

                if (w + 1 < width) //check if we have top right pixel
                {
                    RGBTRIPLE bottomRight = img_copy[h + 1][w + 1];
                    avg_gx_red += bottomRight.rgbtRed;
                    avg_gx_green += bottomRight.rgbtGreen;
                    avg_gx_blue += bottomRight.rgbtBlue;

                    avg_gy_red += bottomRight.rgbtRed;
                    avg_gy_green += bottomRight.rgbtGreen;
                    avg_gy_blue += bottomRight.rgbtBlue;
                }
            }

            //right and left current row pixels
            if (w + 1 < width)
            {
                RGBTRIPLE crtRight = img_copy[h][w + 1];
                avg_gx_red += crtRight.rgbtRed * 2;
                avg_gx_green += crtRight.rgbtGreen * 2;
                avg_gx_blue += crtRight.rgbtBlue * 2;
            }

            if (w - 1 >= 0)
            {
                RGBTRIPLE crtLeft = img_copy[h][w - 1];
                avg_gx_red += crtLeft.rgbtRed * (-2);
                avg_gx_green += crtLeft.rgbtGreen * (-2);
                avg_gx_blue += crtLeft.rgbtBlue * (-2);
            }

            int new_red = sqrt(pow(avg_gx_red, 2) + pow(avg_gy_red, 2));
            int new_green = sqrt(pow(avg_gx_green, 2) + pow(avg_gy_green, 2));
            int new_blue = sqrt(pow(avg_gx_blue, 2) + pow(avg_gy_blue, 2));

            image[h][w].rgbtRed = new_red;
            image[h][w].rgbtGreen = new_green;
            image[h][w].rgbtBlue = new_blue;
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

int get_less(int value)
{
    if (value > 255)
    {
        return 255;
    }

    return value;
}

//void add_to_avg(int height, int width, RGBTRIPLE image[height][width], int *avgRed, int *avgGreen, int *avgBlue, double *counter)
//{
//    RGBTRIPLE pixel = image[height][width];
//    *avgRed = *avgRed + pixel.rgbtRed;
//    *avgGreen = *avgGreen + pixel.rgbtGreen;
//    *avgBlue = *avgBlue + pixel.rgbtBlue;
//    *counter = *counter + 1;
//}