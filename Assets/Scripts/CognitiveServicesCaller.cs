/* 
 * 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
//using System.Net.Http;
using System.IO;

public class CognitiveServicesCaller : MonoBehaviour {

	// The radius of our outer sphere

	const float radius = 0.8f;


	public void DetectFaces(byte[] image) {

		WWWForm form = new WWWForm ();
		Dictionary<string, string> headers = new Dictionary<string, string>();

//		Dictionary<string, string> headers = form.headers;

//		var client = new HTTPClient ();

		string url = "https://api.projectoxford.ai/face/v1.0/detect/";

		// Add a custom header to the request.
		// In this case a basic authentication to access a password protected resource.
		headers["Ocp-Apim-Subscription-Key"] = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("f86c8a3b24114d038f4690f05dbe9d12"));
		headers["Content-Type"] = "application/json";

		// Post a request to an URL with our custom headers
		WWW www = new WWW(url, image, headers);


		StartCoroutine (parseDetect (www));

//		yield return w;
//
//		print("Waiting for sphere definitions\n");
//
//		// Add a wait to make sure we have the definitions
//
//		yield return new WaitForSeconds(1f);
//
//		print("Received sphere definitions\n");
//
//		// Extract the spheres from our JSON results
//
//		ExtractSpheres(w.text);

	}

	IEnumerator parseDetect (WWW www) {
		yield return www;

		if (www.error == null) {
			Debug.Log ("WWW Ok!: " + www.data);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}
	}
}

*/
