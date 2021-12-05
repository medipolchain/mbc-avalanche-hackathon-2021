using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour {

	/// <summary>
	/// this is the main shop manager class.
	/// it gives you the tools and options required for selling virtual items (for real money via IAB)
	/// inside your game. You need to integrate your own IAB system into the project, then use the IAP functions 
	/// in the places that is indicated inside this class.
	/// </summary>

	private float buttonAnimationSpeed = 9;	//speed on animation effect when tapped on button
	private bool canTap = true;				//flag to prevent double tap

	public AudioClip tapSfx;				//buy sfx

	private int playerMoney;
	private int playerEnergy;

	public GameObject BuyEnergyPlane;
	public GameObject BuyCoinPlane;

	//*****************************************************************************
	// Init. 
	//*****************************************************************************
	void Awake (){
		BuyEnergyPlane.SetActive(false);
		BuyCoinPlane.SetActive(false);

		playerMoney = PlayerPrefs.GetInt("PlayerMoney");
		playerEnergy = PlayerPrefs.GetInt("PlayerEnergy");
	}

	//*****************************************************************************
	// FSM 
	//*****************************************************************************
	void Update (){	
		if(canTap) {
			StartCoroutine(tapManager());
		}

		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Menu");
	}

	//*****************************************************************************
	// This function monitors player touches on menu buttons.
	// detects both touch and clicks and can be used with editor, handheld device and 
	// every other platforms at once.
	//*****************************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
	//private string saveName = "";
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
			
			case "BuyEnergy":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				//play sfx
				playSfx(tapSfx);
				//Wait
				yield return new WaitForSeconds(0.25f);
				//activate BuyEnergyPlane
				BuyEnergyPlane.SetActive(true);
				break;

			case "BuyCoin":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				//play sfx
				playSfx(tapSfx);
				//Wait
				yield return new WaitForSeconds(0.25f);
				//activate BuyEnergyPlane
				BuyCoinPlane.SetActive(true);
				break;

			//IAP
			case "BuyEnergyItem-01":
				canTap = false;
				StartCoroutine(animateButton(objectHit));

				//if we have anough ingame money to buy this item...
				if(playerMoney >= 50) {
					playSfx(tapSfx);
					yield return new WaitForSeconds(0.2f);

					///Place your IAP functions here////

					//deduct money
					playerMoney -= 50;
					PlayerPrefs.SetInt("PlayerMoney", playerMoney);
					//add energy
					playerEnergy += 2;
					PlayerPrefs.SetInt("PlayerEnergy", playerEnergy);
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
				break;

			//IAP
			case "BuyEnergyItem-02":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				
				if(playerMoney >= 110) {
					playSfx(tapSfx);
					yield return new WaitForSeconds(0.2f);

					///Place your IAP functions here////

					//deduct money
					playerMoney -= 110;
					PlayerPrefs.SetInt("PlayerMoney", playerMoney);
					//add energy
					playerEnergy += 5;
					PlayerPrefs.SetInt("PlayerEnergy", playerEnergy);
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
				break;

			//IAP
			case "BuyEnergyItem-03":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				
				if(playerMoney >= 200) {
					playSfx(tapSfx);
					yield return new WaitForSeconds(0.2f);

					///Place your IAP functions here////

					//deduct money
					playerMoney -= 200;
					PlayerPrefs.SetInt("PlayerMoney", playerMoney);
					//add energy
					playerEnergy += 10;
					PlayerPrefs.SetInt("PlayerEnergy", playerEnergy);
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
				break;

			//IAP
			case "BuyCoinItem-01":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				playSfx(tapSfx);
				yield return new WaitForSeconds(0.2f);

				///Place your IAP functions here////

				//debug
				playerMoney += 200;
				PlayerPrefs.SetInt("PlayerMoney", playerMoney);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				break;

			//IAP
			case "BuyCoinItem-02":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				playSfx(tapSfx);
				yield return new WaitForSeconds(0.2f);

				///Place your IAP functions here////

				//debug
				playerMoney += 500;
				PlayerPrefs.SetInt("PlayerMoney", playerMoney);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				break;
			
			//IAP
			case "BuyCoinItem-03":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				playSfx(tapSfx);
				yield return new WaitForSeconds(0.2f);

				///Place your IAP functions here////

				//debug
				playerMoney += 1000;
				PlayerPrefs.SetInt("PlayerMoney", playerMoney);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				break;


			case "BtnCancel":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				playSfx(tapSfx);
				yield return new WaitForSeconds(0.15f);
				BuyEnergyPlane.SetActive(false);
				BuyCoinPlane.SetActive(false);
				break;

			case "BtnBack":
				canTap = false;
				StartCoroutine(animateButton(objectHit));
				playSfx(tapSfx);
				yield return new WaitForSeconds(1.0f);
				SceneManager.LoadScene("Menu");
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

}