using UnityEngine;
using System.Collections;

public class EndGame_Menu : MonoBehaviour {

	void OnMouseDown()
	{
		Application.LoadLevel( "Menu" );
	}
}
