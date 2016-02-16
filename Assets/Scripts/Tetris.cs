using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This Script proc gens a tetris board for 1P
 * TODO:
 * We need to modify this script to generate two boards and accept input from 2P 
 * We will also need to attach two cameras to each board.
 * The main methods that need to be modified are GenBoard() and Update()
 * The rest 'should' be ok for now.
 * 
 * Eventually we will change the random shape selection to be a queue but that's down the line
 * */
public class Tetris : MonoBehaviour {
	//Board
	public int[,] board1;
	public int[,] board2;
	//Block
	public Transform block;
	//Spawn Bool
	public bool spawn;
	//Sec before nxt blk spawn
	public float nxtBlkSpawnTime = 0.5f;
	//Block fall speed
	public float blkFallSpeed = 0.5f;
	//Game Over Level
	public int gameOverHeight = 22; // 20 board + 2 edge
	//Current Shape
	private List<Transform> shapes1 = new List<Transform>();
	private List<Transform> shapes2 = new List<Transform>();
	// Is the game over?
	private bool gameOver;
	//Current shape rotation
	private int currentRot = 0;
	//Current pivot of shape
	private GameObject pivot1;
	private GameObject pivot2;
	
	void Start () {
		//Default board is 10x16
		//1+10+1 - Side edge
		//+2 - Space for spawning
		//+1 - Top Edge
		//20 - Height
		//+1 - Down Edge

		board1 = new int[12,24]; // Set board width and height
		board2 = new int[12,24];
		GenBoards ();

		InvokeRepeating ("moveDown", blkFallSpeed, blkFallSpeed); //move blk down
	}

	//Create Game Boards
	//TODO:Modify this function so that two play areas are generated instead of one
	void GenBoards() {
		for (int x=0; x<board1.GetLength(0); x++) {
			for (int y=0; y<board1.GetLength(1);y++) {
				if (x<11 && x>0) {
					if (y>0 && y<board1.GetLength(1)-2) {
						//Board
						board1[x,y] = 0;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = new Vector3 (x,y,1);
						Material material = new Material(Shader.Find("Diffuse"));
						material.color = Color.grey;
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
					} else if (y<board1.GetLength(1)-2) {
						board1[x,y]=1;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = new Vector3(x, y, 0);
						Material material = new Material(Shader.Find ("Diffuse"));
						material.color = Color.black;
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
						cube.GetComponent<Collider>().isTrigger = true;
					}
				} else if ((y<board1.GetLength(1)-2)) {
					// Left and Right edge
					board1[x,y] = 1;
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.position = new Vector3(x, y, 0);
					Material material = new Material(Shader.Find ("Diffuse"));
					material.color = Color.black;
					cube.GetComponent<Renderer>().material = material;
					cube.transform.parent = transform;
				}
			}
		}
		for (int x=0; x<board2.GetLength(0); x++) {
			for (int y=0; y<board2.GetLength(1); y++) {
				if (x < 11 && x > 0) {
					if (y > 0 && y < board2.GetLength (1) - 2) {
						//Board
						board2 [x, y] = 0;
						GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
						cube.transform.position = new Vector3 (x + 20, y, 1);
						Material material = new Material (Shader.Find ("Diffuse"));
						material.color = Color.grey;
						cube.GetComponent<Renderer> ().material = material;
						cube.transform.parent = transform;
					} else if (y < board2.GetLength (1) - 2) {
						board2 [x, y] = 1;
						GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
						cube.transform.position = new Vector3 (x + 20, y, 0);
						Material material = new Material (Shader.Find ("Diffuse"));
						material.color = Color.black;
						cube.GetComponent<Renderer> ().material = material;
						cube.transform.parent = transform;
						cube.GetComponent<Collider> ().isTrigger = true;
					}
				} else if ((y < board2.GetLength (1) - 2)) {
					// Left and Right edge
					board2 [x, y] = 1;
					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.transform.position = new Vector3 (x + 20, y, 0);
					Material material = new Material (Shader.Find ("Diffuse"));
					material.color = Color.black;
					cube.GetComponent<Renderer> ().material = material;
					cube.transform.parent = transform;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// If nothing spawned and game isn't over, then spawn
		// This will spawn for both players at the same time - not desirable will work out logi later
		if (!spawn && !gameOver) {
			StartCoroutine("Wait");
			spawn = true;
			//Reset rotation 
			currentRot = 0;
		}

		//////////////////////////////////////////////////////
		// Begin Player Input Checks
		/////////////////////////////////////////////////////

		//TODO: Modify below to accept input from two players 
		//          -> P1 Controls: <-, ->, space
		//          -> P2 Controls:  A,  D,  S or something, idk

//		//If there is a block
//		if (spawn && shapes.Count == 4) {
//			//Get spawned block pos
//			Vector3 a = shapes[0].transform.position;
//			Vector3 b = shapes[1].transform.position;
//			Vector3 c = shapes[2].transform.position;
//			Vector3 d = shapes[3].transform.position;
//
//			//Move Left
//			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
//				//Can we even move left?
//				if (CheckUserMove(a,b,c,d,true)) {
//					a.x-=1;
//					b.x-=1;
//					c.x-=1;
//					d.x-=1;
//
//					pivot.transform.position = new Vector3(pivot.transform.position.x-1, pivot.transform.position.y, pivot.transform.position.z);
//					
//					shapes[0].transform.position = a;
//					shapes[1].transform.position = b; 
//					shapes[2].transform.position = c; 
//					shapes[3].transform.position = d; 
//				}
//			}
//			//Move Right
//			if (Input.GetKeyDown(KeyCode.RightArrow)) {
//				//Can we even move right?
//				if (CheckUserMove(a,b,c,d,false)) {
//					a.x+=1;
//					b.x+=1;
//					c.x+=1;
//					d.x+=1;
//					
//					pivot.transform.position = new Vector3(pivot.transform.position.x+1, pivot.transform.position.y, pivot.transform.position.z);
//
//					shapes[0].transform.position = a;
//					shapes[1].transform.position = b; 
//					shapes[2].transform.position = c; 
//					shapes[3].transform.position = d; 
//				}
//			}
//			//Drop Piece
//			if (Input.GetKey (KeyCode.DownArrow)) { moveDown(); }
//			//Roatate Piece
//			if(Input.GetKeyDown(KeyCode.Space)){
//				Rotate(shapes[0].transform,shapes[1].transform,shapes[2].transform,shapes[3].transform);	
//			}
//		}
	}

	void SpawnShape(bool isBoard2) {
		int shape = Random.Range (0, 6); //Rand shape
		int height = board1.GetLength (1) - 4;
		int xPos = board1.GetLength (0) / 2 - 1;

		if (isBoard2) {
			//Create pivot
			pivot2 = new GameObject ("RotateAround"); //Pivot of shape
			xPos += 20;
			if (shape == 0) { //S Shape
				pivot2.transform.position = new Vector3 (xPos, height + 1, 0);
				shapes2.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos - 1, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos + 1, height + 1, 0)));
				
				Debug.Log ("Spawned S Shape");
			} else if (shape == 1) { //I Shape
				pivot2.transform.position = new Vector3 (xPos + 0.5f, height + 1.5f, 0);
				shapes2.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 2, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 3, 0)));
				
				Debug.Log ("Spawned I Shape");
			} else if (shape == 2) { //O Shape
				pivot2.transform.position = new Vector3 (xPos + 0.5f, height + 0.5f, 0);
				shapes2.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos + 1, height + 1, 0)));
				
				Debug.Log ("Spawned O Shape");
			} else if (shape == 3) { //J Shape
				pivot2.transform.position = new Vector3 (xPos, height + 2, 0);
				shapes2.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 2, 0)));
				
				Debug.Log ("Spawned J Shape");
			} else if (shape == 4) { //T Shape
				pivot2.transform.position = new Vector3 (xPos, height, 0);
				shapes2.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos - 1, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				
				Debug.Log ("Spawned T Shape");
			} else if (shape == 5) { //L Shape
				pivot2.transform.position = new Vector3 (xPos, height + 1, 0);
				shapes2.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos - 1, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 2, 0)));
				
				Debug.Log ("Spawned L Shape");
			} else { //Z Shape
				pivot2.transform.position = new Vector3 (xPos, height + 1, 0);
				shapes2.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes2.Add (GenBlock (new Vector3 (xPos - 1, height + 1, 0)));
				
				Debug.Log ("Spawned Z Shape");
			} 
		} else {
			//Pivot of shape
			pivot1 = new GameObject ("RotateAround"); 
			if (shape == 0) { //S Shape
				pivot1.transform.position = new Vector3 (xPos, height + 1, 0);
				shapes1.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos - 1, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos + 1, height + 1, 0)));

				Debug.Log ("Spawned S Shape");
			} else if (shape == 1) { //I Shape
				pivot1.transform.position = new Vector3 (xPos + 0.5f, height + 1.5f, 0);
				shapes1.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 2, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 3, 0)));
			
				Debug.Log ("Spawned I Shape");
			} else if (shape == 2) { //O Shape
				pivot1.transform.position = new Vector3 (xPos + 0.5f, height + 0.5f, 0);
				shapes1.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos + 1, height + 1, 0)));
			
				Debug.Log ("Spawned O Shape");
			} else if (shape == 3) { //J Shape
				pivot1.transform.position = new Vector3 (xPos, height + 2, 0);
				shapes1.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 2, 0)));
			
				Debug.Log ("Spawned J Shape");
			} else if (shape == 4) { //T Shape
				pivot1.transform.position = new Vector3 (xPos, height, 0);
				shapes1.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos - 1, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
			
				Debug.Log ("Spawned T Shape");
			} else if (shape == 5) { //L Shape
				pivot1.transform.position = new Vector3 (xPos, height + 1, 0);
				shapes1.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos - 1, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 2, 0)));
			
				Debug.Log ("Spawned L Shape");
			} else { //Z Shape
				pivot1.transform.position = new Vector3 (xPos, height + 1, 0);
				shapes1.Add (GenBlock (new Vector3 (xPos, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos + 1, height, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos, height + 1, 0)));
				shapes1.Add (GenBlock (new Vector3 (xPos - 1, height + 1, 0)));
			
				Debug.Log ("Spawned Z Shape");
			} 
		}
	}

	//Create block at position
	Transform GenBlock(Vector3 pos) {
		Transform obj = (Transform)Instantiate (block.transform, pos, Quaternion.identity) as Transform;
		obj.tag = "Block";

		return obj;
	}

	bool CheckMove(Vector3 a, Vector3 b, Vector3 c, Vector3 d, bool isBoard2) {
		//Check if we move a block will it hit something
		if (isBoard2) {
			if (board2 [Mathf.RoundToInt (a.x-20), Mathf.RoundToInt (a.y - 1)] == 1) 
				return false;
			if (board2 [Mathf.RoundToInt (b.x-20), Mathf.RoundToInt (b.y - 1)] == 1) 
				return false;
			if (board2 [Mathf.RoundToInt (c.x-20), Mathf.RoundToInt (c.y - 1)] == 1) 
				return false;
			if (board2 [Mathf.RoundToInt (d.x-20), Mathf.RoundToInt (d.y - 1)] == 1) 
				return false;
		} else {
			if (board1 [Mathf.RoundToInt (a.x), Mathf.RoundToInt (a.y - 1)] == 1) 
				return false;
			if (board1 [Mathf.RoundToInt (b.x), Mathf.RoundToInt (b.y - 1)] == 1) 
				return false;
			if (board1 [Mathf.RoundToInt (c.x), Mathf.RoundToInt (c.y - 1)] == 1) 
				return false;
			if (board1 [Mathf.RoundToInt (d.x), Mathf.RoundToInt (d.y - 1)] == 1) 
				return false;
		}
		return true;
	}

	void moveDown(){
		//Spawned blocks position 
		if (shapes1.Count != 4 && shapes2.Count != 4)
			return;

		Vector3 a = shapes1 [0].transform.position;
		Vector3 b = shapes1 [1].transform.position;
		Vector3 c = shapes1 [2].transform.position;
		Vector3 d = shapes1 [3].transform.position;

		Vector3 e = shapes2 [0].transform.position;
		Vector3 f = shapes2 [1].transform.position;
		Vector3 g = shapes2 [2].transform.position;
		Vector3 h = shapes2 [3].transform.position;

		//Will we hit anything if we move blck
		if (CheckMove (a, b, c, d, false) == true) {
			//Move block down 1
			a = new Vector3 (Mathf.RoundToInt (a.x), Mathf.RoundToInt (a.y - 1.0f), a.z);
			b = new Vector3 (Mathf.RoundToInt (b.x), Mathf.RoundToInt (b.y - 1.0f), b.z);
			c = new Vector3 (Mathf.RoundToInt (c.x), Mathf.RoundToInt (c.y - 1.0f), c.z);
			d = new Vector3 (Mathf.RoundToInt (d.x), Mathf.RoundToInt (d.y - 1.0f), d.z);

			pivot1.transform.position = new Vector3 (pivot1.transform.position.x, pivot1.transform.position.y - 1, pivot1.transform.position.z);

			shapes1 [0].transform.position = a;
			shapes1 [1].transform.position = b; 
			shapes1 [2].transform.position = c; 
			shapes1 [3].transform.position = d; 
		} else {
			Destroy (pivot1.gameObject);

			board1[Mathf.RoundToInt(a.x),Mathf.RoundToInt(a.y)]=1;
			board1[Mathf.RoundToInt(b.x),Mathf.RoundToInt(b.y)]=1;
			board1[Mathf.RoundToInt(c.x),Mathf.RoundToInt(c.y)]=1;
			board1[Mathf.RoundToInt(d.x),Mathf.RoundToInt(d.y)]=1;
			
			//****************************************************
			checkRow(1, false); //Check for any match
			checkRow(gameOverHeight, false); //Check for game over
			//****************************************************
		}

		if (CheckMove (e, f, g, h, true) == true) {
			e = new Vector3 (Mathf.RoundToInt (e.x), Mathf.RoundToInt (e.y - 1.0f), e.z);
			f = new Vector3 (Mathf.RoundToInt (f.x), Mathf.RoundToInt (f.y - 1.0f), f.z);
			g = new Vector3 (Mathf.RoundToInt (g.x), Mathf.RoundToInt (g.y - 1.0f), g.z);
			h = new Vector3 (Mathf.RoundToInt (h.x), Mathf.RoundToInt (h.y - 1.0f), h.z);

			pivot2.transform.position = new Vector3 (pivot2.transform.position.x, pivot2.transform.position.y - 1, pivot2.transform.position.z);
			
			shapes2 [0].transform.position = e;
			shapes2 [1].transform.position = f; 
			shapes2 [2].transform.position = g; 
			shapes2 [3].transform.position = h; 
		} else {
			// We hit something, stop and mark loc and Destroy pivot
			Destroy(pivot2.gameObject); //Destroy pivot
			
			//Set ID in board
			board2[Mathf.RoundToInt(e.x-20),Mathf.RoundToInt(e.y)]=1;
			board2[Mathf.RoundToInt(f.x-20),Mathf.RoundToInt(f.y)]=1;
			board2[Mathf.RoundToInt(g.x-20),Mathf.RoundToInt(g.y)]=1;
			board2[Mathf.RoundToInt(h.x-20),Mathf.RoundToInt(h.y)]=1;

			//****************************************************
			checkRow(1, true); //Check for any match
			checkRow(gameOverHeight, true); //Check for game over
			//****************************************************
		}

		//Are we ready to spawn a new block yet?
		if (pivot1.gameObject == null && pivot2.gameObject == null) { 
			//Clear spawned blocks from array
			shapes1.Clear ();
			shapes2.Clear ();
			//Spawn a new block
			spawn = false; 
		}
	}

	//Check specific row for match
	void checkRow(int y, bool isBoard2){
		//All blocks in the scene
		GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
		//Blocks found in a row
		int count = 0; 

		if (isBoard2) {
			//Go through each block on this height
			for (int x=1; x<board2.GetLength(0)-1; x++) {
				//If there is any block at this position
				if (board2 [x, y] == 1) {
					//We found +1 block
					count++;
				}
			}
			//If the current height is game over height, and there is more than 0 block, then game over
			if (y == gameOverHeight && count > 0) {
				Debug.LogWarning ("Game over");
				gameOver = true;
			}
			//The row is full
			if (count == 10) {
				//Start from bottom of the board(withouth edge and block spawn space)
				for (int cy=y; cy<board2.GetLength(1)-3; cy++) {
					for (int cx=1; cx<board2.GetLength(0)-1; cx++) {
						foreach (GameObject go in blocks) {
							int height = Mathf.RoundToInt (go.transform.position.y);
							int xPos = Mathf.RoundToInt (go.transform.position.x);
							
							if (xPos == cx && height == cy) {
								//The row we need to destroy
								if (height == y) {
									//Set empty space
									board2 [xPos, height] = 0;
									Destroy (go.gameObject);
								} else if (height > y) {
									board2 [xPos, height] = 0;   //Set old position to empty
									board2 [xPos, height - 1] = 1; //Set new position 
									go.transform.position = new Vector3 (xPos, height - 1, go.transform.position.z);//Move block down
								}
							}
						}
					}
				}
				checkRow (y, true); //We moved blocks down, check again this row
			} else if (y + 1 < board2.GetLength (1) - 3) {
				//Check row above this
				checkRow (y + 1, true); 
			}
		} else {
			//Go through each block on this height
			for (int x=1; x<board1.GetLength(0)-1; x++) {
				//If there is any block at this position
				if (board1 [x, y] == 1) {
					//We found +1 block
					count++;
				}
			}
			//If the current height is game over height, and there is more than 0 block, then game over
			if (y == gameOverHeight && count > 0) {
				Debug.LogWarning ("Game over");
				gameOver = true;
			}
			//The row is full
			if (count == 10) {
				//Start from bottom of the board(withouth edge and block spawn space)
				for (int cy=y; cy<board1.GetLength(1)-3; cy++) {
					for (int cx=1; cx<board1.GetLength(0)-1; cx++) {
						foreach (GameObject go in blocks) {
							int height = Mathf.RoundToInt (go.transform.position.y);
							int xPos = Mathf.RoundToInt (go.transform.position.x);
						
							if (xPos == cx && height == cy) {
								//The row we need to destroy
								if (height == y) {
									//Set empty space
									board1 [xPos, height] = 0;
									Destroy (go.gameObject);
								} else if (height > y) {
									board1 [xPos, height] = 0;   //Set old position to empty
									board1 [xPos, height - 1] = 1; //Set new position 
									go.transform.position = new Vector3 (xPos, height - 1, go.transform.position.z);//Move block down
								}
							}
						}
					}
				}
				checkRow (y, false); //We moved blocks down, check again this row
			} else if (y + 1 < board1.GetLength (1) - 3) {
				//Check row above this
				checkRow (y + 1, false); 
			}
		}
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds(nxtBlkSpawnTime);
		SpawnShape (true);
		SpawnShape (false);
	}

//	///////////////////////////////////////
//	/// Player Control Functions
//	///////////////////////////////////////
//
//	void Rotate(Transform a, Transform b, Transform c, Transform d){
//		//Set parent to pivot so we can rotate
//		a.parent = pivot.transform;
//		b.parent = pivot.transform;
//		c.parent = pivot.transform;
//		d.parent = pivot.transform;
//		
//		currentRot +=90;    //Add rotation
//		if(currentRot==360) //Reset rotation
//			currentRot = 0;
//		
//		pivot.transform.localEulerAngles = new Vector3(0,0,currentRot);
//		
//		a.parent = null;
//		b.parent = null;
//		c.parent = null;
//		d.parent = null;
//		
//		if(CheckRotate(a.position,b.position,c.position,d.position) == false){
//			//Set parent to pivot so we can rotate
//			a.parent = pivot.transform;
//			b.parent = pivot.transform;
//			c.parent = pivot.transform;
//			d.parent = pivot.transform;
//			
//			currentRot-=90;
//			pivot.transform.localEulerAngles = new Vector3(0,0,currentRot);
//			
//			a.parent = null;
//			b.parent = null;
//			c.parent = null;
//			d.parent = null;
//		}
//	} 
//
//	bool CheckUserMove(Vector3 a, Vector3 b, Vector3 c, Vector3 d, bool dir) {
//		//Will Player movement cause collision?
//		if (dir) { //Left
//			if (board[Mathf.RoundToInt(a.x-1),Mathf.RoundToInt(a.y)]==1 || board[Mathf.RoundToInt(b.x-1),Mathf.RoundToInt(b.y)]==1 || board[Mathf.RoundToInt(c.x-1),Mathf.RoundToInt(c.y)]==1 || board[Mathf.RoundToInt(d.x-1),Mathf.RoundToInt(d.y)]==1){
//				return false;
//			}
//		} else { //Right
//			if(board[Mathf.RoundToInt(a.x+1),Mathf.RoundToInt(a.y)]==1 || board[Mathf.RoundToInt(b.x+1),Mathf.RoundToInt(b.y)]==1 || board[Mathf.RoundToInt(c.x+1),Mathf.RoundToInt(c.y)]==1 || board[Mathf.RoundToInt(d.x+1),Mathf.RoundToInt(d.y)]==1){
//				return false;
//			}
//		}
//		return true;
//	}
//	
//	bool CheckRotate(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int[,] board){
//		if(Mathf.RoundToInt(a.x)<board.GetLength(0)-1){//Check if block is in board
//			if(board[Mathf.RoundToInt(a.x),Mathf.RoundToInt(a.y)]==1){
//				//If rotated block hit any other block or edge, after rotation
//				return false; //Rotate in default position - previous
//			}
//		} else {//If the block is not in the board
//			return false;//Do not rotate
//		}
//
//		if(Mathf.RoundToInt(b.x)<board.GetLength(0)-1){
//			if(board[Mathf.RoundToInt(b.x),Mathf.RoundToInt(b.y)]==1)
//				return false; 
//		} else { return false; }
//
//		if(Mathf.RoundToInt(c.x)<board.GetLength(0)-1){
//			if(board[Mathf.RoundToInt(c.x),Mathf.RoundToInt(c.y)]==1)
//				return false; 
//		} else { return false; }
//
//		if(Mathf.RoundToInt(d.x)<board.GetLength(0)-1){
//			if(board[Mathf.RoundToInt(d.x),Mathf.RoundToInt(d.y)]==1)
//				return false;
//		} else { return false; }
//
//		//We can rotate
//		return true; 
//	}
}