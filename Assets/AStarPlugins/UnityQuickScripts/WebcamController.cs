using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class WebcamController : MonoBehaviour
{
	private bool camAvailable;
	private WebCamTexture cameraTexture;
	private Texture defaultBackground;

	public RawImage background;
	public AspectRatioFitter fit;
	public bool frontFacing;

	public bool xInverse = true;
	public string imagestring;
	public enum ImageFormat{
		PNG,
		JPEG,
	}

	// Use this for initialization
	void Start()
	{
		defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;
		
		if (devices.Length == 0)
			return;
		
		for (int i = 0; i < devices.Length; i++)
		{
			var curr = devices[i];
			
			if (curr.isFrontFacing == frontFacing)
			{
				int shorterSide = (Screen.width > Screen.height) ? Screen.height : Screen.width;
				//Debug.Log(shorterSide);
				cameraTexture = new WebCamTexture(curr.name, shorterSide, shorterSide);
				Debug.Log(name);
				break;
			}
		}
		//Debug.Log((float)Screen.width);
		//Debug.Log((float)Screen.height);
		Debug.Log(cameraTexture.name);
		if (cameraTexture == null)
			return;
		Debug.Log("test");
		//PlayCamera();


		//Debug.Log((float)cameraTexture.width);
		//Debug.Log((float)cameraTexture.height);
	}

	public string getWebcamTextureString(ImageFormat format)
    {
		string ImageString = "";
		Texture2D snap = new Texture2D(cameraTexture.width, cameraTexture.height);
		snap.SetPixels(cameraTexture.GetPixels());
		snap.Apply();

		switch (format)
        {
			case ImageFormat.JPEG:
				ImageString = Convert.ToBase64String(snap.EncodeToJPG());
				break;
			case ImageFormat.PNG:
				ImageString = Convert.ToBase64String(snap.EncodeToPNG());
				break;

        }
		return ImageString;
	}

	public void PlayCamera()
    {
		Debug.Log("Play");
		cameraTexture.Play(); // Start the camera
		camAvailable = true; // Set the camAvailable for future purposes.

		float ratio = (float)cameraTexture.width / (float)cameraTexture.height;

		fit.aspectRatio = ratio; // Set the aspect ratio
								 //fit.aspectRatio = ratio; // Set the aspect ratio

		float scaleY = 1f;
#if UNITY_ANDROID && !UNITY_EDITOR
				scaleY = 1f;
#endif
		//float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not

		float scaleX = xInverse ? -1f : 1f; // Find if the camera is mirrored or not

		background.rectTransform.localScale = new Vector3(scaleX, scaleY, 1f); // Swap the mirrored camera

		int orient = -cameraTexture.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

		background.texture = cameraTexture; // Set the texture
	}
	public void StopCamera()
	{
		Debug.Log("Stop");
		cameraTexture.Stop();
		camAvailable = false;
	}
	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
        {
			if(camAvailable)
            {
				StopCamera();

			} else
            {
				PlayCamera();

			}

		}
		if (!camAvailable)
			return;

	}
}
