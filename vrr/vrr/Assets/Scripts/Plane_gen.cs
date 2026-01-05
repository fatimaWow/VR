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

    //plane settings
    [SerializeField] Vector2 planeSize = new Vector2(1,1);
    [SerializeField] int planeResolution = 1;

    //mesh values
    List<Vector3> vertices;
    List<int> triangles;

    //sdafag
    void Awake()
    {
        myMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = myMesh;
    }

     void Update()
    {
        //min avoids errors, max keeps preformance sane
        planeResolution = Mathf.Clamp(planeResolution,1,50);

        GeneratePlane(planeSize,planeResolution);
        Vector3 hillCenter = new Vector3(planeSize.x / 2f, 0, planeSize.y / 2f);
        float hillRadius = Mathf.Min(planeSize.x, planeSize.y) / 5f;
        float hillHeight = 3f;    
        Hill(hillCenter, hillRadius, hillHeight);
        AssignMesh();

        if(x==0){
            collide();
            x++;
        }

      
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
    for(int i = 0; i < vertices.Count; i++)
    {
        Vector3 vertex = vertices[i];

        // Distance from hill center in XZ plane
        float dx = vertex.x - center.x;
        float dz = vertex.z - center.z;
        float dist = Mathf.Sqrt(dx*dx + dz*dz);

        if(dist > radius) continue;

        // Smooth dome falloff
        float falloff = 1f - (dist / radius) * (dist / radius);

        vertex.y = height *falloff; // replace vertex.y
        vertices[i] = vertex;
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





   

    

  

   


   
