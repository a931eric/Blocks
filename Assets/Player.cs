using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    Transform cam;
    Rigidbody rb;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = transform.GetChild(0);
        rb = GetComponent<Rigidbody>();
    }
    float Cos(Vector3 a,Vector3 b)
    {
        if (a.magnitude * b.magnitude == 0)
        {
            return 0;
        }
        return Vector3.Dot(a,b)/(a.magnitude * b.magnitude);
    }
    public float rotSpeed = 1;
    public float baseSpeed = .05f, moveAcc = 1;
    public float jumpSpeed = 1, jumpAcc = 1;
    private void FixedUpdate()
    {
        float moveSpeed = baseSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) moveSpeed *= 1.5f;
        Vector3 targetVel = (Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward)* moveSpeed;
        if (targetVel.magnitude > moveSpeed) targetVel *= moveSpeed / targetVel.magnitude;
        if (targetVel.magnitude>0)
        {
            var force = Vector3.Normalize((targetVel - rb.velocity))*Mathf.Max(0,moveSpeed-Vector3.Dot(targetVel , rb.velocity)/targetVel.magnitude) * moveAcc * rb.mass;
            force.y = 0;
            rb.AddForce(force);
        }
            
        //transform.Translate(new Vector3(Input.GetAxis("Horizontal") ,0,Input.GetAxis("Vertical")) * speed);
        if (ground != null && Input.GetButton("Jump"))
        {
            var jumpForce = new Vector3(0, jumpAcc * Mathf.Max(0,(jumpSpeed - rb.velocity.y)) * rb.mass,0);
            rb.AddForce(jumpForce);
            if (ground.attachedRigidbody != null)
                ground.attachedRigidbody.AddForceAtPosition(-jumpForce, transform.position + new Vector3(0, -1, 0));
        }
    }
    
    Collider ground =null;
    private void OnTriggerStay(Collider other)
    {
        ground = other;
    }
    private void OnTriggerExit(Collider other)
    {
        ground = null;
    }
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X")* rotSpeed, 0);
        cam.Rotate(-Input.GetAxis("Mouse Y") * rotSpeed,0, 0);
        
    }
}
