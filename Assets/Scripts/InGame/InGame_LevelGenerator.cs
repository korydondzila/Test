using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGame_LevelGenerator : MonoBehaviour {
	
	public GameObject[] assets = new GameObject[14];
	public GameObject player;
	
	private Camera cam;
	private bool start = true;
	private List<List<int>> map = new List<List<int>>();
	private List<List<List<int>>> badDirs = new List<List<List<int>>>();
	private List<int> path = new List<int>();
	private int last = 0;
	private int next = 12;
	private int maxY = 0;
	private int current = 0;
	private int currentX = 6;
	private int currentY = 0;
	private int currentJump = 0;
	private int currentDrop = 0;
	private int currentClimb = 0;
	
	// Use this for initialization
	void Start () {
		for( int i = 1; i < assets.Length; i++ )
		{
			assets[i].GetComponent<InGame_AssetNum>().assetNum = i;
		}
		
		cam = Camera.main;
		
		for( int i = -5; i < 0; i++ )
		{
			for( int j = -8; j < 9; j++ )
			{
				/*if( i == -1 && j >= -6 && j <= 6 )
					Instantiate( assets[1], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
				else if( i == -1 && ( j == -7 || j == 7 ) )
				{
					Instantiate( assets[13], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
				}
				else*/
					Instantiate( assets[4], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
			}
		}
		
		map.Add( new List<int>() );
		badDirs.Add( new List<List<int>>() );
		for( int i = 0; i < 13; i++ )
		{
			badDirs[0].Add( new List<int>() );
			if( i == currentX )
				map[0].Add(1);
			else
				map[0].Add(0);
		}
		
		GenerateMap();
		BuildWalls();
		Instantiate( player );
	}
	
	// Update is called once per frame
	void Update()
	{
		if( cam.transform.position.y >= last + 1 )
		{
			GenerateMap();
			BuildWalls();
		}
	}
	
	void GenerateMap()
	{
		if( start )
			start = false;
		else
			next++;
		
		int currentMap = next + 9;
		if( current == 0 )
			currentMap = 0;
		
		for( int i = currentMap; i < next+10; i++ )
		{
			map.Add( new List<int>() );
			badDirs.Add( new List<List<int>>() );
			for( int j = 0; j < 13; j++ )
			{
				map[i+1].Add(0);
				badDirs[i+1].Add( new List<int>() );
			}
			
			while( currentY <= maxY )
			{
				int direction = 0;
				bool go = false;
				bool leftGo = true;
				bool rightGo = true;
				bool upGo = true;
				bool downGo = true;
				
				while( !go )
				{
					bool goodDir = false;
					while( !goodDir )
					{
						switch( currentY )
						{
							case 0:
								switch( currentX )
								{
								case 0:
									direction = Random.Range( 1, 3 );
									leftGo = false;
									downGo = false;
									break;
									
								case 12:
									direction = Random.Range( 0, 2 );
									rightGo = false;
									downGo = false;
									break;
									
								default:
									direction = Random.Range( 0, 3 );
									downGo = false;
									break;
								}
								break;
								
							default:
								switch( currentX )
								{
								case 0:
									direction = Random.Range( 1, 4 );
									leftGo = false;
									break;
									
								case 12:
									direction = Random.Range( -1, 2 );
									if( direction == -1 )
										direction = 3;
									rightGo = false;
									break;
									
								default:
									direction = Random.Range( 0, 4 );
									break;
								}
								break;
						}
						
						if( path.Count == 0 )
							goodDir = true;
						else
						{
							switch( direction )
							{
								case 0:
									if( leftGo )
									{
										if( !badDirs[ currentY ][ currentX ].Contains( 0 ) && path[ path.Count - 1 ] != 2 )
											goodDir = true;
										else
											leftGo = false;
									}
									break;
							
								case 1:
									if( upGo )
									{
										if( !badDirs[ currentY ][ currentX ].Contains( 1 ) && path[ path.Count - 1 ] != 3 )
											goodDir = true;
										else
											upGo = false;
									}
									break;
							
								case 2:
									if( rightGo )
									{
										if( !badDirs[ currentY ][ currentX ].Contains( 2 ) && path[ path.Count - 1 ] != 0 )
											goodDir = true;
										else
											rightGo = false;
									}
									break;
							
								case 3:
									if( downGo )
									{
										if( !badDirs[ currentY ][ currentX ].Contains( 3 ) && path[ path.Count - 1 ] != 1 )
											goodDir = true;
										else
											downGo = false;
									}
									break;
							}
						}
						if( !leftGo && !rightGo && !upGo && !downGo )
							break;
					}
					
					if( leftGo || rightGo || upGo || downGo )
					{
						switch( direction )
						{
							case 0:
								if( map[ currentY ][ currentX - 1 ] == 0 )
								{
									if( currentX - 2 >= 0 )
									{
										if( map[ currentY ][ currentX - 2 ] == 0 )
										{
											if( currentY - 1 >= 0 )
											{
												if( map[ currentY + 1 ][ currentX - 1 ] == 0 && map[ currentY - 1 ][ currentX - 1 ] == 0 )
												{
													currentX--;
													currentJump = 0;
													go = true;
												}
											}
											else
											{
												if( map[ currentY + 1 ][ currentX - 1 ] == 0 )
												{
													currentX--;
													currentJump = 0;
													go = true;
												}
											}
										}
									}
									else
									{
										if( currentY - 1 >= 0 )
										{
											if( map[ currentY + 1 ][ currentX - 1 ] == 0 && map[ currentY - 1 ][ currentX - 1 ] == 0 )
											{
												currentX--;
												currentJump = 0;
												go = true;
											}
										}
										else
										{
											if( map[ currentY + 1 ][ currentX - 1 ] == 0 )
											{
												currentX--;
												currentJump = 0;
												go = true;
											}
										}
									}
								}
								if( !go )
									leftGo = false;
								break;
								
							case 1:
								if( map[ currentY + 1 ][ currentX ] == 0 && currentJump < 3 )
								{
									if( currentY + 2 < maxY + 2 )
									{
										if( map[ currentY + 2 ][ currentX ] == 0 )
										{
											if( currentX - 1 >= 0 && currentX + 1 <= 12 )
											{
												if( map[ currentY + 1 ][ currentX + 1 ] == 0 && map[ currentY + 1 ][ currentX - 1 ] == 0 )
												{
													currentY++;
													currentJump++;
													if( currentDrop > 0 )
														currentClimb++;
													if( currentClimb == 4 )
													{
														currentDrop = 0;
														currentClimb = 0;
													}
													go = true;
												}
											}
											else
											{
												if( currentX - 1 < 0 )
												{
													if( map[ currentY + 1 ][ currentX + 1 ] == 0 )
													{
														currentY++;
														currentJump++;
														if( currentDrop > 0 )
															currentClimb++;
														if( currentClimb == 4 )
														{
															currentDrop = 0;
															currentClimb = 0;
														}
														go = true;
													}
												}
												else
												{
													if( map[ currentY + 1 ][ currentX - 1 ] == 0 )
													{
														currentY++;
														currentJump++;
														if( currentDrop > 0 )
															currentClimb++;
														if( currentClimb == 4 )
														{
															currentDrop = 0;
															currentClimb = 0;
														}
														go = true;
													}
												}
											}
										}
									}
									else
									{
										if( currentX - 1 >= 0 && currentX + 1 <= 12 )
										{
											if( map[ currentY + 1 ][ currentX + 1 ] == 0 && map[ currentY + 1 ][ currentX - 1 ] == 0 )
											{
												currentY++;
												currentJump++;
												if( currentDrop > 0 )
													currentClimb++;
												if( currentClimb == 4 )
												{
													currentDrop = 0;
													currentClimb = 0;
												}
												go = true;
											}
										}
										else
										{
											if( currentX - 1 < 0 )
											{
												if( map[ currentY + 1 ][ currentX + 1 ] == 0 )
												{
													currentY++;
													currentJump++;
													if( currentDrop > 0 )
														currentClimb++;
													if( currentClimb == 4 )
													{
														currentDrop = 0;
														currentClimb = 0;
													}
													go = true;
												}
											}
											else
											{
												if( map[ currentY + 1 ][ currentX - 1 ] == 0 )
												{
													currentY++;
													currentJump++;
													if( currentDrop > 0 )
														currentClimb++;
													if( currentClimb == 4 )
													{
														currentDrop = 0;
														currentClimb = 0;
													}
													go = true;
												}
											}
										}
									}
								}
								if( !go )
									upGo = false;
								break;
								
							case 2:
								if( map[ currentY ][ currentX + 1 ] == 0 )
								{
									if( currentX + 2 <= 12 )
									{
										if( map[ currentY ][ currentX + 2 ] == 0 )
										{
											if( currentY - 1 >= 0 )
											{
												if( map[ currentY + 1 ][ currentX + 1 ] == 0 && map[ currentY - 1 ][ currentX + 1 ] == 0 )
												{
													currentX++;
													currentJump = 0;
													go = true;
												}
											}
											else
											{
												if( map[ currentY + 1 ][ currentX + 1 ] == 0 )
												{
													currentX++;
													currentJump = 0;
													go = true;
												}
											}
										}
									}
									else
									{
										if( currentY - 1 >= 0 )
										{
											if( map[ currentY + 1 ][ currentX + 1 ] == 0 && map[ currentY - 1 ][ currentX + 1 ] == 0 )
											{
												currentX++;
												currentJump = 0;
												go = true;
											}
										}
										else
										{
											if( map[ currentY + 1 ][ currentX + 1 ] == 0 )
											{
												currentX++;
												currentJump = 0;
												go = true;
											}
										}
									}
								}
								if( !go )
									rightGo = false;
								break;
								
							case 3:
								if( map[ currentY - 1 ][ currentX ] == 0 && currentDrop < 4 )
								{
									if( currentY - 2 >= 0 )
									{
										if( map[ currentY - 2 ][ currentX ] == 0 )
										{
											if( currentX - 1 >= 0 && currentX + 1 <= 12 )
											{
												if( map[ currentY - 1 ][ currentX + 1 ] == 0 && map[ currentY - 1 ][ currentX - 1 ] == 0 )
												{
													currentY--;
													currentJump = 0;
													currentDrop++;
													go = true;
												}
											}
											else
											{
												if( currentX - 1 < 0 )
												{
													if( map[ currentY - 1 ][ currentX + 1 ] == 0 )
													{
														currentY--;
														currentJump = 0;
														currentDrop++;
														go = true;
													}
												}
												else
												{
													if( map[ currentY - 1 ][ currentX - 1 ] == 0 )
													{
														currentY--;
														currentJump = 0;
														currentDrop++;
														go = true;
													}
												}
											}
										}
									}
									else
									{
										if( currentX - 1 >= 0 && currentX + 1 <= 12 )
										{
											if( map[ currentY - 1 ][ currentX + 1 ] == 0 && map[ currentY - 1 ][ currentX - 1 ] == 0 )
											{
												currentY--;
												currentJump = 0;
												currentDrop++;
												go = true;
											}
										}
										else
										{
											if( currentX - 1 < 0 )
											{
												if( map[ currentY - 1 ][ currentX + 1 ] == 0 )
												{
													currentY--;
													currentJump = 0;
													currentDrop++;
													go = true;
												}
											}
											else
											{
												if( map[ currentY - 1 ][ currentX - 1 ] == 0 )
												{
													currentY--;
													currentJump = 0;
													currentDrop++;
													go = true;
												}
											}
										}
									}
								}
								if( !go )
									downGo = false;
								break;
						}
					}
					
					if( !go )
						Debug.Log( "Bad direction: " + direction );
					else
						Debug.Log( direction );
					
					if( !leftGo && !rightGo && !upGo && !downGo )
					{
						map[ currentY ][ currentX ] = 0;
						int lastDir = path[ path.Count - 1 ];
						Debug.Log( "No path: " + lastDir );
						path.RemoveAt( path.Count - 1 );
						switch( lastDir )
						{
							case 0:
								currentX++;
								badDirs[ currentY ][ currentX ].Add( 0 );
								break;
								
							case 1:
								currentY--;
								currentJump--;
								if( currentClimb > 0 )
									currentClimb--;
								badDirs[ currentY ][ currentX ].Add( 1 );
								break;
								
							case 2:
								currentX--;
								badDirs[ currentY ][ currentX ].Add( 2 );
								break;
								
							case 3:
								currentY++;
								if( currentDrop > 0 )
									currentDrop--;
								badDirs[ currentY ][ currentX ].Add( 3 );
								break;
						}
						leftGo = true;
						rightGo = true;
						upGo = true;
						downGo = true;
					}
				}
				
				path.Add(direction);
				map[ currentY ][ currentX ] = 1;
				
			}
			
			maxY++;
			
		}
		
		/*for( int i = 0; i < path.Count; i++ )
		{
			Debug.Log( path[i] );
		}*/
		
		for( int i = next+10; i >= 0; i-- )
		{
			Debug.Log( map[i][0] + " " + map[i][1] + " " + map[i][2] + " " + map[i][3] + " " + map[i][4] + " " + map[i][5] + " " + map[i][6] + " " + map[i][7] + " " + map[i][8] + " " + map[i][9] + " " + map[i][10] + " " + map[i][11] + " " + map[i][12] );
		}
	}
	
	GameObject CheckDown( Vector3 pos )
	{
		RaycastHit hit;
		if( Physics.Raycast( pos, Vector3.down, out hit, 1 ) )
		{
			return hit.collider.gameObject;
		}
		else
		{
			return null;
		}
	}
	
	GameObject CheckLeft( Vector3 pos )
	{
		RaycastHit hit;
		if( Physics.Raycast( pos, Vector3.left, out hit, 1 ) )
		{
			return hit.collider.gameObject;
		}
		else
		{
			return null;
		}
	}
	
	GameObject CheckRight( Vector3 pos )
	{
		RaycastHit hit;
		if( Physics.Raycast( pos, Vector3.right, out hit, 1 ) )
		{
			return hit.collider.gameObject;
		}
		else
		{
			return null;
		}
	}
	
	int RandomAsset( List<int> nums )
	{
		return nums[ Random.Range( 0, nums.Count ) ];
	}
	
	void RandomFlip( int nextAsset, GameObject instance )
	{
		if( nextAsset == 10 || nextAsset == 11 )
		{
			int rand = Random.Range( 0, 2 );
			int scale = 1;
			if( rand == 0 )
				scale = -1;
			instance.transform.localScale = new Vector3( scale, 1, 1);
		}
	}
	
	void BuildWalls()
	{
		for( int i = current; i < next; i++ )
		{
			for( int j = -8; j < -6; j++ )
			{
				Instantiate( assets[4], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
				/*if( j == -8 )
					Instantiate( assets[4], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
				else
				{
					if( i == 0 )
					{
						GameObject instance = Instantiate( assets[ 3 ], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) ) as GameObject;
						instance.transform.localScale = new Vector3( -1, 1, 1 );
					}
					else
					{
						GameObject obj = CheckDown( new Vector3( j, i, 0 ) );
						int asset = obj.GetComponent<InGame_AssetNum>().assetNum;
						int nextAsset = 3;
						List<int> nums = new List<int>();
						nums.Add(3);
						nums.Add(5);
						nums.Add(12);
						
						switch( asset )
						{
							case 3:
								nextAsset = RandomAsset( nums );
								break;
							
							case 4:
								nums.Add(4);
								nums.Remove(5);
								nums.Remove(12);
								nextAsset = RandomAsset( nums );
								break;
							
							case 5:
								nums.Add(13);
								nums.Remove(3);
								nums.Remove(5);
								nums.Remove(12);
								nextAsset = RandomAsset( nums );
								break;
							
							case 12:
								nextAsset = RandomAsset( nums );
								break;
							
							default:
								break;
						}
						
						GameObject instance = Instantiate( assets[ nextAsset ], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) ) as GameObject;
						instance.transform.localScale = new Vector3( -1, 1, 1 );
					}
				}*/
			}
			
			for( int j = 8; j > 6; j-- )
			{
				Instantiate( assets[4], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
				/*if( j == 8 )
					Instantiate( assets[4], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
				else
				{
					if( i == 0 )
					{
						Instantiate( assets[ 3 ], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
					}
					else
					{
						GameObject obj = CheckDown( new Vector3( j, i, 0 ) );
						int asset = obj.GetComponent<InGame_AssetNum>().assetNum;
						int nextAsset = 3;
						List<int> nums = new List<int>();
						nums.Add(3);
						nums.Add(5);
						nums.Add(12);
						
						switch( asset )
						{
							case 3:
								nextAsset = RandomAsset( nums );
								break;
							
							case 4:
								nums.Add(4);
								nums.Remove(5);
								nums.Remove(12);
								nextAsset = RandomAsset( nums );
								break;
							
							case 5:
								nums.Add(13);
								nums.Remove(3);
								nums.Remove(5);
								nums.Remove(12);
								nextAsset = RandomAsset( nums );
								break;
							
							case 12:
								nextAsset = RandomAsset( nums );
								break;
							
							default:
								break;
						}
						
						Instantiate( assets[ nextAsset ], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
					}
				}*/
			}
			
			for( int j = -6; j < 7; j++ )
			{
				if( map[ i ][ j+6 ] == 0 )
					Instantiate( assets[4], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) );
				/*GameObject objDown = CheckDown( new Vector3( j, i, 0 ) );
				Debug.Log(objDown);
				int assetDown = 0;
				if( objDown != null )
					assetDown = objDown.GetComponent<InGame_AssetNum>().assetNum;
				
				GameObject objLeft = CheckLeft( new Vector3( j, i, 0 ) );
				int assetLeft = 0;
				if( objLeft != null )
					assetLeft = objLeft.GetComponent<InGame_AssetNum>().assetNum;
				
				/*GameObject objRight = CheckRight( new Vector3( j, i, 0 ) );
				int assetRight = 0;
				if( objRight != null )
					assetRight = objRight.GetComponent<InGame_AssetNum>().assetNum;
				
				int nextAsset = 0;
				List<int> nums = new List<int>();
				
				switch( assetDown )
				{
					case 0:
						switch( assetLeft )
						{
							case 0:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(7);
								nums.Add(8);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								break;
								
							case 2:
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								break;
								
							case 5:
								nums.Add(8);
								nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								nums.Add(6);
								nums.Add(7);
								nextAsset = RandomAsset( nums );
								break;
								
							case 7:
								nums.Add(7);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Remove(7);
								}
								else
									nums.Add(6);
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								nums.Add(8);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Remove(8);
								}
								else
									nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nums.Add(8);
								nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 10:
								break;
								
							case 11:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(7);
								nums.Add(8);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								break;
						}
						break;
					
					case 1:
						switch( assetLeft )
						{
							case 0:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(10);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								break;
								
							case 2:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
								}
								else
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									//nums.Add(8);
									nums.Add(10);
									nums.Add(11);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									//nums.Add(8);
									nums.Add(10);
									nums.Add(11);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								break;
								
							case 5:
								break;
								
							case 6:
								nums.Add(6);
								nums.Add(7);
								nextAsset = RandomAsset( nums );
								break;
								
							case 7:
								nums.Add(7);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(8);
									nums.Add(10);
									nums.Add(11);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								nums.Add(8);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									nums.Add(10);
									nums.Add(11);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nums.Add(8);
								nextAsset = RandomAsset( nums );
								break;
								
							case 10:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(10);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 11:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(10);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									//nums.Add(8);
									nums.Add(10);
									nums.Add(11);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								break;
						}
						break;
					
					case 2:
						switch( assetLeft )
						{
							case 0:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								nextAsset = 2;
								break;
								
							case 2:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									//nums.Add(8);
									nums.Add(11);
								}
								else
								{
									nums.Add(2);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									//nums.Add(8);
									nums.Add(11);
								}
								else
								{
									nums.Add(3);
									nums.Add(5);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(3);
								nums.Add(5);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								nums.Add(8);
								//nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								//nums.Add(6);
								nums.Add(7);
								nextAsset = RandomAsset( nums );
								break;
								
							case 7:
								nums.Add(7);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Remove(7);
									nums.Add(11);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								nums.Add(8);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Remove(8);
									nums.Add(11);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nums.Add(8);
								//nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 10:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 11:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									//nums.Add(8);
									nums.Add(11);
								}
								else
								{
									nums.Add(3);
									nums.Add(5);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(3);
								nums.Add(5);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
						}
						break;
					
					case 3:
						switch( assetLeft )
						{
							case 0:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								nextAsset = 2;
								break;
								
							case 2:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(3);
									nums.Add(12);
								}
								else
									nums.Add(2);
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(3);
								nums.Add(5);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								nums.Add(5);
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								nextAsset = 5;
								break;
								
							case 7:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								else
								{
									nums.Add(5);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nextAsset = 5;
								break;
								
							case 10:
								break;
								
							case 11:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(2);
								nums.Add(3);
								nums.Add(5);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
						}
						break;
					
					case 4:
						switch( assetLeft )
						{
							case 0:
								break;
								
							case 1:
								nums.Add(1);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 2:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(1);
									nums.Add(13);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(4);
									//nums.Add(13);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(4);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(4);
									nums.Add(13);
									nextAsset = RandomAsset( nums );
								}
								break;
								
							case 6:
								break;
								
							case 7:
								break;
								
							case 8:
								break;
								
							case 9:
								break;
								
							case 10:
								break;
								
							case 11:
								break;
								
							case 12:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(4);
									//nums.Add(13);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(1);
								nums.Add(4);
								//nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
						}
						break;
					
					case 5:
						switch( assetLeft )
						{
							case 0:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								nums.Add(1);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 2:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(1);
									nums.Add(13);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(4);
									nums.Add(13);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(4);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(4);
									//nums.Add(13);
									nextAsset = RandomAsset( nums );
								}
								break;
								
							case 6:
								break;
								
							case 7:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								break;
								
							case 10:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 11:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(4);
									nums.Add(13);
								}
								else
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(1);
								nums.Add(4);
								//nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
						}
						break;
					
					case 6:
						switch( assetLeft )
						{
							case 0:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(10);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								break;
								
							case 2:
								break;
								
							case 3:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(10);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								break;
								
							case 5:
								break;
								
							case 6:
								nums.Add(6);
								nums.Add(7);
								nextAsset = RandomAsset( nums );
								break;
								
							case 7:
								nums.Add(7);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Remove(7);
									nums.Add(10);
									nums.Add(11);
								}
								else
								{
									nums.Add(6);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								nums.Add(8);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Remove(8);
									nums.Add(10);
									nums.Add(11);
								}
								else
								{
									nums.Add(9);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nums.Add(8);
								nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 10:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(10);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 11:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(10);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								break;
						
							case 13:
								break;
						}
						break;
					
					case 7:
						switch( assetLeft )
						{
							case 0:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								break;
								
							case 2:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								break;
								
							case 5:
								nums.Add(8);
								//nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								//nums.Add(6);
								nums.Add(7);
								nextAsset = RandomAsset( nums );
								break;
								
							case 7:
								nums.Add(7);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(8);
									nums.Add(11);
									nums.Remove(7);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								nums.Add(8);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									nums.Add(0);
									//nums.Add(7);
									nums.Add(11);
									nums.Remove(8);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nums.Add(8);
								//nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 10:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 11:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								nums.Add(0);
								//nums.Add(7);
								//nums.Add(8);
								nums.Add(11);
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								break;
						}
						break;
					
					case 8:
						switch( assetLeft )
						{
							case 0:
								if( objDown.transform.localScale.x == 1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
									nextAsset = RandomAsset( nums );
								}
								break;
								
							case 1:
								nextAsset = 2;
								break;
								
							case 2:
								nums.Add(2);
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(3);
								nums.Add(5);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								nums.Add(5);
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								nextAsset = 5;
								break;
								
							case 7:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								else
								{
									nums.Add(5);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nextAsset = 5;
								break;
								
							case 10:
								break;
								
							case 11:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nextAsset = 2;
								break;
						}
						break;
					
					case 9:
						switch( assetLeft )
						{
							case 0:
								break;
								
							case 1:
								nums.Add(1);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 2:
								nums.Add(1);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								nums.Add(4);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(4);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								nums.Add(4);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								break;
								
							case 7:
								break;
								
							case 8:
								break;
								
							case 9:
								break;
								
							case 10:
								break;
								
							case 11:
								break;
								
							case 12:
								nums.Add(4);
								nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(1);
								nums.Add(4);
								//nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
						}
						break;
					
					case 10:
						switch( assetLeft )
						{
							case 0:
								break;
								
							case 1:
								break;
								
							case 2:
								break;
								
							case 3:
								break;
								
							case 4:
								break;
								
							case 5:
								nums.Add(8);
								nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								//nums.Add(6);
								nums.Add(7);
								nextAsset = RandomAsset( nums );
								break;
								
							case 7:
								nums.Add(7);
								if( objLeft.transform.localScale.x == -1 )
									nums.Remove(7);
								else
								{
									//nums.Add(6);
									nextAsset = RandomAsset( nums );
								}
								break;
								
							case 8:
								nums.Add(8);
								if( objLeft.transform.localScale.x == -1 )
									nums.Remove(8);
								else
								{
									//nums.Add(9);
									nextAsset = RandomAsset( nums );
								}
								break;
								
							case 9:
								nums.Add(8);
								//nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 10:
								break;
								
							case 11:
								break;
								
							case 12:
								break;
						
							case 13:
								break;
						}
						break;
					
					case 11:
						switch( assetLeft )
						{
							case 0:
								break;
								
							case 1:
								break;
								
							case 2:
								break;
								
							case 3:
								break;
								
							case 4:
								break;
								
							case 5:
								nums.Add(8);
								nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								//nums.Add(6);
								nums.Add(7);
								nextAsset = RandomAsset( nums );
								break;
								
							case 7:
								nums.Add(7);
								if( objLeft.transform.localScale.x == -1 )
									nums.Remove(7);
								else
								{
									//nums.Add(6);
									nextAsset = RandomAsset( nums );
								}
								break;
								
							case 8:
								nums.Add(8);
								if( objLeft.transform.localScale.x == -1 )
									nums.Remove(8);
								else
								{
									//nums.Add(9);
									nextAsset = RandomAsset( nums );
								}
								break;
								
							case 9:
								nums.Add(8);
								//nums.Add(9);
								nextAsset = RandomAsset( nums );
								break;
								
							case 10:
								break;
								
							case 11:
								break;
								
							case 12:
								break;
						
							case 13:
								break;
						}
						break;
					
					case 12:
						switch( assetLeft )
						{
							case 0:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								nextAsset = 2;
								break;
								
							case 2:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(3);
									nums.Add(12);
								}
								else
									nums.Add(2);
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(3);
								nums.Add(5);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								nums.Add(5);
								if( objLeft.transform.localScale.x == 1 )
								{
									nums.Add(3);
									nums.Add(12);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								nextAsset = 5;
								break;
								
							case 7:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								else
								{
									nums.Add(5);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 9:
								nextAsset = 5;
								break;
								
							case 10:
								break;
								
							case 11:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								else
									nums.Add(5);
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(2);
								nums.Add(3);
								nums.Add(5);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
						}
						break;
					
					case 13:
						switch( assetLeft )
						{
							case 0:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 1:
								nextAsset = 2;
								break;
								
							case 2:
								if( objLeft.transform.localScale.x == -1 )
								{
									nums.Add(2);
									nums.Add(3);
									nums.Add(12);
								}
								else
								{
									nums.Add(2);
								}
								nextAsset = RandomAsset( nums );
								break;
								
							case 3:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								nextAsset = RandomAsset( nums );
								break;
								
							case 4:
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 5:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 6:
								break;
								
							case 7:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 8:
								break;
								
							case 9:
								break;
								
							case 10:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 11:
								nums.Add(2);
								nums.Add(3);
								nums.Add(12);
								nextAsset = RandomAsset( nums );
								break;
								
							case 12:
								nums.Add(3);
								nums.Add(12);
								if( objLeft.transform.localScale.x == -1 )
									nums.Add(2);
								nextAsset = RandomAsset( nums );
								break;
						
							case 13:
								nums.Add(1);
								nums.Add(4);
								//nums.Add(13);
								nextAsset = RandomAsset( nums );
								break;
						}
						break;
					
					default:
						break;
				}
				
				if( i == 0 && ( j == 0 || j == -1 || j == 1 ) )
					nextAsset = 0;
				
				if( nextAsset != 0 )
				{
					GameObject instance = Instantiate( assets[ nextAsset ], new Vector3( j, i, 0 ), new Quaternion( 0, 0, 0, 0 ) ) as GameObject;
					
					switch( assetDown )
					{
						case 0:
							switch( assetLeft )
							{
								case 5:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 6:
									if( nextAsset == 7 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 7:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 7 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 8:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 9:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
							
								default:
									break;
							}
							break;
						
						case 1:
							switch( assetLeft )
							{
								case 0:
									RandomFlip( nextAsset, instance );
									break;
									
								case 2:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else
										RandomFlip( nextAsset, instance );
									break;
									
								case 3:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else
										RandomFlip( nextAsset, instance );
									break;
									
								case 6:
									if( nextAsset == 7 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 7:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 7 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else
										RandomFlip( nextAsset, instance );
									break;
									
								case 8:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else
										RandomFlip( nextAsset, instance );
									break;
									
								case 9:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 10:
									RandomFlip( nextAsset, instance );
									break;
									
								case 11:
									RandomFlip( nextAsset, instance );
									break;
									
								case 12:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else
										RandomFlip( nextAsset, instance );
									break;
							
								case 13:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
							
								default:
									break;
							}
							break;
						
						case 2:
							switch( assetLeft )
							{
								case 0:
									if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
							
								case 1:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 2:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 3:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
							
								case 4:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 5:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 6:
									if( nextAsset == 7 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 7:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 7 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 8:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 9:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 10:
									if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 11:
									if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 12:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
							
								case 13:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
							
								default:
									break;
							}
							break;
						
						case 3:
							switch( assetLeft )
							{
								case 1:
									//if( nextAsset == 2 )
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 2:
									if( nextAsset == 2 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 3:
									if( objDown.transform.localScale.x == -1 )
									{
										//if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 4:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 5:
									if( objDown.transform.localScale.x == -1 )
									{
										//if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 12:
									if( objDown.transform.localScale.x == -1 )
									{
										//if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 13:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								default:
									break;
							}
							break;
						
						case 6:
							switch( assetLeft )
							{
								case 0:
									RandomFlip( nextAsset, instance );
									break;
									
								case 3:
									RandomFlip( nextAsset, instance );
									break;
									
								case 6:
									if( nextAsset == 7 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 7:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 7 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else
										RandomFlip( nextAsset, instance );
									break;
									
								case 8:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else
										RandomFlip( nextAsset, instance );
									break;
									
								case 9:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 10:
									RandomFlip( nextAsset, instance );
									break;
									
								case 11:
									RandomFlip( nextAsset, instance );
									break;
							
								default:
									break;
							}
							break;
						
						case 7:
							switch( assetLeft )
							{
								case 0:
									if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 2:
									if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 3:
									if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 5:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 6:
									if( nextAsset == 7 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 7:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 7 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 8:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									else if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 9:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 10:
									if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 11:
									if( objDown.transform.localScale.x == -1 )
									{
										if( nextAsset == 11 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
							
								default:
									break;
							}
							break;
						
						case 8:
							switch( assetLeft )
							{
								case 1:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 2:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 3:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 4:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 5:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 12:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
							
								case 13:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
							
								default:
									break;
							}
							break;
						
						case 10:
							switch( assetLeft )
							{
								case 5:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 6:
									if( nextAsset == 7 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 7:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 7 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 8:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 9:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
							
								default:
									break;
							}
							break;
						
						case 11:
							switch( assetLeft )
							{
								case 5:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 6:
									if( nextAsset == 7 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 7:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 7 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 8:
									if( objLeft.transform.localScale.x == 1 )
									{
										if( nextAsset == 8 )
											instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 9:
									if( nextAsset == 8 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
							
								default:
									break;
							}
							break;
						
						case 12:
							switch( assetLeft )
							{
								case 1:
									//if( nextAsset == 2 )
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 2:
									if( nextAsset == 2 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 3:
									if( objDown.transform.localScale.x == -1 )
									{
										//if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 4:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 5:
									if( objDown.transform.localScale.x == -1 )
									{
										//if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 12:
									if( objDown.transform.localScale.x == -1 )
									{
										//if( nextAsset == 11 )
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 13:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								default:
									break;
							}
							break;
						
						case 13:
							switch( assetLeft )
							{
								case 1:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 2:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 3:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
									
								case 4:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 5:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
									
								case 12:
									if( objLeft.transform.localScale.x == 1 )
									{
										instance.transform.localScale = new Vector3( -1, 1, 1);
									}
									break;
							
								case 13:
									instance.transform.localScale = new Vector3( -1, 1, 1);
									break;
							
								default:
									break;
							}
							break;
						
						default:
							break;
					}
				}*/
			}
		}
		
		current = next;
		last = (int)cam.transform.position.y;
	}
}