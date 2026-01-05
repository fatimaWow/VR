using UnityEngine;

public class atom2 : MonoBehaviour
{

    public Vector3 pos; 
    //public stopOnCollision col;
    public Plane_gen plane;
    public Transform trans;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // col = FindObjectOfType<stopOnCollision>();
        plane = FindObjectOfType<Plane_gen>();
        
    }

   
    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody>().position = pos;

        //if(col.is_collide == true){
        //float ex = gameObject.transform.position.x;
       // float ze = gameObject.transform.position.z;

    
        // plane.setCenter(new Vector3( ex, 0, ze));
        //}
        
    }
}
