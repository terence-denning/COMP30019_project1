using System;
using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour
{

    public Color color;
    private float lightact;
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }

    
}