#pragma warning disable 414

using UnityEngine;
using System.Collections;

public class TruckManager : MonoBehaviour {

	/// <summary>
	/// Truck object is used to sell items collected in the hunger. It will loads all items at once,
	/// get them to city market and gives you the sell money after returning to your farm.
	/// 
	/// In the next update, we will add the option to select the products you want to load the truch to sell.
	/// </summary>

	private bool canTap;
	private GameObject hunger;				//reference to Hunger gameobject

	public GameObject mapTruck;				//reference to small truck in the road map
	public GameObject mapTruckBody;			//Body of the maptruck object
	public GameObject mapTruckMoneyText;	//text title of the maptruck object used to show the price of the items

	public AudioClip carHorn;
	public AudioClip notPossibleSfx;


	void Awake () {
		canTap = true;
		mapTruck.SetActive(false);
		mapTruckMoneyText.GetComponent<TextMesh>().text = "";
		hunger = GameObject.FindGameObjectWithTag("Hunger");
	}
	

	void Update () {
		StartCoroutine(touchManager());
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
				
			case "Truck":
				canTap = false;	
				StartCoroutine(reactiveTap());
				StartCoroutine(exportHungerContent());
				break;	
			}
		}
	}


	/// <summary>
	/// Here we load all available items in the hunger into truck object 
	/// (by calculating their prices and deleting them from hunger master array)
	/// then we wait for a certain time to simulate the going/return of the truck and 
	/// at the end, we will add the price of the sold items to player money.
	/// </summary>
	IEnumerator exportHungerContent() {

		//return if we have nothing to sell
		if(HungerController.totalItemsPrice <= 0) {
			playSfx (notPossibleSfx);
			yield break;
		}

		int itemsPrice = HungerController.totalItemsPrice;
		HungerController.availableProductIDs.Clear();
		mapTruckMoneyText.GetComponent<TextMesh>().text = itemsPrice.ToString();
		mapTruck.SetActive(true);
		GetComponent<Renderer>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;

		//move mapCar to destination and back to origin
		Vector3 origin = mapTruck.transform.position;
		Vector3 Destination = mapTruck.transform.position + new Vector3(2.3f, 0, 0);

		float t = 0;
		while(t < 1) {

			t += Time.deltaTime * 0.1f;
			float v, w, dir;
			v = t * 2;
			w = (t * 2) - 1;

			if(t < 0.5f) {
				dir = 1;
				mapTruck.transform.position = new Vector3(Mathf.Lerp(origin.x, Destination.x, v),
				                                          mapTruck.transform.position.y,
				                                          mapTruck.transform.position.z);
			} else {
				dir = -1;
				mapTruck.transform.position = new Vector3(Mathf.Lerp(Destination.x, origin.x, w),
				                                          mapTruck.transform.position.y,
				                                          mapTruck.transform.position.z);
			}

			mapTruckBody.transform.localScale = new Vector3(Mathf.Abs (mapTruckBody.transform.localScale.x) * dir,
			                                                mapTruckBody.transform.localScale.y,
			                                                mapTruckBody.transform.localScale.z);


			if(mapTruck.transform.position.x <= origin.x) {
				playSfx (carHorn);
				mapTruck.SetActive(false);
				GameController.playerMoney += itemsPrice;
				GetComponent<Renderer>().enabled = true;
				GetComponent<BoxCollider>().enabled = true;
			}
			yield return 0;
		}
	}


	IEnumerator reactiveTap() {
		yield return new WaitForSeconds(0.25f);
		canTap = true;
	}


	void playSfx (AudioClip _sfx){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}
}
