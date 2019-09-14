using UnityEngine;
using System.Collections;

public class SunRotation : MonoBehaviour
{
	public float yloca;
	[HideInInspector]
	public GameObject sun;
	[HideInInspector]
	public Light sunLight;

	public Texture tex;
	[Range(0, 24)]
	public float timeOfDay = 12;
	
	public float secondsPerMinute = 60;
	[HideInInspector]
	public float secondsPerHour;
	[HideInInspector]
	public float secondsPerDay;
	
	public float lightactive = 1;
	public float timeMultiplier = 300000;
	private Color color;

	void Start()
	{
		MeshRenderer rend;
		sun = gameObject;
		sunLight = gameObject.GetComponent<Light>();
		secondsPerHour = secondsPerMinute * 60;
		secondsPerDay = secondsPerHour * 24;
		sun.gameObject.AddComponent<MeshFilter>();
		sun.gameObject.AddComponent<MeshRenderer>();
		rend = sun.GetComponent<MeshRenderer>();
		rend.material.mainTexture = this.tex;

	}

	// Update is called once per frame
	void Update()
	{	
		//Update the rotation y axis according to the terrain position
		yloca = GameObject.Find("Terrain").GetComponent<CreateTerrain>().averageHeight;
		SunUpdate();
		MeshRenderer r = this.gameObject.GetComponent<MeshRenderer>();
		//Sun rotation
		sun.transform.position = new Vector3(0,yloca,0);
		timeOfDay += (Time.deltaTime / secondsPerDay) * timeMultiplier;
		if (timeOfDay >= 24)
		{
			timeOfDay = 0;
		}
		//light factors affect reflections
		if (timeOfDay >17 && timeOfDay < 23 )
		{
			lightactive = 1/(24 - timeOfDay);
		}
		else if (timeOfDay< 6 && timeOfDay > 1 )
		{
			lightactive = 1 -  1 / (7 - timeOfDay);
		}
		else if (timeOfDay > 23 || timeOfDay < 1)
		{
			lightactive = 1;
		}
		else
		{
			lightactive = 0;
		}
	}

	public void SunUpdate()
	{
		sun.transform.localRotation = Quaternion.Euler((timeOfDay / 24)* 360 - 0, 0.0f, 0.0f);
	}	
}