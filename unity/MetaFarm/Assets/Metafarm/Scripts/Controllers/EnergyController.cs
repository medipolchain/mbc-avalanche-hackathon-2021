using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnergyController : MonoBehaviour {

	/// <summary>
	/// Updates player energy on the UI
	/// Also redirects the user to the shop scene, if energy is not enough.
	/// </summary>

	public GameObject energyLable;
	public AudioClip menuTap;
	private bool canTap;

	void Start () {
		canTap = true;
		energyLable.GetComponent<TextMesh>().text = "" + PlayerPrefs.GetInt("PlayerEnergy") + "/10";
	}

	void Update () {
		if(canTap)
			StartCoroutine(touchManager());
	}

	///***********************************************************************
	/// Process user inputs
	///***********************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
	IEnumerator touchManager (){
		
		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			yield break;
		
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			//print(objectHit.name);
			if(objectHit.name == "EnergyUI" || objectHit.name == "MoneyUI") {
				canTap = false;
				playSfx(menuTap);
				yield return new WaitForSeconds(0.75f);
				SceneManager.LoadScene("Shop");
			}

		}
	}

	///***********************************************************************
	/// play audio clip
	///***********************************************************************
	void playSfx ( AudioClip _sfx  ){
		GetComponent<AudioSource>().clip = _sfx;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();
	}
}
