using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class InGame_Player_Death : MonoBehaviour {
	
	public bool makeSave = true;
	
	private GUIText text;
	private Camera cam;
	
	void Start()
	{
		text = (GUIText)GameObject.Find( "Score" ).GetComponent<GUIText>();
		cam = Camera.main;
	}
	
	void Update()
	{
		if( transform.position.y < cam.transform.position.y - 6 )
			EndGame();
	}

	void OnTriggerStay( Collider col )
	{
		if( col.gameObject.tag.Contains( "Kill" ) )
		{
			EndGame();
		}
	}
	
	void EndGame()
	{
		if( makeSave )
			Save();
		Application.LoadLevel( "EndGame" );
	}
	
	void Save()
	{
		BinaryFormatter b = new BinaryFormatter();
		FileStream f = File.Create( Application.persistentDataPath + "/killed.dat" );
		b.Serialize( f, text.text );
		f.Close();
	}
}