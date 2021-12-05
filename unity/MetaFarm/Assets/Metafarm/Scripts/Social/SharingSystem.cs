using UnityEngine;
using System.Collections;
using System;

public class SharingSystem : MonoBehaviour {

	/// <summary>
	/// Android Only!
	/// User can take a screenshot from inisde the game and share it via social apps to promote your game.
	/// </summary>

	public string gameTitle = "FrenzyFarming";
	public AudioClip cameraSfx;
	private bool canTap = true;

	void Update () {

		if(canTap)
			StartCoroutine(touchManager());
	}

	private RaycastHit hitInfo;
	private Ray ray;
	IEnumerator touchManager () {
		
		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			yield break;
		
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			switch(objectHit.name) {
				
			case "ShareButton":
				canTap = false;
				StartCoroutine(reactiveTap());
				playSfx(cameraSfx);
				ScreenCapture.CaptureScreenshot ("gameshot.png");

				yield return new WaitForSeconds(1.5f); //make sure our image has been saved.
				print ("Save Completed!!");

				//print (Application.persistentDataPath + "/gameshot.png");
				ShareImage(Application.persistentDataPath + "/gameshot.png", gameTitle, gameTitle, "I'm enjoying " + gameTitle + " !!");
				break;	
			}
		}
	}

	/// <summary>
	/// Shares the captured image with android Intents.
	/// </summary>
	public static void ShareImage(string imageFileName, string subject, string title, string message) {

		#if UNITY_ANDROID
		
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), title);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);
		
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", imageFileName);
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
		
		bool fileExist = fileObject.Call<bool>("exists");
		Debug.Log("File exist : " + fileExist);
		// Attach image to intent
		if (fileExist)
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call ("startActivity", intentObject);
		
		#endif
	}


	//*****************************************************************************
	// Play sound clips
	//*****************************************************************************
	void playSfx ( AudioClip _clip  ){
		GetComponent<AudioSource>().clip = _clip;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}


	IEnumerator reactiveTap() {
		yield return new WaitForSeconds(2.0f);
		canTap = true;
	}
}
