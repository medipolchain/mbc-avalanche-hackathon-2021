using UnityEngine;
using System.Collections;

public class CareerLevelSetup : MonoBehaviour {
	
	///*************************************************************************///
	/// Simple level setup before starting the game.
	/// This class updates level selection buttons and set their numbers, locked/unlocked status 
	/// and on/off materials.
	/// an unlocked level can not be clicked/touched.
	///*************************************************************************///

	public GameObject label;				//reference to child gameObject
	public int levelID;						//unique level identifier. Starts from 1.
	public Material[] statusMat;			//material for locked/unlocked level

	void Start (){

		label.GetComponent<TextMesh>().text = levelID.ToString();

		//animate the last opened level
		if(CareerMapManager.userLevelAdvance == levelID - 1)
			GetComponent<HeartBeatAnimationEffect>().enabled = true;
		
		if(CareerMapManager.userLevelAdvance >= levelID - 1) {
			//this level is open
			GetComponent<Renderer>().material = statusMat[1];
			GetComponent<BoxCollider>().enabled = true;
		} else {
			//level is locked
			GetComponent<Renderer>().material = statusMat[0];
			GetComponent<BoxCollider>().enabled = false;
		}
	}
}