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
	private MeshRenderer rend;
	private Color color;
	[Range(0, 24)]
	public float timeOfDay = 12;

	public float secondsPerMinute = 60;
	[HideInInspector]
	public float secondsPerHour;
	[HideInInspector]
	public float secondsPerDay;

	public float timeMultiplier = 300000;

	void Start()
	{
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
		yloca = GameObject.Find("Terrain").GetComponent<CreateTerrain>().averageHeight;
		SunUpdate();
		MeshRenderer r = this.gameObject.GetComponent<MeshRenderer>();
		sun.transform.position = new Vector3(0,yloca,0);
		//r.transform.position = new Vector3(0,yloca,0);
		timeOfDay += (Time.deltaTime / secondsPerDay) * timeMultiplier;

		if (timeOfDay >= 24)
		{
			timeOfDay = 0;
		}
	}

	public void SunUpdate()
	{
		sun.transform.localRotation = Quaternion.Euler((timeOfDay / 24) * 360 - 0, 0.0f, 0.0f);
	}
}