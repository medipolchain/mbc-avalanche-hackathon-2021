using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text.RegularExpressions;

public class CareerMapManager : MonoBehaviour {

	///*************************************************************************///
	/// This is the main map controller class. 
	/// It checks user advancement in the game, checks if user has enough energy to play a level,
	/// and loads the correct level based on user input/touch/click.
	///*************************************************************************///

	static public int userLevelAdvance;
	private int totalLevels;
	private GameObject[] levels;

	public AudioClip menuTap;
	public AudioClip insufficentEnergy;

	private bool canTap;
	private float buttonAnimationSpeed = 9;

	private int playerEnergy;
	public GameObject energyUI;

	void Awake (){
		canTap = true; //player can tap on buttons
		playerEnergy = PlayerPrefs.GetInt("PlayerEnergy");

		//check user progress
		if(PlayerPrefs.HasKey("userLevelAdvance"))
			userLevelAdvance = PlayerPrefs.GetInt("userLevelAdvance");
		else
			userLevelAdvance = 0; //default. only level 1 in open.

		//cheat debug
		//userLevelAdvance = 3;
	}


	void Start (){
		//prevent screenDim in handheld devices
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}


	void Update (){
		if(canTap)	
			StartCoroutine(tapManager());
	}


	///***********************************************************************
	/// Process user inputs
	///***********************************************************************
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
			//print(objectHit.name);
			if(objectHit.tag == "levelSelectionItem") {
				canTap = false;
				StartCoroutine(animateButton(objectHit));

				if(playerEnergy >= 1) {
					playSfx(menuTap);
					//deduct one unit from total energy
					playerEnergy--;
					PlayerPrefs.SetInt("PlayerEnergy", playerEnergy);
				} else {
					playSfx(insufficentEnergy);
					yield return new WaitForSeconds(0.3f);
					StartCoroutine(animateButton(energyUI));
					yield break;
				}
				
				//save the game mode (optional)
				PlayerPrefs.SetString("gameMode", "CAREER");
				PlayerPrefs.SetInt("careerLevelID", objectHit.GetComponent<CareerLevelSetup>().levelID);
				
				yield return new WaitForSeconds(0.25f);
			
				//Load the next level
				string fixedPrefix = "";
				if(objectHit.GetComponent<CareerLevelSetup>().levelID < 10)
					fixedPrefix = "GameLevel-0";
				else
					fixedPrefix = "GameLevel-";

				//build the string containing the next level name
				string nextLevelToLoadName = fixedPrefix + objectHit.GetComponent<CareerLevelSetup>().levelID;

				print ("loading level: " + nextLevelToLoadName);
				SceneManager.LoadScene(nextLevelToLoadName);
			}

			if(objectHit.name == "BtnBack") {
				playSfx(menuTap);
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				SceneManager.LoadScene("Menu");
				yield break;
			}
		}
	}


	///***********************************************************************
	/// Animate button by modifying it's scale
	///***********************************************************************
	IEnumerator animateButton ( GameObject _btn  ){
		Vector3 startingScale = _btn.transform.localScale;
		Vector3 destinationScale = startingScale * 1.1f;
		//yield return new WaitForSeconds(0.1f);
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * buttonAnimationSpeed;
			_btn.transform.localScale = new Vector3( Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
			                                        Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
			                                        _btn.transform.localScale.z);
			yield return 0;
		}
		
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


	/// <summary>
	/// This function receives a complex string (with numbers) and returns just the numbers.
	/// </summary>
	int convertToInt ( string stringContainingNumber  ){
		string numbersOnly = Regex.Replace(stringContainingNumber, "[^0-9]", "");
		return Convert.ToInt32(numbersOnly);
	}


	///***********************************************************************
	/// play audio clip
	///***********************************************************************
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}


}