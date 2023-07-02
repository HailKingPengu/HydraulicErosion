using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Eroder
    {

        public int maxLifetime = 600;
        public float erosionAmount = 0.1f;
        public float depositionAmount = 0.1f;
        public float dzThreshold = 2;
        public float maxVelocity = 0.3f;

        float sedimentAmount;
        float velocity;

        public Eroder()
        {
            //Erode(map, mapSize, numIterations);
        }

        public float[,] ErodeMap(float[,] map, int mapSize, int numIterations)
        {

            maxLifetime = 800;
            erosionAmount = 0.2f;
            dzThreshold = 5;
            maxVelocity = 0.5f;

            maxLifetime = 600;
            erosionAmount = 0.4f;
            depositionAmount = 0.002f;
            dzThreshold = 2;
            maxVelocity = 0.2f;

            for (int iteration = 0; iteration < numIterations; iteration++)
            {

                int x = (int)Utils.Random(1, mapSize - 1);
                int y = (int)Utils.Random(1, mapSize - 1);

                SimulateDrop(map, mapSize, x, y);
            }

            float[,] returnedMap = new float[mapSize,mapSize];

            for (int sx = 0; sx < mapSize; sx++)
            {
                for (int sy = 0; sy < mapSize; sy++)
                {
                    returnedMap[sx, sy] = map[sx,sy];
                }
            }

            return returnedMap;

        }

        void SimulateDrop(float[,] map, int mapSize, int x, int y)
        {

            sedimentAmount = 0;
            velocity = 0;

            for (int lifetime = 0; lifetime < maxLifetime; lifetime++)
            {

                float dropHeight = map[x, y];


                float lowestValue = 10000;
                int lowestX = 0;
                int lowestY = 0;

                for (int dx = -1; dx < 2; dx++)
                {
                    for (int dy = -1; dy < 2; dy++)
                    {

                        if (x + dx > 0 && x + dx < mapSize && y + dy > 0 && y + dy < mapSize)
                        {
                            float checkValue = map[x + dx, y + dy];

                            if (checkValue < lowestValue)
                            {
                                lowestValue = checkValue;
                                lowestX = x + dx;
                                lowestY = y + dy;
                            }
                        }
                    }
                }

                float dz = dropHeight - lowestValue;

                if (dz > velocity)
                {
                    if (dz > dzThreshold)
                    {
                        float collectionAmount = dz * erosionAmount;
                        map[x, y] -= collectionAmount;
                        sedimentAmount += collectionAmount;

                        velocity += dz;
                        if (velocity > maxVelocity)
                        {
                            velocity = maxVelocity;
                        }
                    }
                    else
                    {
                        float depositAmount = sedimentAmount * depositionAmount;
                        map[x, y] += depositAmount;
                        sedimentAmount -= depositAmount;

                        velocity += dz;
                        if (velocity > maxVelocity)
                        {
                            velocity = maxVelocity;
                        }
                    }
                }
                else
                {
                    map[x, y] += sedimentAmount;
                    map[x, y] += sedimentAmount;
                    break;
                }

                x = lowestX;
                y = lowestY;
            }
        }
    }
}
