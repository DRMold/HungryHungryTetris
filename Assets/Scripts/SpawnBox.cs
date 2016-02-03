using UnityEngine;
using System.Collections;

public class SpawnBox : MonoBehaviour {

	public GameObject[] boxList;


	void Start () {

		SpawnNewBox();
	}

	public void SpawnNewBox() {
		int i = Random.Range(0, boxList.Length);
		Instantiate(boxList[i], transform.position, Quaternion.identity);
	}
}
