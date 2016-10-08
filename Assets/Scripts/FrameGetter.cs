using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
using System.Net;
//using System.Net.Http;
using System.IO;

public class FrameGetter : MonoBehaviour {

	WebCamTexture webCamTexture;
	private int incrementer = 0;
//	private bool mAccessCameraImage = true;
	      
	    // The desired camera image pixel format
//	    private Image.PIXEL_FORMAT mPixelFormat = Image.PIXEL_FORMAT.GRAYSCALE;// or RGBA8888, RGB888, RGB565, YUV
//	    // Boolean flag telling whether the pixel format has been registered
//	    private bool mFormatRegistered = false;
//	    void Start ()
//	    {
//		        // Register Vuforia life-cycle callbacks:
//		        VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
//		        VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnPause);
//		        VuforiaBehaviour.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
//		    }
//		
//	    /// <summary>
//	    /// Called when Vuforia is started
//	    /// </summary>
//	    private void OnVuforiaStarted()
//	    {
//		        // Try register camera image format
//		        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
//			        {
//			            Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());
//			            mFormatRegistered = true;
//			        }
//		        else
//			        {
//			            Debug.LogError("Failed to register pixel format " + mPixelFormat.ToString() +
//				                "\n the format may be unsupported by your device;" +
//				                "\n consider using a different pixel format.");
//			            mFormatRegistered = false;
//			        }
//		    }
//	    /// <summary>
//	    /// Called when app is paused / resumed
//	    /// </summary>
//	    private void OnPause(bool paused)
//	    {
//		        if (paused)
//			        {
//			            Debug.Log("App was paused");
//			            UnregisterFormat();
//			        }
//		        else
//			        {
//			            Debug.Log("App was resumed");
//			            RegisterFormat();
//			        }
//		    }
//	    /// <summary>
//	    /// Called each time the Vuforia state is updated
//	    /// </summary>
//	    private void OnTrackablesUpdated()
//	    {
//		        if (mFormatRegistered)
//			        {
//			            if (mAccessCameraImage)
//				            {
//				                Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
//								WebCamTexture webCamTexture;
//				                if (image != null)
//					                {
//					                    string imageInfo = mPixelFormat + " image: \n";
//					                    imageInfo += " size: " + image.Width + " x " + image.Height + "\n";
//					                    imageInfo += " bufferSize: " + image.BufferWidth + " x " + image.BufferHeight + "\n";
//					                    imageInfo += " stride: " + image.Stride;
//					                    Debug.Log(imageInfo);
////					                    byte[] pixels = image.Pixels;
//
//										Texture2D photo = new Texture2D(image.Width, image.Height);
////										photo.SetPixels(image.Pixels);
//										photo.Apply();
//
//										//Encode to a PNG
//										byte[] pixels = photo.EncodeToPNG();
//
//					                    if (pixels != null && pixels.Length > 0)
//						                    { 
//												DetectFaces (pixels);
//												
////						                        Debug.Log("Image pixels: " + pixels[0] + "," + pixels[1] + "," + pixels[2] + ",...");
//						                    }
//					                }
//				            }
//			        }
//		    }
//	    /// <summary>
//	    /// Unregister the camera pixel format (e.g. call this when app is paused)
//	    /// </summary>
//	    private void UnregisterFormat()
//	    {
//		        Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
//		        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
//		        mFormatRegistered = false;
//		    }
//	    /// <summary>
//	    /// Register the camera pixel format
//	    /// </summary>
//	    private void RegisterFormat()
//	    {
//		        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
//			        {
//			            Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
//			            mFormatRegistered = true;
//			        }
//		        else
//			        {
//			            Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
//			            mFormatRegistered = false;
//			        }
//		    }

	void Start()
	{
		webCamTexture = new WebCamTexture();
		webCamTexture.Play();
	}

	void Update()
	{
		//GetComponent<RawImage>().texture = webCamTexture;
	}
	void TakePhoto()
	{

		// NOTE - you almost certainly have to do this here:

//		yield return new WaitForEndOfFrame(); 

		// it's a rare case where the Unity doco is pretty clear,
		// http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
		// be sure to scroll down to the SECOND long example on that doco page 

		Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
		photo.SetPixels(webCamTexture.GetPixels());
		photo.Apply();

		//Encode to a PNG
		byte[] bytes = photo.EncodeToPNG();
		//Write out the PNG. Of course you have to substitute your_path for something sensible
		DetectFaces(bytes);
	}

	public void DetectFaces(byte[] image) {

		incrementer++;

		if (incrementer % 10 == 0) {

			WWWForm form = new WWWForm ();
			Dictionary<string, string> headers = new Dictionary<string, string> ();

			//		Dictionary<string, string> headers = form.headers;

			//		var client = new HTTPClient ();

			string url = "https://api.projectoxford.ai/face/v1.0/detect/";

			// Add a custom header to the request.
			// In this case a basic authentication to access a password protected resource.
//		headers["Ocp-Apim-Subscription-Key"] = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("f86c8a3b24114d038f4690f05dbe9d12"));
			headers ["Ocp-Apim-Subscription-Key"] = "f86c8a3b24114d038f4690f05dbe9d12";
//		headers["Content-Type"] = "application/json";
			headers["Content-Type"] = "application/octet-stream";

			// Post a request to an URL with our custom headers
			WWW www = new WWW (url, image, headers);


			StartCoroutine (parseDetect (www));
		}

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
