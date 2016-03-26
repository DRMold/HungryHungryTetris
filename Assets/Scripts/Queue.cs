using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Queue : MonoBehaviour {

	public int[,] QPreview;
	public Queue<int> shapeQueue = new Queue<int>();
	private List<Transform> shapes = new List<Transform>();
	private GameObject pivot;
	public Transform block;

	void Start()
	{
		//Default board is 10x16
		//1+10+1 - Side edge
		//+2 - Space for spawning
		//+1 - Top Edge
		//20 - Height
		//+1 - Down Edge

		QPreview  = new int[3, 7]; // Set board width and height
		GenQueue ();

	}

	void GenQueue()
	{
		for (int x = 0; x < QPreview.GetLength(0); x++)
		{
			for (int y = 0; y < QPreview.GetLength(1); y++)
			{
				if (x < 11 && x > 0)
				{
					if (y > 0 && y < QPreview.GetLength(1) - 2)
					{
						//Board
						QPreview [x, y] = 0;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 1);
						Material material = new Material(Shader.Find("Diffuse"));
						material.color = Color.grey;
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
					}
					else if (y < QPreview.GetLength(1) - 2)
					{
						QPreview [x, y] = 1;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 0);
						Material material = new Material(Shader.Find("Diffuse"));
						material.color = Color.black;
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
						cube.GetComponent<Collider>().isTrigger = true;
					}
				}
				else if ((y < QPreview.GetLength(1) - 2))
				{
					// Left and Right edge
					QPreview [x, y] = 1;
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
		int height = (int)transform.position.y + QPreview.GetLength(1) - 4;
		int xPos = (int)transform.position.x + QPreview.GetLength(0) / 2 - 1;
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

	Transform GenBlock(Vector3 pos)
	{
		Transform obj = (Transform)Instantiate(block.transform, pos, Quaternion.identity) as Transform;
		obj.tag = "Block";

		return obj;
	}
}
