using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAtStart : MonoBehaviour
{
    public Behaviour toEnable;

    // Start is called before the first frame update
    void Start()
    {
        toEnable.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
