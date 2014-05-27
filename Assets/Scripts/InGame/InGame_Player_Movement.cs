using UnityEngine;
using System.Collections;

public class InGame_Player_Movement : MonoBehaviour {
	
	public float speed = 25.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 9.8f;
	public bool OnGround = true;
	
	private Camera cam;
	private float posY = 0;
	private RaycastHit hit;

	// Use this for initialization
	void Start()
	{
		cam = Camera.main;
		cam.transform.position = new Vector3( 0, transform.position.y, -1 );
		GameObject.Find( "Score" ).GetComponent<InGame_Score>().Init();
	}
	
	// Update is called once per frame
	void Update()
	{
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		
		if( Input.GetKey( KeyCode.LeftArrow ) )
			if( !Physics.Raycast( transform.position + Vector3.up * 0.24f, Vector3.left, 0.25f, layerMask ) && !Physics.Raycast( transform.position + Vector3.down * 0.24f, Vector3.left, 0.25f, layerMask ) )
				transform.position += Vector3.left * speed * Time.deltaTime;
			else
			{
				float x = Mathf.Ceil( transform.position.x ) - 0.25f;
				transform.position = new Vector3( x, transform.position.y, transform.position.z );
			}
		
		if( Input.GetKey( KeyCode.RightArrow ) )
			if( !Physics.Raycast( transform.position + Vector3.up * 0.24f, Vector3.right, 0.25f, layerMask ) && !Physics.Raycast( transform.position + Vector3.down * 0.24f, Vector3.right, 0.25f, layerMask ) )
				transform.position += Vector3.right * speed * Time.deltaTime;
			else
			{
				float x = Mathf.Floor( transform.position.x ) + 0.25f;
				transform.position = new Vector3( x, transform.position.y, transform.position.z );
			}
		
		if ( Physics.Raycast( transform.position + Vector3.right * 0.24f, Vector3.down, 0.25f, layerMask ) || Physics.Raycast( transform.position + Vector3.left * 0.24f, Vector3.down, 0.25f, layerMask ) )
		{
			float y = Mathf.Ceil( transform.position.y ) - 0.25f;
			transform.position = new Vector3( transform.position.x, y, transform.position.z );
			if ( Input.GetKey( KeyCode.UpArrow ) )
        		posY = jumpSpeed;
			else
				posY = 0;
		}
        else
			posY -= gravity * Time.deltaTime;
		
		if ( !Physics.Raycast( transform.position + Vector3.right * 0.24f, Vector3.up, 0.25f, layerMask ) && !Physics.Raycast( transform.position + Vector3.left * 0.24f, Vector3.up, 0.25f, layerMask ) || posY < 0 )
			transform.position += Vector3.up * posY * Time.deltaTime;
		else
		{
			posY = 0;
			float y = Mathf.Floor( transform.position.y ) + 0.25f;
			transform.position = new Vector3( transform.position.x, y, transform.position.z );
		}
		
		camPos();
	}
	
	void camPos()
	{
		if( transform.position.y < cam.transform.position.y ) return;
		
		cam.transform.position = new Vector3( 0, transform.position.y, -1 );
	}
}