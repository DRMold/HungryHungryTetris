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
//		int genShape = Random.Range (0, 7);
//		if (genShape == 0) {
//			spawnedShapes.Add(
//				(GameObject)GameObject.Instantiate(Resources.Load ("sBlockPrefab"),
//			                                   new Vector3(100.0f, 20.0f, 0.2f),
//			                                   Quaternion.identity));
//		} else if (genShape == 1) {
//			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("iBlockPrefab"),
//			                                                     new Vector3(100.0f, 20.0f, -0.2f),
//			                                                     Quaternion.identity));
//		} else if (genShape == 2) {
//			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("oBlockPrefab"),
//			                                                     new Vector3(100.0f, 20.0f, -0.2f),
//			                                                     Quaternion.identity));
//		} else if (genShape == 3) {
//			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("jBlockPrefab"),
//			                                                     new Vector3(100.0f, 20.0f, -0.2f),
//			                                                     Quaternion.identity));
//		} else if (genShape == 4) {
//			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("tBlockPrefab"),
//			                                                     new Vector3(100.0f, 20.0f, -0.2f),
//			                                                     Quaternion.identity));
//		} else if (genShape == 5) {
//			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("lBlockPrefab"),
//			                                                     new Vector3(100.0f, 20.0f, -0.2f),
//			                                                     Quaternion.identity));
//		} else if (genShape == 6) {
//			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("zBlockPrefab"),
//			                                                     new Vector3(100.0f, 20.0f, -0.2f),
//			                                                     Quaternion.identity));
//		}	

		spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("L"),
		                                                     new Vector3(100.0f, 20.0f, -1.0f),
	                                                         Quaternion.identity));
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
