using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PetManager : MonoBehaviour {

	/// <summary>
	/// This is the main pet controller. This class can control unlimited number of pets.
	/// You just need to set the ID, product and some minor attributes of each pet to setup a brand new pet.
	/// This class controls all behaviors of a pet inside the game, like:
	/// moving
	/// looking for food
	/// feeling hungry
	/// moving to food
	/// eating
	/// dying
	/// ...
	/// 
	/// We have 2 pets for now, but we will add more pets in the next updates.
	/// It's also very easy to add new pets to the game. once you have the textures, all you have to do is to duplicate an already existing pet prefab,
	/// set it's ID, set it's output product and other minor variables. Then you need to assign correct material & sound clips to the new prefab and in 
	/// the end, you must set a new tag for this pet prefab.
	/// </summary>

	//available pet IDs: 
	/*
	 * chicken = 1
	 * cow = 2
	 * ...
	*/
	public int petID;						//unique ID for this pet.

	private Vector3 targetWaypoint;			//used to wander inside the scene.
	public GameObject product;				//the item this pet produce after eating some food
	public bool canProduce = true;

	public int visitsBeforeEat = 5;			//how many times a pet should move until its ready to eat
	private int feelingHungry = 3;			//if pet reach this limit, they start to make sounds

	public float hungerEndurance = 100;		//if this endurance reaches to zero, the pet will die.
	private float speed = 1.25f;			//movement speed
	private float zOffset = -0.1f;

	private GameObject[] food;				//array of all available foods in the scene
	private GameObject myFood;				//selected food object
	private bool isMovingToFood;
	private bool isSearchingForFood;

	//states (optional)
	//public enum AvailableStates {Hunger, Death, Wander, Eat}
	//public AvailableStates State = AvailableStates.Wander;

	//different states of a pet
	public bool wanderState = false;
	public bool hungerState = false;
	public bool eatState = false;
	public bool deathState = false;

	//movement limitations
	private float limitLeft = -4.0f;
	private float limitRight = 4.0f;
	private float limitUp = 1.8f;
	private float limitDown = -2.0f;

	//self variables
	//private Vector3 startingPosition;
	private Vector3 lastPosition;

	//Audio
	public AudioClip hungerSfx;
	public AudioClip deathSfx;
	public AudioClip produceSfx;

	//Materials
	public Material wanderMat;
	public Material eatMat;
	public Material deathMat;

	void Awake (){
		wanderState = false;
		hungerState = false;
		eatState = false;
		deathState = false;

		food = null;
		myFood = null;
		isMovingToFood = false;
		isSearchingForFood = false;
		GetComponent<Renderer>().material = wanderMat;
	}


	void Start (){
		//startingPosition = transform.position;
		lastPosition = transform.position;
		targetWaypoint = GetNewTarget();
	}


	/// <summary>
	/// Get a new position as the move destination
	/// </summary>
	Vector3 GetNewTarget() {
		Vector3 newTarget;
		newTarget = new Vector3(Random.Range(limitLeft, limitRight),
		                        Random.Range(limitUp, limitDown),
		                        zOffset);
		return newTarget;
	}



	/// <summary>
	/// FSM
	/// </summary>
	void Update (){

		if(deathState || GameController.gameIsFinished) {
			return;
		}

		//if this pet is feeling hungry, swith the state and start searching for food
		if(visitsBeforeEat <= feelingHungry) {
			hungerState = true;
		} else {
			hungerState = false;
		}
		

		if(!hungerState)
			freeride();
		else
			searchForFood();

		//just for debug. Delete in a live game.
		if(Input.GetKeyUp(KeyCode.D))
			die();
	}



	/// <summary>
	/// Checks if there is any food available in the scene.q
	/// </summary>
	void searchForFood() {

		if(isSearchingForFood)
			return;
		isSearchingForFood = true;

		food = GameObject.FindGameObjectsWithTag("Food"); //if food is true, we have atleaest one food, else we have none
		if(food.Length < 1) {

			//there is no food
			playSfx(hungerSfx);

			//if we still have endurance, move in the scene by the hope of finding some food
			if(hungerEndurance > 50)
				freeride();	

			if(visitsBeforeEat < 1) {
				//if pet went through 5 target, then start increasing hunger to find food
				//hungerEndurance -= Time.deltaTime * 20;
				hungerEndurance -= 35;

				//move very fast when pet is hungry
				speed = 1.75f;

				//prepare to die
				if(hungerEndurance <= 0) {
					hungerEndurance = 0;
					die();
				}
			}
		} else {
			//find a random food object
			myFood = food[Random.Range(0, food.Length)];
			//print ("myFood: " + myFood.ToString());
			wanderState = true;
			/*print ("last pos: " + lastPosition.ToString() + 
			       "   || food Pos: " + myFood.transform.position.ToString() + 
			       "   || current pos: " + myFood.transform.position.ToString());*/

			StartCoroutine(goToFood(myFood, lastPosition));
		}
	}


	/// <summary>
	/// Moves towards food.
	/// </summary>
	IEnumerator goToFood (GameObject _food, Vector3 _lastPosition) {

		//if(!_food)
			//yield break;

		if(isMovingToFood)
			yield break;
		isMovingToFood = true;

		float t = 0.0f; 
		while (t <= 1) {

			//*****************
			// Important
			//*****************
			if(!_food) {
				//Caution. If this is executing, it means that we are adressing a null food object
				//to avoid hang bug, we add a unit to visit variable of the pet to make it search for food again.
				//we also must reset all other behaviours manually.
				visitsBeforeEat = feelingHungry + 1;
				//store new position
				lastPosition = transform.position;
				//reset behaviours
				isMovingToFood = false;
				isSearchingForFood = false;
				hungerState = false;
				eatState = false;
				wanderState = false;
				//freeride();
				yield break;
			}

			t += Time.deltaTime * 0.5f; 
			//go to target position.
			transform.position = new Vector3(Mathf.Lerp(_lastPosition.x, _food.transform.position.x, t),
			                                 Mathf.Lerp(_lastPosition.y, _food.transform.position.y, t),
			                                 transform.position.z);

			int dir = 0;
			if(_food.transform.position.x >= _lastPosition.x)
				dir = -1;
			else
				dir = 1;
			transform.localScale = new Vector3(Mathf.Abs (transform.localScale.x) * dir,
			                                   transform.localScale.y,
			                                   transform.localScale.z);

			yield return 0;
		}

		float distancToFood = Vector3.Distance(transform.position, _food.transform.position);

		if(distancToFood <= 1.0f) {	
			//print("I'm here");
			//store new position
			lastPosition = transform.position;

			hungerState = false;
			eatState = true;
			wanderState = false;

			isMovingToFood = false;
			isSearchingForFood = false;

			//yield return new Eat(5, myFood);
			StartCoroutine(Eat(3.5f, myFood));
		}
	}


	/// <summary>
	/// Start the eat process by activating eat animation, resetting hunger state, decreasing the health of the eaten food
	/// and producing the pet product
	/// </summary>
	IEnumerator Eat(float _delay, GameObject _myFood){

		GetComponent<AtlasTextureAnimator>().reset();
		GetComponent<Renderer>().material = eatMat;
		
		visitsBeforeEat = 7;
		yield return new WaitForSeconds(_delay);

		GetComponent<AtlasTextureAnimator>().reset();
		GetComponent<Renderer>().material = wanderMat;

		if(_myFood) 
			_myFood.GetComponent<GrassController>().health -= 40;	
		
		eatState = false;

		//produce something
		StartCoroutine(produceSomething());
		//movingState = false;
	}


	/// <summary>
	/// Produces the pet product.
	/// </summary>
	IEnumerator produceSomething (){

		if(canProduce) {
			//set produce to false, cause we just want to create one instance
			canProduce = false;
			//create a product
			yield return new WaitForSeconds(Random.Range(1, 5));
			playSfx(produceSfx);
			GameObject tmpProduct = Instantiate(product, transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
			tmpProduct.name = "PetProduct-" + Random.Range(0, 1000).ToString();
			tmpProduct.transform.position = new Vector3(tmpProduct.transform.position.x,
			                                            tmpProduct.transform.position.y,
			                                            -0.75f);
			yield return new WaitForSeconds(5);
			canProduce = true;
		}
	}

	
	/// <summary>
	/// If not eating, dying! or moving already, move towards the selected target
	/// </summary>
	void freeride() {
		if(!wanderState && !eatState && !deathState) {
			wanderState = true;
			StartCoroutine(moveToTarget(speed, lastPosition));
		}
	}


	/// <summary>
	/// Moves to target.
	/// </summary>
	IEnumerator moveToTarget (float _speed, Vector3 _lastPosition) {

		float t = 0.0f; 
		while (t <= 1.0f) {

			if(deathState)
				yield break;

			t += Time.deltaTime * 0.2f * _speed; 
			//go to target position
			transform.position = new Vector3(Mathf.Lerp(_lastPosition.x, targetWaypoint.x, t),
			                                 Mathf.Lerp(_lastPosition.y, targetWaypoint.y, t),
			                                 zOffset + (transform.position.y / 100));

			int dir = 0;
			if(targetWaypoint.x >= _lastPosition.x)
				dir = -1;
			else
				dir = 1;
			transform.localScale = new Vector3(Mathf.Abs (transform.localScale.x) * dir,
			                                   transform.localScale.y,
			                                   transform.localScale.z);


			//if we've already reached our target
			float distancToTarget = Vector3.Distance(transform.position, targetWaypoint);
			//print ("distancToTarget: " + distancToTarget);
			if(distancToTarget <= 1.0f) {	
				//debug
				//print ("Arrived at: " + targetWaypoint);
				//store new position
				lastPosition = transform.position;
				//decrease visitBeforeEat
				visitsBeforeEat--;
				targetWaypoint = GetNewTarget();
				//i'm stopped and arrived
				wanderState = false;
				isSearchingForFood = false;
				yield break;
			}
			yield return 0;
		}
	}


	/// <summary>
	/// kill this pet.
	/// </summary>
	void die () {

		deathState = true;
		wanderState = false;
		hungerState = false;

		GetComponent<AtlasTextureAnimator>().kill();
		GetComponent<Renderer>().material = deathMat;

		//play die sound
		playSfx(deathSfx);
	}

	void playSfx (AudioClip _sfx){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}

}