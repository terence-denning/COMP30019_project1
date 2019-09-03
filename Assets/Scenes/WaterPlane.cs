using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterPlane : MonoBehaviour
{
    public Shader shader;
    public int xSize, zSize;
    private Vector3[] vertices;
    private Mesh mesh;
    public Texture tex;
    public float wavespeed;
    Mesh  CreateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh;
        mesh = new Mesh();
        mesh.name = "WaterPlane";
        int i = 0;
        //Size of Plane;
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        //Define UV;
        Vector2[] uv = new Vector2[vertices.Length];
        //tangent;
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        int offsetX = xSize / 2;
        int offsetZ = zSize / 2;

        //Define Vertex;
        for (int y = 0; y <= zSize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x-offsetX,0, y-offsetZ);
                uv[i] = new Vector2((float)x / xSize,(float) y / zSize);
                tangents[i] = tangent;
                i++;
            }
        }
        mesh.vertices = vertices;
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
        mesh.uv = uv;
        mesh.tangents = tangents;
        
        return mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter mesh = this.gameObject.AddComponent<MeshFilter>();
        mesh.mesh = this.CreateMesh();
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
       
        renderer.material.shader = this.shader;
        renderer.material.mainTexture = this.tex;
        

    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer r = this.gameObject.GetComponent<MeshRenderer>();
        MeshFilter m = this.gameObject.GetComponent<MeshFilter>();
        Shader.SetGlobalFloat("WaveSpeed",this.wavespeed);
       

    }
}
