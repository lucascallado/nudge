using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateCubePosition(float x, float y){
		//float x1 = (float)x;
		//float y1 = (float)y;

		transform.position = new Vector3(x, y);
	}
}
