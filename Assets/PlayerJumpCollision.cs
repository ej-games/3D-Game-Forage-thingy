using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor")) GetComponentInParent<CharacterMovement>().canJump = true;
    }

    private void OnTiggerExit(Collider other)
    {
        if(other.CompareTag("Floor")) GetComponentInParent<CharacterMovement>().canJump = false;
    }
}
