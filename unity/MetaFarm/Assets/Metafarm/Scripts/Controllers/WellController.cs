#pragma warning disable 414

using UnityEngine;
using System.Collections;

public class WellController : MonoBehaviour {

	/// <summary>
	/// This is the main controller for water well object.
	/// This object is used to buy water. Water can get used inside the game to plant grass and grass will
	/// be used as the main food for pets. If you ran out of food, you have to buy additional water. 
	/// your pets will die easily if you do not feed them with grass.
	/// </summary>

	//reference to child game objects
	public GameObject waterPriceText;
	public GameObject waterPriceHolder;
	public GameObject wellProgressBar;

	public static int fullCapacity = 5;		//maximum capacity (5 units of water)
	public float currentCapacity;			//available water to use
	public int rechargePrice = 25;			//money needed to refill the well
	public int fillDelay = 5;				//seconds it take to fully charge the water well

	private float fullProgressBarLength;

	private bool canTap;
	private bool canRecharge;
	internal bool isRecharging;

	public AudioClip refillSfx;
	public AudioClip notPossibleSfx;


	void Awake () {
		currentCapacity = fullCapacity;
		fullProgressBarLength = wellProgressBar.transform.localScale.y;

		canRecharge = false;
		isRecharging = false;
		canTap = true;

		//prevent showing any animation
		GetComponent<SizeAnimator>().enabled = false;

		waterPriceText.GetComponent<TextMesh>().text = rechargePrice.ToString();
		waterPriceHolder.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if(currentCapacity < 0)
			currentCapacity = 0;

		setProgressBar();

		if(canTap)
			StartCoroutine(touchManager());

		//check recharge condition
		if(currentCapacity <= 0 && GameController.playerMoney >= rechargePrice) {
			canRecharge = true;
			waterPriceHolder.SetActive(true);
		} else {
			canRecharge = false;
			waterPriceHolder.SetActive(false);
		}
	}


	/// <summary>
	/// show a progress bar inside the well object to demonstrate available water
	/// </summary>
	void setProgressBar() {

		float progressBarLength = fullProgressBarLength * (1 - ((fullCapacity - currentCapacity) / fullCapacity));
		//print (fullProgressBarLength + " - " + progressBarLength);
		wellProgressBar.transform.localScale = new Vector3(wellProgressBar.transform.localScale.x,
			                                               progressBarLength,
		                                                   wellProgressBar.transform.localScale.z);
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
			switch(objectHit.tag) {
				
			case "Well":
				canTap = false;	
				StartCoroutine(reactiveTap());
				StartCoroutine(rechargeWellWater());
				break;	
			}
		}
	}


	IEnumerator reactiveTap() {
		yield return new WaitForSeconds(2.5f);
		canTap = true;
	}


	/// <summary>
	/// Recharge water well.
	/// </summary>
	IEnumerator rechargeWellWater() {
		if(!canRecharge) {
			playSfx(notPossibleSfx);
			yield break;
		}

		//animate well object and fill water level
		GetComponent<SizeAnimator>().enabled = true;

		//deduct recharge price from player money
		GameController.playerMoney -= rechargePrice;

		//set flag
		isRecharging = true;

		float t = 0;
		while(t <= 1) {
			t += Time.deltaTime / fillDelay;
			currentCapacity = Mathf.Lerp(0, fullCapacity, t);
			playSfx(refillSfx);

			if(t >= 1) {
				GetComponent<SizeAnimator>().enabled = false;

				isRecharging = false;
				GetComponent<AudioSource>().Stop();
			}

			yield return 0;
		}
	}

	void playSfx (AudioClip _sfx){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}

}
