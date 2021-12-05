using UnityEngine;
using System.Collections;

public class PlayerInputManager : MonoBehaviour {

	/// <summary>
	/// This class handles player inputs inside game scene.
	/// </summary>

	private GameObject gc;			//reference to main game controller object
	private GameObject well;		//reference to well game object

	public GameObject grass;		//grass object prefab to plant on the ground
	public bool isFree = true;

	//audio
	public AudioClip grassPlantSfx;
	public AudioClip emptySfx;
	public AudioClip cageBuild;

	//redArrow
	public GameObject redArrow;						//used to show if this is not a suitable place to plant grass

	//cage for capturing animals (next updates)
	public GameObject cage;


	void Awake() {
		gc = GameObject.FindGameObjectWithTag("GameController");
		well = GameObject.FindGameObjectWithTag("Well");
	}


	void Update (){

		if(PauseManager.isPaused)
			return;

		checkIsFree ();

		Vector3 a = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 20));

		//always snap PlayerInputManager object to mouse x/y plane
		transform.position = new Vector3(a.x, a.y, -0.1f);
		
		//if we are not picking an egg or killing a bear, then
		if (Input.GetMouseButtonDown(0)) {

			//check if this place is free and available to plant grass
			if (a.x >= gc.GetComponent<GameController>().plantLimitLeft && 
			    a.x <= gc.GetComponent<GameController>().plantLimitRight && 
			    a.y >= gc.GetComponent<GameController>().plantLimitDown && 
			    a.y <= gc.GetComponent<GameController>().plantLimitUp && 
			    checkIsFree() ) {

				//no grass planting if we ran out of water!
				if(well.GetComponent<WellController>().currentCapacity < 1 || 
				   well.GetComponent<WellController>().isRecharging) {

					playSfx(emptySfx);
					StartCoroutine(showRedArrow());
					return;
				}


				//print ("new grass planted.");
				GameObject tmpGrass = Instantiate(grass, new Vector3(a.x, a.y, -0.05f), Quaternion.Euler(0, 180, 0)) as GameObject;
	
				tmpGrass.name = "Grass";
				tmpGrass.tag = "Food";
			
		
				//decrease a unit from total available water
				well.GetComponent<WellController>().currentCapacity -= 1;

				//play sfx
				playSfx(grassPlantSfx);
			}		
		}
	}

	//show the red arrow indicator that we don't have enough water
	private bool isRedArrowActive = false;
	IEnumerator showRedArrow (){

		if(isRedArrowActive)
			yield break;

		if(!isRedArrowActive) {
			isRedArrowActive = true;
			redArrow.SetActive(true);
			yield return new WaitForSeconds(2.0f);
			redArrow.SetActive(false);
			isRedArrowActive = false;
		}
	}



	/// <summary>
	/// Checks if the position we are clicking/touching is suitable to plant grass
	/// </summary>
	private Ray ray;
	private RaycastHit hit;
	public bool checkIsFree () {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Do a raycast
		if (Physics.Raycast (ray, out hit)) {
			if(hit.transform.gameObject.tag == "Product" ||
				hit.transform.gameObject.tag == "Well") {
				isFree = false;
			} else
				isFree = true;
		} else {
			//print ("I'm looking at nothing!");
			isFree = true;
		}

		return isFree;
	}


	void OnTriggerStay ( Collider other  ){
		//print("Collision detected");
	}


	void OnTriggerExit ( Collider other  ){
		//print("No Collision");
	}

	void playSfx (AudioClip _sfx){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}

	void playOneshotSfx (AudioClip _sfx){
		GetComponent<AudioSource>().PlayOneShot(_sfx);
	}
}