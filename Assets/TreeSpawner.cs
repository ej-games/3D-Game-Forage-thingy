using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreeType {
    Standard
}

public class TreeSpawner : SingletonComponent<TreeSpawner>
{
    public static bool finished = false;
    public static List<Vector3> treePositions;
    public bool resetTreesOnStart;
    // Start is called before the first frame update
    void Start()
    {
        if(resetTreesOnStart) WorldSaveData.instance.treePositions = null;
        //StartCoroutine(SpawnTrees());
    }

    public static IEnumerator DataLoad() {
        yield return null;
        if(treePositions.Count > 0) {
            foreach(var pos in treePositions) {
                GameObject newTree = GameObject.Instantiate(Resources.Load<GameObject>("NormalTree"), pos, Quaternion.identity, my.transform);
            }
            finished = true;
        }
        else {
            my.StartCoroutine(SpawnTrees());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static IEnumerator SpawnTrees() {
        DataManager.loadStatus = "Planting trees";
        treePositions = new List<Vector3>();
        for(int x = -375; x < 375; x+= 10) {
            for(int z = -375; z < 375; z+= 10) {
                //print($"Trying position {x.ToString()}, 0, {z.ToString()}");
                string pos = x.ToString() + ", " + z.ToString(); // Debug
                //print(pos);
                if(Random.Range(0, 100) < 7) {
                    bool treeTooClose = false;
                    foreach(var item in GameObject.FindGameObjectsWithTag("Tree")) {
                        if(Vector3.Distance(new Vector3(item.transform.position.x, 0f, item.transform.position.z), new Vector3(x, 0f, z)) < 15) {
                            //print($"At {pos}, there is another tree too close.");
                            treeTooClose = true;
                            break;
                        }
                    }
                    if(treeTooClose) continue;
                    bool treeSpawned = false;
                    GameObject newTree = GameObject.Instantiate(Resources.Load<GameObject>("NormalTree"), new Vector3(x, 60, z), Quaternion.identity, my.transform);
                    for(int trial = 0; trial < 250; trial++) {
                        newTree.transform.Translate(new Vector3(0, -0.25f));
                        RaycastHit[] hits = Physics.RaycastAll(Util.GetFirstChild("BasePosition", newTree.transform).transform.position, Vector3.down, 0.1f);
                        string toPrint = $"Trial {trial}. The {hits.Length} hits are: ";
                        foreach(var hit in hits) {
                            toPrint += hit.collider.name + ", ";
                        }
                        //print(toPrint);
                        foreach(var hit in hits) {
                            if(hit.collider == null) continue;
                            if(hit.collider == newTree.GetComponent<Collider>()) continue;
                            if(hit.collider.CompareTag("Floor")) {
                                treeSpawned = true;
                                //print($"Congratulations! Tree spawned at {pos}");
                                treePositions.Add(newTree.transform.position);
                                break;
                            }
                        }
                        /*if(trial % 250 == 0) {
                            yield return null;
                        }*/
                        if(treeSpawned) break;
                    }
                    if(!treeSpawned) {
                        //print($"All 50 trials failed for {pos}! Not spawning.");
                        newTree.tag = "Untagged";
                        Destroy(newTree);
                    }
                }
                else {
                    //print($"Not spawning a tree at {x.ToString()}, {z.ToString()} because it chose not to.");
                }
                if(z % 500 == 0) {
                    yield return null;
                }
            }
            //DataManager.SaveWorldData();
        }
        finished = true;
        DataManager.loadStatus = "Trees planted";
    }
}
