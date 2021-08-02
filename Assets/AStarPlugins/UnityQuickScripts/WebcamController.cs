using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class WebcamController : MonoBehaviour
{
	private bool camIsLive;
	private WebCamTexture cameraTexture;
	public float webcamRotation { get ; private set ;}
	private Texture defaultBackground;

	[SerializeField]
	private RectTransform PotraitRect;
	[SerializeField]
	private RectTransform MaskRect;
	[SerializeField]
	private RawImage background;
	[SerializeField]
	private AspectRatioFitter fit;
	[SerializeField]
	private bool frontFacing;

	[SerializeField]
	private bool xInverse = true;
	public string imagestring { get; private set; }
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
				//Debug.Log(name);
				break;
			}
		}
		//Debug.Log((float)Screen.width);
		//Debug.Log((float)Screen.height);
		//Debug.Log(cameraTexture.name);
		if (cameraTexture == null)
			return;
		//Debug.Log("test");
		//PlayCamera();


		//Debug.Log((float)cameraTexture.width);
		//Debug.Log((float)cameraTexture.height);
	}

	public bool IsScreenshotReady()
    {
		return camIsLive;
    }
	/// <summary>
	/// Access imagestring to get the imagestring
	/// </summary>
	/// <param name="format"></param>
	public void getWebcamTextureString(ImageFormat format)
    {
		imagestring = "";;
		/*#if UNITY_ANDROID && !UNITY_EDITOR

				float cameraWidthToHeightRatio = (float)cameraTexture.height / (float)cameraTexture.width;
				float potraitWidthToHeightRatio = (float)MaskRect.rect.width / (float)MaskRect.rect.height;
				Debug.Log(cameraTexture.height);
				Debug.Log(cameraTexture.width);
				Debug.Log(cameraWidthToHeightRatio);
				Debug.Log(potraitWidthToHeightRatio);
				float offset = Mathf.Abs(MaskRect.rect.xMin - PotraitRect.rect.xMin) / (float)MaskRect.rect.height * cameraTexture.width;
				Debug.Log("offset" + offset);
				float potraitWidthInCamera = potraitWidthToHeightRatio * cameraTexture.width;
				Debug.Log(potraitWidthInCamera);
				//Texture2D snap = new Texture2D(cameraTexture.width, cameraTexture.height);
				//snap.SetPixels(cameraTexture.GetPixels(0, 0, cameraTexture.width, cameraTexture.height));
				Texture2D snap = new Texture2D((int)potraitWidthInCamera, cameraTexture.width);
				snap.SetPixels(cameraTexture.GetPixels(0, (int)offset, (int)potraitWidthInCamera, cameraTexture.width));
				//snap.SetPixels(cameraTexture.GetPixels(0, (int)offset, (int)potraitWidthInCamera, cameraTexture.width));
				snap.Apply();
#else*/

		float cameraWidthToHeightRatio = (float)cameraTexture.width / (float)cameraTexture.height;
		float potraitWidthToHeightRatio = (float)MaskRect.rect.width / (float)MaskRect.rect.height;


		float offset = Mathf.Abs(MaskRect.rect.xMin - PotraitRect.rect.xMin) / (float)MaskRect.rect.height * cameraTexture.height;

		float potraitWidthInCamera = potraitWidthToHeightRatio * cameraTexture.height;
		Texture2D snap = new Texture2D((int)potraitWidthInCamera, cameraTexture.height);
		snap.SetPixels(cameraTexture.GetPixels((int)offset, 0, (int)potraitWidthInCamera, cameraTexture.height));
		snap.Apply();
//#endif
		switch (format)
        {
			case ImageFormat.JPEG:
				imagestring = Convert.ToBase64String(snap.EncodeToJPG());
				break;
			case ImageFormat.PNG:
				imagestring = Convert.ToBase64String(snap.EncodeToPNG());
				break;

        }
	}

	public void PlayCamera()
    {
		Debug.Log("Play");
		cameraTexture.Play(); // Start the camera
		camIsLive = true;

		float ratio = (float)cameraTexture.width / (float)cameraTexture.height;

		fit.aspectRatio = ratio; // Set the aspect ratio
								 //fit.aspectRatio = ratio; // Set the aspect ratio

		float scaleY = 1f;
		float scaleX = 1f;
#if UNITY_ANDROID && !UNITY_EDITOR
		scaleY = xInverse ? -1f : 1f;
		background.rectTransform.localScale = new Vector3(scaleX, scaleY, 1f); // Swap the mirrored camera
#else
		//float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
		scaleX = xInverse ? -1f : 1f; // Find if the camera is mirrored or not
		background.rectTransform.localScale = new Vector3(scaleX, scaleY, 1f); // Swap the mirrored camera
#endif




		int orient = -cameraTexture.videoRotationAngle;
		//Debug.Log(orient);
		background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

		webcamRotation = orient;
		background.texture = cameraTexture; // Set the texture
	}
	public void StopCamera()
	{
		if(camIsLive)
        {
			Debug.Log("Stop");
			cameraTexture.Stop();
			camIsLive = false;
		}
	}
	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
        {
			if(camIsLive)
            {
				StopCamera();

			} else
            {
				PlayCamera();

			}

		}


		if (!camIsLive)
			return;
		
		if(Input.GetKeyDown(KeyCode.T))
        {
			getWebcamTextureString(ImageFormat.JPEG);
			Debug.Log(imagestring);
			GUIUtility.systemCopyBuffer = imagestring;
		}
		
	}


}
