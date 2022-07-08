using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CharacterController controller;
    public float moveSpeed, jumpHeight, lookSpeed = 1f;
    //public float maxMoveSpeed = 5f;
    public bool canJump = true;
    float rotationX = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float limit = 0.8f;
        if(Mathf.Abs(horizontal) > 0.5f && Mathf.Abs(vertical) > 0.5f) {
            if(horizontal > 0) horizontal = limit;
            else horizontal = -limit;
            if(vertical > 0) vertical = limit;
            else vertical = -limit;
        }
        //rb.AddRelativeForce(new Vector3(horizontal, 0f, vertical) * Time.deltaTime * 20f * moveSpeed, ForceMode.Acceleration);
        //rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxMoveSpeed, maxMoveSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxMoveSpeed, maxMoveSpeed));
        //print(rb.velocity.z);
        if(Input.GetButtonDown("Jump") && canJump) {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            canJump = false;
        }
        //controller.Move(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime * 20f * moveSpeed);
        if(!Input.GetKey(KeyCode.G)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X")));
    }

    private void FixedUpdate()
    {
        //if(!FindObjectOfType<TerrainGenerator>().terrainExists) transform.position = (Vector3.left * 50f) + (Vector3.up * 10f);
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * moveSpeed * 0.09f);
    }
    
}
