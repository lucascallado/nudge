using UnityEngine;
using System.Collections;

public class CubeScript2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateCubePosition(float x, float y){
        //float x1 = (float)x;
        //float y1 = (float)y;
        //Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        //pos.x = Mathf.Clamp01(pos.x);
        //pos.y = Mathf.Clamp01(pos.y);
        //transform.position = Camera.main.ViewportToWorldPoint(pos);

        transform.localPosition = new Vector3(x, y, 250);
    }
}
