using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FileDemosBehaviour : MonoBehaviour {

	//reference to image to change
	public RawImage rawImage;


	//In Memory texture
	private Texture2D texture;


	private readonly string  FULL_FILE_NAME = "screenshot.png";

	void Start () {
	
		texture = new Texture2D(1024,768);

	}


	void Update () {
	
		//ON PRESS KEY L 
		if(Input.GetKeyDown(KeyCode.L)){

			byte[] imageBytes = Utils.GetFileAsBytesOrNull(FULL_FILE_NAME);

			if(imageBytes!=null){
				texture.LoadImage(imageBytes);
				Debug.Log("Image LOADED and converted to Texture");
			}


			//apply new texture to scene
			rawImage.texture = texture;

		}

		//ON PRESS KEY L 
		if(Input.GetKeyDown(KeyCode.S)){
			//Take a screeshot
			Application.CaptureScreenshot(FULL_FILE_NAME);
			Debug.Log( FULL_FILE_NAME + " saved!");
		}


	}









}
