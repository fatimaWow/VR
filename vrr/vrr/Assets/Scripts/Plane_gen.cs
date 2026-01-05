using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Plane_gen : MonoBehaviour
{
    //references
    Mesh myMesh;
    MeshFilter meshFilter;
    int x = 0;
    public stopOnCollision col2;
   // public atom2 sph;
     Vector3 hillCenter;
     stopOnCollision[] spheres;
     Vector3[] baseVertices; // store initial flat plane
   
    
     

    //plane settings
    [SerializeField]  Vector2 planeSize = new Vector2(1,1);
    [SerializeField] int planeResolution = 1;

    //mesh values
    List<Vector3> vertices;
    List<int> triangles;

    
    
void Start()
{
    GeneratePlane(planeSize, planeResolution);
    baseVertices = vertices.ToArray(); // copy flat plane
    AssignMesh();
    collide();
}

    //sdafag
    void Awake()
    {
       
       
        //sph = FindObjectOfType<atom2>();
        myMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = myMesh;

        spheres = FindObjectsOfType<stopOnCollision>();

        
        
    }

    //public void setCenter(Vector3 vector){
     //   hillCenter = vector;
     //}
   

     void Update()
    {
        //min avoids errors, max keeps preformance sane
        planeResolution = Mathf.Clamp(planeResolution,1,50);
        // GeneratePlane(planeSize,planeResolution);

        //GeneratePlane(planeSize,planeResolution);
        //Vector3 hillCenter = new Vector3(planeSize.x / 2f, 0, planeSize.y / 2f);
        // Reset vertices to base
    for (int i = 0; i < vertices.Count; i++)
        vertices[i] = baseVertices[i];
        
        float hillRadius = planeSize.x / 5f;
           

       
       foreach (var col in spheres)
        {
            if (col.is_collide)
            {
                // convert world position to local plane space
                float  x = col.gameObject.transform.position.x;
                float z = col.gameObject.transform.position.z;
                Vector3 hillCenter = new Vector3(x,0,z);
                float hillHeight = 3f * col.type ; 

                Hill(hillCenter, hillRadius, hillHeight);
            }
        }

       
        
        AssignMesh();

        

      
    }

  


    void GeneratePlane (Vector2 size, int resolution)
    {
        //Create vertices
        vertices = new List<Vector3>();
        float xPerStep = size.x/resolution;
        float yPerStep = size.y/resolution;
        for(int y = 0; y<resolution+1; y++)
        {
            for(int x = 0; x<resolution+1; x++)
            {
                vertices.Add(new Vector3(x*xPerStep,0,y*yPerStep));
            }
        }

        //Create triangles
        triangles = new List<int>();
        for(int row = 0; row<resolution; row++)
        {
            for(int column = 0; column<resolution; column++)
            {
                int i = (row*resolution) + row + column;
                //first triangle 
                triangles.Add(i);
                triangles.Add(i+(resolution)+1); 
                triangles.Add(i+(resolution)+2);

                //second triangle 
                triangles.Add(i); 
                triangles.Add(i+resolution+2); 
                triangles.Add(i+1);
            }
        }
    }

    void AssignMesh()
    {
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();
    }

void Hill(Vector3 center, float radius, float height)
{
    for (int i = 0; i < vertices.Count; i++)
    {
        Vector3 v = vertices[i];

        float dx = v.x - center.x;
        float dz = v.z - center.z;
        float dist = Mathf.Sqrt(dx * dx + dz * dz);

        if (dist > radius) continue;

        float falloff = 1f - (dist / radius) * (dist / radius);

        // ADD to vertex.y instead of replacing
        // Use Mathf.Max for merging hills smoothly
        v.y += height * falloff;
        vertices[i] = v;
    }
}


 void collide(){
        myMesh.RecalculateNormals();
        myMesh.RecalculateBounds(); // Important for the collider to work correctly

        // 3. Assign the mesh to the MeshFilter
        meshFilter.mesh = myMesh;

        // 4. Add the MeshCollider component and assign the *same* mesh to it
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = myMesh;
    }
}





   

    

  

   


   
