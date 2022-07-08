using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : SingletonComponent<DataManager>
{
    public static string loadStatus;
    public static bool playerLoaded;
    // Start is called before the first frame update
    void Start()
    {
        Game.fifteenTick += SaveWorldData;
        StartCoroutine(LoadData());
        playerLoaded = false;
    }

    public static IEnumerator LoadData() {
        yield return null;


        // Terrain and world

        // Terrain
        loadStatus = "Loading terrain";
        /*if(WorldSaveData.instance.terrainHeights != null) {
            my.StartCoroutine(Util.MakeLarge2DArray<float>(
            WorldSaveData.instance.terrainHeights, NormalTerrain.my.depth, NormalTerrain.my.width, (arr) => {
                NormalTerrain.terrainHeights = arr;
                NormalTerrain.my.StartCoroutine(NormalTerrain.DataLoad());
            }));
        }
        else {
            print("No terrain to load.");
            WorldSaveData.instance.terrainHeights = new float[NormalTerrain.my.width * NormalTerrain.my.depth];
            NormalTerrain.terrainHeights = new float[NormalTerrain.my.width, NormalTerrain.my.depth];
            hasNewTerrain = true;
        }*/
        NormalTerrain.hasTerrain = WorldSaveData.instance.hasTerrain;
        NormalTerrain.my.StartCoroutine(NormalTerrain.DataLoad());
        yield return new WaitUntil(() => NormalTerrain.finished);
        Debug.Log("Terrain has loaded.");

        // Trees
        loadStatus = "Loading trees";
        if(WorldSaveData.instance.treePositions != null && WorldSaveData.instance.hasTerrain) {
            TreeSpawner.treePositions = WorldSaveData.instance.treePositions.ToList();
        }
        else {
            print("There are no trees to spawn - one or more are null, or there is new terrain.");
            TreeSpawner.treePositions = new List<Vector3>();
            WorldSaveData.instance.treePositions = new Vector3[] {};
        }
        TreeSpawner.my.StartCoroutine(TreeSpawner.DataLoad());
        print($"There are {TreeSpawner.treePositions.Count} trees that should be spawning.");
        yield return new WaitUntil(() => TreeSpawner.finished);
        Debug.Log("Trees have been planted.");


        // Player

        // Position
        loadStatus = "Loading player";
        if(WorldSaveData.instance.playerPosition != null && WorldSaveData.instance.hasTerrain) {
            Util.Player.position = new Vector3(WorldSaveData.instance.playerPosition.x, 50f, WorldSaveData.instance.playerPosition.z);
            print($"Player's position was {WorldSaveData.instance.playerPosition}");
        }
        else {
            print("Player position has not been loaded - reverting to (0,0)");
            Util.Player.position = new Vector3(0, 50f, 0f);
            WorldSaveData.instance.playerPosition = new Vector3();
        }
        bool playerX = Mathf.Approximately(Util.Player.position.x, WorldSaveData.instance.playerPosition.x);
        bool playerZ = Mathf.Approximately(Util.Player.position.z, WorldSaveData.instance.playerPosition.z);
        yield return new WaitUntil(() => playerX && playerZ && Util.Player.GetComponent<CharacterMovement>().canJump);


        // Finished
        loadStatus = "";
        Destroy(GameObject.Find("LoadingScreen"));
    }

    private void OnApplicationQuit()
    {
        SaveWorldData();
    }

    public static void SaveAllData() {

    }

    public static void SaveWorldData() {
        if(TreeSpawner.treePositions == null) {
            TreeSpawner.treePositions = new List<Vector3>();
        }
        /*
        if(NormalTerrain.terrainHeights == null) {
            NormalTerrain.terrainHeights = new float[NormalTerrain.my.width, NormalTerrain.my.depth];
        }*/
        WorldSaveData.instance.treePositions = TreeSpawner.treePositions.ToArray();
        //WorldSaveData.instance.terrainHeights = Util.Make1DFrom2D<float>(NormalTerrain.terrainHeights, NormalTerrain.my.depth, NormalTerrain.my.width);
        /*my.StartCoroutine(Util.MakeLarge1DFrom2D<float>(NormalTerrain.terrainHeights, NormalTerrain.my.depth, NormalTerrain.my.width, (arr) => {
            WorldSaveData.instance.terrainHeights = arr;
            WorldSaveData.Save();
        }));*/
        WorldSaveData.instance.hasTerrain = NormalTerrain.hasTerrain;
        WorldSaveData.instance.playerPosition = Util.Player.position;
        WorldSaveData.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("LoadingTerrainText")) {
            GameObject.Find("LoadingTerrainText").GetComponent<Text>().text = loadStatus;
        }
    }

    private void FixedUpdate()
    {

    }
}

public class WorldSaveData : ThirtySec.Serializable<WorldSaveData> {
    public Vector3[] treePositions;
    //public float[] terrainHeights;
    public bool hasTerrain;
    public Vector3 playerPosition;
}