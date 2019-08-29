﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Code closely follows the tutorial by Ather Omar found at:
 * https://www.youtube.com/watch?v=1HV8GbFnCik&t=539s
 */

public class CreateTerrain : MonoBehaviour
{

    Mesh mesh;
    MeshCollider c;
    Vector3[] vertices;
    Vector2[] uvs;

    public int dimensions;
    public float size;
    public float height;

    void Start()
    {
        createShape();
    }

    void createShape()
    {
        float halfSize = size * 0.5f;
        float divisionSize = size / dimensions;
        int numVertices = (dimensions + 1) * (dimensions + 1);

        vertices = new Vector3[ numVertices ];
        uvs = new Vector2[numVertices];

        int[] triangles = new int[ dimensions * dimensions * 6 ];

        int triOffset = 0;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

         c = GetComponent<MeshCollider>();
        //collider = gameObject.AddComponent<MeshCollider>();
        c.convex = true;

        for ( int i = 0; i <= dimensions; i++)
        {
            for( int j = 0; j <= dimensions; j++)
            {

                vertices[i * (dimensions+1) + j] = new Vector3( -halfSize+j*divisionSize, 0.0f, halfSize-i*divisionSize );
                uvs[i * (dimensions+1) + j] = new Vector2((float)i/dimensions, (float)j/dimensions);

                // build polygons of terrain
                if ( i < dimensions && j < dimensions)
                {
                    int topLeft = i * ( dimensions + 1 ) + j;
                    int bottomLeft = (i + 1) * ( dimensions + 1 ) + j ;
    
                    // first triangle of square
                    triangles[triOffset] = topLeft;
                    triangles[triOffset + 1] = topLeft + 1;
                    triangles[triOffset + 2] = bottomLeft + 1;

                    // second triangle of square
                    triangles[triOffset + 3] = topLeft;
                    triangles[triOffset + 4] = bottomLeft + 1;
                    triangles[triOffset + 5] = bottomLeft;

                    triOffset += 6;

                }

            }
        }

        // initial random heights
        vertices[0].y = Random.Range(-height, height);
        vertices[dimensions].y = Random.Range(-height, height);
        vertices[vertices.Length-1].y = Random.Range(-height, height);
        vertices[vertices.Length-1-dimensions].y = Random.Range(-height, height);


        int iterations = (int)Mathf.Log(dimensions, 2);
        int numSquares = 1;
        int squareSize = dimensions;

        for(int i = 0; i < iterations; i++)
        {
            int row = 0;

            for( int j=0; j < numSquares; j++)
            {
                int col = 0;

                for( int k = 0; k < numSquares; k++ )
                {
                    // run diamond square algorithm to set y values of each vertice
                    diamondSquare(row, col, squareSize, height);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            height *= 0.5f;
        }

        // set mesh values
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void diamondSquare(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (dimensions + 1) + col;
        int bottomLeft = ( row + size ) * ( dimensions + 1 ) + col;

        // diamond part
        int midPoint = (int)(row + halfSize) * (dimensions + 1) + (int)(col + halfSize);
        vertices[midPoint].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[bottomLeft].y + vertices[bottomLeft + size].y)*0.25f + Random.Range(-offset, offset);


        // square part
        vertices[topLeft + halfSize].y = (vertices[topLeft].y + vertices[topLeft + size].y + vertices[midPoint].y) / 3 + Random.Range(-offset, offset);
        vertices[midPoint - halfSize].y = (vertices[topLeft].y + vertices[bottomLeft].y + vertices[midPoint].y) / 3 + Random.Range(-offset, offset);
        vertices[midPoint + halfSize].y = (vertices[topLeft+size].y + vertices[bottomLeft+size].y + vertices[midPoint].y) / 3 + Random.Range(-offset, offset);
        vertices[bottomLeft + halfSize].y = (vertices[bottomLeft].y + vertices[bottomLeft + size].y + vertices[midPoint].y) / 3 + Random.Range(-offset, offset);

    }

   
}