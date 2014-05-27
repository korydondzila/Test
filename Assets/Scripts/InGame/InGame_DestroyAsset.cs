using UnityEngine;
using System.Collections;

public class InGame_DestroyAsset : MonoBehaviour {

	private Camera cam;

	// Use this for initialization
	void Start()
	{
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if( transform.position.y < cam.transform.position.y - 7 )
			Destroy( gameObject );
	}
}