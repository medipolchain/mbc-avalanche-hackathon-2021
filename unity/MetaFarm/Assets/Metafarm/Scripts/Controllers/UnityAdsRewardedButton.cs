using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class UnityAdsRewardedButton : MonoBehaviour {

	private bool canTap = true;
	private string zoneId;			//Important. setting this to null plays the default video.
									//if you want to show rewarded video ads, you have to set your
									//own "Placement Id" here. See Unity Ads dashboard for more info.

	private int rewardQty = 1;		//reward is 1 free energy
	private bool status;
	ShowOptions options = new ShowOptions();

	void Update () {	

		if (string.IsNullOrEmpty (zoneId)) 
			zoneId = null;

		status = Advertisement.IsReady (zoneId) ? true : false;
		options.resultCallback = HandleShowResult;

		//No video button if video ads is not ready to play
		if(status) {
			GetComponent<BoxCollider>().enabled = true;
			GetComponent<Renderer>().enabled = true;
		} else {
			GetComponent<BoxCollider>().enabled = false;
			GetComponent<Renderer>().enabled = false;
		}

		if(canTap)
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
				
			case "BtnVideoAd":
				canTap = false;
				StartCoroutine(reactiveTap());
				Advertisement.Show (zoneId, options);
				break;	
			}
		}
	}

	
	private void HandleShowResult (ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("Video completed. User rewarded " + rewardQty + " energy.");
			//add 1 free energy
			PlayerPrefs.SetInt("PlayerEnergy", PlayerPrefs.GetInt("PlayerEnergy") + 1);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			break;
		case ShowResult.Skipped:
			Debug.LogWarning ("Video was skipped.");
			break;
		case ShowResult.Failed:
			Debug.LogError ("Video failed to show.");
			break;
		}
	}


	IEnumerator reactiveTap() {
		yield return new WaitForSeconds(1.0f);
		canTap = true;
	}
}