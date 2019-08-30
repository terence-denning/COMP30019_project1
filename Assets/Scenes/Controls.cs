using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controls : MonoBehaviour
{
    

    public int speed;
    private bool inBounds;

    float distance = 5f;

    private float size;

    private float x;
    private float y;
    private Vector3 rotateValue;

    void OnTriggerEnter(Collider other)
    {
        transform.position += new Vector3(0.0f, 0.4f, 0.0f);
    }

 

    void Start()
    {
        size = GameObject.Find("Terrain").GetComponent<CreateTerrain>().size;
    }

    // Update is called once per frame
    void Update()
    {
        inBounds = (transform.position.x <= (size / 2)) && (transform.position.x > (-size / 2)) && (transform.position.z < (size / 2)) && (transform.position.z > (-size / 2));

        
        if( !inBounds)
        {
            moveBackIntoGame();
        }
        

        // basic movements
        if (Input.GetKey("w") )
        {
            transform.position = transform.position + Camera.main.transform.forward * distance * Time.deltaTime;
        }
        if (Input.GetKey("s") )
        {
            transform.position = transform.position + -(Camera.main.transform.forward * distance * Time.deltaTime);
        }
        if (Input.GetKey("a") )
        {
            transform.position = transform.position + -(Camera.main.transform.right * distance * Time.deltaTime);
        }
        if (Input.GetKey("d") )
        {
            transform.position = transform.position + Camera.main.transform.right * distance * Time.deltaTime;
        }

        // mouse controls yaw and pitch of camera
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotateValue = new Vector3(x * 5, y * -5, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

    }

    void moveBackIntoGame()
    {
        float xPos = Camera.main.transform.position.x;
        float zPos = Camera.main.transform.position.z;

        if ( xPos > 10)
        {
            transform.position += new Vector3(-1.0f, 0, 0);
        }
        if (xPos < -10)
        {
            transform.position += new Vector3(1.0f, 0, 0);
        }
        if (zPos > 10)
        {
            transform.position += new Vector3(0, 0, -1.0f);
        }
        if (zPos < -10)
        {
            transform.position += new Vector3(0, 0, 1.0f);
        }
    }
}
