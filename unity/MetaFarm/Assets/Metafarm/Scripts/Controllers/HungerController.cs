using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HungerController : MonoBehaviour {

	/// <summary>
	/// This is the main hunger controller class.
	/// Hunger stores all products that is picked up by the player and provide useful reports for other classes when requested.
	/// Hunger has a limited size and each product requires a certain amount of free space to be stored.
	/// If hunger ran out of free space, it lets the player know and player has to use the truck to sell stored items to free up hunger's space.
	/// 
	/// </summary>

	public static int totalSpace = 40;				//Important. Changing this variable needs some tweak in other variables.
	public static float freeSpace;					//available free space to store items (products)
	public static float usedSpace;					//totalSpace - freeSpace = usedSpace
	public static int totalItemsPrice;				//quick reference. price of all the items available in the hunger

	//We store all products inside the hunger in a vector3, because this way we have instant access to all parameters of an item inside the hunger
	public static List<Vector3> availableProductIDs;		//Vector3(productID, requiredSpace, productPrice)

	private GameObject hungerArrow;					//helper arrow

	//UI helpers
	public GameObject hungerProgressBar;
	private float fullProgressBarLength;

	public GameObject[] hungerItems;				//used when we want to use digits to show stored items inside the hunger
	public GameObject[] HungerItemAmount;			//3d text label for items

	public Material[] HungerItemsImage;				//we can show all the stored products (inside the array) in a nice visual icons in the UI.
													//to do so, we need an image of all storable items.
	public GameObject[] hungerItemDummies;			//For a hunger with 40 space, we need 40 dummy objects to show the stored products inside the hunger UI.
													//These dummies can be easily re-positined via unity editor.


	void Awake () {
		freeSpace = totalSpace;
		usedSpace = 0;
		totalItemsPrice = 0;

		fullProgressBarLength = hungerProgressBar.transform.localScale.y;	//Show the completely free hunger space
		hungerArrow = GameObject.FindGameObjectWithTag("HungerArrow");		
		availableProductIDs = new List<Vector3>();							//init the main list of stored products
	}

	void Start() {
		if(hungerArrow) {
			//print ("Found hungerArrow!!");
			hungerArrow.SetActive(false);
			hungerArrow.GetComponent<Renderer>().enabled = true;
		}
	}
	

	void Update () {

		calculateHungerSpace();

		showHungerItemsInGame();

		renderHungerItemsIngame();

		//debug
		/*
		print ("****************************************");
		print ("Total Items in Hunger: " + availableProductIDs.Count);
		for(int i = 0; i < availableProductIDs.Count; i++) {
			print ("Item " + (i+1) + " ID: " + 
			       availableProductIDs[i].x + 
			       "(id) - " + availableProductIDs[i].y +
			       "(space) - " + availableProductIDs[i].z + "(price)");
		}
		print ("****************************************");
		*/
	}


	/// <summary>
	/// This function shows an image for each stored products in the hunger UI.
	/// </summary>
	void renderHungerItemsIngame() {

		//hide free hunger cells
		for(int j = availableProductIDs.Count; j < totalSpace; j++) {
			hungerItemDummies[j].GetComponent<Renderer>().enabled = false;
		}

		//show stored items in the respective positions inside the hunger object
		for(int i = 0; i < availableProductIDs.Count; i++) {
			hungerItemDummies[i].GetComponent<Renderer>().enabled = true;
			hungerItemDummies[i].GetComponent<Renderer>().material = HungerItemsImage[(int)availableProductIDs[i].x - 1];
		}
	}

	//shows the number of each stored items.
	void showHungerItemsInGame() {
		for(int i = 0; i < hungerItems.Length; i++) {
			HungerItemAmount[i].GetComponent<TextMesh>().text = "x" + checkItemAvailability(i+1).ToString();
		}
	}

	//checks if a particular item is available in hunger.
	//returns the number of items if there is any and returns 0 if there is none.
	public static int checkItemAvailability(int _itemID) {
		int itemsAvalable = 0;
		for(int i = 0; i < availableProductIDs.Count; i++) {
			if(availableProductIDs[i].x == _itemID) {
				itemsAvalable++;
			}
		}

		//print ("We have " + itemsAvalable + " product with ID " + _itemID);
		return itemsAvalable;
	}

	/// <summary>
	/// Removes the item from the hunger by removing it from the main array.
	/// </summary>
	/// <param name="_itemID">_item I.</param>
	/// <param name="_amount">_amount.</param>
	public static void removeItem(int _itemID, int _amount) {
		int howMany = 0;
		for(int i = 0; i < availableProductIDs.Count; i++) {
			if(availableProductIDs[i].x == _itemID) {
				availableProductIDs.RemoveAt(i);
				howMany++;

				if(howMany >= _amount)
					return;
			}
		}
	}

	/// <summary>
	/// Calculates the hunger space and update the hunger bars in the UI
	/// </summary>
	void calculateHungerSpace() {
		
		usedSpace = 0;
		totalItemsPrice = 0;
		for(int i = 0; i < availableProductIDs.Count; i++) {
			usedSpace += (int)availableProductIDs[i].y;
			totalItemsPrice += (int)availableProductIDs[i].z;
		}

		freeSpace = totalSpace - usedSpace;

		float progressBarLength = fullProgressBarLength * (1 - ((totalSpace - usedSpace) / totalSpace));
		hungerProgressBar.transform.localScale = new Vector3(hungerProgressBar.transform.localScale.x,
		                                                  	 progressBarLength,
		                                                     hungerProgressBar.transform.localScale.z);

		//print ("Used Space: " + usedSpace);
		//print ("Free Space: " + freeSpace);
		//print ("totalItemsPrice: " + totalItemsPrice);
	}


	public IEnumerator showHungerArrow (){
		hungerArrow.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		hungerArrow.SetActive(false);
	}
}
