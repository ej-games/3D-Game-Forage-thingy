using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peace;

public class TerrainLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    double timeSinceLoad = 0;
    private void FixedUpdate()
    {
        timeSinceLoad += 0.02;
    }
}
