using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Menu_Start : MonoBehaviour {
	
	void Start()
	{
		if( File.Exists( Application.persistentDataPath + "/killed.dat" ) )
		{
			guiText.color = new Color( .1f, .1f, .1f );
		} else {
			guiText.color = new Color( 1, 1, 1 );
		}
	}
	
	void OnMouseDown()
	{
		if( File.Exists( Application.persistentDataPath + "/killed.dat" ) ) return;
		Application.LoadLevel( "InGame" );
	}
}