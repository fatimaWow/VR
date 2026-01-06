using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class stopOnCollision : MonoBehaviour
{
    public Rigidbody rb;
    public bool is_collide;
    public bool oncollision;
    public float radius = 1f;
    public float type = 1f;

   

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // or true if manually moved

       
       

        is_collide = false;
        oncollision = false;
    }

    void Update()
    {
        is_collide = oncollision;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        oncollision = true;
        is_collide = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        oncollision = false;
        is_collide = false;
    }
}
