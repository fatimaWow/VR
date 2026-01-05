using UnityEngine;

public class stopOnCollision : MonoBehaviour
{
     public Rigidbody rb;
     public GameObject plane;
     public bool is_collide;
     public bool oncollision;

    void Start()
    {
        // Get the Rigidbody component attached to this object
        
        rb = GetComponent<Rigidbody>();
        is_collide = false;
        oncollision = false;

        if (rb == null)
        {
            Debug.LogError("StopOnCollision script requires a Rigidbody component!");
            enabled = false; // Disable the script if no Rigidbody is found
        }
    }

    void Update(){
        if( oncollision = false){
            is_collide = false;
        }
       
    }

   

    // This function is called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnCollisionEnter(Collision collision)
    {
        // Check if the Rigidbody component is present before attempting to modify its velocity
        if (rb != null)
        {
            // Set linear velocity to zero (stop movement)
            rb.linearVelocity = Vector3.zero;

            // Set angular velocity to zero (stop rotation)
            rb.angularVelocity = Vector3.zero;

            // Optional: Make the Rigidbody kinematic after stopping to prevent further physics interactions
            // rb.isKinematic = true;
            is_collide = true;
            oncollision = true;
        }
        
    }
}
