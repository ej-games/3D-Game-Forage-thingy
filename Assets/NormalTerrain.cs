using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NormalTerrain : SingletonComponent<NormalTerrain>
{
    public int width, depth, octaves;
    public float persistance, lacunarity, detail, height;
    public static bool finished = false;
    public bool resetTerrainOnStart;
    public static float[,] terrainHeights;
    public static bool hasTerrain;
    // Start is called before the first frame update
    void Start()
    {
        //WorldSaveData.instance.hasTerrain = false;
        //WorldSaveData.instance.treePositions = null;
        /*if(resetTerrainOnStart) {
            StartCoroutine(GetNoise(width, depth, octaves, persistance, lacunarity, detail, height, (noiseMap) => {
                GetComponent<Terrain>().terrainData.SetHeights(0, 0, noiseMap);
                print(GetComponent<Terrain>().terrainData.GetHeights(0, 0, width + 1, width + 1).ToString());
                terrainHeights = noiseMap;
            }));
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if(finished) {
            StartCoroutine(GetNoise(width, depth, octaves, persistance, lacunarity, detail, height, (noiseMap) => {
                GetComponent<Terrain>().terrainData.SetHeights(0, 0, noiseMap);
                print(GetComponent<Terrain>().terrainData.GetHeights(0, 0, width + 1, width + 1).ToString());
            }));
        }*/
        //print($"Terrain height map is {terrainHeights}, but the data is always right and says {WorldSaveData.instance.terrainHeights}");
    }

    public static IEnumerator DataLoad() {
        yield return null;
        /*int i = 0;
        if(!hasTerrain) {
            foreach(var item in terrainHeights) {
                print(item);
                i++;
                if(i % 150 == 0) yield return null;
            }
            my.GetComponent<Terrain>().terrainData.SetHeights(0, 0, terrainHeights);
            finished = true;
        }
        else {
            GenerateTerrain();
        }*/
        if(!hasTerrain) {
            //print("There is no terrain!");
            GenerateTerrain();
        }
        else {
            finished = true;
        }
    }

    public static void GenerateTerrain() {
        my.StartCoroutine(my.GetNoise(my.width, my.depth, my.octaves, my.persistance, my.lacunarity, my.detail, my.height, (noiseMap) => {
            my.GetComponent<Terrain>().terrainData.SetHeights(0, 0, noiseMap);
            print(my.GetComponent<Terrain>().terrainData.GetHeights(0, 0, my.width + 1, my.width + 1).ToString());
            terrainHeights = noiseMap;
        }));
    }

    public IEnumerator GetNoise(int width, int depth, int octaves, float persistance, float lacunarity, float detail, float height, System.Action<float[,]> onFinish) {
        finished = false;
        DataManager.loadStatus = "Generating terrain";
        TerrainData terrainData = GetComponent<Terrain>().terrainData;
        float randomSeed = Random.Range((float)short.MinValue, (float)short.MaxValue);
        float[,] noiseMap = new float[width, depth];
        for(int z = 0; z < depth; z++) {
            for(int x = 0; x < width; x++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int octave = 0; octave < octaves; octave++) {
                    float perlinY = Mathf.PerlinNoise(((float)x + randomSeed) * detail * frequency, ((float)z + randomSeed) * detail * frequency) * height;
                    noiseHeight += perlinY * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                noiseMap[x, z] = noiseHeight;
            }
            if(z % depth / 50 == 0) yield return null;
        }
        terrainData.size = new Vector3(width, 100, depth);
        terrainData.heightmapResolution = width + 1;
        onFinish(noiseMap);
        finished = true;
        hasTerrain = true;
        DataManager.loadStatus = "Terrain generated!";
    }
}
