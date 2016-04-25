using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HockeyController : MonoBehaviour {
	private List<GameObject> spawnedShapes = new List<GameObject>();
	private bool shouldISpawn;

	public float nxtBlckSpawnTime;

	// Use this for initialization
	void Start () {
		shouldISpawn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (shouldISpawn)
			StartCoroutine ("Wait");
	}

	void SpawnShape() {
		// shape      -        int
		//   s                  0
		//   i                  1
		//   o                  2
		//   j                  3
		//   t                  4
		//   l                  5
		//   z                  6
		int genShape = Random.Range (0, 10);
		if (genShape == 0) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("S"),
			                                                     new Vector3(100.0f, 20.0f, -1.0f),
			                                                     Quaternion.identity));
		} else if (genShape == 1) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("I"),
			                                                     new Vector3(100.0f, 20.0f, -1.0f),
			                                                     Quaternion.identity));
		} else if (genShape == 2) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("O"),
			                                                     new Vector3(100.0f, 20.0f, -1.0f),
			                                                     Quaternion.identity));
		} else if (genShape == 3) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("J"),
			                                                     new Vector3(100.0f, 20.0f, -1.0f),
			                                                     Quaternion.identity));
		} else if (genShape == 4) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("T"),
			                                                     new Vector3(100.0f, 20.0f, -1.0f),
			                                                     Quaternion.identity));
		} else if (genShape == 5) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("L"),
			                                                     new Vector3(100.0f, 20.0f, -1.0f),
			                                                     Quaternion.identity));
		} else if (genShape == 6) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("Z"),
			                                                       new Vector3(100.0f, 20.0f, -1.0f),
			                                                       Quaternion.identity));
		} else if (genShape == 7) {
			spawnedShapes.Add ((GameObject)GameObject.Instantiate (Resources.Load ("Fast Fall Speed"),
				new Vector3 (100.0f, 20.0f, -1.0f),
				//rotate 90 degrees to show powerup's icon
				Quaternion.Euler(new Vector3(-90,0,0))));
		} else if (genShape == 8) {
			spawnedShapes.Add ((GameObject)GameObject.Instantiate (Resources.Load ("Slow Fall Speed"),
				new Vector3 (100.0f, 20.0f, -1.0f),
				//rotate 90 degrees to show powerup's icon
				Quaternion.Euler(new Vector3(-90,0,0))));
		} else if (genShape == 9) {
			spawnedShapes.Add ((GameObject)GameObject.Instantiate (Resources.Load ("P"),
				new Vector3 (100.0f, 20.0f, -1.0f),
				//rotate 90 degrees to show powerup's icon
				Quaternion.Euler(new Vector3(-90,0,0))));
		}
	}

	IEnumerator Wait()
	{
		shouldISpawn = false;
		yield return new WaitForSeconds(nxtBlckSpawnTime);
		SpawnShape();
		yield return new WaitForSeconds(nxtBlckSpawnTime);
		shouldISpawn = true;
	}
}
