using UnityEngine;
using System.Collections;

public class ProductController : MonoBehaviour {

	/// <summary>
	/// The main product controller. This single class will manage the attributes of all available (and new)
	/// products used inside the game.
	/// Each product has a unique ID which will be recognized by all other classes.
	/// Each product has a name, sell price and required space (when moves to hunger and take space)
	/// </summary>

	public int productID = 1;
	internal string productName = "";
	internal int requiredSpace = 0;
	internal int productPrice = 0;

	public int lifeTime = 20; 		//in seconds. After this time, the product gets destroyed.
	public int howManyFlash = 5; 	//flash this seconds before destroy

	private float creationTime;
	private float endTime;

	private bool canTap;
	private GameObject hunger;

	//audio
	public AudioClip takeSfx;
	public AudioClip notPossibleSfx;

	void Awake () {
		resolveProductID();
	}

	void resolveProductID() {
		//resolve product information
		switch(productID) {
		case 1:
			//this ia an egg
			productName = "EGG";
			requiredSpace = 1;
			productPrice = 15;
			break;
		case 2:
			//this ia a Bread
			productName = "BREAD";
			requiredSpace = 2;
			productPrice = 35;
			break;
		case 3:
			//this ia a Cake
			productName = "CAKE";
			requiredSpace = 3;
			productPrice = 60;
			break;
		case 4:
			//this ia a Meat
			productName = "MEAT";
			requiredSpace = 4;
			productPrice = 100;
			break;
		case 5:
			//this ia a Milk
			productName = "MILK";
			requiredSpace = 1;
			productPrice = 30;
			break;
		//here you can add unlimited number of new products
			//...
		}
	}


	void Start () {
		creationTime = Time.time;
		endTime = creationTime + lifeTime - howManyFlash;
		canTap = true;
		hunger = GameObject.FindGameObjectWithTag("Hunger");
		//destroy if idle for lifetime
		Destroy(gameObject, lifeTime);
	}

	void Update (){

		if(canTap)
			StartCoroutine(touchManager());
		
		//start to flash
		if(Time.time >= endTime) {
			StartCoroutine (flash());
		}
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

			if(objectHit.tag != "Product" || objectHit.name != gameObject.name)
				yield break;

			canTap = false;	
			StartCoroutine(reactiveTap());

			if(!checkCollectionPossibility()) {
				playSfx(notPossibleSfx);
				yield break;
			}

			playSfx(takeSfx);							//play touch sound
			StartCoroutine(moveToHunger());				//collect this product

		}
	}


	IEnumerator reactiveTap() {
		yield return new WaitForSeconds(0.5f);
		canTap = true;
	}


	//collect and move this product to hunger
	IEnumerator moveToHunger() {

		//add this products ID to main hunger product's array.
		HungerController.availableProductIDs.Add(new Vector3(productID, requiredSpace, productPrice));

		//move it to hunger
		GameObject hunger = GameObject.FindGameObjectWithTag("Hunger");
		Vector3 lastPos = transform.position;
		float t = 0.0f; 
		while (t <= 1.0f) {		
			t += Time.deltaTime * 2; 
			//go to target position.
			transform.position = new Vector3(Mathf.Lerp(lastPos.x, hunger.transform.position.x, t),
		                               		 Mathf.Lerp(lastPos.y, hunger.transform.position.y, t),
		                                     -0.75f);
			yield return 0;
		}


		Destroy(gameObject);
	}


	//checks if we have enough free space in the hunger to store this product
	bool checkCollectionPossibility() {

		if(HungerController.freeSpace < requiredSpace) {
			StartCoroutine(hunger.GetComponent<HungerController>().showHungerArrow());
			return false;
		} else
			return true;

	}

	/// <summary>
	/// flash the product renderer to help player pick up this object asap.
	/// </summary>
	private bool isFlashing = false;
	IEnumerator flash() {

		if(isFlashing)
			yield break;
		isFlashing = true;

		while(isFlashing) {
			for(int i = 0; i < 1000; i++) {
				//GetComponent<Renderer>().material.color = new Color(1,1,1,0.5f);
				GetComponent<Renderer>().enabled = false;
				yield return new WaitForSeconds(0.25f);
				//GetComponent<Renderer>().material.color = new Color(1,1,1,1);
				GetComponent<Renderer>().enabled = true;
				yield return new WaitForSeconds(0.25f);
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