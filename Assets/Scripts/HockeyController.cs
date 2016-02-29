using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HockeyController : MonoBehaviour {
	private List<GameObject> spawnedShapes = new List<GameObject>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// shape      -        int
		//   s                  0
		//   i                  1
		//   o                  2
		//   j                  3
		//   t                  4
		//   l                  5
		//   z                  6
		int genShape = Random.Range (0, 7);
		if (genShape == 0) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("sBlockPrefab")));
		} else if (genShape == 1) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("iBlockPrefab")));
		} else if (genShape == 2) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("oBlockPrefab")));
		} else if (genShape == 3) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("jBlockPrefab")));
		} else if (genShape == 4) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("tBlockPrefab")));
		} else if (genShape == 5) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("lBlockPrefab")));
		} else if (genShape == 6) {
			spawnedShapes.Add((GameObject)GameObject.Instantiate(Resources.Load ("zBlockPrefab")));
		}
	}
}
