#pragma warning disable 414

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour {

	/// <summary>
	/// Main Game Controller class.
	/// This class handles all game events like mission, money, prize, time, pets, etc...
	/// Please continue reading through the class to get to know these elements.
	/// </summary>

	//***************************************************
	// Mission related settings (adjust for each new level)
	// we can have up to 4 mission for each level. You are free in setting the number of missions for each level. 
	// Usually a level with full 4 mission requires more time and as you decrease the missions, you can lower the time reqauired to beat the level.
	//***************************************************
	public MissionConfig[] missions;	//BASE CLASS. You can choose the missions from one of these predefined missions. 
										//if you are familiar with coding, you can eaily add your own mission type.

	// Set these variables for each new level
	//--------------------------
	public int startingMoney = 75;		//Amount of starting money to play the game
	public int missionPrize = 300;		//amount of gold awarded to player after beating the level
	public int missionGoldPrize = 500; 	//additional amount of gold awarded after beating the level in specific time
	public int levelGoldTime = 150;		//(in seconds) beating the level in less than this time, gives extra reward.
	public int levelFailTime = 250; 	//maximum available time for this mission. if play time exceed this timer, the game is over.
	//***************************************************
	//***************************************************

	//***************************************************
	// YO DO NOT NEED TO TWEAK ANYTHING BELOW THIS LINE!!
	//***************************************************
	public GameObject[] availableMissionObjects;		//reference to the objects that shows the missions on the UI
	public Material[] availableMissionImages;			//actual images of the missions
	public GameObject[] missionCurrentAmountsText;		//current available amount of each mission item
	public GameObject[] missionTargetAmountsText;		//required amount of each mission item

	//Static falgs
	public static bool gameIsFinished;
	public static bool gameIsWon;

	internal float plantLimitLeft = -4;
	internal float plantLimitRight = 4;
	internal float plantLimitUp = 1.8f;
	internal float plantLimitDown = -2;

	public static int playerMoney;						//total available money

	//Pet HUD buttons	
	public GameObject[] petButtons;						//available pet buttons in the level that player can buy 
														//(you can leave this array as is and active/deactive the game object in the hierarcy.)
	public GameObject[] petButtonsPriceText;
	public int[] petButtonsPrice;						//set the price of each available pet
	public PetButtonImage[] petBtnImage;				//available images for locked/open pet buttons
	public GameObject[] availablePets;					//reference to available pet prefabs in the game (must be updated with the full list of available pets)

	//reference to gameobjects (UI)
	public GameObject moneyText;
	public GameObject gameTimer;
	public GameObject missionGoldTime;
	public GameObject missionFailTime;

	private bool canTap;

	//Audioclips
	public AudioClip buySfx;
	public AudioClip notPossibleSfx;
	public AudioClip singleMissionPassSfx;
	public AudioClip levelWonSfx;
	public AudioClip menuTap;

	//non-editable game timer variables
	private string remainingTime;
	private float gameTime;
	private int seconds;
	private int minutes;

	//Optional Objectives at the start of game (3d UI)
	public GameObject[] availableMissionObjectsAtStart;
	public GameObject[] missionTargetAmountsTextAtStart;
	public GameObject missionGoldTimeAtStart;
	public GameObject missionFailTimeAtStart;

	//Finish plane settings.
	//These variable are used to show the level statistics on the finish plane (3d UI)
	public GameObject finishPlane;
	public GameObject levelStatus;
	public GameObject levelName;
	public GameObject levelPrize;
	public GameObject levelExtraPrize;
	public GameObject totalMoney;
	public GameObject yourTime;
	public GameObject goldTime;
	public Texture2D[] statusImages;
	private float buttonAnimationSpeed = 9;
	private bool isFinishPlaneActive;

	void Awake () {

		//debug
		//PlayerPrefs.DeleteAll();

		gameIsFinished = false;
		gameIsWon = false;
		playerMoney = startingMoney;
		gameTime = 0;
		seconds = 0;
		minutes = 0;

		missionGoldTime.SetActive(true);		//shows gold time with extra prize
		missionFailTime.SetActive(false);		//when the gold time has passed, we show the standard time instead.

		missionGoldTime.GetComponent<TextMesh>().text = formatTime(levelGoldTime);
		missionFailTime.GetComponent<TextMesh>().text = formatTime(levelFailTime);

		missionGoldTimeAtStart.GetComponent<TextMesh>().text = formatTime(levelGoldTime);	//Optional
		missionFailTimeAtStart.GetComponent<TextMesh>().text = formatTime(levelFailTime);	//Optional

		canTap = true;
		isFinishPlaneActive = false;
		finishPlane.SetActive(false);
	}

	/// <summary>
	/// show the missions with a slight delay
	/// </summary>
	IEnumerator Start(){
		yield return new WaitForSeconds(0.02f);
		setupMissionObjectives();
	}


	/// <summary>
	/// Show the missions on the UI
	/// </summary>
	void setupMissionObjectives() {
		print ("We have " + missions.Length + " missions in this level.");

		//disable unused missions
		if(missions.Length < 4) {
			for(int i = missions.Length; i < 4; i++) {
				availableMissionObjects[i].SetActive(false);
				availableMissionObjectsAtStart[i].SetActive(false); //Optional
			}
		}

		//set mission images & values
		for(int j = 0; j < missions.Length; j++) {
			availableMissionObjects[j].GetComponent<Renderer>().material = resolveMissionMaterial(missions[j]);
			missionTargetAmountsText[j].GetComponent<TextMesh>().text = missions[j].requiredAmount.ToString();

			availableMissionObjectsAtStart[j].GetComponent<Renderer>().material = resolveMissionMaterial(missions[j]); 	//Optional
			missionTargetAmountsTextAtStart[j].GetComponent<TextMesh>().text = missions[j].requiredAmount.ToString();	//Optional
		}
	}


	/// <summary>
	/// Change the mission image based on the type of the mission
	/// Note: if you define new mission types, you have to create new cases in here...
	/// </summary>
	/// <returns>The mission material.</returns>
	/// <param name="missionType">Mission type.</param>
	public Material resolveMissionMaterial(MissionConfig missionType) {
		Material outMat = null;
		switch(missionType.missionType) {
		case MissionConfig.missions.MoneyRequired:
			outMat = availableMissionImages[0];
			break;
		case MissionConfig.missions.EggsRequired:
			outMat = availableMissionImages[1];
			break;
		case MissionConfig.missions.BreadRequired:
			outMat = availableMissionImages[2];
			break;
		case MissionConfig.missions.CakeRequired:
			outMat = availableMissionImages[3];
			break;
		case MissionConfig.missions.MilkRequired:
			outMat = availableMissionImages[4];
			break;
		case MissionConfig.missions.MeatRequired:
			outMat = availableMissionImages[5];
			break;
		case MissionConfig.missions.ChickenRequired:
			outMat = availableMissionImages[6];
			break;
		case MissionConfig.missions.CowRequired:
			outMat = availableMissionImages[7];
			break;
		default:
			outMat = availableMissionImages[0];
			break;
		}
		return outMat;
	}
	

	/// <summary>
	/// FSM
	/// </summary>
	void Update () {

		//Check user inputs
		StartCoroutine(touchManager());

		if(gameIsFinished)
			return;

		//check for gameover by running out of time
		if(gameTime > levelFailTime) {
			gameIsFinished = true;
			gameIsWon = false;
			initFinishPlane();
		}

		//show player money in UI
		moneyText.GetComponent<TextMesh>().text = playerMoney.ToString();

		//Check if player can buy new pets ingame
		checkPetButtonState();

		//update game timer
		runGameTimer();

		//StartCoroutine(touchManager());

		monitorMissionObjectives();

		//Debug. fast restart
		if(Input.GetKeyUp(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		//cheat. more money
		if(Input.GetKeyUp(KeyCode.M))
			playerMoney += 1000;
	}


	/// <summary>
	/// Check if player has passed the assigned missions
	/// </summary>
	void monitorMissionObjectives() {

		int passedMissions = 0;

		//get mission stats
		for(int i = 0; i < missions.Length; i++) {

			//check current amount of each required items
			missionCurrentAmountsText[i].GetComponent<TextMesh>().text = resolveMissionCurrentInventory(missions[i]).ToString();

			//check if all missions are passed
			if(resolveMissionCurrentInventory(missions[i]) >= missions[i].requiredAmount)
				passedMissions++;

			if(passedMissions == missions.Length) {
				//Stop the game. We have sucessfully beat all missions.
				gameIsFinished = true;
				gameIsWon = true;
				print ("Level WON!!!");
				playSfx (levelWonSfx);
				//activate win plane
				initFinishPlane();
			}
		}
	}


	/// <summary>
	/// Show the finish statistics plane
	/// </summary>
	void initFinishPlane() {

		if(isFinishPlaneActive)
			return;

		isFinishPlaneActive = true;
		finishPlane.SetActive(true);

		//set level status
		if(gameIsWon)
			levelStatus.GetComponent<Renderer>().material.mainTexture = statusImages[0];
		else
			levelStatus.GetComponent<Renderer>().material.mainTexture = statusImages[1];
		
		//set level name
		levelName.GetComponent<TextMesh>().text = "Level " + convertToInt(SceneManager.GetActiveScene().name);

		//set times
		yourTime.GetComponent<TextMesh>().text = formatTime((int)gameTime);
		goldTime.GetComponent<TextMesh>().text = formatTime(levelGoldTime);
		
		//set prizes
		if(gameIsWon) {
			levelPrize.GetComponent<TextMesh>().text = missionPrize.ToString();
			levelExtraPrize.GetComponent<TextMesh>().text = missionGoldPrize.ToString();
			totalMoney.GetComponent<TextMesh>().text = (PlayerPrefs.GetInt("PlayerMoney") + missionPrize + missionGoldPrize).ToString();
		} else {
			levelPrize.GetComponent<TextMesh>().text = "0";
			levelExtraPrize.GetComponent<TextMesh>().text = "0";
			totalMoney.GetComponent<TextMesh>().text = PlayerPrefs.GetInt("PlayerMoney").ToString();
		}

		//save the status
		if(gameIsWon) {
			PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + missionPrize + missionGoldPrize);
			//save the progress, if and only if this is the first time player is beating this level
			if(PlayerPrefs.GetInt("userLevelAdvance") < convertToInt(SceneManager.GetActiveScene().name))
				PlayerPrefs.SetInt("userLevelAdvance", PlayerPrefs.GetInt("userLevelAdvance") + 1);
		}
	}


	/// <summary>
	/// Check the amount of stored item required to beat the level missions
	/// </summary>
	/// <returns>The mission current inventory.</returns>
	/// <param name="missionType">Mission type.</param>
	public int resolveMissionCurrentInventory(MissionConfig missionType) {
		int currentInv = 0;
		switch(missionType.missionType) {
			
		case MissionConfig.missions.MoneyRequired:
			currentInv = playerMoney;
			break;

		//query the Hunger to get th amount of items
		
		case MissionConfig.missions.EggsRequired:
			currentInv = HungerController.checkItemAvailability(1);
			break;
			
		case MissionConfig.missions.BreadRequired:
			currentInv = HungerController.checkItemAvailability(2);
			break;
			
		case MissionConfig.missions.CakeRequired:
			currentInv = HungerController.checkItemAvailability(3);
			break;
			
		case MissionConfig.missions.MeatRequired:
			currentInv = HungerController.checkItemAvailability(4);
			break;
			
		case MissionConfig.missions.MilkRequired:
			currentInv = HungerController.checkItemAvailability(5);
			break;
		
		//Find all pets

		case MissionConfig.missions.ChickenRequired:
			currentInv = GameObject.FindGameObjectsWithTag("Chicken").Length;
			break;
			
		case MissionConfig.missions.CowRequired:
			currentInv = GameObject.FindGameObjectsWithTag("Cow").Length;
			break;
			
		default:
			currentInv = 0;
			break;
		}
		return currentInv;
	}


	/// <summary>
	/// check available pets in this level that can be purchased inside the game.
	/// if player doesn't have enough money, the pet purchase button will deactivate automatically.
	/// </summary>
	public void checkPetButtonState() {
		for(int i = 0; i < petButtons.Length; i++) {
			petButtonsPriceText[i].GetComponent<TextMesh>().text = petButtonsPrice[i].ToString();
			if(playerMoney >= petButtonsPrice[i]) {
				petButtons[i].GetComponent<Renderer>().material.mainTexture = petBtnImage[i].onImage;
				petButtons[i].GetComponent<BoxCollider>().enabled = true;
			} else {
				petButtons[i].GetComponent<Renderer>().material.mainTexture = petBtnImage[i].offImage;
				petButtons[i].GetComponent<BoxCollider>().enabled = false;
			}
		}
	}


	//***************************************************************************//
	// Game clock manager
	//***************************************************************************//
	void runGameTimer (){
		
		if(gameIsFinished)
			return;
			
		gameTime = (int)Time.timeSinceLevelLoad;
		seconds = Mathf.CeilToInt(gameTime) % 60;
		minutes = Mathf.CeilToInt(gameTime) / 60; 
		//remainingTime = string.Format("{0:00} : {1:00}", minutes, seconds); 
		gameTimer.GetComponent<TextMesh>().text = string.Format("{0:00}:{1:00}", minutes, seconds).ToString();

		if(gameTime > levelGoldTime && gameTime < levelFailTime) {

			//disable extra prize
			missionGoldPrize = 0;

			missionGoldTime.SetActive(false);
			missionFailTime.SetActive(true);
		}
	}

	/// <summary>
	/// Gets the time as int and returns it in a nicley formatted string (for example 100 => 01':40")
	/// </summary>
	/// <returns>The time.</returns>
	/// <param name="_time">_time.</param>
	string formatTime(int _time) {
		int s = Mathf.CeilToInt(_time) % 60;
		int m = Mathf.CeilToInt(_time) / 60; 
		return string.Format("{0:0}:{1:00}", m, s).ToString();
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
			case "Button-Pet-01":
				canTap = false;	
				buyPet(0);
				StartCoroutine(reactiveTap());
				break;
			case "Button-Pet-02":
				canTap = false;	
				buyPet(1);
				StartCoroutine(reactiveTap());
				break;

			case "MenuBtn":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				SceneManager.LoadScene("Menu");
				break;
			case "RestartBtn":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				break;
			case "NextLevelBtn":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				yield return new WaitForSeconds(1.0f);
				//load next level
				SceneManager.LoadScene("Map");
				break;
			}			
		}
	}


	/// <summary>
	/// Buy a new pet in the game
	/// </summary>
	/// <param name="_petId">_pet identifier.</param>
	void buyPet(int _petId) {
		//****************************************
		// ID 0 = Chicken
		// ID 1 = Cow
		// ...
		//****************************************
		playSfx (buySfx);
		playerMoney -= petButtonsPrice[_petId];
		GameObject newPet = Instantiate(availablePets[_petId], new Vector3(0,0,-0.1f), Quaternion.Euler(0,180,0)) as GameObject;
		newPet.name = "Pet-" + UnityEngine.Random.Range(0, 1000).ToString();
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
		
		//if(r >= 1)
			//canTap = true;
	}
	
	
	int convertToInt ( string stringContainingNumber  ){
		string numbersOnly = Regex.Replace(stringContainingNumber, "[^0-9]", "");
		return Convert.ToInt32(numbersOnly);
	}


	IEnumerator reactiveTap() {
		yield return new WaitForSeconds(0.5f);
		canTap = true;
	}

	void playSfx (AudioClip _sfx){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}

}
