using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterPlane : MonoBehaviour
{
    public Shader shader;
    public float wavespeed;
    public float wavereduct =2 ;
    public PointLight pointLight;
    public float yloc;
    public int xSize, zSize;
    private Vector3[] vertices;
    private Mesh mesh;
    private float lightact;
    Mesh  CreateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh;
        mesh = new Mesh();
        mesh.name = "WaterPlane";
        int i = 0;
        //Size of Plane;
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        //tangent;
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        Color[] colors = new Color[vertices.Length];
        int offsetX = xSize / 2;
        int offsetZ = zSize / 2;

        //Define Vertex;
        for (int y = 0; y <= zSize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x - offsetX, 0, y - offsetZ);
                tangents[i] = tangent;
                i++;
            }
        }
        mesh.vertices = vertices;

        for (int k = 0; k < vertices.Length; k++)
        {
            colors[k] = new Color(0.5f,0.8f,0.97f,0.7f);
        }

        mesh.colors = colors;
        //Define Triangle;

        int[] triangles = new int[xSize * zSize* 6];
        for (int t = 0, v = 0, y = 0; y < zSize; y++, v++)
        {
            for (int x = 0; x < xSize; x++, t += 6, v++)
            {
                triangles[t] = v;
                triangles[t + 1] = v + xSize + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + xSize + 1;
                triangles[t + 5] = v +  xSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.tangents = tangents;
        
        return mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter mesh = this.gameObject.AddComponent<MeshFilter>();
        mesh.mesh = this.CreateMesh();
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().convex = true;
        renderer.material.shader = this.shader;

    }
    
    void Update()
    {
        MeshRenderer r = this.gameObject.GetComponent<MeshRenderer>();
        MeshFilter m = this.gameObject.GetComponent<MeshFilter>();
        r.material.SetFloat("WaveSpeed",this.wavespeed);
        r.material.SetFloat("WaveReduct",wavereduct);
        r.material.SetColor("_PointLightColor", this.pointLight.color);
        //Update the y axis accroding to terrain location
        r.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
        yloc = GameObject.Find("Terrain").GetComponent<CreateTerrain>().averageHeight;
        r.transform.position = new Vector3(0, yloc, 0);
        lightact = GameObject.Find("Sun").GetComponent<SunRotation>().lightactive;
        r.material.SetFloat("_lightact", lightact);
    }
}
