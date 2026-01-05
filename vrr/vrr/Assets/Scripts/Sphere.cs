using UnityEngine;

public class Sphere : MonoBehaviour
{

    public Vector3 pos; 
    public stopOnCollision col;
    public Plane_gen plane;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
   
    }

   
    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().position = pos;

        // if(getcol() == true){
        //     plane.hillCenter = new Vector3(pos.x / 2f, 0, pos.y / 2f);
        // }
        
    }
}
