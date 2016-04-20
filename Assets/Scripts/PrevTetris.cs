using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures;

public class PrevTetris : MonoBehaviour
{
    //Board
    public int[,] board;
    private int curBottom;
    //Block
    public Transform block;
    //Spawn Bool
    public bool spawn, spawning;
    //Sec before nxt blk spawn
    public static float NXTBLKSPAWNTIME = 0.5f;
    public float nxtBlkSpawnTime = NXTBLKSPAWNTIME;
    private bool curWaiting = false;
    private bool movingDown = false;
    private int  cyclesEmpty = 0;
    public  int  MAX_ALLOWED_CYCLES_EMPTY = 8;
    //Block fall speed
    public static float BLKFALLSPEED = 0.5f;
    public float blkFallSpeed = BLKFALLSPEED;
    //Game Over Level
    public int gameOverHeight = 22;
    // 20 board + 2 edge
    //Queue
    public Queue<int> shapeQueue = new Queue<int>();
	public int[,] qPreview;
	int qPrev;
    //Current Shape
    private List<Transform> shapes = new List<Transform>();
	//Current Shape in Queue
	private List<Transform> qShapes = new List<Transform> ();

	private List<GameObject> queueShapes = new List<GameObject>();

    // Camera attached to this board
    private Camera myCam;
    private bool notDragged;

    private static bool gameOver;
    //Current shape rotation
    private int currentRot = 0;
    //Current pivot of shape
    private GameObject pivot;

    Material IMat;
    Material JMat;
    Material LMat;
    Material OMat;
    Material SMat;
    Material TMat;
    Material ZMat;
    bool lsd;

    // Previous mouse position
    private Vector3 previousPosition;

    // Controls
    public KeyCode rot, down, left, right;

	//slows down tetris game for 5 seconds
	public void slowDown() 
	{
		blkFallSpeed = 1.0f;
		Invoke ("resetSpeed", 5.0f);
	}

	private void resetSpeed()
	{
		blkFallSpeed = BLKFALLSPEED;
	}

	public void speedUp() {
		blkFallSpeed = 0.25f;
		Invoke ("resetSpeed", 5.0f);
	}
    private int score;
    public Text UI_Score;

    void Start()
    {
        IMat = (Material)Resources.Load("ICube");
        JMat = (Material)Resources.Load("JCube");
        LMat = (Material)Resources.Load("LCube");
        OMat = (Material)Resources.Load("OCube");
        SMat = (Material)Resources.Load("SCube");
        TMat = (Material)Resources.Load("TCube");
        ZMat = (Material)Resources.Load("ZCube");
        lsd = false;

        if (!lsd)
            SetMats();
        
        //Default board is 10x16
        //1+10+1 - Side edge
        //+2 - Space for spawning
        //+1 - Top Edge
        //20 - Height
        //+1 - Down Edge
        score = 0;
        gameOver = false;
        myCam = transform.GetChild(0).gameObject.GetComponent<Camera>();
        board = new int[12, 24]; // Set board width and height
        GenBoard();
        curBottom = 1;

		qPreview  = new int[7, 9]; // Set queue width and height
		GenQueue ();
		
		spawning = false;
        shapeQueue.Enqueue(Random.Range(0, 6));

        //InvokeRepeating("MoveDown", blkFallSpeed, blkFallSpeed); //move blk down
    }

    // Update is called once per frame
    void Update()
    {
        if (lsd)
            FWithMats();
		//Fix spawn freezing when queue becomes empty
		if (shapes.Count == 0 && shapeQueue.Count == 0)
		{ 
            spawn = false; 
        }
        if (curBottom == gameOverHeight)
        {
            Debug.LogWarning("Game over");
            gameOver = true;
        }
        // If nothing spawned and game isn't over, then spawn
        if (!spawning && !spawn && !gameOver && !curWaiting)
        {
            curWaiting = true;
            spawning = true;
            StartCoroutine("Wait");
            //Reset rotation 
            currentRot = 0;
        }
        if (!movingDown)
        {
            movingDown = true;
            StartCoroutine("WaitMoveDown");
        }
        if (gameOver)
        {
            Time.timeScale = 0;
        }
        //////////////////////////////////////////////////////
        // Begin Player Input Checks
        /////////////////////////////////////////////////////

        //If there is a block
        if (spawn && shapes.Count > 0 && !gameOver)
        {
            //Move Left
            if (Input.GetKeyDown(left))
            {
                //Can we even move left?
                if (CheckUserMove(-1f))
                {
                    pivot.transform.position = new Vector3(pivot.transform.position.x - 1, 
                                                           pivot.transform.position.y, 
                                                           pivot.transform.position.z);

                    for (int i = 0; i < shapes.Count; i++)
                    {
                        shapes[i].position = new Vector3(shapes[i].position.x - 1, 
                                                         shapes[i].position.y, 
                                                         shapes[i].position.z);
                    }
                }
            }
            //Move Right
            if (Input.GetKeyDown(right))
            {
                //Can we even move right?
                if (CheckUserMove(1f))
                {
                    pivot.transform.position = new Vector3(pivot.transform.position.x + 1, 
                                                           pivot.transform.position.y, 
                                                           pivot.transform.position.z);

                    for (int i = 0; i < shapes.Count; i++)
                    {
                        shapes[i].position = new Vector3(shapes[i].position.x + 1, 
                                                         shapes[i].position.y, 
                                                         shapes[i].position.z);
                    }
                }
            }
            //Drop Piece
            if (Input.GetKey(down))
            {
                MoveDown();
            }
            //Roatate Piece
            if (Input.GetKeyDown(rot))
            {
                Rotate();	
            }
        }
    }

    void GenBoard()
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (x < 11 && x > 0)
                {
                    if (y > 0 && y < board.GetLength(1) - 2)
                    {
                        //Board
                        board[x, y] = 0;
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 1);
                        Material material = new Material(Shader.Find("Diffuse"));
                        material.color = Color.grey;
                        cube.GetComponent<Renderer>().material = material;
                        cube.transform.parent = transform;
                    }
                    else if (y < board.GetLength(1) - 2)
                    {
                        board[x, y] = 1;
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 0);
                        Material material = new Material(Shader.Find("Diffuse"));
                        material.color = Color.black;
                        cube.GetComponent<Renderer>().material = material;
                        cube.transform.parent = transform;
                        cube.GetComponent<Collider>().isTrigger = true;
                    }
                }
                else if ((y < board.GetLength(1) - 2))
                {
                    // Left and Right edge
                    board[x, y] = 1;
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 0);
                    Material material = new Material(Shader.Find("Diffuse"));
                    material.color = Color.black;
                    cube.GetComponent<Renderer>().material = material;
                    cube.transform.parent = transform;
                }
            }
        }
    }

	void GenQueue()
	{
		for (int x = 0; x < qPreview.GetLength(0); x++)
		{
			for (int y = 0; y < qPreview.GetLength(1); y++)
			{
				if (x < 6 && x > 0)
				{
					if (y > 0 && y < qPreview.GetLength(1) - 2)
					{
						//Back
						qPreview [x, y] = 0;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = new Vector3(x + 14 + transform.position.x, y + 15 + transform.position.y, 1);
						Material material = new Material(Shader.Find("Diffuse"));
						material.color = Color.grey;
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
					}
					else if (y < qPreview.GetLength(1) - 2 || y == qPreview.GetLength(1) - 2) 
					{
						qPreview [x, y] = 1;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = new Vector3(x + 14 + transform.position.x, y + 15 + transform.position.y, 0);
						Material material = new Material(Shader.Find("Diffuse"));
						material.color = Color.black;
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
						cube.GetComponent<Collider>().isTrigger = true;
					}
				}
				else if ((y < qPreview.GetLength(1) - 1))
				{
					// Left and Right edge
					qPreview [x, y] = 1;
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.position = new Vector3(x +  14 + transform.position.x, y + 15 + transform.position.y, 0);
					Material material = new Material(Shader.Find("Diffuse"));
					material.color = Color.black;
					cube.GetComponent<Renderer>().material = material;
					cube.transform.parent = transform;
				}
			}
		}
	}

    void GenBottomAndMoveShapesUp()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block"); //All blocks in the scene
        for (int y = board.GetLength(1) - 2; y >= curBottom; y--)
        {
            for (int x = 1; x < board.GetLength(0) - 1; x++)
            {
                board[x, y + 1] = board[x, y];
            }
        }
        foreach (GameObject go in blocks)
        {
            int blkX = Mathf.RoundToInt(go.transform.position.x - transform.position.x);
            int blkY = Mathf.RoundToInt(go.transform.position.y - transform.position.y);
            if (blkX > 0 && blkX < board.GetLength(0) - 1 &&
                blkY > 0 && blkY < board.GetLength(1) - 2)
                go.transform.position = new Vector3(go.transform.position.x, 
                                                    go.transform.position.y + 1, 
                                                    go.transform.position.z);
        }
        for (int x = 1; x < board.GetLength(0) - 1; x++)
        {
            board[x, curBottom] = 1;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(x + transform.position.x, curBottom + transform.position.y, 0);
            Material material = new Material(Shader.Find("Diffuse"));
            material.color = Color.black;
            cube.GetComponent<Renderer>().material = material;
            cube.transform.parent = transform;
            cube.GetComponent<Collider>().isTrigger = true;
        }
        curBottom++;
        CheckRow(gameOverHeight); //Check for game over
    }

    void SpawnShape()
    {
        if (shapeQueue.Count > 0)
        {
            cyclesEmpty = 0;
            int shape = shapeQueue.Dequeue();

            // Refresh preview block
			destroyQueue();
			
            int height = (int)transform.position.y + board.GetLength(1) - 4;
            int xPos = (int)transform.position.x + board.GetLength(0) / 2 - 1;
            //Create pivot
            pivot = new GameObject("RotateAround"); //Pivot of shape
            List<Vector3> cubePosList = new List<Vector3>();
            Material mat;

            if (shape == 0)
            { //S Shape
                cubePosList.Add(new Vector3(xPos, height, 0));
                cubePosList.Add(new Vector3(xPos - 1, height, 0));
                cubePosList.Add(new Vector3(xPos, height + 1, 0));
                cubePosList.Add(new Vector3(xPos + 1, height + 1, 0));

                SetCubePositions(new Vector3(xPos, height + 1, 0), cubePosList, SMat);
            }
            else if (shape == 1)
            { //I Shape
                cubePosList.Add(new Vector3(xPos, height, 0));
                cubePosList.Add(new Vector3(xPos, height + 1, 0));
                cubePosList.Add(new Vector3(xPos, height + 2, 0));
                cubePosList.Add(new Vector3(xPos, height + 3, 0));

                SetCubePositions(new Vector3(xPos + 0.5f, height + 1.5f, 0), cubePosList, IMat);
            }
            else if (shape == 2)
            { //O Shape
                cubePosList.Add(new Vector3(xPos, height, 0));
                cubePosList.Add(new Vector3(xPos + 1, height, 0));
                cubePosList.Add(new Vector3(xPos, height + 1, 0));
                cubePosList.Add(new Vector3(xPos + 1, height + 1, 0));

                SetCubePositions(new Vector3(xPos + 0.5f, height + 0.5f, 0), cubePosList, OMat);
            }
            else if (shape == 3)
            { //J Shape
                cubePosList.Add(new Vector3(xPos, height, 0));
                cubePosList.Add(new Vector3(xPos + 1, height, 0));
                cubePosList.Add(new Vector3(xPos, height + 1, 0));
                cubePosList.Add(new Vector3(xPos, height + 2, 0));

                SetCubePositions(new Vector3(xPos, height + 1, 0), cubePosList, JMat);
            }
            else if (shape == 4)
            { //T Shape
                cubePosList.Add(new Vector3(xPos, height, 0));
                cubePosList.Add(new Vector3(xPos - 1, height, 0));
                cubePosList.Add(new Vector3(xPos + 1, height, 0));
                cubePosList.Add(new Vector3(xPos, height + 1, 0));

                SetCubePositions(new Vector3(xPos, height, 0), cubePosList, TMat);
            }
            else if (shape == 5)
            { //L Shape
                cubePosList.Add(new Vector3(xPos, height, 0));
                cubePosList.Add(new Vector3(xPos - 1, height, 0));
                cubePosList.Add(new Vector3(xPos, height + 1, 0));
                cubePosList.Add(new Vector3(xPos, height + 2, 0));

                SetCubePositions(new Vector3(xPos, height + 1, 0), cubePosList, LMat);
            }
            else if (shape == 6)
            { //Z Shape
                cubePosList.Add(new Vector3(xPos, height, 0));
                cubePosList.Add(new Vector3(xPos + 1, height, 0));
                cubePosList.Add(new Vector3(xPos, height + 1, 0));
                cubePosList.Add(new Vector3(xPos - 1, height + 1, 0));

                SetCubePositions(new Vector3(xPos, height + 1, 0), cubePosList, ZMat);
            }
            else
            {
                Debug.Log("Illegal shape code: " + shape);
            }
        }
        else
        {
            cyclesEmpty++;
            if (cyclesEmpty > MAX_ALLOWED_CYCLES_EMPTY)
            {
                cyclesEmpty = 0;
                GenBottomAndMoveShapesUp();
            }
        }
    }

	void populateQueue ()
    {
 		//Creates shape in Queue
 		int Qheight = (int)transform.position.y + qPreview.GetLength (1) + 9;
 		int QxPos = (int)transform.position.x + 15 + qPreview.GetLength (0) / 2 - 1;

 		qPrev = shapeQueue.Peek ();
 
 		List<Vector3> cubePosList = new List<Vector3> ();
        Material mat;
 
 		if (qPrev == 0) { //S Shape
 			cubePosList.Add (new Vector3 (QxPos, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos - 1, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 1, 0));
 			cubePosList.Add (new Vector3 (QxPos + 1, Qheight + 1, 0));
 
 			SetQueuePositions(cubePosList, SMat);
 		} else if (qPrev == 1) { //I Shape
 			cubePosList.Add (new Vector3 (QxPos, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 1, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 2, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 3, 0));
 
 			SetQueuePositions(cubePosList, IMat);
 		} else if (qPrev == 2) { //O Shape
 			cubePosList.Add (new Vector3 (QxPos, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos + 1, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 1, 0));
 			cubePosList.Add (new Vector3 (QxPos + 1, Qheight + 1, 0));
 
 			SetQueuePositions(cubePosList, OMat);
 		} else if (qPrev == 3) { //J Shape
 			cubePosList.Add (new Vector3 (QxPos, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos + 1, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 1, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 2, 0));
 
 			SetQueuePositions(cubePosList, JMat);
 		} else if (qPrev == 4) { //T Shape
 			cubePosList.Add (new Vector3 (QxPos, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos - 1, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos + 1, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 1, 0));
 
 			SetQueuePositions(cubePosList, TMat);
 		} else if (qPrev == 5) { //L Shape
 			cubePosList.Add (new Vector3 (QxPos, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos - 1, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 1, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 2, 0));
 
 			SetQueuePositions(cubePosList, LMat);
 		} else if (qPrev == 6) { //Z Shape
 			cubePosList.Add (new Vector3 (QxPos, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos + 1, Qheight, 0));
 			cubePosList.Add (new Vector3 (QxPos, Qheight + 1, 0));
 			cubePosList.Add (new Vector3 (QxPos - 1, Qheight + 1, 0));
 
 			SetQueuePositions(cubePosList, ZMat);
 		} else {
 			Debug.Log ("Illegal shape code: " + qPrev);
 		}
 	}
 			
    // Creates a pivot and individual blocks to form a tetris shape
    // List format allows for any number of blocks to be spawned
    void SetCubePositions(Vector3 piv, List<Vector3> cubePosList, Material mat)
    {
        pivot.transform.position = piv;
        for (int i = 0; i < cubePosList.Count; i++)
            shapes.Add(GenBlock(cubePosList[i], mat));
    }

    void SetQueuePositions(List<Vector3> cubePosList, Material mat)
	{
		for (int i = 0; i < cubePosList.Count; i++)
 			qShapes.Add(GenBlock(cubePosList[i], mat));
	}

	void textureShape()
	{
		int qShape = qPrev;

		int Qheight = (int)transform.position.y + qPreview.GetLength (1) + 9;
		int QxPos = (int)transform.position.x + 15 + qPreview.GetLength (0) / 2 - 1;

		if ( qShape == 0) {
			queueShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("S"),
				new Vector3(QxPos, Qheight, 0),
				Quaternion.identity));
		} else if (qShape == 1) {
			queueShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("I"),
				new Vector3(QxPos, Qheight, 0),
				Quaternion.identity));
		} else if (qShape == 2) {
			queueShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("O"),
				new Vector3(QxPos, Qheight, 0),
				Quaternion.identity));
		} else if (qShape == 3) {
			queueShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("J"),
				new Vector3(QxPos, Qheight, 0),
				Quaternion.identity));
		} else if (qShape == 4) {
			queueShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("T"),
				new Vector3(QxPos, Qheight, 0),
				Quaternion.identity));
		} else if (qShape == 5) {
			queueShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("L"),
				new Vector3(QxPos, Qheight, 0),
				Quaternion.identity));
		} else if (qShape == 6) {
			queueShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("Z"),
				new Vector3(QxPos, Qheight, 0),
				Quaternion.identity));
		}	
	}

    //Create block at position
    Transform GenBlock(Vector3 pos, Material mat)
    {
        Transform obj = (Transform)Instantiate(block.transform, pos, Quaternion.identity) as Transform;
        obj.tag = "Block";
        obj.GetComponent<Renderer>().material = mat;

        return obj;
    }

    IEnumerator WaitMoveDown()
    {
        // NOTE: shared BLKFALLSPEED that is affected by game timer
        //       individual blkFallSpeed based on BLKFALLSPEED that can be affected by powerups

        //if (!spedUp)
		yield return new WaitForSeconds(blkFallSpeed);
        //else
        //yield return new WaitForSeconds(blkFallSpeed);

        MoveDown();
        movingDown = false;
    }

    void SetMats()
    {
        IMat.mainTextureOffset = Vector2.zero;
        JMat.mainTextureOffset = Vector2.zero;
        LMat.mainTextureOffset = Vector2.zero;
        OMat.mainTextureOffset = Vector2.zero;
        SMat.mainTextureOffset = Vector2.zero;
        TMat.mainTextureOffset = Vector2.zero;
        ZMat.mainTextureOffset = Vector2.zero;
    }
    void FWithMats()
    {
        IMat.mainTextureOffset += new Vector2(0.2f, 0.2f) * Time.deltaTime;
        JMat.mainTextureOffset += new Vector2(0.2f, 0.2f) * Time.deltaTime;
        LMat.mainTextureOffset += new Vector2(0.2f, 0.2f) * Time.deltaTime;
        OMat.mainTextureOffset += new Vector2(0.2f, 0.2f) * Time.deltaTime;
        SMat.mainTextureOffset += new Vector2(0.2f, 0.2f) * Time.deltaTime;
        TMat.mainTextureOffset += new Vector2(0.2f, 0.2f) * Time.deltaTime;
        ZMat.mainTextureOffset += new Vector2(0.2f, 0.2f) * Time.deltaTime;
    }

    void MoveDown()
    {
        //Spawned blocks position 
        if (shapes.Count <= 0)
            return;

        //Will we hit anything if we move blck
        if (CheckMove() == true)
        {
            //Move block down 1
            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].position = new Vector3(shapes[i].position.x, 
                                                 Mathf.RoundToInt(shapes[i].position.y - 1.0f), 
                                                 shapes[i].position.z);
            }

            pivot.transform.position = new Vector3(pivot.transform.position.x, 
                                                   pivot.transform.position.y - 1, 
                                                   pivot.transform.position.z);
        }
        else
        {
            // We hit something, stop and mark loc and Destroy pivot
            Destroy(pivot.gameObject); //Destroy pivot

            //Set ID in board
            for (int i = 0; i < shapes.Count; i++)
            {
                board[Mathf.RoundToInt(shapes[i].position.x - transform.position.x), 
                      Mathf.RoundToInt(shapes[i].position.y - transform.position.y)] = 1;
            }

            //****************************************************
            CheckRow(curBottom); //Check for any match
            CheckRow(gameOverHeight); //Check for game over
            //****************************************************

            shapes.Clear(); //Clear spawned blocks from array
            spawn = false; //Spawn a new block
        }
    }

    bool CheckMove()
    {
        bool hit = false;
        for (int i = 0; i < shapes.Count; i++)
        {
            if (board[Mathf.RoundToInt(shapes[i].position.x - transform.position.x), 
                      Mathf.RoundToInt(shapes[i].position.y - 1 - transform.position.y)] == 1)
            {
                hit = true;
                break;
            }
        }

        //Check if we move a block will it hit something
        if (hit)
        {
            // Snap to grid on contact
            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].position = new Vector3(Mathf.Round(shapes[i].position.x), shapes[i].position.y, shapes[i].position.z);
            }

            return false;
        }

        return true;
    }

    //Check specific row for match
    void CheckRow(int y)
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block"); //All blocks in the scene
        int count = 0; //Blocks found in a row

        for (int x = 1; x < board.GetLength(0) - 1; x++)
        {//Go through each block on this height
            if (board[x, y] == 1)
            {//If there is any block at this position
                count++;//We found +1 block
            }
        }

        if (y == gameOverHeight && count > 0)
        {//If the current height is game over height, and there is more than 0 block, then game over
            Debug.LogWarning("Game over");
            gameOver = true;
        }
        if (count == 10)
        {//The row is full
            //Start from bottom of the board(withouth edge and block spawn space)
            score += 100;
            UI_Score.text = score + "";
            for (int cy = y; cy < board.GetLength(1) - 3; cy++)
            {
                for (int cx = 1; cx < board.GetLength(0) - 1; cx++)
                {
                    foreach (GameObject go in blocks)
                    {
                        int height = Mathf.RoundToInt(go.transform.position.y - transform.position.y);
                        int xPos = Mathf.RoundToInt(go.transform.position.x - transform.position.x);

                        if (xPos == cx && height == cy)
                        {
                            if (height == y)
                            {//The row we need to destroy
                                board[xPos, height] = 0;//Set empty space
                                Destroy(go.gameObject);
                            }
                            else if (height > y)
                            {
                                board[xPos, height] = 0;//Set old position to empty
                                board[xPos, height - 1] = 1;//Set new position 
                                //Move block down
                                go.transform.position = new Vector3(xPos + transform.position.x, 
                                                                    height - 1 + transform.position.y, 
                                                                    go.transform.position.z);
                            }
                        }
                    }
                }
            }
            CheckRow(y); //We moved blocks down, check again this row
        }
        else if (y + 1 < board.GetLength(1) - 3)
        {
            CheckRow(y + 1); //Check row above this
        }
    }

	public void AddToQueue(int shape)
    {
		shapeQueue.Enqueue(shape);
        destroyQueue();
	}
	
	void destroyQueue()
	{
		int length = qShapes.Count;
		int i;

		for (i = 0; i < length; i++) {
			Destroy (qShapes [i].gameObject);
		}

		qShapes.Clear();

        if (shapeQueue.Count > 0)
            populateQueue();
	}
	
    IEnumerator Wait()
    {

        // NOTE: shared NXTBLKSPAWNTIME that is affected by game timer
        //       individual nxtBlkSpawnTime based on NXTBLKSPAWNTIME that can be affected by powerups

        //if (!spedUp)
        yield return new WaitForSeconds(NXTBLKSPAWNTIME);
        //else
        //yield return new WaitForSeconds(nxtBlkSpawnTime);

        SpawnShape();
        curWaiting = false;

        spawn = true;
		spawning = false;
    }

    ///////////////////////////////////////
    /// Player Control Functions
    ///////////////////////////////////////

    void Rotate()
    {
        //Set parent to pivot so we can rotate
        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].parent = pivot.transform;
        }

        currentRot += 90;//Add rotation
        if (currentRot == 360)
        { //Reset rotation
            currentRot = 0;
        }

        pivot.transform.localEulerAngles = new Vector3(0, 0, currentRot);

        if (CheckRotate() == false)
        {
            currentRot -= 90;
            pivot.transform.localEulerAngles = new Vector3(0, 0, currentRot);
        }

        //Set parent back to null
        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].parent = null;
        }
    }

    bool CheckUserMove(float moveAmount)
    {
        // Buffer to ensure better collision detection when shape is off x grid
        if (moveAmount < 0)
            moveAmount -= 0.49f;
        else
            moveAmount += 0.49f;
        
        //Will Player movement cause collision?
        for (int i = 0; i < shapes.Count; i++)
            if (board[Mathf.RoundToInt(shapes[i].position.x + moveAmount - transform.position.x), 
                      Mathf.RoundToInt(shapes[i].position.y - transform.position.y)] == 1)
                return false;
        
        return true;
    }

    bool CheckRotate()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            if (Mathf.RoundToInt(shapes[i].position.x - transform.position.x) < board.GetLength(0) - 1)
            {//Check if block is in board
                if (board[Mathf.RoundToInt(shapes[i].position.x - transform.position.x), 
                          Mathf.RoundToInt(shapes[i].position.y - transform.position.y)] == 1)
                {
                    //If rotated block hit any other block or edge, after rotation
                    return false; //Rotate in default position - previous
                }
            }
            else
            {//If the block is not in the board
                return false;//Do not rotate
            }
        }

        return true; //We can rotate
    }

    ///////////////////////////////////////
    /// Mouse/Touch Functions
    ///////////////////////////////////////

    private void OnEnable()
    {
        GetComponent<PressGesture>().AddFriendlyGesture(GetComponent<TransformGesture>());
        GetComponent<TapGesture>().Tapped += tappedHandler;
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<TransformGesture>().Transformed += transformHandler;
    }

    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= tappedHandler;
        GetComponent<PressGesture>().Pressed += pressedHandler;
        GetComponent<TransformGesture>().Transformed -= transformHandler;
    }

    private void pressedHandler(object sender, System.EventArgs e)
    {
        if (!gameOver)
        {
            PressGesture message = (PressGesture)sender;
            previousPosition = myCam.ScreenToWorldPoint(message.ScreenPosition);
        }
    }

    private void transformHandler(object sender, System.EventArgs e)
    {
        if (!gameOver && pivot != null)
        {
            TransformGesture message = (TransformGesture)sender;
            Vector3 cursorPos = myCam.ScreenToWorldPoint(message.ScreenPosition);
            CheckDrag(cursorPos);
        }
    }

    private void tappedHandler(object sender, System.EventArgs e)
    {
        if (!gameOver && pivot != null)
            Rotate();
    }

    /*
    void OnMouseDown()
    {
        if (!gameOver)
        {
            previousPosition = myCam.ScreenToWorldPoint(Input.mousePosition);
            notDragged = true;
        }
    }

    void OnMouseUp()
    {
        if (!gameOver)
        {
            if (notDragged && pivot != null)
                Rotate(); 
            notDragged = false;
        }
    }

    void OnMouseDrag()
    {
        if (!gameOver && pivot != null)
        {
            notDragged = false;
            Vector3 cursorPos = myCam.ScreenToWorldPoint(Input.mousePosition);
            CheckDrag(cursorPos);
        }
    }
    */

   /*
    * NOTE: Fine grain control over horizontal movement for better feel.
    *       Coarse grain control over vertical movement to avoid accidental dropping.
    */
    void CheckDrag(Vector3 cursorPos)
    {
        Vector2 boardPos = new Vector2(Mathf.RoundToInt(cursorPos.x - transform.position.x), 
                                       Mathf.RoundToInt(cursorPos.y - transform.position.y));
        // Check if input is on the board
        if (boardPos.x >= 0 && boardPos.x < 12 &&
            boardPos.y >= 0 && boardPos.y < 24)
        {
            //Get spawned block pos
            float moveAmount = cursorPos.x - previousPosition.x;

            // Check direction and if we can move in that direction
            if (moveAmount != 0 && CheckUserMove(moveAmount))
            {
                pivot.transform.position = new Vector3(pivot.transform.position.x + moveAmount,
                                                       pivot.transform.position.y, 
                                                       pivot.transform.position.z);

                for (int i = 0; i < shapes.Count; i++)
                {
                    shapes[i].position = new Vector3(shapes[i].position.x + moveAmount, shapes[i].position.y, shapes[i].position.z);
                }
            }

            previousPosition.x = cursorPos.x;

            // Move down if deltaY > 0
            if (previousPosition.y - cursorPos.y > 1)
            {
                MoveDown();
                previousPosition.y = cursorPos.y;
            }
        }
    }
}