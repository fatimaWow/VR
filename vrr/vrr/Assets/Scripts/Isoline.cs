using UnityEngine;
using System.Collections.Generic;

//Makes sure any GameObject this script is attached to always has a MeshFilter and MeshRenderer.
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
//[ExecuteAlways] 
// make script run in editor




public class Isoline : MonoBehaviour
{
    [Header("Ring Settings")]
    [Min(0.01f)] public float innerRadius = 1.5f;
    [Min(0.01f)] public float outerRadius = 2f;
    [Min(3)] public int segments = 32;

    [Header("Optiona;")]
    public float yPosition = 0f; // height of the ring

    private Mesh mesh;
    stopOnCollision sphere;
    stopOnCollision sphere2;

   // private Isoline sphere2child;

   

    void Awake()
    {
        //spheres = FindObjectsOfType<stopOnCollision>();
        sphere = GetComponentInParent<stopOnCollision>();


        GameObject[] projectileObjects = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject obj in projectileObjects)
        {
            // Skip self
            if (obj == gameObject) continue;

            sphere2 = obj.GetComponent<stopOnCollision>();

            if (sphere2 != null)
            {
             Debug.Log("Found another projectile: " + obj.name);
             break; // only get the first one
            }
        }

        //shpere2child = sphere2.GetComponentInChildren<Isoline>();



        InitializeMesh();
        GenerateRingMesh();
    }

 

    void OnValidate()
    {
        // Keep parameters valid
        innerRadius = Mathf.Clamp(innerRadius, 0f, outerRadius);
        segments = Mathf.Max(3, segments);

        InitializeMesh();
        
    }

    void InitializeMesh()
    {
        // Create or get mesh
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "IsolineRing";
            GetComponent<MeshFilter>().mesh = mesh;
        }
        else
        {
            mesh.Clear();
        }
    }

    void Update(){

       //  foreach (var sphere in spheres)
        //{
            if (sphere.is_collide)
            {

                transform.position = new Vector3((float)sphere.gameObject.transform.position.x, (float)sphere.gameObject.transform.position.y, (float)sphere.gameObject.transform.position.z);
                //Vector3 hillCenter = new Vector3(x,0,z);
                outerRadius = sphere.radius*1.5f;
                innerRadius = sphere.radius *1.45f;   
                  
                GenerateRingMesh();
            }
        }

        public bool Intersect(Vector3 vertex, Vector3 sphereCenter, float sphereRadius)
{
    Vector3 offset = vertex - sphereCenter;  // vector from sphere center to vertex
    return offset.sqrMagnitude < sphereRadius * sphereRadius; // use squared distance for efficiency
}


    

   
    public void GenerateRingMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        float step = Mathf.PI * 2f / segments;

        // Generate vertices and UVs
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * step;
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);

            // Outer vertex
             vertices.Add(new Vector3(x * outerRadius, yPosition, z * outerRadius));
            uv.Add(new Vector2((float)i / segments, 1f));


            Vector3 globalOuter = transform.TransformPoint(vertices[vertices.Count - 1]); 
            if (sphere2 != null && Intersect(globalOuter, sphere2.transform.position, sphere2.radius * 1.45f))
            {

                Debug.Log("intersect");
              // vertex is inside sphere2, hide it by moving it to zero or y=-100 etc.
             vertices[vertices.Count - 1]= Vector3.zero; 
            }   


            // Inner vertex
            vertices.Add(new Vector3(x * innerRadius, yPosition, z * innerRadius));
            uv.Add(new Vector2((float)i / segments, 0f));



            Vector3 globalInner = transform.TransformPoint(vertices[vertices.Count - 1]);
            if (sphere2 != null && Intersect(globalOuter, sphere2.transform.position, sphere2.radius * 1.45f))
            {
                // vertex is inside sphere2, hide it by moving it to zero or y=-100 etc.
               vertices[vertices.Count - 1] = Vector3.zero; 

                 Debug.Log("intersect");
            }
        }

        // Generate triangles (quad strips)
        for (int i = 0; i < segments * 2; i += 2)
        {
            triangles.Add(i);
            triangles.Add(i + 1);
            triangles.Add(i + 2);

            triangles.Add(i + 1);
            triangles.Add(i + 3);
            triangles.Add(i + 2);
        }

        // Assign to mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    // Optional: call at runtime to update size dynamically
    //public void UpdateRing(float newInner, float newOuter, int newSegments)
   // {
    //    innerRadius = Mathf.Clamp(newInner, 0f, newOuter);
     //   outerRadius = Mathf.Max(0.01f, newOuter);
     //   segments = Mathf.Max(3, newSegments);

      //  GenerateRingMesh();
   // }
}

