using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Menu_Score : MonoBehaviour {
	
	private string score = "0";
	
	// Use this for initialization
	void Start () {
		if( File.Exists( Application.persistentDataPath + "/killed.dat" ) )
		{
			BinaryFormatter b = new BinaryFormatter();
			FileStream f = File.OpenRead( Application.persistentDataPath + "/killed.dat" );
			score = (string)b.Deserialize( f );
			f.Close();
		}
		
		guiText.text = "Score: " + score.ToString();
	}
}