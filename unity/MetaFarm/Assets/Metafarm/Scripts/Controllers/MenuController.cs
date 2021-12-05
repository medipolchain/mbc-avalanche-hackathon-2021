using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	///*************************************************************************///
	/// Main Menu Controller.
	/// This class handles all touch events on buttons. It also gives the user a chance to
	/// win some hourly energy prize , and this boosts player engagement with the game.
	///*************************************************************************///
	public string level;
	private float buttonAnimationSpeed = 9;		//speed on animation effect when tapped on button
	private bool canTap = false;				//flag to prevent double tap
	public AudioClip tapSfx;					//tap sound for buttons click

	void Awake (){

		//debug
		//PlayerPrefs.DeleteAll();
		
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02f;

		giveTimePrize();
		StartCoroutine(reactiveTap());

		//fill player's energy for the first run
		if(!PlayerPrefs.HasKey("PlayerEnergy"))
			PlayerPrefs.SetInt("PlayerEnergy", 10);
	}
	
	//*****************************************************************************
	// FSM
	//*****************************************************************************
	void Update (){	
		if(canTap) {
			StartCoroutine(tapManager());
		}
	}



	//time prize
	private DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
	private int timestamp;
	public GameObject prizeCoin;
	void giveTimePrize() {

		prizeCoin.SetActive(false);
		timestamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
		print("Global System Time is: " + timestamp.ToString());
		
		//User can get a prize every 1 hours (3600 seconds)
		//when he enters the game for the first time, we save the time and then recheck it we passed the 21600, 
		//if so we give the coins and resave the new time. else we bypass everything.
		int delaTimeForPrize = 3600; //in seconds
		
		//runs just once
		if(!PlayerPrefs.HasKey("lastPlayTime")) {
			PlayerPrefs.SetInt("lastPlayTime", timestamp);
			print("Initial Timestamp saved...");
			//prize for the first run
			prizeCoin.SetActive(true);
			return;
		}

		//give prize every #N hours
		if(timestamp - PlayerPrefs.GetInt("lastPlayTime") >= delaTimeForPrize) {
			print("Wow, welcome back. You won something!");
			PlayerPrefs.SetInt("lastPlayTime", timestamp);
			prizeCoin.SetActive(true);
		} else {
			print("no prize. come back later!");
			//prizeCoin.SetActive(false);
		}
	}

	
	//*****************************************************************************
	// This function monitors player touches on menu buttons.
	// detects both touch and clicks and can be used with editor, handheld device and 
	// every other platforms at once.
	//*****************************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
	IEnumerator tapManager (){
		
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
				
				//Game Modes
			case "BtnStart":								
				playSfx(tapSfx);							//play touch sound
				StartCoroutine(animateButton(objectHit));	//touch animation effect
				yield return new WaitForSeconds(1.0f);		//Wait for the animation to end
				SceneManager.LoadScene(level);				//Load the next scene
				break;

			case "BtnShop":								
				playSfx(tapSfx);							//play touch sound
				StartCoroutine(animateButton(objectHit));	//touch animation effect
				yield return new WaitForSeconds(1.0f);		//Wait for the animation to end
				SceneManager.LoadScene("Shop");				//Load the next scene
				break;

			case "PrizeIcon":
				playSfx(tapSfx);
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				prizeCoin.SetActive(false);
				//add 3 free energy
				PlayerPrefs.SetInt("PlayerEnergy", PlayerPrefs.GetInt("PlayerEnergy") + 3);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				break;	

			case "BtnVideoAd":
				playSfx(tapSfx);
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				break;

			case "BtnExit":
				playSfx(tapSfx);
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				Application.Quit();
				break;	
			}	
		}
	}
	
	//*****************************************************************************
	// This function animates a button by modifying it's scales on x-y plane.
	// can be used on any element to simulate the tap effect.
	//*****************************************************************************
	IEnumerator animateButton ( GameObject _btn  ){
		canTap = false;
		Vector3 startingScale = _btn.transform.localScale;	//initial scale	
		Vector3 destinationScale = startingScale * 1.1f;		//target scale
		
		//Scale up
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * buttonAnimationSpeed;
			_btn.transform.localScale = new Vector3( Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
			                                        Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
			                                        _btn.transform.localScale.z);
			yield return 0;
		}
		
		//Scale down
		float r = 0.0f; 
		if(_btn.transform.localScale.x >= destinationScale.x) {
			while (r <= 1.0f) {
				r += Time.deltaTime * buttonAnimationSpeed;
				_btn.transform.localScale = new Vector3( Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
				                                        Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
				                                        _btn.transform.localScale.z);
				yield return 0;
			}
		}
		
		if(r >= 1)
			canTap = true;
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
		yield return new WaitForSeconds(1.0f);
		canTap = true;
	}
	
}