using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoise(int width, int height, float scale) {
        if(scale <= 0) scale = 0.001f;

        float[,] noise = new float[width, height];
        
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                float scaledX = x / scale;
                float scaledY = y / scale;

                float perlin = Mathf.PerlinNoise(scaledX, scaledY);
                noise[x, y] = perlin;
            }
        }
        return noise;
    }
}
