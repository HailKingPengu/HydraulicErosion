using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Noise
    {
        public Noise() { }

        //the noise generator can generate two dimensional noise based on the diamond-square algorithm

        float[,] thisPixels;
        float randomization;

        int[,] clampedNoise;

        public int[,] GenerateNoise(int screenSize, float randomization, int range)
        {

            int numDivisions = 0;
            this.randomization = randomization;

            float maxVal = 0;
            float minVal = 255;

            //this type of noise is based on exponentially smaller increments of a two dimensional array.
            //therefor it can only use map sizes that are a power or two.
            //(plus one, so there is a value in the middle of the array)

            //this checks how many divisions/steps it can make before reaching the last step
            for (int i = screenSize; i > 2; i = i / 2)
            {
                numDivisions++;
            }

            thisPixels = new float[screenSize, screenSize];
            clampedNoise = new int[screenSize, screenSize];

            //all values are initialized to 0
            for (int x = 0; x < screenSize; x++)
            {
                for (int y = 0; y < screenSize; y++)
                {
                    thisPixels[x, y] = 0;
                }
            }

            //the four corners are initialized with randomized values
            thisPixels[0, 0] = (Utils.Random(127, 129));
            thisPixels[0, screenSize - 1] = (Utils.Random(127, 129));
            thisPixels[screenSize - 1, 0] = (Utils.Random(127, 129));
            thisPixels[screenSize - 1, screenSize - 1] = (Utils.Random(127, 129));

            //thisPixels[0, 0] = 128;
            //thisPixels[0,screenSize - 1] = 128;
            //thisPixels[screenSize - 1,0] = 128;
            //thisPixels[screenSize - 1,screenSize - 1] = 128;

            //12345678

            //it incrementally fills in all values, adding a bit of randomization in each step
            for (int i = 0; i < numDivisions + 1; i++)
            {
                for (int x = 0; x < Convert.ToInt32(Math.Pow(2, i)); x++)
                {
                    for (int y = 0; y < Convert.ToInt32(Math.Pow(2, i)); y++)
                    {

                        //in each step a certain section of the screen is taken, first one in a 1x1 grid, then one in a 2x2 grid, then 4x4 and so on
                        //until every value is filled in

                        //in the grid section the four corners are already filled in by the previous step.
                        //it then, based on the corners, first goes through the square step
                        //where it calculates the averages of the corners, and adds a random value on top (random value power decreased per step) and applies it to the center value
                        //next it goes through the diamond step, where it calculates the midpoints of the edges
                        //by taking the values of the two corners and the middle point, averages that and adds a random value.

                        int fraction = Convert.ToInt32(Math.Pow(2, i));
                        int gridSize = (screenSize - 1) / Convert.ToInt32(Math.Pow(2, i));

                        float midSquare;

                        //[SQUARE STEP]//
                        midSquare = thisPixels[(x * screenSize / fraction) + (screenSize / 2 / fraction), (y * screenSize / fraction) + (screenSize / 2 / fraction)] =
                        (thisPixels[x * gridSize, y * gridSize] + thisPixels[(x + 1) * gridSize, y * gridSize] + thisPixels[x * gridSize, (y + 1) * gridSize] + thisPixels[(x + 1) * gridSize, (y + 1) * gridSize]) / 4 + getRandom(i);

                        //[DIAMOND STEP]//
                        //x = 0   y = mid
                        thisPixels[x * gridSize, y * gridSize + gridSize / 2] = (thisPixels[x * gridSize, y * gridSize] + thisPixels[x * gridSize, (y + 1) * gridSize] + midSquare) / 3 + getRandom(i);

                        //x = mid   y = 0
                        thisPixels[x * gridSize + gridSize / 2, y * gridSize] = (thisPixels[x * gridSize, y * gridSize] + thisPixels[(x + 1) * gridSize, y * gridSize] + midSquare) / 3 + getRandom(i);

                        //x = max   y = mid
                        thisPixels[(x + 1) * gridSize, y * gridSize + gridSize / 2] = (thisPixels[(x + 1) * gridSize, y * gridSize] + thisPixels[(x + 1) * gridSize, (y + 1) * gridSize] + midSquare) / 3 + getRandom(i);

                        //x = mid   y = max
                        thisPixels[x * gridSize + gridSize / 2, (y + 1) * gridSize] = (thisPixels[x * gridSize, (y + 1) * gridSize] + thisPixels[(x + 1) * gridSize, (y + 1) * gridSize] + midSquare) / 3 + getRandom(i);

                    }
                }
            }

            //to average the values and to make sure none of the values exceed the maximum and minimum, all values are checked and the maximum and minimum values in the array are extracted
            for (int x = 0; x < screenSize; x++)
            {
                for (int y = 0; y < screenSize; y++)
                {
                    if (thisPixels[x, y] < minVal)
                    {
                        minVal = thisPixels[x, y];
                    }
                    if (thisPixels[x, y] > maxVal)
                    {
                        maxVal = thisPixels[x, y];
                    }
                }
            }

            //it is then normalized to the maximum and minimum value and the range
            for (int x = 0; x < screenSize; x++)
            {
                for (int y = 0; y < screenSize; y++)
                {
                    clampedNoise[x, y] = Convert.ToInt32(((thisPixels[x, y] - minVal)) / ((maxVal - minVal) / range));
                }
            }

            Console.WriteLine("noise generation complete");

            return clampedNoise;

        }

        float getRandom(int i)
        {
            return (Utils.Random((int)-(randomization * (1 - Mathf.Sqrt(i) * 0.20)), (int)(randomization * (1 - Mathf.Sqrt(i) * 0.20))));
        }
    }
}
