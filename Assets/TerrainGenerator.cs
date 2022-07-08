using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    public int xWidth = 30;
    public int zDepth = 30;
    public int octaves;
    public float persistance, lacunarity;
    public float height;
    public float detail;
    Vector3[] vertices;
    Mesh mesh;
    int[] triangles;
    public Transform player;
    public bool terrainExists = false;
    public float randomSeed;
    // Start is called before the first frame update
    void Start()
    {
        randomSeed = Random.Range((float)short.MinValue, (float)short.MaxValue);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine(CreateGeometry());
        terrainExists = false;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }

    // Update is called once per frame

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            //Gizmos.DrawSphere(vertices[i], .1f);
        }
    }

    double sinceLastGeneration = 0;
    private void FixedUpdate()
    {
        sinceLastGeneration += 0.02;
        if(sinceLastGeneration > 5) {
            StartCoroutine(CreateGeometry());
            sinceLastGeneration = 0;
        }
    }

    IEnumerator CreateGeometry() {
        vertices = new Vector3[(xWidth + 1) * (zDepth + 1)];
        int i = 0;
        for(float z = -(zDepth / 2); z <= zDepth/2; z++) {
            for(float x = -(xWidth / 2); x <= xWidth / 2; x++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int octave = 0; octave < octaves; octave++) {
                    float perlinY = Mathf.PerlinNoise((player.position.x + x + randomSeed) * detail * frequency, (player.position.z + z + randomSeed) * detail * frequency) * height;
                    noiseHeight += perlinY * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                vertices[i] = new Vector3(player.position.x + x, noiseHeight, player.position.z + z);
                i++;
            }
        }
        
        triangles = new int[xWidth * zDepth * 6];

        int vert = 0;
        int tris = 0;
        int vertsSinceWait = 0;

        for(int z = 0; z < zDepth; z++) {
            for(int x = 0; x < xWidth; x++) {
                triangles[tris] = vert + 0;
                triangles[tris+1] = vert + xWidth + 1;
                triangles[tris+2] = vert + 1;
                triangles[tris+3] = vert + 1;
                triangles[tris+4] = vert + xWidth + 1;
                triangles[tris+5] = vert + xWidth + 2;
                vert++;
                vertsSinceWait++;

                tris += 6;
                if(vertsSinceWait > 1500) {
                    vertsSinceWait = 0;
                    yield return null;
                }
            }
            vert++;
            vertsSinceWait++;
        }
        UpdateMesh();
        terrainExists = true;
    }

    public void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
