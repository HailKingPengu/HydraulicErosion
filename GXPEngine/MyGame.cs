using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions

public class Erosion : Game
{

    AnimationSprite[,] cubes;

    Noise noise = new Noise();
    float[,] generatedMap;

    Eroder ero = new Eroder();
    
    int mapSize = 257;

    float distX = 3;
    float distY = 1.5f;

    public Erosion() : base(1600, 1000, false)
    {     

        cubes = new AnimationSprite[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                cubes[x, y] = new AnimationSprite("cube.png", 2, 1, -1, true, false);
                cubes[x, y].scale = 0.5f;
                cubes[x, y].SetXY(width / 2 + distX * x - distX * y, (height / 2 - (mapSize) * distY) + distY * y + distY * x);
                AddChild(cubes[x, y]);
            }
        }

        int[,] intMap = noise.GenerateNoise(mapSize, 2, 200);
        generatedMap = new float[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for(int y = 0; y < mapSize; y++)
            {
                generatedMap[x, y] = intMap[x, y];
            }
        }

        ApplyMap();

        Console.WriteLine("MyGame initialized");
    }

    void ApplyMap()
    {

        float highestValue = GetHighestValue();

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                cubes[x, y].SetXY(width / 2 + distX * x - distX * y, (height / 2 - (mapSize) * distY) + distY * y + distY * x + generatedMap[x, y]);
                cubes[x, y].SetColor(0, 0, generatedMap[x, y]/ highestValue);
            }
        }
    }

    float GetHighestValue()
    {

        float highestValue = 0;

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (generatedMap[x,y] > highestValue)
                {
                    highestValue = generatedMap[x, y];
                }
            }
        }

        return highestValue;
    }

    void Update()
    {
        if (Input.GetKeyDown(Key.E) && Input.GetKey(Key.LEFT_SHIFT))
        {
            generatedMap = ero.ErodeMap(generatedMap, mapSize, 100000);
            ApplyMap();
        }
        else if (Input.GetKeyDown(Key.E))
        {
            generatedMap = ero.ErodeMap(generatedMap, mapSize, 10000);
            ApplyMap();
        }

        if (Input.GetKeyDown(Key.P))
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    generatedMap[x, y] *= 1.2f;
                }
            }
            ApplyMap();
        }

        if (Input.GetKeyDown(Key.O))
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    generatedMap[x, y] *= 0.8f;
                }
            }
            ApplyMap();
        }

        if (Input.GetKey(Key.UP))
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    generatedMap[x, y] -= 5;
                }
            }
            ApplyMap();
        }

        if (Input.GetKey(Key.DOWN))
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    generatedMap[x, y] += 5;
                }
            }
            ApplyMap();
        }

        if (Input.GetKeyDown(Key.S))
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    cubes[x, y].NextFrame();
                }
            }
            ApplyMap();
        }
    }

    static void Main()
    {
        new Erosion().Start();
    }
}
