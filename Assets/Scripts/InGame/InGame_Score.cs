using UnityEngine;
using System.Collections;

public class InGame_Score : MonoBehaviour {
	
	private GameObject player;
	private float score = 0;
	private bool on = false;

	// Use this for initialization
	public void Init()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
		on = true;
	}
	
	// Update is called once per frame
	void Update()
	{
		if( !on ) return;
		
		if( Mathf.Floor( player.transform.position.y ) > score )
		{
			score = Mathf.Floor( player.transform.position.y );
			guiText.text = Mathf.Floor( player.transform.position.y ).ToString();
		}
	}
}