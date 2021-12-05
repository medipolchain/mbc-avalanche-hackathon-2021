using UnityEngine;
using System.Collections;

public class RateUs : MonoBehaviour {

	/// <summary>
	/// Player can rate your app from within the game.
	/// You just need to update the "packageName" with your bundle name, set in unity settings.
	/// </summary>

	public string packageName = "com.yourcompany.gametitle";
	
	void Update () {	
		touchManager();
	}


	private RaycastHit hitInfo;
	private Ray ray;
	void touchManager () {
		
		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			return;
		
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			switch(objectHit.name) {
				
			case "BtnRate":
				Rate ();
				break;	
			}
		}
	}

	void Rate() {
		//debug
		print ("Ready to Rate!");
		Application.OpenURL("market://details?id=" + packageName);
	}
}
