using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using System.Net;
//using System.Net.Http;
using System.IO;
using System;

public class FrameGetter : MonoBehaviour {

	WebCamTexture webCamTexture;
	private int incrementer = 0;
	private int incrementer2  =0;
	private bool mAccessCameraImage = true;

	private float cameraHeight;
	private float cameraWidth;


//	private Image.PIXEL_FORMAT mPixelFormat = Image.PIXEL_FORMAT.RGB888;// or RGBA8888, RGB888, RGB565, YUV //TANGO
	private Image.PIXEL_FORMAT mPixelFormat = Image.PIXEL_FORMAT.RGBA8888;// or RGBA8888, RGB888, RGB565, YUV //LOCAL
	// Boolean flag telling whether the pixel format has been registered
	private bool mFormatRegistered = false;
	void Start ()
	{
		// Register Vuforia life-cycle callbacks:
		VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
		VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnPause);
		VuforiaBehaviour.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
	}

	/// <summary>
	/// Called when Vuforia is started
	/// </summary>
	private void OnVuforiaStarted()
	{
		// Try register camera image format
		if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
		{
			Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());
			mFormatRegistered = true;
		}
		else
		{
			Debug.LogError("Failed to register pixel format " + mPixelFormat.ToString() +
				"\n the format may be unsupported by your device;" +
				"\n consider using a different pixel format.");
			mFormatRegistered = false;
		}
	}
	/// <summary>
	/// Called when app is paused / resumed
	/// </summary>
	private void OnPause(bool paused)
	{
		if (paused)
		{
			Debug.Log("App was paused");
			UnregisterFormat();
		}
		else
		{
			Debug.Log("App was resumed");
			RegisterFormat();
		}
	}
	/// <summary>
	/// Called each time the Vuforia state is updated
	/// </summary>
	private void OnTrackablesUpdated()
	{
		incrementer++;
		if (mFormatRegistered )
		{
			if (mAccessCameraImage && incrementer %60 == 0)
			{
				Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
				WebCamTexture webCamTexture;
				if (image != null)
				{
					string imageInfo = mPixelFormat + " image: \n";
					imageInfo += " size: " + image.Width + " x " + image.Height + "\n";
					imageInfo += " bufferSize: " + image.BufferWidth + " x " + image.BufferHeight + "\n";
					imageInfo += " stride: " + image.Stride;
					byte[] pixels = image.Pixels;

//					Array.Reverse(pixels);
					//array reversal
					for (int i = 0; i < pixels.Length / 2; i++)
					{
						byte tmp = pixels[i];
						pixels[i] = pixels[pixels.Length - i - 1];
						pixels[pixels.Length - i - 1] = tmp;
					}

					Texture2D texture = new Texture2D(image.Width, image.Height);

					cameraHeight = (float) image.Height;
					cameraWidth = (float) image.Width;


					image.CopyToTexture (texture);

					texture.Apply ();

					byte[] pixxels = texture.EncodeToPNG ();

					//byte[] reversed = pixxels.Reverse().ToArray();

					//byte[] bytes = GetTheBytes();

					//

//					Debug.Log (Application.dataPath);
//
//
//					File.WriteAllBytes(Application.dataPath + "SavedScreen.png", pixxels);

					//										MemoryStream ms = new MemoryStream(pixels);
					//					Image returnImage = System.Drawing.Image.FromStream(ms);
					//					returnImage.CopyToTexture

					//
					//										Texture2D photo = new Texture2D(image.Width, image.Height);
					////										photo.SetPixels(image.Pixels);
					//										photo.Apply();
					//
					//										//Encode to a PNG
					//										byte[] pixels = photo.EncodeToPNG();

					if (pixxels != null && pixxels.Length > 0)
					{ 
						DetectFaces (pixxels);

						//						                        Debug.Log("Image pixels: " + pixels[0] + "," + pixels[1] + "," + pixels[2] + ",...");
					}
				}
			}
		}
	}
	/// <summary>
	/// Unregister the camera pixel format (e.g. call this when app is paused)
	/// </summary>
	private void UnregisterFormat()
	{
		Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
		CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
		mFormatRegistered = false;
	}
	/// <summary>
	/// Register the camera pixel format
	/// </summary>
	private void RegisterFormat()
	{
		if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
		{
			Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
			mFormatRegistered = true;
		}
		else
		{
			Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
			mFormatRegistered = false;
		}
	}

	//	void Start()
	//	{
	//		webCamTexture = new WebCamTexture();
	//		webCamTexture.Play();
	//	}
	//
	//	void Update()
	//	{
	//		TakePhoto ();
	//	}
	//	IEnumerator TakePhoto()
	//	{
	//
	//		// NOTE - you almost certainly have to do this here:
	//		if (incrementer % 10 == 0) {
	//			Debug.Log ("waitin for photo");
	//
	//			yield return new WaitForEndOfFrame (); 
	//			Debug.Log ("Waiting finished");
	//
	//			// it's a rare case where the Unity doco is pretty clear,
	//			// http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
	//			// be sure to scroll down to the SECOND long example on that doco page 
	//
	//			Texture2D photo = new Texture2D (webCamTexture.width, webCamTexture.height);
	//			photo.SetPixels (webCamTexture.GetPixels ());
	//			photo.Apply ();
	//
	//			//Encode to a PNG
	//			byte[] bytes = photo.EncodeToPNG ();
	//			//Write out the PNG. Of course you have to substitute your_path for something sensible
	//			DetectFaces (bytes);
	//		}
	//	}

	public void DetectFaces(byte[] image) {
		incrementer2++;

		string key;
		if (incrementer2 % 2 == 0) {
			key = "f86c8a3b24114d038f4690f05dbe9d12";
		} else {
			key = "c8b77439b26d45c28a1d331a27fd38fd";
		}

		Debug.Log ("key: " + key);

		//		if (incrementer % 60 == 0) {

		WWWForm form = new WWWForm ();
		Dictionary<string, string> headers = new Dictionary<string, string> ();

		//		Dictionary<string, string> headers = form.headers;

		//		var client = new HTTPClient ();

		string url = "https://api.projectoxford.ai/face/v1.0/detect/";

		// Add a custom header to the request.
		// In this case a basic authentication to access a password protected resource.
		//		headers["Ocp-Apim-Subscription-Key"] = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("f86c8a3b24114d038f4690f05dbe9d12"));
		headers ["Ocp-Apim-Subscription-Key"] = key;
		//		headers["Content-Type"] = "application/json";
		headers["Content-Type"] = "application/octet-stream";

		// Post a request to an URL with our custom headers
		WWW www = new WWW (url, image, headers);


		StartCoroutine (parseDetect (www));
		//		}

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

	void accessData(JSONObject obj){
		switch(obj.type){
		case JSONObject.Type.OBJECT:
			for(int i = 0; i < obj.list.Count; i++){
				string key = (string)obj.keys[i];
				JSONObject j = (JSONObject)obj.list[i];
				Debug.Log(key);
				accessData(j);
			}
			break;
		case JSONObject.Type.ARRAY:
			foreach(JSONObject j in obj.list){
				accessData(j);
			}
			break;
		case JSONObject.Type.STRING:
			Debug.Log(obj.str);
			break;
		case JSONObject.Type.NUMBER:
			Debug.Log(obj.n);
			break;
		case JSONObject.Type.BOOL:
			Debug.Log(obj.b);
			break;
		case JSONObject.Type.NULL:
			Debug.Log("NULL");
			break;
		}
	}




	IEnumerator parseDetect (WWW www) {
		yield return www;

		CubeScript cubescript = FindObjectOfType<CubeScript> ();
		cubescript.updateCubePosition ((float)900000, (float)900000);//off screen
		Debug.Log ("SCREEN HEIGHT" + cameraHeight);
		Debug.Log ("SCREEN Width" + cameraWidth);



		if (www.error == null) {
			Debug.Log ("WWW Ok!: " + www.data);
//
//			string myJson = www.data.ToString();
//			JSONObject j = new JSONObject(myJson);
//
//			accessData (j);



			string[] arr = www.data.Split('"');

			//arr [8] = arr [8].Remove (0,1);
			//arr [10] = arr [10].Remove (0,1);
		
			arr [8] = arr [8].Replace(",", "");
			arr [10] = arr [10].Replace(",", "");

			arr [8] = arr [8].Replace(":", "");
			arr [10] = arr [10].Replace(":", "");

			float myLeft = float.Parse(arr [8]);
			float myTop = float.Parse(arr [10]);

			Debug.Log("my LEFT (top): " + arr[8]);
			Debug.Log("my TOP (left): " + arr[10]);




            //float myX = -((float)cameraWidth - myLeft) + ((float)cameraWidth /2);

            //float myY = -((float)cameraHeight - myTop) + ((float)cameraHeight /2 );

            //float myX = -myLeft;
            //float myY = -myTop;

			//WORKS LOCALLY
//            float myX = -myTop/2 + 120;
//
//            float myY = -myLeft/2 + 120;

//			cameraHeight = (float)Screen.height * (float) .27;
//			cameraWidth = (float)Screen.width * (float) .132;

			float myX = -myTop/2 + cameraWidth/6;

			float myY = -myLeft/2 + cameraHeight/4;

            //float myX = (cameraWidth - (float)myLeft);

            //float myY = (cameraHeight - (float)myTop);

            //float myX = myLeft - ((float)cameraWidth / 2);
            //float myY = myTop - ((float)cameraHeight / 2);



            Debug.Log("my X: " + myX);
            Debug.Log("my Y: " + myY);

            //myX = (float) 25.0;
            //myY = (float )25.0;

            cubescript.updateCubePosition (myX, myY);

		} else {
			Debug.Log ("WWW Error: " + www.error);
		}
	}

//	public void idFace(string faceId) {
//		string personGroupId = "nudge_hackathon";
//		string personIdMatch = "f5c22d5a-21e1-42c0-b990-6f342fc6da29";
//		string key;
//		if (incrementer2 % 2 == 0) {
//			key = "f86c8a3b24114d038f4690f05dbe9d12";
//		} else {
//			key = "c8b77439b26d45c28a1d331a27fd38fd";
//		}
//
//		WWWForm form = new WWWForm ();
//		Dictionary<string, string> headers = new Dictionary<string, string> ();
//
//		string url = "https://api.projectoxford.ai/face/v1.0/identify/";
//
//		// Add a custom header to the request.
//		// In this case a basic authentication to access a password protected resource.
//		//		headers["Ocp-Apim-Subscription-Key"] = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("f86c8a3b24114d038f4690f05dbe9d12"));
//		headers ["Ocp-Apim-Subscription-Key"] = key;
//		//		headers["Content-Type"] = "application/json";
//		headers["Content-Type"] = "application/json";
//
//		//encode as json
//
//		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
//		// number
//		j.AddField("personGroupId", personGroupId);
//		// string
//		// array
//		JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
//		j.AddField("faceIds", arr);
//
//		arr.Add(faceId);
//
//		string encodedString = j.print ();
//
//
//		// Post a request to an URL with our custom headers
//		WWW www = new WWW (url, encodedString, headers);
//
//		StartCoroutine (parseIdentify (www, personIdMatch));
//	}
//
//	IEnumerator parseIdentify (WWW www, string personIdMatch) {
//		yield return www;
//
//		CubeScript cubescript = FindObjectOfType<CubeScript> ();
//
//
//		if (www.error == null) {
//			Debug.Log ("WWW Ok!: " + www.data);
//
//			//parse www.data.candidates[0].personId;
//			//get real personId
//			string personId = "SomeID";
//
//			if (personId == personIdMatch) {
//
//			}
//
//
//
//		} else {
//			Debug.Log ("WWW Error: " + www.error);
//		}
//	}
}