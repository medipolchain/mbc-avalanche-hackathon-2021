using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using admob;

public class PauseManager : MonoBehaviour {
		
	//***************************************************************************//
	// This class manages pause and unpause states.
	// Please note that the pause scene is the best place to show your full screen ads.
	// in this project, we are calling a full sized ad when user hits the pause button.
	//***************************************************************************//

	//static bool  soundEnabled;
	public static bool isPaused;
	private float savedTimeScale;
	public GameObject pausePlane;
	public GameObject targetPlane;

	private GameObject AdManagerObject;

	enum Page {
		PLAY, PAUSE
	}
	private Page currentPage = Page.PLAY;

	//*****************************************************************************
	// Init.
	//*****************************************************************************
	IEnumerator Start (){		

		isPaused = false;	
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02f;

		AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");

		//pause plane is off at start
		if(pausePlane)
	    	pausePlane.SetActive(false); 

		//target plane (objectives) is on at start
		if(targetPlane)
			targetPlane.SetActive(true); 
		
		//pause the game
		yield return new WaitForSeconds(0.05f);
		PauseGame(); 
	}

	//*****************************************************************************
	// FSM
	//*****************************************************************************
	void Update (){

		//touch control
		touchManager();
		
		//optional pause in Editor & Windows (just for debug)
		if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape)) {
			//PAUSE THE GAME
			switch (currentPage) {
	            case Page.PLAY: 
	            	PauseGame(); 
	            	break;
	            case Page.PAUSE: 
	            	UnPauseGame(); 
	            	break;
	            default: 
	            	currentPage = Page.PLAY;
	            	break;
	        }

			//prevent objectives plane bug at the start of the game
			if(targetPlane.activeSelf)
				targetPlane.SetActive(false);
		}
		
		//debug restart
		if(Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	//*****************************************************************************
	// This function monitors player touches on menu buttons.
	// detects both touch and clicks and can be used with editor, handheld device and 
	// every other platforms at once.
	//*****************************************************************************
	void touchManager (){
		if(Input.GetMouseButtonUp(0)) {
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo)) {
				string objectHitName = hitInfo.transform.gameObject.name;
				switch(objectHitName) {
					case "PauseBtn":
						switch (currentPage) {
				            case Page.PLAY: 
				            	PauseGame();
				            	break;
				            case Page.PAUSE: 
				            	UnPauseGame(); 
				            	break;
				            default: 
				            	currentPage = Page.PLAY;
				            	break;
				        }
						break;

					case "ResumeBtn":
						switch (currentPage) {
				            case Page.PLAY: 
				            	PauseGame();
				            	break;
				            case Page.PAUSE: 
				            	UnPauseGame(); 
				            	break;
				            default: 
				            	currentPage = Page.PLAY;
				            	break;
				        }
						break;
					
					case "RestartBtn":
						UnPauseGame();
						SceneManager.LoadScene(SceneManager.GetActiveScene().name);
						break;

					case "StartBtn":
						UnPauseGame();
						targetPlane.SetActive(false); 
						break;
						
					case "MenuBtn":
						UnPauseGame();
						SceneManager.LoadScene("Menu");
						break;
				}
			}
		}
	}

	void PauseGame (){
		print("Game is Paused...");

		//show an Interstitial Ad when the game is paused
		if(AdManagerObject)
			AdManagerObject.GetComponent<AdManager>().showInterstitial();

		isPaused = true;
	    Time.timeScale = 0;
		Time.fixedDeltaTime = 0;
	    AudioListener.volume = 0;
	    pausePlane.SetActive(false);
	    currentPage = Page.PAUSE;
	}

	void UnPauseGame (){
		print("Unpause");
	    isPaused = false;
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02f;
	    AudioListener.volume = 1.0f;
		pausePlane.SetActive(false);   
	    currentPage = Page.PLAY;
	}
}