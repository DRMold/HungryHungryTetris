using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrevTetris : MonoBehaviour
{
    //Board
    public int[,] board;
    //Block
    public Transform block;
    //Spawn Bool
    public bool spawn;
    //Sec before nxt blk spawn
    public float nxtBlkSpawnTime = 0.5f;
    //Block fall speed
    public float blkFallSpeed = 0.5f;
    //Game Over Level
    public int gameOverHeight = 22;
    // 20 board + 2 edge
    //Queue
    public Queue<int> shapeQueue = new Queue<int>();
    //Current Shape
    private List<Transform> shapes = new List<Transform>();
    private bool gameOver;
    //Current shape rotation
    private int currentRot = 0;
    //Current pivot of shape
    private GameObject pivot;

    // Controls
    public KeyCode rot, down, left, right;

    void Start()
    {
        //Default board is 10x16
        //1+10+1 - Side edge
        //+2 - Space for spawning
        //+1 - Top Edge
        //20 - Height
        //+1 - Down Edge

        board = new int[12, 24]; // Set board width and height
        GenBoard();

        shapeQueue.Enqueue(Random.Range(0, 6));

        InvokeRepeating("moveDown", blkFallSpeed, blkFallSpeed); //move blk down
    }

    // Update is called once per frame
    void Update()
    {
        // If nothing spawned and game isn't over, then spawn
        if (!spawn && !gameOver)
        {
            StartCoroutine("Wait");
            spawn = true;
            //Reset rotation 
            currentRot = 0;
        }
        if (gameOver)
        {
            Time.timeScale = 0;
        }
        //////////////////////////////////////////////////////
        // Begin Player Input Checks
        /////////////////////////////////////////////////////

        //If there is a block
        if (spawn && shapes.Count == 4)
        {
            //Get spawned block pos
            Vector3 a = shapes[0].transform.position;
            Vector3 b = shapes[1].transform.position;
            Vector3 c = shapes[2].transform.position;
            Vector3 d = shapes[3].transform.position;

            //Move Left
            if (Input.GetKeyDown(left))
            {
                //Can we even move left?
                if (CheckUserMove(a, b, c, d, true))
                {
                    a.x -= 1;
                    b.x -= 1;
                    c.x -= 1;
                    d.x -= 1;

                    pivot.transform.position = new Vector3(pivot.transform.position.x - 1, pivot.transform.position.y, pivot.transform.position.z);

                    shapes[0].transform.position = a;
                    shapes[1].transform.position = b; 
                    shapes[2].transform.position = c; 
                    shapes[3].transform.position = d; 
                }
            }
            //Move Right
            if (Input.GetKeyDown(right))
            {
                //Can we even move right?
                if (CheckUserMove(a, b, c, d, false))
                {
                    a.x += 1;
                    b.x += 1;
                    c.x += 1;
                    d.x += 1;

                    pivot.transform.position = new Vector3(pivot.transform.position.x + 1, pivot.transform.position.y, pivot.transform.position.z);

                    shapes[0].transform.position = a;
                    shapes[1].transform.position = b; 
                    shapes[2].transform.position = c; 
                    shapes[3].transform.position = d; 
                }
            }
            //Drop Piece
            if (Input.GetKey(down))
            {
                moveDown();
            }
            //Roatate Piece
            if (Input.GetKeyDown(rot))
            {
                Rotate(shapes[0].transform, shapes[1].transform, shapes[2].transform, shapes[3].transform);	
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

    void SpawnShape()
    {
//		int shape = Random.Range(0, 6); //Rand shape
        int shape = shapeQueue.Dequeue();
        shapeQueue.Enqueue(Random.Range(0, 6));
        int height = (int)transform.position.y + board.GetLength(1) - 4;
        int xPos = (int)transform.position.x + board.GetLength(0) / 2 - 1;
        //Create pivot
        pivot = new GameObject("RotateAround"); //Pivot of shape

        if (shape == 0)
        { //S Shape
            SetCubePositions(new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos, height, 0),
                             new Vector3(xPos - 1, height, 0),
                             new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos + 1, height + 1, 0));
        }
        else if (shape == 1)
        { //I Shape
            SetCubePositions(new Vector3(xPos + 0.5f, height + 1.5f, 0),
                             new Vector3(xPos, height, 0),
                             new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos, height + 2, 0),
                             new Vector3(xPos, height + 3, 0));
        }
        else if (shape == 2)
        { //O Shape
            SetCubePositions(new Vector3(xPos + 0.5f, height + 0.5f, 0),
                             new Vector3(xPos, height, 0),
                             new Vector3(xPos + 1, height, 0),
                             new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos + 1, height + 1, 0));
        }
        else if (shape == 3)
        { //J Shape
            SetCubePositions(new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos, height, 0),
                             new Vector3(xPos + 1, height, 0),
                             new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos, height + 2, 0));
        }
        else if (shape == 4)
        { //T Shape
            SetCubePositions(new Vector3(xPos, height, 0),
                             new Vector3(xPos, height, 0),
                             new Vector3(xPos - 1, height, 0),
                             new Vector3(xPos + 1, height, 0),
                             new Vector3(xPos, height + 1, 0));
        }
        else if (shape == 5)
        { //L Shape
            SetCubePositions(new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos, height, 0),
                             new Vector3(xPos - 1, height, 0),
                             new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos, height + 2, 0));
        }
        else if (shape == 6)
        { //Z Shape
            SetCubePositions(new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos, height, 0),
                             new Vector3(xPos + 1, height, 0),
                             new Vector3(xPos, height + 1, 0),
                             new Vector3(xPos - 1, height + 1, 0));
        }
        else
        {
            Debug.Log("Illegal shape code: " + shape);
        }
    }

    void SetCubePositions(Vector3 piv, Vector3 b1, Vector3 b2, Vector3 b3, Vector3 b4)
    {
        pivot.transform.position = piv;
        shapes.Add(GenBlock(b1));
        shapes.Add(GenBlock(b2));
        shapes.Add(GenBlock(b3));
        shapes.Add(GenBlock(b4));
    }

    //Create block at position
    Transform GenBlock(Vector3 pos)
    {
        Transform obj = (Transform)Instantiate(block.transform, pos, Quaternion.identity) as Transform;
        obj.tag = "Block";

        return obj;
    }

    void moveDown()
    {
        //Spawned blocks position 
        if (shapes.Count != 4)
            return;
        Vector3 a = shapes[0].transform.position;
        Vector3 b = shapes[1].transform.position;
        Vector3 c = shapes[2].transform.position;
        Vector3 d = shapes[3].transform.position;

        //Will we hit anything if we move blck
        if (CheckMove(a, b, c, d) == true)
        {
            //Move block down 1
            a = new Vector3(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y - 1.0f), a.z);
            b = new Vector3(Mathf.RoundToInt(b.x), Mathf.RoundToInt(b.y - 1.0f), b.z);
            c = new Vector3(Mathf.RoundToInt(c.x), Mathf.RoundToInt(c.y - 1.0f), c.z);
            d = new Vector3(Mathf.RoundToInt(d.x), Mathf.RoundToInt(d.y - 1.0f), d.z);

            pivot.transform.position = new Vector3(pivot.transform.position.x, pivot.transform.position.y - 1, pivot.transform.position.z);

            shapes[0].transform.position = a;
            shapes[1].transform.position = b; 
            shapes[2].transform.position = c; 
            shapes[3].transform.position = d; 
        }
        else
        {
            // We hit something, stop and mark loc and Destroy pivot
            Destroy(pivot.gameObject); //Destroy pivot

            //Set ID in board
            board[Mathf.RoundToInt(a.x - transform.position.x), Mathf.RoundToInt(a.y - transform.position.y)] = 1;
            board[Mathf.RoundToInt(b.x - transform.position.x), Mathf.RoundToInt(b.y - transform.position.y)] = 1;
            board[Mathf.RoundToInt(c.x - transform.position.x), Mathf.RoundToInt(c.y - transform.position.y)] = 1;
            board[Mathf.RoundToInt(d.x - transform.position.x), Mathf.RoundToInt(d.y - transform.position.y)] = 1;

            //****************************************************
            checkRow(1); //Check for any match
            checkRow(gameOverHeight); //Check for game over
            //****************************************************

            shapes.Clear(); //Clear spawned blocks from array
            spawn = false; //Spawn a new block
        }
    }

    bool CheckMove(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        //Check if we move a block will it hit something
        if (board[Mathf.RoundToInt(a.x - transform.position.x), Mathf.RoundToInt(a.y - 1 - transform.position.y)] == 1 ||
            board[Mathf.RoundToInt(b.x - transform.position.x), Mathf.RoundToInt(b.y - 1 - transform.position.y)] == 1 ||
            board[Mathf.RoundToInt(c.x - transform.position.x), Mathf.RoundToInt(c.y - 1 - transform.position.y)] == 1 ||
            board[Mathf.RoundToInt(d.x - transform.position.x), Mathf.RoundToInt(d.y - 1 - transform.position.y)] == 1)
        {
            return false;
        }

        return true;
    }

    //Check specific row for match
    void checkRow(int y)
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
                                go.transform.position = new Vector3(xPos + transform.position.x, height - 1 + transform.position.y, go.transform.position.z);//Move block down
                            }
                        }
                    }
                }
            }
            checkRow(y); //We moved blocks down, check again this row
        }
        else if (y + 1 < board.GetLength(1) - 3)
            {
                checkRow(y + 1); //Check row above this
            }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(nxtBlkSpawnTime);
        SpawnShape();
    }

    ///////////////////////////////////////
    /// Player Control Functions
    ///////////////////////////////////////

    void Rotate(Transform a, Transform b, Transform c, Transform d)
    {
        //Set parent to pivot so we can rotate
        a.parent = pivot.transform;
        b.parent = pivot.transform;
        c.parent = pivot.transform;
        d.parent = pivot.transform;

        currentRot += 90;//Add rotation
        if (currentRot == 360)
        { //Reset rotation
            currentRot = 0;
        }

        pivot.transform.localEulerAngles = new Vector3(0, 0, currentRot);

        a.parent = null;
        b.parent = null;
        c.parent = null;
        d.parent = null;

        if (CheckRotate(a.position, b.position, c.position, d.position) == false)
        {
            //Set parent to pivot so we can rotate
            a.parent = pivot.transform;
            b.parent = pivot.transform;
            c.parent = pivot.transform;
            d.parent = pivot.transform;

            currentRot -= 90;
            pivot.transform.localEulerAngles = new Vector3(0, 0, currentRot);

            a.parent = null;
            b.parent = null;
            c.parent = null;
            d.parent = null;
        }
    }

    bool CheckUserMove(Vector3 a, Vector3 b, Vector3 c, Vector3 d, bool dir)
    {
        //Will Player movement cause collision?
        if (dir)
        { //Left
            if (board[Mathf.RoundToInt(a.x - 1 - transform.position.x), Mathf.RoundToInt(a.y - transform.position.y)] == 1 ||
                board[Mathf.RoundToInt(b.x - 1 - transform.position.x), Mathf.RoundToInt(b.y - transform.position.y)] == 1 ||
                board[Mathf.RoundToInt(c.x - 1 - transform.position.x), Mathf.RoundToInt(c.y - transform.position.y)] == 1 ||
                board[Mathf.RoundToInt(d.x - 1 - transform.position.x), Mathf.RoundToInt(d.y - transform.position.y)] == 1)
            {
                return false;
            }
        }
        else
        { //Right
            if (board[Mathf.RoundToInt(a.x + 1 - transform.position.x), Mathf.RoundToInt(a.y - transform.position.y)] == 1 ||
                board[Mathf.RoundToInt(b.x + 1 - transform.position.x), Mathf.RoundToInt(b.y - transform.position.y)] == 1 ||
                board[Mathf.RoundToInt(c.x + 1 - transform.position.x), Mathf.RoundToInt(c.y - transform.position.y)] == 1 ||
                board[Mathf.RoundToInt(d.x + 1 - transform.position.x), Mathf.RoundToInt(d.y - transform.position.y)] == 1)
            {
                return false;
            }
        }
        return true;
    }

    bool CheckRotate(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        if (Mathf.RoundToInt(a.x - transform.position.x) < board.GetLength(0) - 1)
        {//Check if block is in board
            if (board[Mathf.RoundToInt(a.x - transform.position.x), Mathf.RoundToInt(a.y - transform.position.y)] == 1)
            {
                //If rotated block hit any other block or edge, after rotation
                return false; //Rotate in default position - previous
            }
        }
        else
        {//If the block is not in the board
            return false;//Do not rotate
        }
        if (Mathf.RoundToInt(b.x - transform.position.x) < board.GetLength(0) - 1)
        {
            if (board[Mathf.RoundToInt(b.x - transform.position.x), Mathf.RoundToInt(b.y - transform.position.y)] == 1)
            {
                return false; 
            }
        }
        else
        {
            return false;
        }
        if (Mathf.RoundToInt(c.x - transform.position.x) < board.GetLength(0) - 1)
        {
            if (board[Mathf.RoundToInt(c.x - transform.position.x), Mathf.RoundToInt(c.y - transform.position.y)] == 1)
            {

                return false; 
            }
        }
        else
        {
            return false;
        }
        if (Mathf.RoundToInt(d.x - transform.position.x) < board.GetLength(0) - 1)
        {
            if (board[Mathf.RoundToInt(d.x - transform.position.x), Mathf.RoundToInt(d.y - transform.position.y)] == 1)
            {

                return false;
            }
        }
        else
        {
            return false;
        }
        Debug.Log("Can Rotate");
        return true; //We can rotate
    }
}