using UnityEngine;
using System.Collections;

public class FactoryController : MonoBehaviour {

	/// <summary>
	/// This is the main factory controller class.
	/// A factory is a gameobject which takes a product, performs some processing, and delivers another product
	/// as output. We already have 2 factories that can be used inside the kit. (we will add more in the next updates)
	/// Each factory needs some specific properties in order to work. continue reading through the script to get to know
	/// the variables.
	/// </summary>

	//available factory types
	public enum Factories { EggToBread, BreadToCake, AnimalToMeat }
	public Factories FactoryType = Factories.EggToBread;

	//Gameobject references
	private GameObject hunger;

	public int requiredItems = 1;			//how many items as the input is accepted?
	public bool isWorking = false;			
	public float productionTime = 5.0f;		//total processing time

	//production
	public GameObject inputItem;			//the item that should be fed to the factory
	public GameObject outputItem;			//the item that factory delivers after procesing
	internal Vector3 inputPosition;			//position where the input item starts to move from
	public Vector3 outputPosition;			//position where the output item will appear

	//gui vars 
	public GameObject factoryProgressBar;
	private float fullProgressBarLength;

	//audio
	public AudioClip clickSfx;


	void Awake() {

		//prevent showing any animation
		GetComponent<SizeAnimator>().enabled = false;
		hunger = GameObject.FindGameObjectWithTag("Hunger");
		inputPosition = hunger.transform.position;
		outputPosition = transform.position + new Vector3(0, -0.5f, 0);

		fullProgressBarLength = factoryProgressBar.transform.localScale.y;
		factoryProgressBar.transform.localScale = new Vector3(factoryProgressBar.transform.localScale.x,
		                                                      0.001f,
		                                                      factoryProgressBar.transform.localScale.z);
	}


	void OnMouseDown (){
		//first check to see if user has enough input items to sent to this factory
		if( hasEnoughInputItems(FactoryType) && !isWorking) {

			//prevent double routine
			isWorking = true;	

			//decrease a unit from hunger items
			HungerController.removeItem( factoryTypeToInt(FactoryType), requiredItems);

			//animate : send one item from hunger to factory
			StartCoroutine(animateItem(inputItem, inputPosition, gameObject.transform.position, 1.0f));

			//play factory animation (only if we are going to use an atlas image for the animation)
			//GetComponent<SizeAnimator>().enabled = false;

			//play some sfx
			playOneshotSFX(clickSfx);	

		} else {
			//play fools sfx
			playOneshotSFX(clickSfx);
		}
	}

	/// <summary>
	/// Check if player has enough starting items to send to this factory and let it start?
	/// </summary>
	bool hasEnoughInputItems (Factories _fType){

		int howManyInputItemsAvailable = 0;
		//query hunger class and get the number of available items
		howManyInputItemsAvailable = HungerController.checkItemAvailability( factoryTypeToInt(_fType) );

		if(howManyInputItemsAvailable >= requiredItems)
			return true;
		else
			return false;
	}	

	/// <summary>
	/// We assign an ID (int) to each factory to ease the process of working with them. 
	/// (working with IDs instead of names)
	/// </summary>
	int factoryTypeToInt(Factories _fType) {
		int type = 1;
		switch(_fType) {
		case Factories.EggToBread:
			type = 1;
			break;
		case Factories.BreadToCake:
			type = 2;
			break;
		case Factories.AnimalToMeat:
			type = 3;
			break;
		}
		return type;
	}

	/// <summary>
	/// move an item form Hunger to the Factory
	/// </summary>
	IEnumerator animateItem (GameObject ga, Vector3 i, Vector3 o, float _time){
		float t = 0.0f; 
		GameObject item = Instantiate(ga, i, Quaternion.Euler(0, 180, 0)) as GameObject;
		while (t <= 1.0f) {		
			t += Time.deltaTime * _time; 
			//go to target position.
			item.transform.position = new Vector3(Mathf.Lerp(i.x, o.x, t),
			                                      Mathf.Lerp(i.y, o.y, t),
			                                      -0.75f);
			yield return 0;
		}

		if(Vector3.Distance(item.transform.position, o) <= 1.0f) {
			//destroy the item
			Destroy(item);
			//animate the factory's production system
			StartCoroutine(animateFactory(productionTime));
			//calculate progressBar
			StartCoroutine(setProgressBar(productionTime));

		}
	}

	/// <summary>
	/// Resize factory's scales to simulate a simple animation.
	/// </summary>
	IEnumerator animateFactory (float _time){
		GetComponent<SizeAnimator>().enabled = true;
		yield return new WaitForSeconds(_time);
		GetComponent<SizeAnimator>().enabled = false;
		//activate again
		isWorking = false;
		//reset progressBar
		factoryProgressBar.transform.localScale = new Vector3(factoryProgressBar.transform.localScale.x,
		                                                      0.001f,
		                                                      factoryProgressBar.transform.localScale.z);
		//create the product
		productCreate(outputItem, outputPosition);
	}


	/// <summary>
	/// Create the output item
	/// </summary>
	void productCreate (GameObject o, Vector3 p){
		Vector3 outPos = p;
		outPos += new Vector3(0, -1.75f, 0);
		GameObject outputItem = Instantiate(o, outPos, Quaternion.Euler(0, 180, 0)) as GameObject;
		outputItem.name = "FactoryProduct-" + Random.Range(0, 1000).ToString();
	}


	/// <summary>
	/// Sets a 3d progress bar inside the factory to show the progress of making the item.
	/// you can easily use the "progressBarLength" inside your custome GUI system to show the progress
	/// in a different way.
	/// </summary>
	IEnumerator setProgressBar(float _time) {

		float t = 0;
		float startTime = Time.time;
		float endTime = startTime + _time;
		float progressBarLength = 0;

		while(t <= _time) {
			t += Time.deltaTime * 1.0f;
			progressBarLength = fullProgressBarLength * (1 - ((endTime - Time.time) / _time));
			//print (fullProgressBarLength + " - " + progressBarLength);
			factoryProgressBar.transform.localScale = new Vector3(factoryProgressBar.transform.localScale.x,
			                                                  	  progressBarLength,
			                                                      factoryProgressBar.transform.localScale.z);
			//print (startTime + "-" +  endTime + "-" + t + "-" + progressBarLength);
			yield return 0;
		}
	}

	void playOneshotSFX (AudioClip _sfx){
		GetComponent<AudioSource>().PlayOneShot(_sfx);
	}

	void playSfx (AudioClip _sfx){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}
}